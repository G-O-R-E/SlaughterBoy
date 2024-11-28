using UnityEngine;

public class DesactivateTouchBetweenScene : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject,0.5f);
    }
}
