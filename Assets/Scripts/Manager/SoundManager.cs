using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioSource audioSource2;
        [SerializeField] AudioClip clear;
        [SerializeField] AudioClip failed;
        [SerializeField] AudioClip swipe;
        [SerializeField] AudioClip destroy;
        [SerializeField] AudioClip generate;

        public void Clear()
        {
            audioSource.clip = clear;
            audioSource.Play();
        }
        public void Failed()
        {
            audioSource.clip = failed;
            audioSource.Play();
        }
        public void Swipe()
        {
            audioSource.clip = swipe;
            audioSource.Play();
        }
        public void Destroy()
        {
            audioSource.clip = destroy;
            audioSource.Play();
        }
        public void Generate()=>
            audioSource2.Play();
    }
}
