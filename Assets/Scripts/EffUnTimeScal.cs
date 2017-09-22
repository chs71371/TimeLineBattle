using UnityEngine;
using System.Collections;

public class EffUnTimeScal : MonoBehaviour
{
    ParticleSystem emitter;
    void Start()
    {
        emitter = gameObject.GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        emitter.Simulate(Time.unscaledDeltaTime, true, false);
    }
}