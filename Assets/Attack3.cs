using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class Attack3 : MonoBehaviour
{
    public GameObject snakeHead;

    public float headAttackStretchSpeed = 50f;
    public float headAttackRetractSpeed = 80f;

    public float headAttackMaxStretch = 13f;

    Vector3 startScale;


    void Start()
    {
        startScale = snakeHead.transform.localScale;
        snakeHead.SetActive(false);
        //Attack(); //to test the code
    }



    
    public async void Attack(SnakeBoss sb)
    {
        snakeHead.SetActive(true);
        while (!snakeHead.IsUnityNull() && snakeHead.transform.localScale.x < headAttackMaxStretch)
        {

            snakeHead.transform.localScale += new Vector3(headAttackStretchSpeed * Time.deltaTime, 0);

            snakeHead.transform.position += -1 * headAttackStretchSpeed * (Time.deltaTime / 2f) * snakeHead.transform.right;
            
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }

        while (snakeHead.transform.localScale.x > startScale.x)
        {
            snakeHead.transform.localScale += -1 * new Vector3(headAttackStretchSpeed * Time.deltaTime, 0);

            snakeHead.transform.position += headAttackRetractSpeed * (Time.deltaTime / 2f) * snakeHead.transform.right;
            
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        snakeHead.SetActive(false);
        sb.setAttackInProgress(false);
    }


    
    
}

