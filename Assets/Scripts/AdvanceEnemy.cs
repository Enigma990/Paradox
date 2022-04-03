using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceEnemy : Enemies
{
    private enum AttackType { BasicAttack, AdvanceAttack}
    private AttackType currentAttack;

    private Vector2 playerPosition;
    private float basicAttackRange = 2.5f;

    private bool isSelected;
    private bool canTeleport;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //Initializing Enemy Attack Data
        attackFrequency = 2f;
        lastAttack = 1f;
        damage = 25;
        isSelected = false;
        canTeleport = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        //switch(currentAttack)
        //{
        //    case AttackType.BasicAttack:
        //        Debug.Log("KILL");
        //        if (Vector2.Distance(transform.position, playerPosition) > basicAttackRange)
        //        {
        //            //Change Enemy from idle to running 
        //            currentState = EnemyStates.Running;
        //            Move(playerPosition);
        //        }

        //        else
        //        {
        //            enemyAnim.SetTrigger("Attack1");
        //            lastAttack = 0;

        //            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

        //            if (hit != null)
        //            {
        //                DamagePlayer(damage);
        //            }
        //        }
        //        break;

        //    case AttackType.AdvanceAttack:
        //        Debug.Log("Teleport");

        //        break;
        //}

        switch(currentAttack)
        {
            case AttackType.BasicAttack:
                enemyAnim.SetTrigger("Attack1");
                break;
            case AttackType.AdvanceAttack:
                enemyAnim.SetTrigger("Attack2");
                break;
        }

        lastAttack = 0;

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

        if (hit != null)
        {
            DamagePlayer(damage);
        }

    }

    private void ChooseAttack()
    {
        if (Random.Range(0, 11) < 5)
        {
            currentAttack = AttackType.BasicAttack;
        }
        else
        {
            currentAttack = AttackType.AdvanceAttack;
        }

        isSelected = true;
        canTeleport = true;
        StartCoroutine(ResetSelectedAttack());
    }

    IEnumerator ResetSelectedAttack()
    {
        yield return new WaitForSeconds(2f);

        isSelected = false;
    }

    void Teleport(Vector2 _playerPosition)
    {
        lastAttack = attackFrequency;

        if (isFacingRight)
            transform.position = new Vector2(_playerPosition.x + 2f, _playerPosition.y);

        if (!isFacingRight)
            transform.position = new Vector2(_playerPosition.x - 2f, _playerPosition.y);


        Flip(_playerPosition);
        currentState = EnemyStates.Attacking;

        canTeleport = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    if (!isSelected)
        //        ChooseAttack();

        //    playerPosition = collision.transform.position;
        //}

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isSelected)
                ChooseAttack();

            switch(currentAttack)
            {
                case AttackType.BasicAttack:
                    //Check Distance for attackRange
                    if (Vector2.Distance(transform.position, collision.transform.position) <= basicAttackRange)
                    {
                        currentState = EnemyStates.Attacking;
                        
                        return;
                    }

                    //Change Enemy from idle to running 
                    currentState = EnemyStates.Running;
                    Move(collision.transform.position);
                    break;

                case AttackType.AdvanceAttack:
                    if(canTeleport)
                        Teleport(collision.transform.position);
                    break;
            }    
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
