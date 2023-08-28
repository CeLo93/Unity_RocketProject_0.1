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
    public LancamentoFoguete lancamentoFoguete;
    public AudioSource soundSource4;

    [Header("Setting")]
    [SerializeField] private float rotationSmoothing = 1.0f; // Ajusta a velocidade da suavização.

    // interpolação linear drag--v
    private bool increasingDrag = false;

    private float dragStartTime;
    private float dragStartValue;
    private float dragTargetValue = 1.0f;
    private float dragDuration = 17.0f; // Tempo para atingir o drag máximo
    // interpolação linear drag--^

    private void Update()
    {
        if (!capsulaEjetada)
        {
            EjectCapsula();
        }
        // Se estamos aumentando o drag...
        if (increasingDrag)
        {
            float elapsedTime = Time.time - dragStartTime;
            float t = Mathf.Clamp01(elapsedTime / dragDuration); // Normalizar o tempo

            // Interpolar linearmente entre o valor inicial e o valor alvo do drag
            float newDrag = Mathf.Lerp(dragStartValue, dragTargetValue, t);
            fogueteRigidbody.drag = newDrag;

            // Verificar se atingimos o valor alvo do drag
            if (t >= 1.0f)
            {
                increasingDrag = false;
            }
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

                dragStartTime = Time.time;
                dragStartValue = fogueteRigidbody.drag;
                increasingDrag = true;

                animatorParaquedas.SetBool("parachuteFly", true);

                lancamentoFoguete.SoundSourceLauncherAndPlayNewAudio(0.4f);
                if (soundSource4 != null)
                {
                    soundSource4.Play();
                }
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

        capsulaRigidbody.AddForce(new Vector3(Random.value, Random.value, Random.value) * ejectionForce, ForceMode.Impulse);
        separacaoAtivada = true;
        capsulaEjetada = true;

        StartCoroutine(SmoothStabilizeRotation()); // Iniciar a corrotina para suavizar a rotação
    }
}