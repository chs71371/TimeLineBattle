using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableTime : BasicPlayableBehaviour
{

    /// <summary>
    /// 曲线
    /// </summary>
    public AnimationCurve mCurve;
    /// <summary>
    /// 当前时间进度
    /// </summary>
    private float curTime;
    /// <summary>
    /// 最大时间
    /// </summary>
    private float maxTime;

 
	public override void OnGraphStart(Playable playable) {
		
	}

 
	public override void OnGraphStop(Playable playable) {
        Time.timeScale = 1;

    }

 
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        maxTime= (float)PlayableExtensions.GetDuration(playable);
    }

 
	public override void OnBehaviourPause(Playable playable, FrameData info) {
      
    }

 
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        curTime += info.deltaTime;
        //曲线值
        Time.timeScale = mCurve.Evaluate(curTime/ maxTime);
    }
}
