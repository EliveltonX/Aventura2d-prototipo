using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    private AudioSource myAudioSource;
    public ParticleSystem _partSystem;
    

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        
    }


    public void PlaySound(AudioClip WhatSound)
    {
        myAudioSource.PlayOneShot(WhatSound);
        
    }

    public void EmiteParticles()
    {
       
        _partSystem.Emit(1);
    }

    public void PlaySoundAndParticles() { }
}
