using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Singleton Instance
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    //Player Basic Data
    public const int MAXHEALTH = 100;
    private const int MAXSTAMINA = 100;
    private int staminaRegenRate = 10;
    private float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }
    public bool inRageMode;


    //Player Movement Data
    private Rigidbody2D playerRb = null;
    private float speed = 5f;
    private float moveX = 0;
    private float jumpForce = 350f;
    private bool isFacingRight = false;
    public bool IsFacingRight { get { return isFacingRight; } }
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isFalling = false;
    public bool isKnockback = false;
    private bool isDead = false;

    //Ground Check Variables
    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPos = null;
    private float groundCheckRadius = 0.3f;
    private bool isGrounded = false;


    //Player Animation
    private Animator playerAnim = null;

    //Player Death FX
    private Material dissolveMaterial;
    private float dissolveAmount;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

        //Initializing Player
        currentHealth = MAXHEALTH;
        currentStamina = MAXSTAMINA;
        inRageMode = false;
        transform.position = RespawnManager.Instance.lastCheckpoint;

        // Initializing FX
        dissolveMaterial = GetComponent<Renderer>().material;
        dissolveAmount = 1f;

        //------------------Adding Events------------------------
        // Stamina Update Event
        GameManager.Instance.UpdateStamina += ChangeInStamina;

        //Health Update Event
        GameManager.Instance.UpdateHealth += PlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //-------------Movement input-------------------------
        moveX = Input.GetAxisRaw("Horizontal");
        Flip();

        //-----------------Jump--------------------------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) 
        {
            playerRb.AddForce(Vector3.up * jumpForce);
            isJumping = true;
        }
        //Jump
        if (playerRb.velocity.y > 0.1f && !isJumping && !isKnockback)
        {
            isJumping = true;
            playerAnim.SetBool("isJumping", isJumping);
        }
        //Fall
        if (playerRb.velocity.y < 0.1f && !isKnockback)
        {
            isFalling = true;
            isJumping = false;
            playerAnim.SetBool("isFalling", isFalling);
            playerAnim.SetBool("isJumping", isJumping);
        }
        //On Ground
        if(isGrounded)
        {
            if(isJumping)
            {
                isJumping = false;
                playerAnim.SetBool("isJumping", isJumping);
                playerAnim.SetBool("isJumping", isJumping);
            }

            isFalling = false;
            playerAnim.SetBool("isFalling", isFalling);
        }

        //------------------------------Restoring Stamina----------------------
        if (currentStamina < MAXSTAMINA && !inRageMode)
        {
            GameManager.Instance.OnUpdateStamina(staminaRegenRate * Time.deltaTime);
        }

        if(inRageMode)
        {
            if (currentStamina <= 0)
            {
                inRageMode = false;
                PostProcecssingManager.Instance.DeactivateRageMode();
            }

            GameManager.Instance.OnUpdateStamina(-staminaRegenRate * 2f * Time.deltaTime);
        }

        if (isDead)
        {
            dissolveAmount = dissolveAmount - Time.deltaTime * 0.5f;
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            if (dissolveAmount <= 0)
            {
                MenuManager.Instance.ActivateDeathMenu();
                this.gameObject.SetActive(false);
            }
        }

    }

    private void FixedUpdate()
    {
        //-----------------GroundCheck------------------------
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayer);

        isRunning = moveX != 0 ? true : false;
        playerAnim.SetBool("isRunning", isRunning);
        if(!isKnockback)
            playerRb.velocity = new Vector2(moveX * speed, playerRb.velocity.y);

    }

    public IEnumerator KnockBackPlayer(int _direction)
    {
        isKnockback = true;

        playerRb.AddForce(Vector2.right * 15f * _direction + Vector2.up * 2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        isKnockback = false;
    }

    void Flip()
    {
        if (moveX < 0 && isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            isFacingRight = false;
        }
        if (moveX > 0 && !isFacingRight)
        {
            transform.rotation = Quaternion.identity;
            isFacingRight = true;
        }
    }

    void ChangeInStamina(float _amount)
    {
        currentStamina += _amount;
    }

    public void PlayerHealth(float _amount)
    {
        currentHealth += _amount;

        if(_amount < 0)
            playerAnim.SetTrigger("isHurt");

        if (currentHealth <= 0)
        {
            playerAnim.SetTrigger("isDead");
            isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            RespawnManager.Instance.lastCheckpoint = collision.transform.position;
        }
    }
}