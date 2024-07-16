using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class ZombieController : MonoBehaviour
{
    public float SpeedChangeRate = 10.0f;
    public float CheckRange = 10.0f;
    public float AttackRange = 1.0f;
    public float MoveSpeed = 2.0f;
    [SerializeField] private Transform player;
    [SerializeField] private Collider attackCollider;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private float animationBlend;
    private int animIDSpeed;
    private int animIDAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        AssignAnimationIDs();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
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
            //transform.forward = Vector3.Lerp(transform.forward, MoveDir, Time.deltaTime * 2.0f);
            navMeshAgent.SetDestination(player.position);
        }
        else {
            animator.SetBool(animIDAttack, true);
        }
    }

    IEnumerator Attack()
    {   
        yield return new WaitForSeconds(AttackRange);
    }

    //Animation Event Function
    private void Start_Zombie_Attack()
    {
        attackCollider.enabled = true;
        //Debug.Log("StartAttack");
    }

    //Animation Event Function
    private void End_Zombie_Attack()
    {
        attackCollider.enabled = false;
        //Debug.Log("EndAttack");
    }
}
