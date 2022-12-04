using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;
[System.Serializable]
public class Music
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float pitch;
}

public class AudioManager : MonoBehaviour
{
    public Music[] musics;
    public Sounds[] sounds;
    private void Awake()
    {
        foreach (Music s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlayMusic(string name)
    {
        Music s = Array.Find(musics, sound => sound.name == name);
        s.source.Play();
    }
    public void StopMusic(string name)
    {
        Music s = Array.Find(musics, sound => sound.name == name);
        s.source.Stop();
    }

    public void PlaySound(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        AudioSource src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 0f;
        src.maxDistance = 40;
        src.clip = s.clip;
        src.volume = s.Volume;
        src.pitch = s.pitch;
        src.Play();
        Destroy(src, src.clip.length);
        Debug.Log(s.name);
    }

}
