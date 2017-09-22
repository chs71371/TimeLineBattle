using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using DG.Tweening;

public class TimeLineManager : MonoBehaviour
{

    public PlayableDirector mDirector;


    // Use this for initialization
    void Start()
    {


    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            mDirector.Play();
        }
    }
}
