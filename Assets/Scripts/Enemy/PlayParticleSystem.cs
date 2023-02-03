using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void PlayParticles()
    {
        particleSystem.Play();
    }
}
