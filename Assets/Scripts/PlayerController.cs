using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;


public class PlayerController : MonoBehaviour
{
    // Character Movement 
    [SerializeField] float moveSpeed;
    [SerializeField] Animator animatorCharacter;
    [SerializeField] Animator animatorWeapon;

    // Primary Attack
    [SerializeField] GameObject primaryAttackObj;
    [SerializeField] float primaryChargeLevel;
    [SerializeField] float primaryChargeMinLevel;
    [SerializeField] float primaryChargeMax;
    [SerializeField] float primaryDamage;
    [SerializeField] bool attackCharging;
    [SerializeField] GameObject chargeBar;
    [SerializeField] Image barFill;
    private Vector3 mouseLoc = new Vector3(0, 0, 0);

    // Inits
    PhotonView view;
    GameObject mainCamera;
    [SerializeField] GameObject overheadUI;
    [SerializeField] TextMeshProUGUI charNameText;
    [SerializeField] GameObject charSprite;
    [SerializeField] GameObject weaponSprite;






    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
        mainCamera = GameObject.Find("Main Camera");
        charNameText.text = PhotonNetwork.NickName;
        primaryChargeMinLevel = .4f;
    }
    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            // Move & Set Animation            
            Move(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            RunAnim(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //

            // Primary Attack
            if (Input.GetMouseButtonDown(0))
            {
                attackCharging = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if(primaryChargeLevel *.1f / primaryChargeMax > primaryChargeMinLevel)
                {

                 //   PhotonView.Get(this).RPC("Attack", RpcTarget.Others, primaryChargeLevel * .1f, primaryDamage, mouseLoc);

                    gameObject.GetComponent<PhotonView>().RPC("Attack",RpcTarget.All, primaryChargeLevel * .1f, primaryDamage, mouseLoc);
                  //  Attack(primaryChargeLevel * .1f, primaryDamage, mouseLoc);
                }
                else
                {
                     gameObject.GetComponent<PhotonView>().RPC("Attack", RpcTarget.All, 0, 0, mouseLoc);

                 //   PhotonView.Get(this).RPC("Attack", RpcTarget.Others, 0, 0, mouseLoc);

               //     Attack(0, 0, mouseLoc);
                }
                attackCharging = false;
            }

        }
    }
    private void FixedUpdate()
    {
        if (attackCharging == true)
        {
            AttackCharge(.5f);
        }
    }


    private void AttackCharge(float inceaseAmt)
    {
        chargeBar.SetActive(true);
        if (primaryChargeLevel*.1f < primaryChargeMax)
        {
            primaryChargeLevel = primaryChargeLevel + inceaseAmt;
        }
        if ((primaryChargeLevel * .1f / primaryChargeMax) > primaryChargeMinLevel)
        {
            barFill.color = Color.yellow;
        }
        if ((primaryChargeLevel * .1f / primaryChargeMax) < primaryChargeMinLevel)
        {
            barFill.color = Color.red;
        }
        if ((primaryChargeLevel * .1f >= primaryChargeMax - .005f))
        {
            barFill.color = Color.green;
        }
        barFill.fillAmount = (primaryChargeLevel*.1f) / primaryChargeMax;
    }
    private void RunAnim(float horizontal, float vertical)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            mouseLoc = hit.point;
        }
        if(mouseLoc.x < gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3 (-1,1,1);
            overheadUI.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3 (1, 1, 1);
            overheadUI.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) >= .2f)
        {
            animatorCharacter.Play("Run");
        }
        else
        {
            animatorCharacter.Play("Stand");
        }
    }
    private void Move(float horizontal, float vertical)
    {
        Vector3 tempVect = new Vector3(horizontal, vertical, 0);
        tempVect = tempVect.normalized * moveSpeed * Time.deltaTime;
        gameObject.transform.position += tempVect;
    }
    [PunRPC]
    void Attack(float launchSpeed, float damage, Vector3 target)
    {
        chargeBar.SetActive(false);
        if (launchSpeed > .5)
        {
            PhotonView spawnedAttack = PhotonNetwork.Instantiate(primaryAttackObj.name, transform.position, Quaternion.identity,0).GetComponent<PhotonView>();
            spawnedAttack.gameObject.GetComponent<RockAttack>().SetStats(launchSpeed, damage, target);
            if((primaryChargeLevel * .1f >= primaryChargeMax - .005f))
            {
                spawnedAttack.gameObject.GetComponent<RockAttack>().damage = damage * 2;
                spawnedAttack.gameObject.GetComponent<RockAttack>().chargeLevel = 1;
            }
            primaryChargeLevel = 0;
            animatorWeapon.Play("attack");
        }
        else
        {
            primaryChargeLevel = 0;
        }
    }
}
