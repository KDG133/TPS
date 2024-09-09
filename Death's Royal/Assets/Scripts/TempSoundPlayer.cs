using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSoundPlayer : MonoBehaviour
{
    private SoundType soundType;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        if (!audioSource.isPlaying)
            SoundManager.Instance.insertValue(gameObject.name, gameObject);
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
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
