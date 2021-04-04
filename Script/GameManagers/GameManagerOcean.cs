using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class GameManagerOcean : MonoBehaviour
{
    void Update()
    {
        if (SpawnManagerRacing.MusicFlagOcean)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            audio.Play(44100);
            SpawnManagerRacing.MusicFlagOcean = false;
        }
        
    }
}
