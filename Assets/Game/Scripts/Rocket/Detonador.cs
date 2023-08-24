using UnityEngine;

public class Detonador : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            gameObject.SetActive(false);
        }
    }
}