using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    protected Rigidbody2D enemyRb = null;
    protected Animator enemyAnim = null;

    //Enemy Data
    [SerializeField] protected int maxHealth = 10;
    protected float health;
    [SerializeField] protected float moveSpeed = 5f;
    protected bool isRunning = false;
    protected bool isFacingRight = true;
    protected bool isAlive = true;

    //Enemy Attack Data
    [SerializeField] protected Transform attackPoint = null;
    [SerializeField] protected LayerMask playerLayer;
    protected float attackRadius = 0.3f;
    protected float attackFrequency;
    protected float lastAttack;
    protected int damage;

    //Enemy States
    protected enum EnemyStates { Idle, Running, Attacking, Dead }
    protected EnemyStates currentState = EnemyStates.Idle;

    //Enemy Death FX
    private Material dissolveMaterial;
    private float dissolveAmount;
    //private ParticleSystem deathParticle = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //------------------------Initializing Player-----------------------
        //playerLayer = LayerMask.NameToLayer("Player");
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponentInChildren<Animator>();

        health = maxHealth;

        // Initializing FX
        dissolveMaterial = GetComponentInChildren<Renderer>().material;
        dissolveAmount = 1f;
        //deathParticle = GetComponent<ParticleSystem>();
        //deathParticle.Stop();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                if (isRunning)
                {
                    isRunning = false;
                    enemyAnim.SetBool("isRunning", isRunning);
                }
                break;

            case EnemyStates.Running:
                if (!isRunning)
                {
                    isRunning = true;
                    enemyAnim.SetBool("isRunning", isRunning);
                }
                break;

            case EnemyStates.Attacking:
                //Stop if Running
                if (isRunning)
                {
                    isRunning = false;
                    enemyAnim.SetBool("isRunning", isRunning);
                }

                //Attacking
                lastAttack += Time.deltaTime;
                if (lastAttack > attackFrequency)
                {
                    Attack();
                }

                break;

            case EnemyStates.Dead:
                if (isAlive)
                {
                    isAlive = false;
                    enemyAnim.SetTrigger("isDead");
                }

                dissolveAmount = dissolveAmount - Time.deltaTime * 0.5f;
                dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);

                //Death Particle
                //deathParticle.Play();

                if (dissolveAmount <= 0)

                {
                    this.gameObject.SetActive(false);
                }
                break;
        }
    }

    protected void Move(Vector2 _position)
    {
        //Flip Enemy
        Flip(_position);

        //Move Enemy
        transform.position = Vector2.MoveTowards(transform.position, _position, moveSpeed * Time.deltaTime);
    }

    protected void Flip(Vector2 _position)
    {
        if (_position.x > transform.position.x && !isFacingRight)
        {
            transform.rotation = Quaternion.identity;
            isFacingRight = true;
        }
        if (_position.x < transform.position.x && isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;
        }
    }

    protected abstract void Attack();

    protected void DamagePlayer(int _damage)
    {
        GameManager.Instance.OnUpdateHeatlh(-_damage);
    }

    public void HurtEnemy(float _damage)
    {
        if (!isAlive)
            return;

        health -= _damage;
        enemyAnim.SetTrigger("isHurt");

        if (health <= 0)
        {
            currentState = EnemyStates.Dead;
            //this.gameObject.SetActive(false);
        }
    }
}
