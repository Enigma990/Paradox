using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnim = null;

    //Player Attack Data
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private float attackRange = 0.3f;
    [SerializeField] private LayerMask enemyLayers;
    private const float SWORDDAMAGE = 5;
    private float damageMultiplier = 0.5f;
    private float currentDamage;
    private float flashStepDistance = 10f;
    private float flashStepCost = 30f;
    private float flashStepDamage = 25f;

    //Attack Combo
    private int attackNum = 0;
    private float fireRate = 1f;
    private float resetTimer = 0f;

    //Attack FX
    private TrailRenderer flashStepTrailFx = null;

    // Start is called before the first frame update
    void Start()
    {
        //Getting Animator
        playerAnim = GetComponent<Animator>();

        //Getting Trail Renederer
        flashStepTrailFx = GetComponent<TrailRenderer>();
        flashStepTrailFx.enabled = false;

        //initiating deafault damage
        currentDamage = SWORDDAMAGE;
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------Basic Attack---------------------------------
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (attackNum == 0)                                     //Attack 1
            {
                playerAnim.SetTrigger("Attack1");
                attackNum++;
            }
            else if (attackNum == 1)                                //Attack 2
            {
                playerAnim.SetTrigger("Attack2");                                   
                currentDamage += SWORDDAMAGE * damageMultiplier;
                attackNum++;
                resetTimer--;
            }
            else if (attackNum == 2)                                //Attack 3
            {
                playerAnim.SetTrigger("Attack3");
                currentDamage += SWORDDAMAGE * damageMultiplier;
                attackNum++;
                resetTimer++;
            }
            else                                                    // No Spam
            {
                return;   
            }

            Attack();
        }
 
        if (attackNum != 0)                                         //Reset Combo
        {
            resetTimer += Time.deltaTime;
            if(resetTimer > fireRate)
            {
                playerAnim.ResetTrigger("Attack1");
                playerAnim.ResetTrigger("Attack2");
                playerAnim.ResetTrigger("Attack3");

                currentDamage = SWORDDAMAGE;
                resetTimer = 0;
                attackNum = 0;
            }
        }

        //--------------------------------Flash Step--------------------------------
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.Instance.CurrentStamina > flashStepCost)
        {
            playerAnim.SetTrigger("FlashStep");
            FlashStep();
        }

        //-------------------------------Rage Mode---------------------------------
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (!PlayerController.Instance.inRageMode && PlayerController.Instance.CurrentStamina > 50)
            {
                PlayerController.Instance.inRageMode = true;
                PostProcecssingManager.Instance.ActivateRageMode();
            }

            else if (PlayerController.Instance.inRageMode)
            {
                PlayerController.Instance.inRageMode = false;
                PostProcecssingManager.Instance.DeactivateRageMode();
            }   
        }
    }
    void Attack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInParent<Enemies>().HurtEnemy(currentDamage);
        }
    }

    void FlashStep()
    {
        GameManager.Instance.OnUpdateStamina(-30);

        //Start Trail FX
        StartCoroutine(EnableTrailFX());

        //Calculating direction according to player's face
        int direction = PlayerController.Instance.IsFacingRight ? 1 : -1;

        //Calculating all raycast hits
        RaycastHit2D[] totalHits = Physics2D.RaycastAll(new Vector2(this.transform.position.x + direction, this.transform.position.y + 0.5f), attackPoint.right, flashStepDistance);

        //Checking each raycast hits
        foreach(RaycastHit2D hit in totalHits)
        {
            if (hit.collider.CompareTag("Enemy")) 
            {
                hit.transform.GetComponent<Enemies>().HurtEnemy(flashStepDamage);
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                transform.position = new Vector2(hit.point.x, transform.position.y);
                return;
            }
        }
        transform.position = new Vector2(transform.position.x + flashStepDistance * direction, transform.position.y);
    }

    IEnumerator EnableTrailFX()
    {
        flashStepTrailFx.enabled = true;

        yield return new WaitForSeconds(0.5f);

        flashStepTrailFx.enabled = false;
    }
 /*
    void OnDrawGizmosSelected()         //Remove this later
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
*/
}