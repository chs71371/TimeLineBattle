using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine.Timeline;
using Cinemachine;

public class KillControl : MonoBehaviour
{
    public GameObject mKillInfo;
    [HideInInspector]
    public PlayableDirector mDirector;
    Dictionary<string, PlayableBinding> bindingDict = new Dictionary<string, PlayableBinding>();
    public CharaControl playerControl;
    private CharaControl mControl;
    public Vector3 offsetPos;
    public Vector3 offsetRot;
    public UnityAction OnFinishEvent;

 
    public void Start()
    {
        mControl = GetComponent<CharaControl>();

        mKillInfo = Instantiate(mKillInfo);

        mKillInfo.transform.SetParent(mControl.transform);

        mDirector = mKillInfo.GetComponent<PlayableDirector>();

        foreach (var at in mDirector.playableAsset.outputs)
        {
            if (!bindingDict.ContainsKey(at.streamName))
            {
                bindingDict.Add(at.streamName, at);
            }
        }
    }

    public void Play()
    {
        mControl.transform.position = playerControl.transform.position + playerControl.transform.rotation * offsetPos;
        mControl.transform.rotation = Quaternion.LookRotation(playerControl.transform.position - mControl.transform.position);
        mControl.transform.localEulerAngles += offsetRot;


        mKillInfo.gameObject.SetActive(true);
        mDirector.Play();

        mDirector.SetGenericBinding(bindingDict["Player"].sourceObject, playerControl.mAim);
        mDirector.SetGenericBinding(bindingDict["Enemy"].sourceObject, mControl.mAim);
        mDirector.SetGenericBinding(bindingDict["Cinemachine"].sourceObject, Camera.main.GetComponent<Cinemachine.CinemachineBrain>());
        var CinemachineTrack = bindingDict["Cinemachine"].sourceObject as Cinemachine.Timeline.CinemachineTrack;

        foreach (var info in CinemachineTrack.GetClips())
        {
            if (info.displayName == "CinemachineShot")
            {
                var cameraInfo = info.asset as Cinemachine.Timeline.CinemachineShot;
                var vcam1 = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCameraBase>();
                var setCam = new ExposedReference<CinemachineVirtualCameraBase>();
                setCam.defaultValue = vcam1;
                cameraInfo.VirtualCamera = setCam;
            }

            if (info.displayName == "CM vcam2")
            {
                var cameraInfo = info.asset as Cinemachine.Timeline.CinemachineShot;
                var vcam2 = cameraInfo.VirtualCamera.Resolve(mDirector.playableGraph.GetResolver());
                vcam2.LookAt = mControl.transform.Find("Tran_Chest").transform;
                vcam2.Follow = mControl.transform;
            }
        }
    }

    private void Update()
    {
        if (mDirector.gameObject.activeInHierarchy)
        {
            if (mDirector.state == PlayState.Paused)
            {
                mDirector.gameObject.SetActive(false);
                if (OnFinishEvent != null)
                {
                    OnFinishEvent();
                }
            }
        }
    }
}
