using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TempSoundPlayer : MonoBehaviour
{
    private SoundType soundType;
    private AudioSource audioSource;
    private bool loop;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && !loop)
            Managers.Sound.insertValue(gameObject.name, gameObject);
    }

    public void InitSound2D()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
    }

    public void InitSound3D()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 30f;
    }

    public void Play(AudioClip clip, bool isLoop)
    {
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        loop = isLoop;
        audioSource.Play();
    }
}
