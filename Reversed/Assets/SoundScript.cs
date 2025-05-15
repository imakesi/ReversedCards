using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {
    private AudioSource audioSource;
    private AudioClip clickButton;
    private AudioClip reversed;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        clickButton = Resources.Load<AudioClip>("click_button.wav");
        reversed = Resources.Load<AudioClip>("reversed.wav");
    }

    public void PlayReversed() {
        audioSource.clip = reversed;
        audioSource.Play();
    }

    public void PlayClick() {
        audioSource.clip = clickButton;
        audioSource.Play();
    }
}
