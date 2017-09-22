using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharaControl : MonoBehaviour {

    /// <summary>
    /// 是否是当前操作的玩家
    /// </summary>
    public bool isLoaclPlayer;

    public bool isPause=false;

    public Animator mAim;
    public Rigidbody mRigid;
    /// <summary>
    /// 当前打击碰撞触发器的字典
    /// </summary>
    public Dictionary<string, HitTrigger> mTriggerDict = new Dictionary<string, HitTrigger>();
    /// <summary>
    /// 当前动画时间存储时间
    /// </summary>
    public Dictionary<string, float> mAimLenDict = new Dictionary<string, float>();

    public enum eState
    {
        Idle,
        Hit,
        Air,
        Atk,
    }
    /// <summary>
    /// 当前角色状态
    /// </summary>
    public eState curState = eState.Idle;

    public enum eAtkState
    {
        None,
        Atk,
        AtkWait,
    }

    /// <summary>
    /// 当前攻击状态
    /// </summary>
    public eAtkState curAtkState = eAtkState.None;


    public Vector3 moveDir= Vector3.zero;

 
    public enum eAtkType
    {
        None,
        Atk01,
        Atk02,
        Pick,
        Air_Atk01,
        Air_Atk02,
    }

    /// <summary>
    /// 当前攻击类型
    /// </summary>
    public eAtkType curAtkType = eAtkType.None;

    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;
    
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotSpeed;

    /// <summary>
    /// 当前状态的Key值
    /// </summary>
    public float curAimKey;
    /// <summary>
    /// 当前攻击的key值
    /// </summary>
    public float curAtkKey;
    /// <summary>
    /// 碰撞停留检测事件
    /// </summary>
    public UnityAction<GameObject> collisonStayEvent;
    /// <summary>
    /// 得到打击事件
    /// </summary>
    public UnityAction<HitTrigger> OnHitEvent;

    void Start()
    {
        mAim = gameObject.GetComponent<Animator>();
        mRigid = gameObject.transform.GetComponent<Rigidbody>();

        if (mAim != null)
        {
            foreach (var info in mAim.runtimeAnimatorController.animationClips)
            {
                mAimLenDict.Add(info.name, info.length);
            }
        }

        InitCol();
    }

    /// <summary>
    /// 初始化打击碰撞字典
    /// </summary>
    public void InitCol()
    {
       var mTriggers = gameObject.GetComponentsInChildren<HitTrigger>();

        foreach (var obj in mTriggers)
        {
            obj.mParent = this;
            obj.TriggerFuncEvent += OnColliderHit;
            if (!mTriggerDict.ContainsKey(obj.index))
            {
                mTriggerDict.Add(obj.index, obj);
            }
        }
    }

    /// <summary>
    /// 当物体进入被击
    /// </summary>
    public void OnColliderHit(HitTrigger rCol)
    {
        rCol.mParent.SetState(eState.Hit);

        rCol.SetColActive(false);

        if (OnHitEvent != null)
        {
            OnHitEvent(rCol);
        }
    }

    /// <summary>
    /// 设置触发器激活
    /// </summary>
    public void SetColActiveOrUn(string rIndex,bool rActive)
    {
        mTriggerDict[rIndex].SetColActive(rActive);
    }

    /// <summary>
    /// 设置触发器关闭
    /// </summary>
    public void SetAllColUnActive()
    {
        foreach (var info in mTriggerDict.Values)
        {
            info.SetColActive(false);
        }

       
    }



    void Update()
    {
        if (isPause)
        {
            return;
        }
        if (!isLoaclPlayer)
        {
            return;
        }

        if (Input.GetKey(KeyCode.J))
        {
            CheckActorAtk("J");
        }

        if (Input.GetKey(KeyCode.K))
        {
            CheckActorAtk("K");
        }


        if (Input.GetKey(KeyCode.L))
        {
            CheckActorAtk("L");
            // rCol.mParent.GetComponent<KillControl>().Play();
        }


        if (Input.GetKey(KeyCode.Space))
        {
            CheckActorMove("Space");
        }


        moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            CheckActorMove("W");
        }

        if (Input.GetKey(KeyCode.S))
        {
            CheckActorMove("S");
        }

        if (Input.GetKey(KeyCode.A))
        {
            CheckActorMove("A");
        }

        if (Input.GetKey(KeyCode.D))
        {
            CheckActorMove("D");
        }

 
        ActorMove();
    }


    public void CheckActorAtk(string rInput)
    {
        var inputAtkState = eAtkType.None;

        float ActiveTriggerTime = 0;
        List<string> ActiveTriggerIndex = new  List<string>();
        float SwichAtkTime = 0;
   

        if ((curState == eState.Idle) || (curState == eState.Atk && curAtkState == eAtkState.AtkWait))
        {
            switch (rInput)
            {
                case "J":
                    inputAtkState = eAtkType.Atk01;
                    ActiveTriggerTime = 0.15f;
                    ActiveTriggerIndex.Add("01");
                    SwichAtkTime = 0.3f;
                    break;
                case "K":
                    inputAtkState = eAtkType.Atk02;
                    ActiveTriggerTime = 0.2f;
                    ActiveTriggerIndex.Add("04");
                    SwichAtkTime = 0.5f;
                    break;
                case "L":
                    inputAtkState = eAtkType.Pick;
                    ActiveTriggerIndex.Add("02");
                    ActiveTriggerTime = 0.1f;
                    SwichAtkTime = 0.5f;
            
                    OnHitEvent += (o) =>
                    {
                        SetAllColUnActive();

                        var control = o.mParent.GetComponent<KillControl>();
                        isPause = true;

                        control.OnFinishEvent = () => 
                        {
                            isPause = false;
                        };

                        control.Play();
                    };
                    break;
            }
        }
        else if (curState == eState.Air && curAtkType == eAtkType.None)
        {
            switch (rInput)
            {
                case "J":
                    inputAtkState = eAtkType.Air_Atk01;
                    ActiveTriggerTime = 0.1f;
                    ActiveTriggerIndex.Add("01");
                    SwichAtkTime = 0.25f;
                    break;
                case "K":
                    inputAtkState = eAtkType.Air_Atk02;
                    ActiveTriggerTime = 0.2f;
                    ActiveTriggerIndex.Add("03");
                    ActiveTriggerIndex.Add("04");
                    SwichAtkTime = 0.7f;
                  
                    break;
            }
        }

        if (inputAtkState != eAtkType.None&& inputAtkState!= curAtkType)
        {
            SetAtkState(inputAtkState, ActiveTriggerTime, ActiveTriggerIndex, SwichAtkTime, 0.25f);
        }
    }

    public void CheckActorMove(string rInput)
    {
        switch (rInput)
        {
            case "S":
                moveDir += -Vector3.forward;
                break;
            case "W":
                moveDir += Vector3.forward;
                break;
            case "A":
                moveDir += -Vector3.right;
                break;
            case "D":
                moveDir += Vector3.right;
                break;
            case "Space":
                mRigid.AddForce(new Vector3(0, 1, 1) * 200);
                SetState(eState.Air);
                break;
        }
    }


    public void ActorMove()
    {

        if (moveDir != Vector3.zero)
        {
            if (curState == eState.Idle)
            {
                mAim.Play("Run");
                mAim.SetBool("isRun", true);
                gameObject.transform.position += moveDir.normalized * Time.deltaTime * moveSpeed;
                gameObject.transform.rotation = Quaternion.LookRotation(moveDir);
            }

            
        }
        else
        {
            mAim.SetBool("isRun", false);
        }

        if (curState == eState.Idle)
        {
            moveDir = Vector3.zero;
        }
    }




    public void CheckIsGround(GameObject rObj)
    {
        if (curState == eState.Air)
        {
            if (rObj.tag == "Ground")
            {
                mAim.SetBool("isAir", false);
                curState = eState.Idle;
                 
                collisonStayEvent -= CheckIsGround;
            }
        }
    }

    public void SetAtkState(eAtkType rType,float rActiveTriggerTime,List<string> rActiveTriggerIndex,float rSwichAtkTime,float crossTime)
    {
 
        SetAllColUnActive();
        SetState(eState.Atk);


        curAtkKey = Time.time;
        float tempKey = curAtkKey;

        float aimTime = PlayAim(rType.ToString(), crossTime);

        Util.DelayCall(rActiveTriggerTime, () =>
        {
            if (tempKey != curAtkKey)
            {
                return;
            }
            for (int i = 0; i < rActiveTriggerIndex.Count; i++)
            {
                SetColActiveOrUn(rActiveTriggerIndex[i], true);
            }
        });

        Util.DelayCall(rSwichAtkTime, () =>
        {
            if (tempKey != curAtkKey)
            {
                return;
            }

            OnHitEvent = null;
            SetAllColUnActive();
            curAtkState = eAtkState.AtkWait;
        });

        Util.DelayCall(aimTime, () =>
        {
            if (tempKey != curAtkKey)
            {
                return;
            }
          
            curAtkState = eAtkState.None;
            curAtkType = eAtkType.None;
        });

        curAtkType = rType;
    }


    public void SetState(eState rState)
    {
        switch (rState)
        {
            case eState.Idle:

                break;
            case eState.Hit:
                PlayAim("Hit");
                break;
            case eState.Atk:
                curAtkState = eAtkState.Atk;
                if (moveDir != Vector3.zero)
                {
                    gameObject.transform.rotation = Quaternion.LookRotation(moveDir);
                }
                break;
            case eState.Air:
                collisonStayEvent += CheckIsGround;
                PlayAim("Jump_Short");
                mAim.SetBool("isAir",true);
                break;
        }

        curState = rState;
    }

    public float PlayAim(string rName, float rCrossFade = 0)
    {
        if (rCrossFade == 0)
        {
            mAim.Play(rName, 0, 0);
        }
        else
        {
            mAim.CrossFade(rName, rCrossFade);
        }

        curAimKey = Time.time;
        float tempkey = curAimKey;

        Util.DelayCall(mAimLenDict[rName], () =>
         {
             if (tempkey != curAimKey)
             {
                 return;
             }

             if (curState != eState.Air)
             {
                 curState = eState.Idle;
             }
         });

        return mAimLenDict[rName];
    }



    public void OnTriggerEnter(Collider other)
    {
        if (collisonStayEvent != null)
        {
            collisonStayEvent(other.gameObject);
        }
    }
}