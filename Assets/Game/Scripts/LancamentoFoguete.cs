using UnityEngine;

public class LancamentoFoguete : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody fogueteRigidbody;

    [Header("Settings")]
    [SerializeField] private float velocidadeInicial = 10f; // Velocidade inicial acima da gravidade

    [SerializeField] private float aumentoVelocidade = 2f; // Aumento de velocidade por segundo
    [SerializeField] private float atrasoInicioLancamento = 3f; // Atraso para iniciar o lan�amento ap�s pressionar "L"
    [SerializeField] private float duracaoAceleracao = 5f; // Dura��o da acelera��o em segundos

    private bool lancamentoRealizado = false;
    private bool mensagemMostrada = false;
    private float distanciaPercorrida = 0f; // Dist�ncia percorrida durante a acelera��o
    private float velocidadeAtual;
    private float tempoInicioLancamento;

    private void Update()
    {
        // Iniciar o lan�amento ap�s pressionar "L" e ap�s o atraso especificado
        if (Input.GetKeyDown(KeyCode.L) && !lancamentoRealizado)
        {
            lancamentoRealizado = true;
            tempoInicioLancamento = Time.time + atrasoInicioLancamento;
        }

        // Mostrar a mensagem de velocidade atual ap�s o lan�amento ser iniciado
        if (lancamentoRealizado)
        {
            mensagemMostrada = true;
            Debug.Log("Lan�amento iniciado! Velocidade Atual: " + velocidadeAtual);
        }
    }

    private void FixedUpdate()
    {
        // Se o lan�amento foi iniciado ap�s o atraso, aplicar a for�a no eixo local Y
        if (lancamentoRealizado && Time.time >= tempoInicioLancamento && Time.time - tempoInicioLancamento <= duracaoAceleracao)
        {
            // Calcular a for�a baseada na velocidade inicial e no aumento de velocidade
            velocidadeAtual = velocidadeInicial + aumentoVelocidade * (Time.time - tempoInicioLancamento);

            // Calcular a dist�ncia percorrida durante a acelera��o
            distanciaPercorrida = (velocidadeInicial * (Time.time - tempoInicioLancamento)) + (0.5f * aumentoVelocidade * Mathf.Pow((Time.time - tempoInicioLancamento), 2));

            // Aplicar a for�a no eixo local Y usando ForceMode.Acceleration
            fogueteRigidbody.AddForce(transform.up * velocidadeAtual, ForceMode.Acceleration);
        }
    }
}