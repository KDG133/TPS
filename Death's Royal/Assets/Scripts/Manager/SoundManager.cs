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

public class SoundManager
{
    public int spawnCount;

    [SerializeField] private AudioClip[] _loadClips;
    private Dictionary<string, AudioClip> _clipsDictionary;
    private Dictionary<string, Queue<GameObject>> _soundDictionary;
    
    public void Init()
    {
        _loadClips = Resources.LoadAll<AudioClip>("Audio");

        _clipsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in _loadClips)
        {
            _clipsDictionary.Add(clip.name, clip);
        }

        _soundDictionary = new Dictionary<string, Queue<GameObject>>();
        for (int i = 2; i < 4; i++)
        {
            string typeName = "TempSoundPlayer_" + i.ToString() + "D";
            AddQueue(typeName);
            for (int j = 0; j < spawnCount; ++j)
                AddValue(typeName);
        }

        PlaySound2D("bgm", true, SoundType.BGM);
    }

    private void AddQueue(string name)
    {
        if (!_soundDictionary.ContainsKey(name))
            _soundDictionary[name] = new Queue<GameObject>();
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

        //soundObj.transform.parent = gameObject.transform;
        insertValue(name, soundObj);
    }

    public void insertValue(string name, GameObject soundObj)
    {
        if (_soundDictionary.ContainsKey(name))
        {
            _soundDictionary[name].Enqueue(soundObj);
            soundObj.SetActive(false);
        }
        else
            Debug.Log(name + "doesn't exist");
    }

    public void PlaySound2D(string clipname, bool isLoop = false, SoundType soundType = SoundType.EFFECT)
    {
        if(_soundDictionary["TempSoundPlayer_2D"].Count > 0)
        {
            GameObject soundObj = _soundDictionary["TempSoundPlayer_2D"].Dequeue();
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname), isLoop);
        }
        else
        {
            AddValue("TempSoundPlayer_2D");
            GameObject soundObj = _soundDictionary["TempSoundPlayer_2D"].Dequeue();
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname), isLoop);
        }       
    }

    public void PlaySound3D(string clipname, Transform audioTarget, bool isLoop = false, SoundType soundType = SoundType.EFFECT)
    {
        if (_soundDictionary["TempSoundPlayer_3D"].Count > 0)
        {
            GameObject soundObj = _soundDictionary["TempSoundPlayer_3D"].Dequeue();
            soundObj.transform.position = audioTarget.position;
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname), isLoop);
        }
        else
        {
            AddValue("TempSoundPlayer_3D");
            GameObject soundObj = _soundDictionary["TempSoundPlayer_3D"].Dequeue();
            soundObj.transform.position = audioTarget.position;
            TempSoundPlayer soundPlayer = soundObj.GetComponent<TempSoundPlayer>();
            soundObj.SetActive(true);
            soundPlayer.Play(GetClip(clipname), isLoop);
        }
    }

    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = _clipsDictionary[clipName];

        if (clip == null)
            Debug.LogError(clipName + "Doesn't exist");

        return clip;
    }
}
