using UnityEngine;

public class LancamentoFoguete : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody fogueteRigidbody;

    [Header("Settings")]
    [SerializeField] private float velocidadeInicial = 10f; // Velocidade inicial acima da gravidade

    [SerializeField] private float aumentoVelocidade = 2f; // Aumento de velocidade por segundo
    [SerializeField] private float atrasoInicioLancamento = 3f; // Atraso para iniciar o lançamento após pressionar "L"

    private bool lancamentoRealizado = false;
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
    }

    private void FixedUpdate()
    {
        // Se o lançamento foi iniciado após o atraso, aplicar a força apenas no eixo Y
        if (lancamentoRealizado && Time.time >= tempoInicioLancamento)
        {
            // Calcular a força baseada na velocidade inicial e no aumento de velocidade
            velocidadeAtual = velocidadeInicial + aumentoVelocidade * (Time.time - tempoInicioLancamento);

            // Aplicar a força no eixo Y usando ForceMode.Acceleration
            fogueteRigidbody.AddForce(Vector3.up * velocidadeAtual, ForceMode.Acceleration);

            // Mostrar a mensagem de velocidade atual
            Debug.Log("Lançamento iniciado! Velocidade Atual: " + velocidadeAtual);
        }
    }
}