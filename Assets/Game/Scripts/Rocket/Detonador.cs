using UnityEngine;

public class Detonador : MonoBehaviour
{
    public GameObject FogueteRef;

    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            FogueteRef.SetActive(false);
        }
    }
}