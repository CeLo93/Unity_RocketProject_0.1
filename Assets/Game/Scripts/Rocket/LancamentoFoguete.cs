using UnityEngine;

public class LancamentoFoguete : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody fogueteRigidbody;

    public Animator animatorParaquedas;

    [SerializeField] private GameObject baseFoguete;

    [Header("Settings")]
    [SerializeField] private float velocidadeInicial = 10f;

    [SerializeField] private float alturaFaseDois;
    [SerializeField] private float aumentoVelocidade = 2f;
    [SerializeField] private float atrasoInicioLancamento = 3f;
    [SerializeField] private float duracaoAceleracao = 5f;

    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground";

    private bool lancamentoRealizado = false;
    private bool subindo = true;
    private float velocidadeAtual;
    private float tempoInicioLancamento;
    private float altitude = 0f;

    private void Start()
    {
        fogueteRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !lancamentoRealizado)
        {
            lancamentoRealizado = true;
            tempoInicioLancamento = Time.time + atrasoInicioLancamento;
        }

        float tempoDecorrido = Time.time - tempoInicioLancamento;

        velocidadeAtual = subindo
            ? velocidadeInicial + aumentoVelocidade * tempoDecorrido
            : velocidadeInicial - aumentoVelocidade * (tempoDecorrido - duracaoAceleracao);

        if (lancamentoRealizado)
        {
            altitude = transform.position.y;

            Debug.Log("Tempo: " + tempoDecorrido + " segundos | Velocidade Atual: " + velocidadeAtual + " km/h | " + "Altitude: " + altitude + " metros");

            if (tempoDecorrido >= duracaoAceleracao)
            {
                subindo = false;
            }

            if (altitude >= alturaFaseDois)
            {
                velocidadeAtual *= 2;
                baseFoguete.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (lancamentoRealizado && Time.time >= tempoInicioLancamento && Time.time - tempoInicioLancamento <= duracaoAceleracao)
        {
            fogueteRigidbody.AddForce(transform.up * velocidadeAtual, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            //animatorParaquedas.SetBool("parachuteFly", false);

            animatorParaquedas.SetBool("parachuteGround", true);

            lancamentoRealizado = false;
            fogueteRigidbody.velocity = Vector3.zero;
        }
    }
}