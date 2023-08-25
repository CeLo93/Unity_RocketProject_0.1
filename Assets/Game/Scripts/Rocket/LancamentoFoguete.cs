using UnityEngine;
using System.Collections;

public class LancamentoFoguete : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody fogueteRigidbody;

    public Animator animatorParaquedas;
    [SerializeField] private GameObject baseFoguete;
    public ParticleSystem particulaFaseDois; // Refer�ncia ao componente ParticleSystem
    public ParticleSystem particulaCruzeiro; // Refer�ncia � nova part�cula a ser ativada

    [Header("Settings")]
    [SerializeField] private float velocidadeInicial = 10f;

    [SerializeField] private float alturaFaseDois;
    [SerializeField] private float aumentoVelocidade = 2f;
    [SerializeField] private float atrasoInicioLancamento = 3f;
    [SerializeField] private float duracaoAceleracao = 5f;
    [SerializeField] private float rotationSmoothing = 1.0f; // Ajuste a velocidade da suaviza��o da estabiliza��o de rota��o

    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground";

    private bool lancamentoRealizado = false;
    private bool subindo = true;
    private float velocidadeAtual;
    private float tempoInicioLancamento;
    private float altitude = 0f;

    //----------------------------------------------------VARIAVEIS
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
            if (particulaCruzeiro != null)
            {
                particulaCruzeiro.Play(); // Ativar a part�cula do cruzeiro quando o lan�amento � realizado
            }
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

            //Fase Dois-----
            if (altitude >= alturaFaseDois)
            {
                velocidadeAtual *= 2;
                baseFoguete.SetActive(true);

                // Ativar o sistema de part�culas
                if (particulaFaseDois != null)
                {
                    particulaFaseDois.Play();

                    // Definir a dura��o da part�cula
                    float duracaoParticula = 4.0f; // Dura��o desejada em segundos
                    StartCoroutine(DesativarParticulaAposDuracao(particulaFaseDois, duracaoParticula));

                    // Iniciar a corrotina para ativar a nova part�cula ap�s a pausa da particulaFaseDois
                    StartCoroutine(AtivarParticulaAposPausa(particulaFaseDois, particulaCruzeiro));
                }
            }
            //Fase Dois-----
        }
    }

    private IEnumerator AtivarParticulaAposPausa(ParticleSystem particleSystem, ParticleSystem newParticleSystem)
    {
        // Esperar at� que a part�cula atual seja pausada
        while (particleSystem.isPlaying)
        {
            yield return null;
        }

        // Ativar a nova part�cula
        newParticleSystem.Play();
    }

    // Corrotinas-----
    private IEnumerator DesativarParticulaAposDuracao(ParticleSystem particleSystem, float duracao)
    {
        yield return new WaitForSeconds(duracao);

        // Parar a part�cula e desativar o sistema de part�culas
        particleSystem.Stop();
        particleSystem.gameObject.SetActive(false);
    }

    // Corrotinas-----

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

            // Suavizar as rota��es X, Y e Z para zero
            StartCoroutine(SmoothResetRotations());
        }
    }

    private IEnumerator SmoothResetRotations()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f); // Rota��es X, Y e Z s�o zeradas
        float elapsedTime = 0.0f;

        while (elapsedTime < rotationSmoothing)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationSmoothing);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Garantir que as rota��es X, Y e Z sejam exatamente zero no final
    }
}