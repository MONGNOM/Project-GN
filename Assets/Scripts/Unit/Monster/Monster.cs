using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : UnitBase
{
    [SerializeField, Range(0, 1000000)] private float maxhp;
    [SerializeField]                    private float curhp;
    [Range(0, 1000)]                    public float damage;
    private NavMeshAgent agent;
    private Animator anim;
    private Human target;   
    private BoxCollider box;
    public Image hpbar;
    public Human[] humans; // GameManager --> stage clear --> humans[] clear; && Win or Lose
    public List<Human> targets = new List<Human>();
    // Round Monster Y or N = RoundTime --> BreakTime
    public Dictionary<string,Monster> roundMonster = new Dictionary<string,Monster>();

    private void Awake()
    {
        target  = FindObjectOfType<Human>();
        agent   = GetComponent<NavMeshAgent>();
        anim    = GetComponent<Animator>();
        box     = GetComponentInChildren<BoxCollider>();
    }

    private void Start()
    {
        curhp = maxhp;
        humans = GameObject.FindObjectsOfType<Human>();
        for (int i = 0; i < humans.Length; i++) 
        {
            targets.Add(target); 
            targets[i] = humans[i]; // targets --> human != null but not find?
        }
    }
    private void Update()
    {
        humans = GameObject.FindObjectsOfType<Human>();

        hpbar.fillAmount = curhp / maxhp;
        if (curhp <= 0)
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
        anim.SetBool("Attack", false);
        WaveManager.instance.DestroyUnit();
        Destroy(gameObject);
    }

    public override void Move()
    {
        if (humans.Length == 0)
            Idle();
        else
        {
            transform.LookAt(humans[0].transform.position);
            agent.SetDestination(humans[0].transform.position);
            anim.SetBool("Idle", false);
        }
    }

    public void Idle()
    {
        anim.SetBool("Idle", true);
        humans = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack();
        }
        else if (other.CompareTag("PlayerWeapon"))
        {
            Human human = other.gameObject.GetComponentInParent<Human>();
            curhp -= human.damage;
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
