using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PolygonImage : MonoBehaviour
{
    public List<Image> mImgList;

    public Color clickColor=new Color(0.9f, 0.9f, 0.9f, 1);

    public Color normalColor= Color.white;

    private Vector3 perMousePos;
 
    UnityAction<GameObject> onClickEvent;

 
    private void Awake()
    {
        var mImg = gameObject.GetComponent<Image>();
        if (!mImgList.Contains(mImg))
        {
            mImgList.Add(mImg);
        }
    }

    public void SetClickEvent(UnityAction<GameObject> rFunc)
    {
        onClickEvent = rFunc;
    }

    private void OnMouseDown()
    {
        perMousePos = Input.mousePosition;
        for (int i=0;i< mImgList.Count;i++)
        {
            mImgList[i].color = clickColor;
        }
    }

    private void OnMouseUp()
    {
        for (int i = 0; i < mImgList.Count; i++)
        {
            mImgList[i].color = normalColor;
        }
        if (Vector3.Distance(perMousePos, Input.mousePosition) < Screen.width / 20)
        {
            if (onClickEvent != null)
            {
                onClickEvent(gameObject);
            }
        }
    }
}
