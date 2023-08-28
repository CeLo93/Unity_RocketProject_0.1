using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton; // Agora � p�blico

    private void Start()
    {
        // Obter o componente Button do bot�o
        exitButton.onClick.AddListener(ExitGame);
    }

    public void ExitGame() // Agora � p�blico
    {
        // Sair do jogo (apenas em build standalone)
        Application.Quit();
    }
}
