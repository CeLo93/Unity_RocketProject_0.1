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

    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground"; // Tag do objeto de chão

    private bool lancamentoRealizado = false;
    private bool subindo = true; // Indica se o foguete está subindo ou caindo
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

        // Calcular o tempo decorrido desde o início do lançamento
        float tempoDecorrido = Time.time - tempoInicioLancamento;

        // Calcula a velocidade atual considerando a direção (subindo ou caindo). Obs.: Operadores ternários
        velocidadeAtual = subindo
            ? velocidadeInicial + aumentoVelocidade * tempoDecorrido
            : velocidadeInicial - aumentoVelocidade * (tempoDecorrido - duracaoAceleracao);

        if (lancamentoRealizado)
        {
            // Mostrar a mensagem da velocidade atual
            Debug.Log("Tempo: " + tempoDecorrido + " segundos | Velocidade Atual: " + velocidadeAtual);

            // Verifica se a aceleração terminou e inverte a direção
            if (tempoDecorrido >= duracaoAceleracao)
            {
                subindo = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Se o lançamento foi iniciado após o atraso, aplicar a força no eixo local Y
        if (lancamentoRealizado && Time.time >= tempoInicioLancamento && Time.time - tempoInicioLancamento <= duracaoAceleracao)
        {
            // Aplicar a força no eixo local Y usando ForceMode.Acceleration
            fogueteRigidbody.AddForce(transform.up * velocidadeAtual, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar se a colisão foi com o objeto de chão (tag "Ground")
        if (collision.gameObject.CompareTag(groundTag))
        {
            // Parar a aceleração e desativar o foguete
            lancamentoRealizado = false;
            fogueteRigidbody.velocity = Vector3.zero; // Parar a velocidade
            gameObject.SetActive(false); // Desativar o foguete
        }
    }
}