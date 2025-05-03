using UnityEngine;

public class Attacks : MonoBehaviour
{
    public GameObject snakeHead;
    bool headAttack = false; //Change this to true if you want to test the head attack without using the method. Also see line 20
    bool HeadAttackIsStretching = true;

    public float headAttackStretchSpeed = 12f;

    public float headAttackMaxStretch = 12f;

    Vector3 startScale;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = snakeHead.transform.localScale;
        snakeHead.SetActive(false); //Comment this out if you want to test the head attack without using the method. Also see line 6
    }

    // Update is called once per frame
    void Update()
    {
        if(headAttack)
        {
            StretchHead();
            //Debug.Log("stretch!");
        }
    }

    
    public void Attack1(){
        //check if other attacks are active! - Add this part when more attacks are added to the script
        snakeHead.SetActive(true);
        headAttack = true;
        HeadAttackIsStretching = true;
    }

    public void Attack2(){
        //...
    }

    public void Attack3(){
        //...
    }

    void StretchHead()
    {
        // Get the head's current scale
        Vector3 currentScale = snakeHead.transform.localScale;

        // Check if we are currently stretching or retracting
        if (HeadAttackIsStretching)
        {
            // Increase the scale
            currentScale.x += headAttackStretchSpeed * Time.deltaTime;

            // Apply the new scale to the head
            snakeHead.transform.localScale = currentScale;

            // Adjust the position to keep one end fixed while stretching
            Vector3 currentPosition = snakeHead.transform.position;
            snakeHead.transform.position = currentPosition - snakeHead.transform.right * headAttackStretchSpeed * Time.deltaTime / 2f;

            // If we've reached the max stretch, start retracting
            if (currentScale.x >= headAttackMaxStretch)
            {
                HeadAttackIsStretching = false; // Switch to retracting
            }
        }
        else
        {
            // Decrease the scale
            currentScale.x -= headAttackStretchSpeed * Time.deltaTime;

            // Apply the new scale to the head
            snakeHead.transform.localScale = currentScale;

            // Adjust the position to keep one end fixed while retracting
            Vector3 currentPosition = snakeHead.transform.position;
            snakeHead.transform.position = currentPosition + snakeHead.transform.right * headAttackStretchSpeed * Time.deltaTime / 2f;

            // If we've reached the original size, stop retracting
            if (currentScale.x <= startScale.x)
            {
                headAttack = false; //Turn off attack
                HeadAttackIsStretching = true; // Switch to stretching
                snakeHead.SetActive(false); //Turn off snakeHead after the attack is done
            }
        }
        
        
    }


    


    
}

