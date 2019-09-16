using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using MTFrame;
using System;

[System.Serializable]
public class WorksData
{
    public string Model_name;
    public string[] Texture_Path;
    public string Jpg_path;
}

public class WorksDataGroup
{
    public WorksData[] worksDatas;
}


public class WorksDataControl : MonoBehaviour
{
    public static WorksDataControl Instance;
    public List<WorksData> worksDatas = new List<WorksData>();
    private string WorksJsonDataPath = "/WorksDatas";
    private string WorksJsonDataName = "WorksJsonDatas.Json";
    //public Texture2D texture;
    public List<Texture2D[]> WorksTexture = new List<Texture2D[]>();
    public List<Texture2D> WorksDisplayTexture = new List<Texture2D>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.streamingAssetsPath + WorksJsonDataPath + "/" + WorksJsonDataName);
        //texture = Resources.Load<Texture>("SavePng/44.03922");
        if (System.IO.File.Exists(Application.streamingAssetsPath + WorksJsonDataPath + "/" + WorksJsonDataName))
        {
            //Debug.Log("读取到文件WorksJsonDatas");
            //string str = Resources.Load<TextAsset>("WorksDatas/WorksJsonDatas").text;
            string str = File.ReadAllText(Application.streamingAssetsPath + WorksJsonDataPath + "/" + WorksJsonDataName);
            WorksDataGroup worksDatasGroup = JsonConvert.DeserializeObject<WorksDataGroup>(str);
            List<Texture2D> texture2Ds = new List<Texture2D>();
            for (int i = 0; i < worksDatasGroup.worksDatas.Length; i++)
            {
                worksDatas.Add(worksDatasGroup.worksDatas[i]);
                
                for (int j = 0; j < worksDatasGroup.worksDatas[i].Texture_Path.Length; j++)
                {
                    texture2Ds.Add(LoadByIO(Application.streamingAssetsPath + "/SavePng/" + worksDatasGroup.worksDatas[i].Texture_Path[j] + ".png"));
                }
                WorksTexture.Add(texture2Ds.ToArray());
                WorksDisplayTexture.Add(LoadByIO(Application.streamingAssetsPath + "/SaveImage/" + worksDatasGroup.worksDatas[i].Jpg_path + ".jpg"));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 120.0f, BackMainMenu);
            
        }

        if(Input.GetMouseButtonDown(0))
        {
            TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, BackMainMenu);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void BackMainMenu()
    {
        TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);
        ModelControl.Instance.CloseModel();
        ModelControl.Instance.CloseModel2();
        GC.Collect();
    }

    public void SaveFile(string msg, string FilePath)
    {
        var fss = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        var sws = new System.IO.StreamWriter(fss);
        sws.Write(msg);
        sws.Close();
        fss.Close();
        Debug.Log("保存数据成功===" + msg);
    }

    private void OnDestroy()
    {
        if (worksDatas.Count > 0)
        {
            WorksDataGroup worksDataGroup = new WorksDataGroup();
            worksDataGroup.worksDatas = worksDatas.ToArray();
            string str = JsonConvert.SerializeObject(worksDataGroup);
            SaveFile(str, Application.streamingAssetsPath+WorksJsonDataPath + "/" + WorksJsonDataName);
        }
    }

    public Texture2D LoadByIO(string path)
    {
        //创建文件读取流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        //创建Texture
        int width = 256;
        int height = 256;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);
        return texture2D;
    }

    public void DeleteTexture()
    {
        string jpgname = worksDatas[0].Jpg_path;
        string[] PngName = worksDatas[0].Texture_Path;
        string jpgPath = Application.streamingAssetsPath + "/SaveImage/" + jpgname + ".jpg";
        
        if(File.Exists(jpgPath))
        {
            File.Delete(jpgPath);
        }

        for (int i = 0; i < PngName.Length; i++)
        {
            string PngPath = Application.streamingAssetsPath + "/SavePng/" + PngName[i] + ".png";
            if (File.Exists(PngPath))
            {
                File.Delete(PngPath);
            }
        }

        worksDatas.RemoveAt(0);
        WorksTexture.RemoveAt(0);
        WorksDisplayTexture.RemoveAt(0);
    }
}
