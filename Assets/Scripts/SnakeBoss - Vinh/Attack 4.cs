using UnityEngine;

public class Attack4 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.transform.position = new Vector2(gameObject.transform.position.x-1, gameObject.transform.position.y);
    }

    public void AttackType4()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }
}
