using UnityEngine;
using System.Collections;

public class MovingSquare : MonoBehaviour
{
    public Vector2 startPosition; // Original position
    public Vector2 centerPosition = new Vector2(0, 0); // Target position
    public float moveSpeed = 5f; // Speed of movement
    public float lingerTime = 1.5f; // Time to stay in center
    private bool isMoving = false;

    void Start()
    {
        startPosition = transform.position; // Save the initial position
    }

    void Update()
    {
        // Move when J is pressed and it's not already moving
        if (Input.GetKeyDown(KeyCode.J) && !isMoving)
        {
            StartCoroutine(MoveToCenterAndBack());
        }
    }

    IEnumerator MoveToCenterAndBack()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Moving square collided with the player!");
        }
    }
}