using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : UnitBase
{
    [SerializeField, Range(0, 100000)]  private float hp;
    [Range(0, 1000000)]                 public float damage;
    [HideInInspector]                   public  Monster monster;
                                        private NavMeshAgent agent;
                                        private BoxCollider box;
                                        private Animator anim;
                                        public  Monster[] monsters;
                                        public  List<Monster> monsterTarget = new List<Monster>();
    

    private void Awake()
    {
        monster = FindObjectOfType<Monster>();
        box     = GetComponentInChildren<BoxCollider>();
        agent   = GetComponent<NavMeshAgent>();
        anim    = GetComponentInChildren<Animator>();
    }   
        
    private void Start()    
    {
        box.enabled = false;
        monsters    = FindObjectsOfType<Monster>();
        for (int i = 0; i < monsters.Length; i++)
        {
            monsterTarget.Add(monster);
            monsterTarget[i] = monsters[i];
        }
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
        monster.targets.Remove(monster.targets[0]);
        Destroy(gameObject);
        
    }

    public override void Move()
    {
        if (monster != null)
        {
            agent.SetDestination(monster.transform.position);
            transform.LookAt(monster.transform.position);
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
          Monster monster = other.gameObject.GetComponentInParent<Monster>();
          hp -= monster.damage;
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