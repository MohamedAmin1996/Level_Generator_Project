using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSongGenerator : MonoBehaviour
{
    [SerializeField] AudioClip[] levelSongs;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        int rand = Random.Range(0, levelSongs.Length);
        audioSource.clip = levelSongs[rand];
        audioSource.Play();
    }
}
