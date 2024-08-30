using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    [SerializeField] protected float Health;
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected int plusUpgradePoint;
    protected bool isDead = false;
    protected bool preisDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetUpgradePoint()
    {
        if (isDead == true && isDead != preisDead)
        {
            preisDead = !preisDead;
            UpgradeManager.Instance.upgradePoint += plusUpgradePoint;
        }
        else if(isDead == false && isDead != preisDead)
        {
            preisDead = !preisDead;
        }
    }

    public void Hit(float damage)
    {
        Health -= damage;
    }
}
