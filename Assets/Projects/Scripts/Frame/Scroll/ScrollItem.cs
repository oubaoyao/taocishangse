using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrollItem : MonoBehaviour
{
    public RawImage[] ImageGroup;
    public Texture[] ImageItem;
    public string ImageItemPath;

    private int Index = 0;
    // Start is called before the first frame update
    void Start()
    {
        ImageItem = Resources.LoadAll<Texture>(ImageItemPath);
        ImageGroup = GetComponentsInChildren<RawImage>();

        if(ImageItem.Length != 0)
        {
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < ImageItem.Length)
                {
                    ImageGroup[i].texture = ImageItem[i];
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Left();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            Right();
        }

    }

    public virtual void Left()
    {
        if(ImageItem.Length != 0)
        {
            Index--;
            if (Index < 0)
            {
                Index = 0;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < ImageItem.Length)
                {
                    ImageGroup[i].texture = ImageItem[i + Index];
                }
            }
        }

    }

    public virtual void Right()
    {
        if (ImageItem.Length != 0)
        {
            Index++;
            if (Index + ImageGroup.Length >= ImageItem.Length)
            {
                Index--;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < ImageItem.Length)
                {
                    ImageGroup[i].texture = ImageItem[i + Index];
                }
            }
        }

    }


}
