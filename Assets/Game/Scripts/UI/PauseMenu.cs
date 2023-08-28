using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Refer�ncia ao objeto do menu de pausa
    public LancamentoFoguete lancamentoFoguete; // Refer�ncia ao script do lan�amento

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false); // Desativar o menu de pausa no in�cio do jogo
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Retomar o tempo do jogo
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pausar o tempo do jogo
        isPaused = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; // Certificar que o jogo n�o est� pausado ap�s reiniciar a cena
    }

    public void ExitGame()
    {
        Application.Quit(); // Fechar o jogo
    }
}