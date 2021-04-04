using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class GameManagerCity: MonoBehaviour
{
    void Update()
    {
        if (SpawnManagerRacing.MusicFlagCity)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            audio.Play(44100);
            SpawnManagerRacing.MusicFlagCity = false;
        }
        
    }
}
