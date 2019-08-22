using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class WorksData
{
    public int Model_number;
    public string Texture2D_Path;
    public string Jpg_path;
}


public class GamePanel : BasePanel
{
    public static Transform CurrentModel = null;

    //作为模板数据的material
    private Material material;

    //复制模板得到的要使用的material
    private Material Oldmaterial;
    //复制模板得到的只在选择模型界面使用的展示用material
    private Material NewMaterial;
  
    //替换MeshRanderer里面的material所需的list
    public List<Material> CanvasBaseMaterial;
    public List<Material> Newmaterial;

    public ChooseUIPanel chooseuipanel;
    public GameUIPanel gameuiPanel;

    public override void InitFind()
    {
        base.InitFind();
        material = Resources.Load<Material>("TexturePainter ( Place Me Out of Resources)/Materials/BaseMaterial");

        chooseuipanel = FindTool.FindChildComponent<ChooseUIPanel>(transform, "ChooseUIPanel");
        gameuiPanel = FindTool.FindChildComponent<GameUIPanel>(transform, "GameUIPanel");

    }

    public override void InitEvent()
    {
        base.InitEvent();

    }

    public override void Open()
    {
        base.Open();
        gameuiPanel.Hide();
        gameuiPanel.completePanel.Hide();
    }

    //protected override void Start()
    //{
    //    base.Start();
    //    NewMaterial = new Material(material);
    //    Newmaterial.Add(NewMaterial);
    //}

    public void rotation_left()
    {
        //CurrentModel.Rotate(Vector3.back * Time.deltaTime * 5);
        CurrentModel.transform.eulerAngles += new Vector3(0, 0, 5);
    }

    public void ratation_right()
    {
       // CurrentModel.Rotate(Vector3.forward*Time.deltaTime*5);
        CurrentModel.transform.eulerAngles -= new Vector3(0, 0, 5);
    }


    public void InitModel()
    {
        ResetMaterial();

        ImageAddListen(chooseuipanel.ImageButtonGroup, 0);
        foreach (Transform item in TexturePainter.Instance.ModelGroup)
        {
            item.gameObject.SetActive(false);
            if (item.name == "Cylinder003")
            {
                item.localEulerAngles = new Vector3(-90, 0, 90);
            }
            else
            {
                item.localEulerAngles = new Vector3(-90, 0, 0);
            }

        }
        TexturePainter.Instance.ModelGroup[0].gameObject.SetActive(true);
        CurrentModel = TexturePainter.Instance.ModelGroup[0];       
    }


    static void InitButtons(Button btn,int i,int index)
    {
        btn.onClick.AddListener(delegate () {
            foreach (Transform item in TexturePainter.Instance.ModelGroup)
            {
                item.gameObject.SetActive(false);
            }
            //Debug.LogFormat("i=={0},index=={1},i+index=={2}", i, index, i + index);
            TexturePainter.Instance.ModelGroup[i+index].gameObject.SetActive(true);
            CurrentModel = TexturePainter.Instance.ModelGroup[i + index];
        });
    }

    public static void ImageAddListen(Button[] buttons,int index)
    {
        foreach (Button item in buttons)
        {
            item.onClick.RemoveAllListeners();
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            InitButtons(buttons[i], i, index);
        }
    }

    public void ResetMaterial()
    {
        TexturePainter.Instance.SaveTexture();
        CanvasBaseMaterial.Clear();
        NewMaterial = new Material(material);
        NewMaterial.mainTexture = null;
        CanvasBaseMaterial.Add(NewMaterial);
        TexturePainter.Instance.meshRenderer.materials = CanvasBaseMaterial.ToArray();
    }

}
