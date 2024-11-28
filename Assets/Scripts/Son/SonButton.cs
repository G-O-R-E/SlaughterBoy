using UnityEngine;

public class SonButton: MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip over;
    [SerializeField] AudioClip click;

    public void Over()
    {
        audioSource.clip = over;
        audioSource.Play();
    }

    public void Click()
    {
        audioSource.clip = click;
        audioSource.Play();
    }
}
