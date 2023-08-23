using UnityEngine;

public class LancamentoFoguete : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody fogueteRigidbody;

    [Header("Settings")]
    [SerializeField] private float velocidadeInicial = 10f; // Velocidade inicial acima da gravidade

    [SerializeField] private float aumentoVelocidade = 2f; // Aumento de velocidade por segundo
    [SerializeField] private float atrasoInicioLancamento = 3f; // Atraso para iniciar o lançamento após pressionar "L"
    [SerializeField] private float duracaoAceleracao = 5f; // Duração da aceleração em segundos

    private bool lancamentoRealizado = false;
    private bool mensagemMostrada = false;
    private float distanciaPercorrida = 0f; // Distância percorrida durante a aceleração
    private float velocidadeAtual;
    private float tempoInicioLancamento;

    private void Update()
    {
        // Iniciar o lançamento após pressionar "L" e após o atraso especificado
        if (Input.GetKeyDown(KeyCode.L) && !lancamentoRealizado)
        {
            lancamentoRealizado = true;
            tempoInicioLancamento = Time.time + atrasoInicioLancamento;
        }

        // Mostrar a mensagem de velocidade atual após o lançamento ser iniciado
        if (lancamentoRealizado)
        {
            mensagemMostrada = true;
            Debug.Log("Lançamento iniciado! Velocidade Atual: " + velocidadeAtual);
        }
    }

    private void FixedUpdate()
    {
        // Se o lançamento foi iniciado após o atraso, aplicar a força no eixo local Y
        if (lancamentoRealizado && Time.time >= tempoInicioLancamento && Time.time - tempoInicioLancamento <= duracaoAceleracao)
        {
            // Calcular a força baseada na velocidade inicial e no aumento de velocidade
            velocidadeAtual = velocidadeInicial + aumentoVelocidade * (Time.time - tempoInicioLancamento);

            // Calcular a distância percorrida durante a aceleração
            distanciaPercorrida = (velocidadeInicial * (Time.time - tempoInicioLancamento)) + (0.5f * aumentoVelocidade * Mathf.Pow((Time.time - tempoInicioLancamento), 2));

            // Aplicar a força no eixo local Y usando ForceMode.Acceleration
            fogueteRigidbody.AddForce(transform.up * velocidadeAtual, ForceMode.Acceleration);
        }
    }
}