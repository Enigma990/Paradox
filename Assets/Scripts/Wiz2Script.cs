using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiz2Script : Enemies
{
    private float knockBackRange = 3f;

    [SerializeField] private GameObject advEnemy = null;
    [SerializeField] private GameObject basicEnemy = null;

    enum AttackType { Knockback, Summon}
    AttackType currentAttack;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        attackFrequency = 5f;
        lastAttack = attackFrequency / 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        switch(currentAttack)
        {
            case AttackType.Knockback:
                StartCoroutine(KnockBack());
                break;

            case AttackType.Summon:
                StartCoroutine(SummonEnemy());
                break;
        }

        lastAttack = 0f;
    }

    private IEnumerator KnockBack()
    {
        enemyAnim.SetTrigger("Attack2");

        yield return new WaitForSeconds(0.3f);

        if (isFacingRight)
            StartCoroutine(PlayerController.Instance.KnockBackPlayer(1));

        if (!isFacingRight)
            StartCoroutine(PlayerController.Instance.KnockBackPlayer(-1));
    }

    private IEnumerator SummonEnemy()
    {
        enemyAnim.SetTrigger("Attack1");

        yield return new WaitForSeconds(0.3f);

        Instantiate(advEnemy, attackPoint.position,Quaternion.identity);
        Instantiate(basicEnemy, attackPoint.position, Quaternion.identity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        //Check Flip
        Flip(collision.transform.position);

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Vector2.Distance(transform.position, collision.transform.position) <= knockBackRange)
            {
                currentAttack = AttackType.Knockback;
            }
            else
            {
                currentAttack = AttackType.Summon;
            }

            currentState = EnemyStates.Attacking;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentState = EnemyStates.Idle;
        }
    }

}
