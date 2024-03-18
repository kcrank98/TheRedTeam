using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRoomMusic : MonoBehaviour
{
    [Header("-----Audio-----")]
    public AudioSource aud;
    [SerializeField] AudioClip[] bossMusic;
    [Range(0, 1)][SerializeField] float bossMusicVol;


    private void Start()
    {
        aud = gameManager.instance.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aud.Stop();
            aud.PlayOneShot(bossMusic[Random.Range(0, bossMusic.Length)], bossMusicVol);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aud.Stop();
            aud.PlayOneShot(gameManager.instance.backgroundMusic[Random.Range(0, gameManager.instance.backgroundMusic.Length)], gameManager.instance.backgroundMusicVol);
        }
    }
}
