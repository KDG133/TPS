using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    public GameObject Zombie;
    public int spawnCount;
    public int spawnPointCnt = 3;
    [SerializeField] private Transform[] Spawnpoints;
    private Queue<GameObject> monsterQue;
    // Start is called before the first frame update
    public void Init()
    {
        Zombie = Resources.Load<GameObject>("Normal_Zombie");
        Spawnpoints = new Transform[spawnPointCnt];

        for (int i = 0; i < spawnPointCnt; ++i)
            Spawnpoints[i] = GameObject.Find("SpawnPoint" + i.ToString()).GetComponent<Transform>();

        monsterQue = new Queue<GameObject>();
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject zombie = Object.Instantiate(Zombie);
            insertQueue(zombie);
        }

        //CoroutineHelper.StartCoroutine(MonsterSpawn());
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
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void Clear()
    {

    }
}
