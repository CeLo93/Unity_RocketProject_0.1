using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton; // Agora é público

    private void Start()
    {
        // Obter o componente Button do botão
        exitButton.onClick.AddListener(ExitGame);
    }

    public void ExitGame() // Agora é público
    {
        // Sair do jogo (apenas em build standalone)
        Application.Quit();
    }
}
