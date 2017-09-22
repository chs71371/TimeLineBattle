using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitTrigger : MonoBehaviour
{
    /// <summary>
    /// 触发器
    /// </summary>
    public Collider mCol;
    /// <summary>
    /// 控制该触发器的角色
    /// </summary>
    public CharaControl mParent;
    /// <summary>
    /// 碰撞器Id
    /// </summary>
    public string index;
    /// <summary>
    /// 是否激活
    /// </summary>
    public bool isActive;
    /// <summary>
    /// 当前碰撞到的物体列表
    /// </summary>
    public List<GameObject> colList = new List<GameObject>();
    /// <summary>
    /// 碰撞事件
    /// </summary>
    public UnityAction<HitTrigger> TriggerFuncEvent;


    public void Start()
    {
        mCol = gameObject.GetComponent<Collider>();
        if (mCol.isTrigger)
        {
            mCol.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetHitTrigger(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetHitTrigger(other);
    }


    public void SetHitTrigger(Collider other)
    {
        if (!isActive)
        {
            return;
        }

        var hitObj = other.gameObject;

        if (!colList.Contains(hitObj))
        {
           var triggerScr=   hitObj.GetComponent<HitTrigger>();

            if (triggerScr==null||triggerScr.mParent == mParent)
            {
                return;
            }

            colList.Add(hitObj);

            if (TriggerFuncEvent != null)
            {
                TriggerFuncEvent(triggerScr);
            }
        }
    }



    public void SetColActive(bool rActive)
    {
        isActive = rActive;
        if (mCol.isTrigger)
        {
            mCol.enabled = rActive;
        }
      
        if (!isActive)
        {
            colList.Clear();
        }
    }

}
