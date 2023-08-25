using UnityEngine;
using System.Collections;

public class SepararCapsula : MonoBehaviour
{
    public Rigidbody fogueteRigidbody;
    public Rigidbody capsulaRigidbody;
    public GameObject paraquedasRef;
    public Animator animatorParaquedas;

    public float ejectionForce = 10.0f;
    public float ejectionHeight;

    private bool separacaoAtivada = false;
    private bool capsulaEjetada = false;
    [SerializeField] private float rotationSmoothing = 1.0f; // Ajuste a velocidade da suavização

    private void Update()
    {
        if (!capsulaEjetada)
        {
            EjectCapsula();
        }
    }

    //-----------------------EJECT CAPSULE--v
    public void EjectCapsula()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!separacaoAtivada && transform.position.y < ejectionHeight && fogueteRigidbody.velocity.y < -1f)
            {
                AtivarSeparacao();
                paraquedasRef.SetActive(true);

                fogueteRigidbody.drag = 1.0f;
                animatorParaquedas.SetBool("parachuteFly", true);
            }
        }
    }

    private IEnumerator SmoothStabilizeRotation()
    {
        Quaternion startRotation = fogueteRigidbody.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
        float elapsedTime = 0.0f;

        while (elapsedTime < rotationSmoothing)
        {
            fogueteRigidbody.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationSmoothing);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fogueteRigidbody.rotation = targetRotation; // Garantir que as rotações X, Y e Z sejam exatamente zero no final
        fogueteRigidbody.freezeRotation = true; // Congelar as rotações para evitar qualquer movimento adicional
    }

    private void AtivarSeparacao()
    {
        capsulaRigidbody.isKinematic = false;

        capsulaRigidbody.AddForce(new Vector3(1, 10, 0) * ejectionForce, ForceMode.Impulse);
        separacaoAtivada = true;
        capsulaEjetada = true;

        StartCoroutine(SmoothStabilizeRotation()); // Iniciar a corrotina para suavizar a rotação
    }
}