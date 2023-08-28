using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ShowInstructions()
    {
        //  l�gica para mostrar as instru��es na tela. Implementa��o futura. Simplifiquei no menu mesmo, por causa do tempo de desenvolvimento
    }
    public void ExitGame()
    {
        Application.Quit(); // Fechar o jogo
    }
}
