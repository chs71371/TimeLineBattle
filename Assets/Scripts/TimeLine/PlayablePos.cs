using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayablePos : BasicPlayableBehaviour
{

   
    public ExposedReference<Transform> mTran;
    public ExposedReference<Transform> relativeTarget;

    private Transform _mTran;
    private Transform _relativeTarget;

    public Vector3 offsetPos;

    public Vector3 offsetRot;


    public override void OnGraphStart(Playable playable)
    {
        _mTran = null;
        _relativeTarget = null;
    }


    public override void OnGraphStop(Playable playable)
    {
        _mTran = null;
        _relativeTarget = null;
    }


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _mTran= mTran.Resolve(playable.GetGraph().GetResolver());
        _relativeTarget= relativeTarget.Resolve(playable.GetGraph().GetResolver());

        if (_mTran != null && _relativeTarget != null)
        {
            _mTran.position = _relativeTarget.position + _relativeTarget.rotation * offsetPos;
            _mTran.rotation = Quaternion.LookRotation(_relativeTarget.position - _mTran.position);
            _mTran.transform.localEulerAngles += offsetRot;
        }
    }


    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        _mTran = null;
        _relativeTarget = null;
    }
 


    public override void PrepareFrame(Playable playable, FrameData info)
    {
       
    }

 
}
