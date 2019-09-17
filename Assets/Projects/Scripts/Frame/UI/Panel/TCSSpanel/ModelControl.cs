using Es.InkPainter;
using Es.InkPainter.Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControl : MonoBehaviour
{
    public static ModelControl Instance;

    public Transform ModelTransform;
    public Transform ModelTransform2;

    public List<Transform> ModelGroup;
    public List<Transform> ModelGroup2;

    public List<InkCanvas> ModelInkGroup;
    //public Texture WhiteTexture;
    //public Material WhiteMaterial;

    public GameObject ColorSelector;
    public GameObject Buttons;

    private float RotateValue = 0, factor = 0.2f,maxfactor = 1.0f,minfactor = 0.6f;

    //public static Vector3 LocalPosition = new Vector3(0, -1.68f, 3.83f);


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in ModelTransform)
        {
            ModelGroup.Add(item);
            foreach (InkCanvas ink in item.gameObject.GetComponentsInChildren<InkCanvas>())
            {
                ModelInkGroup.Add(ink.GetComponent<InkCanvas>());
            }         
        }

        foreach (Transform item in ModelTransform2)
        {
            ModelGroup2.Add(item);
        }
    }

    public void CloseModel()
    {
        ResetMaterial();
        foreach (Transform item in ModelGroup)
        {
            item.gameObject.SetActive(false);
            if(item.name == "Cylinder004")
            {
                item.localPosition = new Vector3(-0.55f, -2.19f, 3.9f);
            }
            else if(item.name == "Cylinder002")
            {
                item.localPosition = new Vector3(0.25f, -2.01f, 3.79f);
            }
            else
            {
                item.localPosition = new Vector3(0, -2.01f, 3.9f);
            }

            item.localEulerAngles = new Vector3(-90, 0, 0);
            item.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        ModelGroup[2].localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void CloseModel2()
    {
        foreach (Transform item in ModelGroup2)
        {
            item.gameObject.SetActive(false);

            item.localEulerAngles = new Vector3(-90, 0, 0);

        }
    }

    public void ResetMaterial()
    {
        foreach (InkCanvas item in ModelInkGroup)
        {
            item.ResetPaint();
            //Debug.Log("复原");
        }
    }

    public void PointDown_Right()
    {
        AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        GamePanel.CurrentModel.Rotate(Vector3.forward * -18);
        RotateValue = -20;

        //ModelViewControls.Instance.Start_Rotate_Right();
    }

    public void PointDown_Left()
    {
        AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        GamePanel.CurrentModel.Rotate(Vector3.forward * 18);
        RotateValue = 20;
        //GamePanel.CurrentModel.Rotate(Vector3.back * Time.deltaTime);
        //ModelViewControls.Instance.Start_Rotate_Left();
    }

    public void PointUp()
    {
        RotateValue = 0;
        //ModelViewControls.Instance.Stop_Rotate();
    }

    public void EnlargeButton()
    {
        float temp = GamePanel.CurrentModel.localScale.x;
        temp = temp * (1 + factor);
        if (GamePanel.CurrentModel.name == "Cylinder003")
        {
            if (temp > 0.8f)
            {
                temp = 0.8f;
            }
        }
        else if(GamePanel.CurrentModel.name == "Cylinder001")
        {
            if (temp > 0.9f)
            {
                temp = 0.9f;
            }
        }
        else
        {
            if (temp > maxfactor)
            {
                temp = maxfactor;
            }
        }     
        GamePanel.CurrentModel.localScale = new Vector3(temp, temp, temp);
    }

    public void NarrowButton()
    {
        float temp = GamePanel.CurrentModel.localScale.x;
        temp = temp * (1 - factor);
        if (GamePanel.CurrentModel.name == "Cylinder003")
        {
            if (temp < 0.5f)
            {
                temp = 0.5f;
            }
        }
        else
        {
            if (temp < minfactor)
            {
                temp = minfactor;
            }
        }
        GamePanel.CurrentModel.localScale = new Vector3(temp, temp, temp);
    }

    private void Update()
    {
        if (MousePainter.Instance.IsGamestart)
        {
            GamePanel.CurrentModel.Rotate(Vector3.forward * Time.deltaTime * RotateValue);
        }
    }

    
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        ResetMaterial();
    //    }
    //}
}
