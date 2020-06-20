using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float wallCheckerRadius;
    public Transform wallChecker;
    public Transform sledgechecker;
    public float sledgeCheckerRadius;
    public float velocity;
    public float stunDuration = 1f;
    public float launchForce = 100;
    public LayerMask layerMask;
    public int life = 30;

    private Rigidbody2D rb;
    private float movimento = 1;
    private float stunCounter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetComponent<Animator>().SetFloat("velocityX", rb.velocity.x);

        if (Physics2D.OverlapCircle(wallChecker.position,wallCheckerRadius,layerMask)) 
        {
            //Debug.Log("I see wall");
            invertValues();
        }

        if (!Physics2D.OverlapCircle(sledgechecker.position, sledgeCheckerRadius, layerMask)) 
        {
            //Debug.Log("Hole shit");
            invertValues();
        }

        stunCounter -= Time.deltaTime;

        if (stunCounter <=0) 
        {
            rb.velocity = new Vector2(velocity * movimento, rb.velocity.y);
        }
    }

    public void Stun(CharacterController2D _playerController) 
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        stunCounter = 1;
        
    }
    public void push(Transform _playerPos,float _pushForce) 
    {
        Vector2 direction = (transform.position - _playerPos.position).normalized;
        rb.AddForce(direction * _pushForce);

    }

    public void Damage(int _amount,CharacterController2D _playerController) 
    {
        //do damage
        GameObject player = _playerController.gameObject;
        life -= _amount;
        Stun(_playerController);
        push(_playerController.transform, _playerController.lauchForce);
        GetComponent<Animator>().SetTrigger("hurt");

        if (life <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void invertValues() 
    {
        transform.localScale = new Vector3 (transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
        movimento = movimento * -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colidiu :" + collision.collider.name + " com :" + collision.otherCollider.name);
        if (collision.collider.CompareTag("Player"))
        {


            // Debug.Log("Collidiu com : " + collision.collider.name);
            collision.collider.GetComponent<CharacterController2D>().Damage(10,launchForce , 0.5f, transform.position);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(wallChecker.position, wallCheckerRadius);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(sledgechecker.position, sledgeCheckerRadius);
    }
}
