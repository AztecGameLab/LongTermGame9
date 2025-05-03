using UnityEngine;

public class ActivateWalls : MonoBehaviour
{

    public GameObject walls;
    
    public void Activate()
    {
        walls.SetActive(true);
    }
}
