using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiz1Script : Enemies
{
    private float meleeRange = 4f;
    private float spellRange = 10f;

    private enum AttackType { meleeAttack,rangedAttack1,rangedAttack2 }
    private AttackType currentAttack;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //Initializing Enemy Attack Data
        attackFrequency = 3f;
        lastAttack = 0f;
        damage = 40;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //switch(currentState)
        //{
        //    case EnemyStates.Attacking:

        //        lastAttack += Time.deltaTime;
        //        if (lastAttack > attackFrequency)
        //        {
        //            Attack();
        //        }

        //        break;
        //}
    }

    protected override void Attack()
    {
        if (!isAlive)
            return;

        switch (currentAttack)
        {
            case AttackType.meleeAttack:            //Fast melee attack at close range
                enemyAnim.SetTrigger("meleeAttack");
                Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

                if (hit != null)
                {
                    DamagePlayer(damage);
                }

                break;

            case AttackType.rangedAttack1:          //Single Spell at long range
                enemyAnim.SetTrigger("spellAttack");
                StartCoroutine(SpawnSpell(0.25f));
                break;

            case AttackType.rangedAttack2:          //Double Spell at medium range 
                enemyAnim.SetTrigger("spellAttack");
                StartCoroutine(SpawnSpell(0.25f));
                StartCoroutine(SpawnSpell(0.5f));

                break;
        }

        lastAttack = 0f;
    }

    IEnumerator SpawnSpell(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        GameObject spell = SpellPooler.Instance.GetSpell();
        spell.transform.position = attackPoint.position;
        spell.transform.rotation = Quaternion.identity;
        spell.SetActive(true);
        spell.GetComponent<SpellScript>().isFacingRight = isFacingRight;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            //Check Flip
            Flip(collision.transform.position);

            float distance = Vector2.Distance(transform.position, collision.transform.position);

            if (distance > spellRange)
                currentAttack = AttackType.rangedAttack1;
            else if (distance < spellRange && distance > meleeRange)
                currentAttack = AttackType.rangedAttack2;
            else if (distance < meleeRange)
                currentAttack = AttackType.meleeAttack;


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
