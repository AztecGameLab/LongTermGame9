using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    
    public GameObject itemDrop;
    public int dropAmount = 3;

    public void TriggerItemDrop()
    {
        for (int i = 0; i < dropAmount; i++)
        {
            Instantiate(itemDrop, transform.position + new Vector3(Random.value, Random.value), new Quaternion());
        }
    }
}
