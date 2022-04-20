using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyControllerAi : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform target;
    public float healthCurrent;
    public float healthMax;
    public float attackDamage;
    

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void InitializeMonster()
    {
        StartCoroutine(SetDest());
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if(target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            InitializeMonster();
        }
        if(target != null)
        {
            if (target.transform.position.x < gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    private IEnumerator SetDest()
    {
        yield return new WaitForSeconds(.25f);
        agent.SetDestination(target.position);
        StartCoroutine(SetDest());
    }
    
    public void ReceiveKnockback(float length,float power, float times, Vector3 hitPos)
    {
        agent.isStopped = true; 
        StartCoroutine(Knockedback(length,power,times,hitPos));
    }
    private IEnumerator Knockedback(float length, float power,float times, Vector3 hitPos)
    { 
        Vector3 dir = hitPos - transform.position;
        dir = - dir.normalized;
        GetComponent<Rigidbody2D>().AddForce(dir * power);
        yield return new WaitForSeconds(length);
        if (times > 1)
        {
            times--;
            StartCoroutine(Knockedback(length, power, times, hitPos));
        }
        else
        {
            if(healthCurrent != 0)
            {
                agent.isStopped = false;
                StartCoroutine(SetDest());
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

    }
    public void Death()
    {
       Destroy(gameObject);
    }
    public void ReceiveDamage(float damage)
    {
        animator.Play("monsterHurt");
        healthCurrent = healthCurrent - damage;
        if (healthCurrent <= 0)
        {
            agent.isStopped = true;
            agent.speed = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            animator.Play("monsterDeath");
        }
    }
}
