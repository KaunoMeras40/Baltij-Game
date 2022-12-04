using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] footsteps;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        int rand = Random.Range(0, footsteps.Length);
        AudioClip footstep = footsteps[rand];
        audioSource.PlayOneShot(footstep);
        Debug.Log("PLAY ONCE");
    }
}
