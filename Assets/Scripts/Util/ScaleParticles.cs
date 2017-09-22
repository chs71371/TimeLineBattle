using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleParticles : MonoBehaviour
{
    public float ScaleSize = 1.0f;
    private List<float> initialSizes = new List<float>();
    private ParticleSystem[] particlesArr;

    void Awake()
    {
        particlesArr = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particlesArr)
        {
            initialSizes.Add(particle.startSize);
            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (renderer)
            {
                initialSizes.Add(renderer.lengthScale);
                initialSizes.Add(renderer.velocityScale);
            }
        }
    }

    public void Run()
    {
        float scale = ScaleSize;
        int arrayIndex = 0;
        foreach (ParticleSystem particle in particlesArr)
        {
            particle.startSize = initialSizes[arrayIndex++] * scale;
            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (renderer)
            {
                renderer.lengthScale = initialSizes[arrayIndex++] *
                    scale;
                renderer.velocityScale = initialSizes[arrayIndex++] *
                    scale;
            }
        }
    }

}