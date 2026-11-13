using UnityEngine;

public class WeaponSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip tiro;

    public void Atirar()
    {
        audioSource.PlayOneShot(tiro);
    }
}