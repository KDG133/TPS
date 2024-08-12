using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    [SerializeField] protected float Health;
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        Health -= 1;
    }
}
