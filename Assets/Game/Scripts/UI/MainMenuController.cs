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
        //  l�gica para mostrar as instru��es na tela
    }
}
