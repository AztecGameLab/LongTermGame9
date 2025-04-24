using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class VegetationSpawner : MonoBehaviour
{
    
    public GameObject itemDrop;
    public int dropAmount = 3;
    public float spread = 3;

    public void TriggerItemDrop()
    {
        for (var i = 0; i < dropAmount; i++)
        {
            var item = Instantiate(itemDrop, transform.position + new Vector3(Random.value, Random.value), new Quaternion());
            var direction = Random.value < 0.5 ? -1 : 1;
            var velocity = new Vector2
            {
                x = direction * spread,
                y = spread / 1.2f
            };
            item.GetComponent<Rigidbody2D>().linearVelocity = velocity;
        }
    }
}
