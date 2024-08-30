using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    [SerializeField] private GameObject bullets;
    private Queue<GameObject> bulletQue;
    public int spawnCount;
    // Start is called before the first frame update
    void Start()
    {
        bulletQue = new Queue<GameObject>();
        for (int i = 0; i < spawnCount; i++)
            AddBullet();
    }

    public void AddBullet()
    {
        GameObject Bullet = Instantiate(bullets, gameObject.transform);
        insertQueue(Bullet);
    }

    public void insertQueue(GameObject b_object)
    {
        bulletQue.Enqueue(b_object);
        b_object.SetActive(false);
    }

    public GameObject GetQueue()
    {
        if(bulletQue.Count <= 0)
            AddBullet();

        GameObject b_object = bulletQue.Dequeue();
        b_object.SetActive(true);
        return b_object;
    }
}
