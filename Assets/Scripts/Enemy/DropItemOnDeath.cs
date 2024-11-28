using UnityEngine;

public class DropItemOnDeath : MonoBehaviour
{
    [SerializeField] MonoBehaviour m_gameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItem()
    {
        Instantiate(m_gameObject, transform);
    }
}
