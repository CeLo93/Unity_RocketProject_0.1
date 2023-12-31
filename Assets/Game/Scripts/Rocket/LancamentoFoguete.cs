using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using TMPro;

public class LancamentoFoguete : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody fogueteRigidbody;

    public Animator animatorParaquedas;
    public GameObject baseFoguete;

    public ParticleSystem particulaFaseDois; // Refer�ncia ao componente ParticleSystem
    public ParticleSystem particulaCruzeiro; // Refer�ncia � nova part�cula a ser ativada

    public PauseMenu pauseMenu;

    [Header("AudioSource")]
    public AudioSource soundSourceLauncher; // Refer�ncia ao componente AudioSource

    public AudioSource soundSource2; // Segundo componente AudioSource
    public AudioSource soundSource3; // Terceiro componente AudioSource
    public AudioSource soundSourceGround; // Quarto componente AudioSource

    [Header("Settings")]
    [SerializeField] private float velocidadeInicial = 10f;

    [SerializeField] private float alturaFaseDois;
    [SerializeField] private float aumentoVelocidade = 2f;
    [SerializeField] private float atrasoInicioLancamento = 3f;
    [SerializeField] private float duracaoAceleracao = 5f;
    [SerializeField] private float rotationSmoothing = 1.0f; // Ajuste da velocidade de suaviza��o da estabiliza��o de rota��o

    private bool activeSource3 = false;
    //private bool isPaused = false;

    [Header("Collision")]
    private string groundTag = "Ground";

    private bool lancamentoRealizado = false;
    private bool subindo = true;
    private float velocidadeAtual;
    private float tempoInicioLancamento;
    private float altitude = 0f;

    [Header("UI Text Elements")]
    public TextMeshProUGUI tempoTextMesh;

    public TextMeshProUGUI velocidadeTextMesh;
    public TextMeshProUGUI altitudeTextMesh;

    //----------------------------------------------------VARIAVEIS
    private void Start()
    {
        fogueteRigidbody = GetComponent<Rigidbody>();
        soundSourceLauncher = GetComponent<AudioSource>();
        //soundSource2 = GetComponent<AudioSource>();
        activeSource3 = true;
    }

    private void Update()
    {
        // Verificar se a tecla "Escape" foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(); // Chamar fun��o para pausar o jogo
        }
        if (Input.GetKeyDown(KeyCode.L) && !lancamentoRealizado)
        {
            soundSourceGround.Stop(); // Reduzir o volume pela metade

            lancamentoRealizado = true;
            tempoInicioLancamento = Time.time + atrasoInicioLancamento;

            if (particulaCruzeiro != null)
            {
                particulaCruzeiro.Play(); // Ativar a part�cula do cruzeiro quando o lan�amento � realizado
            }
            if (soundSource2 != null)
            {
                soundSource2.Play(); // Iniciar a reprodu��o do segundo som
            }
            if (soundSourceLauncher != null)
            {
                soundSourceLauncher.Play(); // Iniciar a reprodu��o do som em loop
            }
        }

        float tempoDecorrido = Time.time - tempoInicioLancamento;

        velocidadeAtual = subindo
            ? velocidadeInicial + aumentoVelocidade * tempoDecorrido
            : velocidadeInicial - aumentoVelocidade * (tempoDecorrido - duracaoAceleracao);

        if (lancamentoRealizado)
        {
            altitude = transform.position.y;

            // Atualizar os textos do TextMeshPro Text
            tempoTextMesh.text = "Tempo: " + tempoDecorrido + " segundos";
            velocidadeTextMesh.text = "Velocidade Atual: " + velocidadeAtual + " km/h";
            altitudeTextMesh.text = "Altitude: " + altitude + " metros";

            //Debug.Log("Tempo: " + tempoDecorrido + " segundos | Velocidade Atual: " + velocidadeAtual + " km/h | " + "Altitude: " + altitude + " metros");

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
                    float duracaoParticula = 9.0f; // Dura��o desejada em segundos
                    StartCoroutine(DesativarParticulaAposDuracao(particulaFaseDois, duracaoParticula));

                    // Iniciar a corrotina para ativar a nova part�cula ap�s a pausa da particulaFaseDois
                    StartCoroutine(AtivarParticulaAposPausa(particulaFaseDois, particulaCruzeiro));
                }
                if (soundSource3 != null && activeSource3 == true)
                {
                    soundSource3.Play(); // Iniciar a reprodu��o do segundo som
                    activeSource3 = false;
                }
            }
            //Fase Dois-----
        }
    }//------UPDATE()

    public void SoundSourceLauncherAndPlayNewAudio(float volume)
    {
        if (soundSourceLauncher != null)
        {
            soundSourceLauncher.volume = volume; // Reduzir o volume pela metade
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

            // Suavizar as rota��es X, Y e Z para zero para garantir que o foguete se estabilize
            StartCoroutine(SmoothResetRotations());

            if (soundSourceGround != null)
            {
                soundSourceGround.Play(); // Ativar a part�cula do cruzeiro quando o lan�amento � realizado
                soundSourceLauncher.volume = 0.1f; // Reduzir o volume

                soundSourceGround.volume = 0.1f; // Reduzir o volume
            }
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

    private void PauseGame()
    {
        // Pausar ou despausar o jogo
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f; // Despausar o jogo
            // Esconder o menu de pausa
            pauseMenu.pauseMenuUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f; // Pausar o jogo
            // Exibir o menu de pausa
            pauseMenu.pauseMenuUI.SetActive(true);
        }
    }
}