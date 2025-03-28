using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    [SerializeField] Image heartImage;
    [SerializeField] private TextMeshProUGUI heartText;
    TextMeshProUGUI heartTextObject;
    [SerializeField] int health = 0;
    [SerializeField] private Canvas playerUICanvas;
    private Image[] hearts = new Image[5];
    private int index = -1;

    public void addHeart()
    {
        Image heart;
        if (index == -1)
        {
            //heart = Instantiate(heartImage, new Vector2(-9.2f,4), Quaternion.identity, playerUICanvas.transform);
            heart = Instantiate(heartImage, /*new Vector3(-370,170), Quaternion.identity,*/ playerUICanvas.transform);
            heart.transform.position = new Vector2(35, 360);
            index++;
            hearts[index] = heart;
            health = 1;
        }else if (index >= hearts.Length - 1)
        {
            health++;
            healthHide();
            heartTextObject.text = health.ToString();
        }
        else
        {
            heart = Instantiate(
                heartImage, hearts[index].transform.position + new Vector3(/*heartImage.transform.localScale.x * 1.5f*/ 60, 0), 
                Quaternion.identity, playerUICanvas.transform);
            index++;
            hearts[index] = heart;
            health++;
        }
    }

    public void healthHide()
    {
        heartTextObject = Instantiate(heartText, playerUICanvas.transform);
        heartTextObject.transform.position = new Vector2(170, 360);
        heartTextObject.text = health.ToString();
        for (int i = 1; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }

    public void healthShow()
    {
        for (int i = 1; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        addHeart();
        addHeart();
        addHeart();
        addHeart();
        addHeart();
        addHeart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
