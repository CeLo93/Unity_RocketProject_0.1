using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Referência ao objeto do menu de pausa
    public LancamentoFoguete lancamentoFoguete; // Referência ao script do lançamento

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false); // Desativar o menu de pausa no início do jogo
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
        Time.timeScale = 1f; // Certificar que o jogo não está pausado após reiniciar a cena
    }

    public void ExitGame()
    {
        Application.Quit(); // Fechar o jogo
    }
}