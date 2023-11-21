using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : UnitBase
{
    [SerializeField, Range(0, 1000)] private float hp;
    [Range(0, 1000)] public float damage;
    private NavMeshAgent agent;
    private Animator anim;
    public Human target;
    private BoxCollider box;
    public List<Human> targets = new List<Human>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        box = GetComponentInChildren<BoxCollider>();
    }

    private void Start()
    {
        //for (int i = 0; i <= target. )
    }
    private void Update()
    {
        if (hp <= 0)
            Death();
        else
            Move();
    }
    public virtual void DistanceAttack()
    {
        anim.SetBool("Attack", true);
    }

    public override void Attack()
    {
        DistanceAttack();
    }

    public override void Death()
    {
        anim.SetBool("Death", true);
        agent.isStopped = true;
    }

    public override void Move()
    {
        target = FindObjectOfType<Human>();
        if (!target)
            Idle();
        else
        {
            transform.LookAt(target.transform.position);
            agent.SetDestination(target.transform.position);
            anim.SetBool("Idle", false);
        }
    }

    public void Idle()
    {
        anim.SetBool("Idle", true);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Attack", false);
        }
    }

    public void OnWeapon()
    {
        box.enabled = true;
    }

    public void OffWeapon()
    {
        box.enabled = false;
    }

}
