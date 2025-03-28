using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    
    public GameObject itemDrop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            Instantiate(itemDrop, transform.position, new Quaternion());
            enabled = false;
        }
    }
}
