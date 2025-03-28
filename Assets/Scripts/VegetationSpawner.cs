using Unity.Mathematics.Geometry;
using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    
    public GameObject itemDrop;
    public int dropAmount = 3;
    
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
            for (int i = 0; i < dropAmount; i++)
            {
                Instantiate(itemDrop, transform.position + new Vector3(Random.value * 0.5f, Random.value * 0.5f), new Quaternion());
                enabled = false;
            }
        }
    }
}
