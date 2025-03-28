using TMPro;
using UnityEngine;
 
public class playerHealth : MonoBehaviour
{
    [SerializeField] GameObject heartImage;
    private RectTransform rectT;
    [SerializeField] private TextMeshProUGUI heartText;
    TextMeshProUGUI heartTextObject;
    [SerializeField] private int heartsToShow = 5;
    [SerializeField] private Canvas playerUICanvas;
    private GameObject[] hearts;
    private int index = -1;

    public void SetHearts(int health)
    {
        var change = health - hearts.Length;
        if (change < 0)
        {
            for (var i = 0; i < -change; i++)
            {
                removeHeart();
            }
        }
        else if (change > 0)
        {
            for (var i = 0; i < change; i++)
            {
                addHeart();
            }
        }
    }
     
    public void addHeart()
    {
        GameObject heart;
        if (index == -1)
        {
            heart = Instantiate(heartImage, new Vector3(-865,450), Quaternion.identity, playerUICanvas.transform);
            heart.SetActive(true);
            heart.transform.localPosition = new Vector2(-865, 450);
            index++;
            hearts[index] = heart;
        }else if (index >= hearts.Length - 1)
        {
            if(index == hearts.Length - 1) healthHide();
            index++;
            heartTextObject.text = "x" + (index + 1);
        }
        else
        {
            heart = Instantiate(
                heartImage, hearts[index].transform.position + new Vector3(rectT.sizeDelta.x, 0), 
                Quaternion.identity, playerUICanvas.transform);
            heart.SetActive(true);
            index++;
            hearts[index] = heart;
        }
    }

    public void removeHeart()
    {
        if (index == 0)
        {
            Destroy(hearts[index]);
            index--;
        }
        else if (index >= hearts.Length)
        {
            if (index == hearts.Length) healthShow();
            heartTextObject.text = "x" + (index + 1);
            index--;
        }
        else
        {
            Destroy(hearts[index]);
            hearts[index] = null;
            index--;
        }
    }

    private void healthHide()
    {
        if (!heartTextObject)
        {
            heartTextObject = Instantiate(heartText, playerUICanvas.transform);
            heartTextObject.transform.localPosition = new Vector2(hearts[1].transform.localPosition.x, 450);
            heartTextObject.alignment = TextAlignmentOptions.Center;
        }
        heartTextObject.enabled = true;
        heartTextObject.text = "x" + (index + 1);
        for (int i = 1; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }

    private void healthShow()
    {
        heartTextObject.enabled = false; 
        for (int i = 1; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
     
    private void Start()
    {
        heartImage.SetActive(false);
        heartText.enabled = false;
        hearts = new GameObject[heartsToShow];
        rectT = heartImage.transform.GetComponent<RectTransform>();
        addHeart();
        addHeart();
        addHeart();
    }
}