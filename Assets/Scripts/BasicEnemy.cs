using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemies
{
    private float attackRange = 2.5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //Initializing Enemy Attack Data
        attackFrequency = 2f;
        lastAttack = attackFrequency;
        damage = 20;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //switch (currentState)
        //{
        //    case EnemyStates.Attacking:
        //        //Stop if Running
        //        if(isRunning)
        //        {
        //            isRunning = false;
        //            enemyAnim.SetBool("isRunning", isRunning);
        //        }

        //        //Attacking
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
        enemyAnim.SetTrigger("Attack");
        lastAttack = 0;

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

        if(hit != null)
        {
            DamagePlayer(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            //Check Distance for attackRange
            if (Vector2.Distance(transform.position, collision.transform.position) <= attackRange)
            {
                currentState = EnemyStates.Attacking;

                return;
            }

            //Change Enemy from idle to running 
            currentState = EnemyStates.Running;
            Move(collision.transform.position);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            currentState = EnemyStates.Idle;
        }
    }
}
