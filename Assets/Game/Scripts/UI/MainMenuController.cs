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
        //  lógica para mostrar as instruções na tela
    }
}
