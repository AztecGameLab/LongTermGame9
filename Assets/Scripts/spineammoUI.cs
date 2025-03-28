using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
 
public class spineammoUI : MonoBehaviour
{
    [SerializeField] GameObject spineImage;
    [SerializeField] private TextMeshProUGUI spineText;
    [SerializeField] private Canvas playerUICanvas;
    private int spineIndex = 0;

    public void SetSpines(int spines)
    {
        if (spineIndex == 0)
        {
            spineImage.SetActive(true);
            spineText.enabled = true;
        }
        
        spineIndex = spines;
        spineText.text = "x" + spineIndex;         
    }
     
    public void addSpine()
    {
        if (spineIndex == 0)
        {
            spineImage.SetActive(true);
            spineText.enabled = true;
        }
        spineIndex++;
        spineText.text = "x" + spineIndex;
    }
 
    public void removeSpine() 
    {

        spineIndex--;
        spineText.text = "x" + spineIndex;
    }
 
    private void Start() 
    {
        spineImage.SetActive(true);
        spineText.enabled = true;
    }
}