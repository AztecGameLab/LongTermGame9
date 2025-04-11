using UnityEngine;
using System.Collections;

public class TESTSNAKE : MonoBehaviour
{
    //starting pos is x = 6.8892, y = 2.2692
    //ending pos is x = -3.75, y = -2.05
    [SerializeField] private int damage;
    public Vector2 startPosition; // Original position
    public Vector2 centerPosition = new Vector2(0, 0); // Target position
    public float moveSpeed = 10f; // Speed of movement
    public float lingerTime = 1.5f; // Time to stay in center
    private bool isMoving = false;
    void Start()
    {
        startPosition = transform.position; // Save the initial position
    }
    //for debug purposes, will implement the attack into boss behavior later
    void Update()
    {
        // Move when J is pressed and it's not already moving
        if (Input.GetKeyDown(KeyCode.J) && !isMoving)
        {
            StartCoroutine(attack2());
        }
    }

    IEnumerator attack2()
    {
        isMoving = true;

        // Move to the center
        yield return StartCoroutine(MoveToPosition(centerPosition));

        // Wait for some time
        yield return new WaitForSeconds(lingerTime);

        // Move back to original position
        yield return StartCoroutine(MoveToPosition(startPosition));

        isMoving = false;
    }

    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //detect if attack 2 hits player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Saguaro hit");
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                //put actual health values where it says damage once level design team decides
                health.ApplyDamage(damage, DamageType.Enemy, gameObject);
            }
        }
    }
}