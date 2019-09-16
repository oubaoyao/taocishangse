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

        }
        
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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        ResetMaterial();
    //    }
    //}
}
