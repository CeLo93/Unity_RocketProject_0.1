using UnityEngine;

public class SepararCapsula : MonoBehaviour
{
    public Rigidbody fogueteRigidbody;
    public Rigidbody capsulaRigidbody;
    public GameObject paraquedasRef;
    public Animator animatorParaquedas;

    public float ejectionForce = 10.0f;
    public float ejectionHeight = 100.0f;

    private bool separacaoAtivada = false;
    private bool capsulaEjetada = false;

    private void Update()
    {
        if (!capsulaEjetada)
        {
            EjectCapsula();
        }
    }

    public void EjectCapsula()
    {
        if (!separacaoAtivada && transform.position.y < ejectionHeight && fogueteRigidbody.velocity.y < -10f)
        {
            AtivarSeparacao();
            paraquedasRef.SetActive(true);

            fogueteRigidbody.drag = 1.0f;
            animatorParaquedas.SetBool("parachuteFly", true);
        }
    }

    private void AtivarSeparacao()
    {
        capsulaRigidbody.isKinematic = false;

        capsulaRigidbody.AddForce(new Vector3(1, 1, 0) * ejectionForce, ForceMode.Impulse);
        separacaoAtivada = true;
        capsulaEjetada = true;
    }
}