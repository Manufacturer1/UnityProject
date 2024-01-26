using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Acest script Unity, numit MusicPlayer, este conceput pentru a gestiona redarea muzicii, inclusiv a unei părți de introducere (introSource) 
 * și a unei părți de buclă (loopSource)
 */

public class MusicPlayer : MonoBehaviour
{
    // Referință la sursa audio pentru introducerea muzicii
    public AudioSource introSource;

    // Referință la sursa audio pentru bucla muzicii
    public AudioSource loopSource;

    // Se execută la începutul jocului
    void Start()
    {
        // Redă introducerea muzicii imediat la începutul jocului
        introSource.Play();

        // Programează redarea buclei muzicii pentru a începe după sfârșitul introducerii
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }
}
