using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;                             
    private bool isGrounded = true;
    private int JumpCounter = 0;                    //conta cada pulo que o player dá
    private float dashTimeCounter = 0;              //contador do tempo do intervalo dos Dashes
    private Vector3 velocity = Vector3.zero;        //A velocidade atual do player no momento do Move
    private float movementSmoothing = .05f;         //Smooth no movimento
    private bool facingRight = true;                //Pra saber que lado esta virado o player
    private float timeBtwAtack = 0f;                //Timer dos Atacks
    [System.NonSerialized] public bool isStuned = false;
    private float StunCounter = 0f;
    private Animator animator;
    private PlayerStatus playerStatus;
    //private int currentHealth;
    private Vector2 targetVelocity = Vector2.zero;
    private bool isOnWall = false;

    [Header("Velocidades")]
    public float RunSpeed = 40f;
    public float crouchSpeedFactor = .35f;
    [Space(2)]
    [Header("Pulos")]
    public float jumpingForce = 400f;
    //public bool extraJumps = true;                  //se ele pode dar pulos extras
    public int howManyJumps = 2;                    //Quantidade de pulos que o player podera alem do primeiro
    //public float jumpDuration = 0.3f;
    [Space(2)]
    [Header("Atacks")]
    public float atackRate = 2f;                    //quantos ataques por segundo
    public Transform atackPos;
    public float atackRadius = .2f;
    public LayerMask whatIsEnemies;
    public int damage = 30;
    public float lauchForce = 30f;
    [Space(2)]
    [Header("WallSlide")]
    public Transform rightWallCheck;
    public float wallCheckerRadius = 0.2f;
    public LayerMask wallLayerMask;
    [Space(2)]
    [Header("Dash")]
    public float dashForce = 4f;
    public float dashInterval = 1f;
    [Space(2)]
    [Header("Ceiling / Crouching")]                 //Referente a ver quando o player pode Agachar
    public Transform ceilingChecker;
    public Collider2D playerCeiliingCollider;       //collider q sera desativado ao agachar
    public float ceilingCheckRadius = 0.2f;
    public LayerMask WhatIsCeiling;
    [Space(2)]
    [Header("Ground Checkers")]                     //referente a saber se o player esta no chao
    public Transform groundChecker1;
    public Transform groundChecker2;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
       // currentHealth = playerStatus.MaxHealth;
        
    }

    private void Update()
    {
        // Logica do IsGrounded
        #region IsGrounded*
        if (Physics2D.Raycast(groundChecker1.position, Vector2.down, groundCheckRadius, whatIsGround) || Physics2D.Raycast(groundChecker2.position, Vector2.down, groundCheckRadius, whatIsGround))
        {
            isGrounded = true;
            JumpCounter = 1;
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);

        }

        if (isOnWall) 
        {
            JumpCounter = 1;
        }

        

        animator.SetFloat("YVelocity", rb.velocity.y);

        #endregion 

        WallSlide(); // calling wall slide metod.
    }


    private void FixedUpdate()
    {   
       

        //Contador do Stun
        if (isStuned)
        {
            if (StunCounter <= 0)
            {
                isStuned = false;
            }
            StunCounter -= Time.fixedDeltaTime;
        }
        

        timeBtwAtack -= Time.deltaTime; // Contador de tempo do AtackRate;

        if (dashTimeCounter > 0) // contador de tempo do Dash Rate;
        {
            dashTimeCounter -= Time.fixedDeltaTime;
        }
        
    }

    public void Move(float _move, bool _crounch, bool _jump)
    {   

        // se o char estiver agachado ver se ele pode levantar
        if (!_crounch)
        {
            //manter o char agachado se nao ouver espaço sobre a cabeca
            if (Physics2D.OverlapCircle(ceilingChecker.position, ceilingCheckRadius, WhatIsCeiling))
            {
            _crounch = true;
            }
        }
        
        // Se ele estiver Agachado
        if (_crounch)
        {
            _move *= crouchSpeedFactor;

            // abilitando e desabilitando Collider do player
            if (playerCeiliingCollider != null)
            {
                playerCeiliingCollider.enabled = false;
            }
            else
            {
                if (playerCeiliingCollider != null)
                {
                    playerCeiliingCollider.enabled = true;
                }
            }
        }

        

        //move character by finding target velocity
        targetVelocity = new Vector2(_move * 10f * RunSpeed, rb.velocity.y);

        if (isStuned) { targetVelocity = new Vector2(0, rb.velocity.y); } // se o player estiver stunado na podera se mover

        //smothing and apply
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        // Flipando Char
        #region Fliping

        
            if (_move > 0 && !facingRight)
            {
                //Call flip
                Flip();
            }
            else if (_move < 0 && facingRight)
            {
                //call flip
                Flip();
            }
        
        #endregion

    }

    public void Jump() {


        if (isGrounded || isOnWall)
        {
            if (isOnWall && !isGrounded)
            {
                rb.velocity = new Vector2(transform.localScale.x * -1 * jumpingForce * 5, jumpingForce);
                return; 
            }
            else if (!isOnWall)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
               
            }

           
            JumpCounter++;
           // Debug.Log("Jump1");
           

        }
        else if (JumpCounter < howManyJumps && !isOnWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
            JumpCounter++;
           // Debug.Log("Jump2");
        }
        
    }
    public void EndJump()

    {   if (rb.velocity.y>0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            
        }
    }

    public void Dash()
    {
        if (dashTimeCounter <= 0)
        {
            animator.SetTrigger("Dash");
            rb.AddForce(new Vector2(transform.localScale.x,0f) * dashForce,ForceMode2D.Impulse);
            dashTimeCounter = dashInterval;

        }
    }

    public void WallSlide()
    {
        Collider2D isWall = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckerRadius, wallLayerMask.value);
        if (isWall != null && !isGrounded)
        {
           
            isOnWall = true;
            animator.SetBool("WallSliding", true);
            if (rb.velocity.y < -1f) 
            {
                rb.velocity = new Vector2(rb.velocity.x, -0.95f);
            }
            
        }
        else
        {
            
            isOnWall = false;
            animator.SetBool("WallSliding", false);
        }
    }

    public void RangedAtack()
    {
        //here I put some ranged atack logic;
    }

    private void Flip() {

        if (animator.GetCurrentAnimatorStateInfo(2).IsName("Atack")) { return; }

            facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        
    }

    public void Atack()
    {
        if (timeBtwAtack <= 0 && !isStuned)
        {
            //Debug.Log("ATAQUE");
            rb.velocity = Vector3.zero;
            targetVelocity = Vector2.zero;
            animator.SetTrigger("Atack");
            timeBtwAtack = 1f/atackRate;
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(atackPos.position, atackRadius, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyBase>().Damage(damage,GetComponent<CharacterController2D>());
            }
        }
    }

    public void Damage(int _amount, float _launchForce,float _StunTime,Vector3 _enemyPos)
    {
        if (isStuned) { return; }

        playerStatus.currentHealth -= _amount;
        StunCounter = _StunTime;
        isStuned = true;
        rb.velocity = Vector2.zero;

        animator.SetTrigger("Hurt");
        

        Vector2 deltaPos = transform.position - _enemyPos;

        
        if (deltaPos.x <= 0)
        {
            
            //lançar para esquerda;
            rb.AddForce(new Vector2(-4, 1) * _launchForce, ForceMode2D.Impulse);
        }
        if (deltaPos.x >= 0)
        {
            //lancar para direita;
            rb.AddForce(new Vector2(4, 1) * _launchForce, ForceMode2D.Impulse);
        }
 
    }

    private void OnDrawGizmosSelected()
    {
        //Atack Gizmo;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(atackPos.position, atackRadius); // Atack Gizmo

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(ceilingChecker.position, ceilingCheckRadius);// Ceiling Cheker
        Gizmos.DrawRay(groundChecker1.position, Vector2.down*groundCheckRadius); // ground checker1
        Gizmos.DrawRay(groundChecker2.position, Vector2.down * groundCheckRadius); //ground cheker 2

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rightWallCheck.position, wallCheckerRadius);//right wall checker
    }
}
