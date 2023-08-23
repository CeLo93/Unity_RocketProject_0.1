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

    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground"; // Tag do objeto de ch�o

    private bool lancamentoRealizado = false;
    private bool subindo = true; // Indica se o foguete est� subindo ou caindo
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

        // Calcular o tempo decorrido desde o in�cio do lan�amento
        float tempoDecorrido = Time.time - tempoInicioLancamento;

        // Calcula a velocidade atual considerando a dire��o (subindo ou caindo). Obs.: Operadores tern�rios
        velocidadeAtual = subindo
            ? velocidadeInicial + aumentoVelocidade * tempoDecorrido
            : velocidadeInicial - aumentoVelocidade * (tempoDecorrido - duracaoAceleracao);

        if (lancamentoRealizado)
        {
            // Mostrar a mensagem da velocidade atual
            Debug.Log("Tempo: " + tempoDecorrido + " segundos | Velocidade Atual: " + velocidadeAtual);

            // Verifica se a acelera��o terminou e inverte a dire��o
            if (tempoDecorrido >= duracaoAceleracao)
            {
                subindo = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Se o lan�amento foi iniciado ap�s o atraso, aplicar a for�a no eixo local Y
        if (lancamentoRealizado && Time.time >= tempoInicioLancamento && Time.time - tempoInicioLancamento <= duracaoAceleracao)
        {
            // Aplicar a for�a no eixo local Y usando ForceMode.Acceleration
            fogueteRigidbody.AddForce(transform.up * velocidadeAtual, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar se a colis�o foi com o objeto de ch�o (tag "Ground")
        if (collision.gameObject.CompareTag(groundTag))
        {
            // Parar a acelera��o e desativar o foguete
            lancamentoRealizado = false;
            fogueteRigidbody.velocity = Vector3.zero; // Parar a velocidade
            gameObject.SetActive(false); // Desativar o foguete
        }
    }
}