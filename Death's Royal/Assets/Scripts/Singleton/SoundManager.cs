using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    PLAYER,
    EFFECT,
}

public class SoundManager : Singleton<SoundManager>
{
    public int spawnCount;

    [SerializeField] private AudioClip[] loadClips;
    private Dictionary<string, AudioClip> clipsDictionary;
    private Dictionary<string, Queue<GameObject>> SoundDictionary;
    

    // Start is called before the first frame update
    void Start()
    {
        clipsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in loadClips)
        {
            clipsDictionary.Add(clip.name, clip);
        }

        SoundDictionary = new Dictionary<string, Queue<GameObject>>();
        for (int i = 2; i < 4; i++)
        {
            string typeName = "TempSoundPlayer_" + i.ToString() + "D";
            AddQueue(typeName);
            for (int j = 0; j < spawnCount; ++j)
                AddValue(typeName);
        }
    }

    private void AddQueue(string name)
    {
        if (!SoundDictionary.ContainsKey(name))
            SoundDictionary[name] = new Queue<GameObject>();
        else
            Debug.Log(name + "is already exist");
    }

    private void AddValue(string name)
    {
        GameObject soundObj = new GameObject(name);
        TempSoundPlayer soundPlayer = soundObj.AddComponent<TempSoundPlayer>();

        int type = int.Parse(name.Substring(name.Length - 2, 1));

        if (type % 2 == 0)
            soundPlayer.InitSound2D();
        else
            soundPlayer.InitSound3D();

        soundObj.transform.parent = gameObject.transform;
        insertValue(name, soundObj);
    }

    public void insertValue(string name, GameObject soundObj)
    {
        if (SoundDictionary.ContainsKey(name))
        {
            SoundDictionary[name].Enqueue(soundObj);
            soundObj.SetActive(false);
        }
        else
            Debug.Log(name + "doesn't exist");
    }

    public void PlaySound2D(string clipname, SoundType soundType = SoundType.EFFECT, bool isLoop = false)
    {
        if(SoundDictionary["TempSoundPlayer_2D"].Count > 0)
        {
            GameObject soundObj = SoundDictionary["TempSoundPlayer_2D"].Dequeue();
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname));
        }
        else
        {
            AddValue("TempSoundPlayer_2D");
            GameObject soundObj = SoundDictionary["TempSoundPlayer_2D"].Dequeue();
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname));
        }       
    }

    public void PlaySound3D(string clipname, Transform audioTarget, SoundType soundType = SoundType.EFFECT, bool isLoop = false)
    {
        if (SoundDictionary["TempSoundPlayer_3D"].Count > 0)
        {
            GameObject soundObj = SoundDictionary["TempSoundPlayer_3D"].Dequeue();
            soundObj.transform.position = audioTarget.position;
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname));
        }
        else
        {
            AddValue("TempSoundPlayer_3D");
            GameObject soundObj = SoundDictionary["TempSoundPlayer_3D"].Dequeue();
            soundObj.transform.position = audioTarget.position;
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname));
        }
    }

    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDictionary[clipName];

        if (clip == null)
            Debug.LogError(clipName + "Doesn't exist");

        return clip;
    }
}
