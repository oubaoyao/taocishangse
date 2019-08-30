using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSprite : MonoBehaviour
{
    private Image buttonImage;
    //private Button button;
    public Sprite UpSprite, DownSprite;

    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<Image>())
        {
            transform.gameObject.AddComponent<Image>();
        }

        if(!GetComponent<Button>())
        {
            transform.gameObject.AddComponent<Button>();
        }
        buttonImage = GetComponent<Image>();
        //button = GetComponent<Button>();
        //button.transition = Selectable.Transition.SpriteSwap;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitButtonSprite()
    {
        buttonImage.sprite = UpSprite;
    }

    public void DownButtonSprite()
    {
        buttonImage.sprite = DownSprite;
    }
}
