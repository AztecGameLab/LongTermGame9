using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class Attack3 : MonoBehaviour
{
    public GameObject snakeHead;

    public float headAttackStretchSpeed = 12f;

    public float headAttackMaxStretch = 12f;

    Vector3 startScale;


    void Start()
    {
        startScale = snakeHead.transform.localScale;
        snakeHead.SetActive(false);
        //Attack(); //to test the code
    }



    
    async void Attack()
    {
        snakeHead.SetActive(true);
        while (snakeHead.transform.localScale.x < headAttackMaxStretch)
        {

            snakeHead.transform.localScale += new Vector3(headAttackStretchSpeed * Time.deltaTime, 0);

            snakeHead.transform.position += -1 * headAttackStretchSpeed * (Time.deltaTime / 2f) * snakeHead.transform.right;
            
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }

        while (snakeHead.transform.localScale.x > startScale.x)
        {
            snakeHead.transform.localScale += -1 * new Vector3(headAttackStretchSpeed * Time.deltaTime, 0);

            snakeHead.transform.position += headAttackStretchSpeed * (Time.deltaTime / 2f) * snakeHead.transform.right;
            
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        snakeHead.SetActive(false);
    }


    
    
}

