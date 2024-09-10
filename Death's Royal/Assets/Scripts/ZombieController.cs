using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class ZombieController : BulletTarget
{
    public float SpeedChangeRate = 10.0f;
    public float CheckRange = 20.0f;
    public float AttackRange = 1.0f;
    public float MoveSpeed = 2.0f;
    private Transform player;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Collider col;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private float animationBlend;
    private int animIDSpeed;
    private int animIDAttack;   

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        AssignAnimationIDs();

        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            col.enabled = false;
            isDead = true;
            animator.SetTrigger("Dead");
        }

        if (!isDead)
            ChasePlayer();
        else
            StartCoroutine(DeadDelete());

        GetUpgradePoint();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDAttack = Animator.StringToHash("isAttack");
    }

    private void ChasePlayer()
    {
        Vector3 MoveDir = player.position - transform.position;
        float dist = Vector3.Magnitude(transform.position - player.position);

        if(dist > AttackRange) {
            animator.SetBool(animIDAttack, false);
            float targetSpeed = dist < CheckRange ? MoveSpeed : 0.0f;
            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            animator.SetFloat(animIDSpeed, animationBlend);
            navMeshAgent.SetDestination(player.position);
        }
        else {
            animator.SetBool(animIDAttack, true);
        }
    }

    private void OnEnable()
    {
        Health = MaxHealth;
        isDead = false;
        col.enabled = true;
    }

    IEnumerator Attack()
    {   
        yield return new WaitForSeconds(AttackRange);
    }

    IEnumerator DeadDelete()
    {
        if (isDead == true && isDead != preisDead)
        {
            SoundManager.Instance.PlaySound3D("zombie_die", gameObject.transform);
        }
        yield return new WaitForSeconds(3.2f);
        SpawnManager.Instance.insertQueue(gameObject);
        OnEnable();
    }

    //Animation Event Function
    private void Start_Zombie_Attack()
    {
        SoundManager.Instance.PlaySound3D("zombie_attack", gameObject.transform);
        attackCollider.enabled = true;
    }

    //Animation Event Function
    private void End_Zombie_Attack()
    {
        attackCollider.enabled = false;
    }
}
