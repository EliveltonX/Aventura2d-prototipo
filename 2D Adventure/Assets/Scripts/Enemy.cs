using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum EnemyState { Idle, Patrol, Follow, Stunned };          // possible States of Player
    private Rigidbody2D rb;
    private bool isFacingRight = true;                                  //saber se ele esta virado a direita
    private Vector2 movimento = Vector2.zero;                           //o movimento do char
    private EnemyState state = EnemyState.Idle;                         //o state atual do player
    private Transform player;
    private float stunedCounter = 0f;                                   //contador do tempo de stun
    private bool isStuned = false;                                      //player esta estunado?
    private Vector3 currentVelocity = Vector3.zero;
    private Animator animator;
    private int currentHealth = 100;
        
    [Header("EnemyStatus")]
    public int Health = 100;
    public float speed = 10f;
    public float launchForce = 10f;
    [SerializeField] private EnemyState defaultState = EnemyState.Idle; // state padrao do inimigo
    public float enemyFollowDist = 5f;                                  //distancia que podera ver o player
    public LayerMask playerLayer;                                       //player layer
    [Space(2)]
    [Header("Looking for walls")]
    public float wallCheckDistance = 1f;                                //distancia para mudar de direção ao ver um muro
    public LayerMask whatIsWalls;                                       //layer do scenario
    [Space(2)]
    [Header("Looking for ledges")]
    public Transform ledgeCheckerRight;                                 //posicão do checker de buracos
    public float ledgeChekerDistance = 0.55f;                           //tamanho do checker
    [Space(2)]
    [Header("Looking for Ground")]
    public float groundCheckDist = 1.05f;                               //distancia do ground cheker
    
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = Health;
    }

    private void Update()
    {
        if (!isStuned) {
            WhatState();
        }
        StateCaller();
        rb.velocity = Vector3.SmoothDamp(rb.velocity, movimento, ref currentVelocity, 0.35f);

        animator.SetFloat("WalkSpeed",movimento.x);
    }


    // >>>>>>>>>>>>>>>>>>>

    public void Follow()
    {
        //Debug.Log("follow");

        Vector2 deltaPos;

        deltaPos = transform.position - player.position;

        if (deltaPos.x > 0)
        {
            //va para esquerda
            if (isFacingRight) { Flip(); }

            movimento = new Vector2(-1 * speed * Time.deltaTime, rb.velocity.y);
        }
        if (deltaPos.x < 0)
        {
            //va para direita
            if (!isFacingRight) { Flip(); }

            movimento = new Vector2(1 * speed * Time.deltaTime, rb.velocity.y);
        }



    }

    public void Patrol()
    {
       
        
            if (isFacingRight) {
                movimento = new Vector2(1 * speed * Time.deltaTime, rb.velocity.y);
            }
            if (!isFacingRight) {
                movimento = new Vector2(-1 * speed * Time.deltaTime, rb.velocity.y);
            }
        
        Fliper();
    }

    public void Stunned()
    {
        //Debug.Log("stuned");
        stunedCounter -= Time.deltaTime;

        if (stunedCounter <= 0)
        {
            isStuned = false;
        }
    }

    public void Idle()
    {

        if (IsGrounded())
        {
            movimento = new Vector2(0, rb.velocity.y);
        }

    }

    public void WhatState()
    {

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, enemyFollowDist, playerLayer);

        if (playerCollider != null)
        {
            player = playerCollider.transform;
            state = EnemyState.Follow;
        }
        else
        {
            state = defaultState;
            playerCollider = null;
        }

    }

    public void Damage(int _amount, Vector3 playerPos, float _launchForce)
    {
        currentHealth -= _amount;
        stunedCounter = 1f;
        isStuned = true;
        state = EnemyState.Stunned;
        movimento = Vector2.zero;
        rb.velocity = Vector2.zero;
        Vector3 deltaPos = transform.position - playerPos;

        animator.SetTrigger("Hurt");

        if (deltaPos.x <= 0)
        {
            //lançar para esquerda;
            rb.AddForce(new Vector2(-1, 1) * _launchForce, ForceMode2D.Impulse);
        }
        if (deltaPos.x >= 0)
        {
            //lancar para direita;
            rb.AddForce(new Vector2(1, 1) * _launchForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {   
            //Die
            Destroy(gameObject,1);
        }

    }

    private void Fliper()
    {

        if (Physics2D.Raycast(transform.position, transform.right, wallCheckDistance, whatIsWalls) && isFacingRight)
        {
            Flip();
        }
        if (Physics2D.Raycast(transform.position, -transform.right, wallCheckDistance, whatIsWalls) && !isFacingRight)
        {
            Flip();
        }
        if (IsGrounded())
        {
            RaycastHit2D rightLedge = Physics2D.Raycast(ledgeCheckerRight.position, Vector2.down, ledgeChekerDistance, whatIsWalls);

            if (rightLedge.collider == null)
            {
                Flip();
               // Debug.Log("Turn");
            }
        }

    }

    private void Flip()
    {
        Vector3 newScala = transform.localScale;
        rb.velocity = new Vector2(0, rb.velocity.y);
        newScala.x = newScala.x * -1;
        transform.localScale = newScala;
        isFacingRight = !isFacingRight;
    }

    private bool IsGrounded()
    {

        if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckDist, whatIsWalls))
        {
            return true;
        }
        else
        { return false; }
    }

    private void StateCaller()
    {

        if (state == EnemyState.Follow)
        {
            Follow();
        }
        if (state == EnemyState.Idle)
        {
            Idle();
        }
        if (state == EnemyState.Patrol)
        {
            Patrol();
        }
        if (state == EnemyState.Stunned)
        {
            Stunned();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colidiu :" + collision.collider.name + " com :" + collision.otherCollider.name);
        if (collision.collider.CompareTag("Player")) {


           // Debug.Log("Collidiu com : " + collision.collider.name);
            collision.collider.GetComponent<CharacterController2D>().Damage(10,launchForce,0.5f,transform.position);

        }
    }

    private void OnDrawGizmosSelected()
    {
        // right
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.right * wallCheckDistance);
        Gizmos.DrawRay(ledgeCheckerRight.position, Vector2.down * ledgeChekerDistance);

        //Follow
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyFollowDist);

        // Down
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDist);



    }

}
