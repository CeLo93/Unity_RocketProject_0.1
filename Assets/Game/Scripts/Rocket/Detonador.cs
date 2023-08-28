using UnityEngine;

public class Detonador : MonoBehaviour
{
    public GameObject BodyRef;
    public GameObject CapsuleRef;

    public Rigidbody FogueteRb;

    [Header("Collision")]
    [SerializeField] private string groundTag = "Ground";

    public ParticleSystem crashGround; // Referência ao componente ParticleSystem
    public AudioSource soundCrash;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            crashGround.Play();
            soundCrash.Play();
            FogueteRb.isKinematic = true; // Tornar o Rigidbody kinematic
            BodyRef.SetActive(false);
            CapsuleRef.SetActive(false);
        }
    }
}