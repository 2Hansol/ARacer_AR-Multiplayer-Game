using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class GameManagerDesert : MonoBehaviour
{
    void Update()
    {
        if (SpawnManager.MusicFlagDesert)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            audio.Play(44100);
            SpawnManager.MusicFlagDesert = false;
        }
        
    }
}
