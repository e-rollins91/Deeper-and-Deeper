using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterDeath : MonoBehaviour
{
    private void Start()
    {
        Death();
    }
    public void Death()
    {
        gameObject.transform.parent.gameObject.GetComponent<EnemyControllerAi>().Death();
    }
}
