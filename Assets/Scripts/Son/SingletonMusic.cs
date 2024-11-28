using UnityEngine;

public class SingletonMusic : MonoBehaviour
{
    private static SingletonMusic instance;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
}