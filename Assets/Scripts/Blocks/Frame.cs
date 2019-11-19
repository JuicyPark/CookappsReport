using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blocks;

public class Frame : MonoBehaviour
{
    public Block block;
    [SerializeField] ParticleSystem particleSystem;

    public void ParticleBoom()
    {
        particleSystem.Play();
    }
}
