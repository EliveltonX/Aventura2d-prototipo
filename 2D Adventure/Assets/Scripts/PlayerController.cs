using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector2 k_horizontal;                //input horizontal
    private bool jump, crounch;                     //agachado ou pular
    private CharacterController2D controller;       
    private Animator animator;
    private Controles controle;
    

    void Awake()
    {
        controle = new Controles();
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();

        controle.Gameplay.MeleeAttack.started += ctx => MeleeAtack(); // Melee Atack
        controle.Gameplay.Dash.started += ctx => Dash(); // Dash

        controle.Gameplay.Move.performed += ctx => k_horizontal = ctx.ReadValue<Vector2>(); // MovimentoLateral
        controle.Gameplay.Move.canceled += ctx => k_horizontal = Vector2.zero;

        controle.Gameplay.Jump.performed += ctx => StartJump();
        controle.Gameplay.Jump.canceled += ctx => EndJump();
    }
    void MeleeAtack()
    {
        controller.Atack();
    }
    void StartJump()
    {
        controller.Jump();
    }
    void EndJump()
    {
        controller.EndJump();
    }
    void Dash()
    {
        controller.Dash();
    }

    
   
    void Update()
    {   
        //set do animator se esta parado ou andando
        if (k_horizontal.x != 0) { animator.SetBool("isWalking", true); }
        else { animator.SetBool("isWalking", false); }
       
    }


    private void FixedUpdate()
    {
        controller.Move(k_horizontal.x * Time.fixedDeltaTime, crounch, jump);//chamando o movimento
    }


    private void OnEnable()
    {
        controle.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controle.Gameplay.Disable();
    }
}
