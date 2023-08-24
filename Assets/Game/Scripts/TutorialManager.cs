using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialText;

    private void Start()
    {
        // Desativa o texto do tutorial no início
        tutorialText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Quando o jogador entrar em uma área específica, ativa o texto do tutorial
        if (other.CompareTag("TutorialArea"))
        {
            tutorialText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Quando o jogador sair da área, desativa o texto do tutorial
        if (other.CompareTag("TutorialArea"))
        {
            tutorialText.SetActive(false);
        }
    }
}
