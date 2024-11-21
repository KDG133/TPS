using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    public int spawnCount;

    [SerializeField] private GameObject[] effects;
    private Dictionary<string, Queue<GameObject>> effectDictionary;
    // Start is called before the first frame update
    public void Init()
    {
        effects = Resources.LoadAll<GameObject>("Effect");

        effectDictionary = new Dictionary<string, Queue<GameObject>>();

        for (int i = 0; i < effects.Length; i++)
        {
            AddQueue(effects[i].name);
            for (int j = 0; j < spawnCount; ++j)
            {               
                GameObject Effect = Object.Instantiate(effects[i]);
                insertValue(effects[i].name, Effect);               
            }
        }
    }

    private void AddQueue(string name)
    {
        if (!effectDictionary.ContainsKey(name))
            effectDictionary[name] = new Queue<GameObject>();
        else
            Debug.Log(name + "is already exist");
    }

    public void insertValue(string name, GameObject effect)
    {
        if (effectDictionary.ContainsKey(name))
        {
            effectDictionary[name].Enqueue(effect);
            effect.SetActive(false);
        }
        else
            Debug.Log(name + "doesn't exist");
    }

    public GameObject GetValue(string name)
    {
        if(effectDictionary.ContainsKey(name) && effectDictionary[name].Count > 0)
        {
            GameObject Effect = effectDictionary[name].Dequeue();
            Effect.SetActive(true);
            return Effect;
        }
        else if(effectDictionary.ContainsKey(name) && effectDictionary[name].Count <= 0)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].name == name)
                {
                    GameObject newEffect = Object.Instantiate(effects[i]);
                    insertValue(effects[i].name, newEffect);
                }
            }

            GameObject Effect = effectDictionary[name].Dequeue();
            Effect.SetActive(true);
            return Effect;
        }
        else
            return null;
    }
}
