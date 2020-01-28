using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource explosion;
    public AudioSource keyboard;

    public AudioClip[] keyPresses;
    public AudioClip[] musicLoops;
    public AudioClip[] explosions;

    public AudioClip bigExplosion;

    private int loopIndex = 0;
    private bool nextLoop = false;

    public void PlayMusic()
    {
        music.clip = musicLoops[loopIndex];
        music.Play();
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void SetIndex(int index)
    {
        if(index != loopIndex && index < musicLoops.Length && index > 0)
        {
            loopIndex = index;
            music.loop = false;
            nextLoop = true;
        }
    }

    public void PlayKeyboardClick()
    {
        keyboard.clip = keyPresses[Random.Range(0, keyPresses.Length - 1)];
        keyboard.pitch = Random.Range(0.9f, 1f);
        keyboard.Play();
    }

    public void PlayExplosion()
    {
        explosion.clip = explosions[Random.Range(0, explosions.Length - 1)];
        explosion.pitch = Random.Range(0.95f, 1f);
        explosion.Play();
    }

    public void PlayBigExplosion()
    {
        explosion.clip = bigExplosion;
        explosion.Play();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (nextLoop && !music.isPlaying)
        {
            nextLoop = false;
            music.loop = true;
            PlayMusic();
        }

        if (Input.anyKeyDown)
        {
            PlayKeyboardClick();
        }
    }
}
