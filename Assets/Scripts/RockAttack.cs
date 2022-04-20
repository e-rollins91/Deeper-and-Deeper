using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RockAttack : MonoBehaviour
{
    public float speed;
    public float damage;
    public Vector3 target;
    private Vector3 targetDir;

    [SerializeField] GameObject rockExplosionParticle;

    public int chargeLevel; // 0 min, 1 max

    bool deathable;

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(DelayInit());
        targetDir = (target - transform.position).normalized;
    }
    void Update()
    {
        Move(speed, target);
    }
    private IEnumerator DelayInit()
    {
        yield return new WaitForSeconds(.2f);
        if (chargeLevel == 1)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        }
        yield return new WaitForSeconds(.35f);
        if (chargeLevel == 1)
        {
            yield return new WaitForSeconds(.25f);
            StartCoroutine(Fizzle());

        }
        else
        {
            StartCoroutine(Fizzle());

        }

    }

    private IEnumerator Fizzle()
    {
        yield return new WaitForSeconds(.2f);
        transform.localScale = new Vector3(transform.localScale.x - .085f, transform.localScale.y - .085f, transform.localScale.z - .085f);
        if(transform.localScale.x <= .4f)
        {
           Destroy(gameObject);
        }
        StartCoroutine(Fizzle());
    }
    private void Move(float speed, Vector3 target)
    {
        //   transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position = transform.position + (targetDir * (speed * Time.deltaTime));

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyControllerAi>().ReceiveDamage(damage);
            collision.gameObject.GetComponent<EnemyControllerAi>().ReceiveKnockback(.25f, 75f, 2, transform.position);
            GameObject.Instantiate(rockExplosionParticle, gameObject.transform.position, Quaternion.identity);
            if (chargeLevel == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    public void SetStats(float launchSpeedRec, float damageRec, Vector3 targetRec)
    {
        speed = launchSpeedRec;
        damage = damageRec;
        target = targetRec;
    }

}
