using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject Zombie;
    public int spawnCount;
    [SerializeField] private Transform[] Spawnpoints;
    private Queue<GameObject> monsterQue;
    // Start is called before the first frame update
    void Start()
    {
        monsterQue = new Queue<GameObject>();
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject zombie = Instantiate(Zombie, gameObject.transform);
            insertQueue(zombie);
        }

        StartCoroutine(MonsterSpawn());
    }

    public void insertQueue(GameObject p_object)
    {
        monsterQue.Enqueue (p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue()
    {
        GameObject g_object = monsterQue.Dequeue();
        g_object.SetActive(true);

        return g_object;
    }

    IEnumerator MonsterSpawn()
    {
        while(true) 
        {
            if(monsterQue.Count != 0)
            {
                GameObject zombie = GetQueue();
                zombie.transform.position = Spawnpoints[Random.Range(0,3)].transform.position;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
