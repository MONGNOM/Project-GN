    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Human : UnitBase
{
    [SerializeField, Range(0, 100000)]  private float Maxhp;
    [Range(0, 1000000)]                 public  float damage;
    [HideInInspector]                   public  Monster monster;
    /*[HideInInspector]*/               public float curhp;
                                        private NavMeshAgent agent;
                                        private BoxCollider box;    
                                        public Animator anim;
                                        public Image hpbar;
    

    private void Awake()
    {
       
        box     = GetComponentInChildren<BoxCollider>();
        agent   = GetComponent<NavMeshAgent>();
        anim    = GetComponentInChildren<Animator>();
       

    }

    private void Start()    
    {
        monster = FindObjectOfType<Monster>();
        curhp = Maxhp;
        box.enabled = false;
        
    }
    private void Update()
    {
        hpbar.fillAmount = curhp / Maxhp;
        if (curhp <= 0)
            Death();    
        else
            Move();
    }
    public virtual void DistanceAttack()
    {
        anim.SetBool("Attack",true);
    }       
 
    public override void Attack()
    {
        DistanceAttack();   
    }

    public override void Death()
    {
        anim.SetBool("Death", true);
        agent.isStopped = true;
        WaveManager.instance.humans.Remove(WaveManager.instance.humans[0]);
        Destroy(gameObject);
        
    }

    public override void Move()
    {
        if (WaveManager.instance.roundMonster != null)
        {
            anim.SetBool("Idle", false);
            agent.SetDestination(WaveManager.instance.roundMonster.transform.position);
            transform.LookAt(WaveManager.instance.roundMonster.transform.position);
        }
        else
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Monster"))
        {
            Attack();
        }
        else if (other.CompareTag("MonsterWeapon"))
        {
            // Umm hp collider cap and Attackcol ?
             Monster monster = other.gameObject.GetComponentInParent<Monster>();
            curhp -= monster.damage;
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            anim.SetBool("Attack", false);
        }
    }

    public void OnPlayerWeapon()
    {
        box.enabled = true;
    }

    public void OffPlayerWeapon()
    {
        box.enabled = false;
    }
}
