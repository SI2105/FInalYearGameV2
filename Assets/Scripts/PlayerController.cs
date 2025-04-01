using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private bool inputsEnabled = true;
   
    SpriteRenderer sprite;
    [Header("Movement")]
    public float movespeed = 5.0f;
    float horizontalmovement;
    [Header("Jump")]
    public float jumpforce = 1.0f;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float gravity = 1.0f;
    public float maxfallspeed = 10f;
    public float fallMultiplier = 2.5f;

    private Animator _animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
 

    void Start()
    {
        
      
        rigidbody2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();



    }


    void FixedUpdate()
    {
        
        rigidbody2d.velocity = new Vector2(horizontalmovement * movespeed, rigidbody2d.velocity.y);
        _animator.SetFloat("xVelocity", Mathf.Abs(rigidbody2d.velocity.x));
        _animator.SetFloat("yVelocity", rigidbody2d.velocity.x);
        if (isGrounded())
        {
            _animator.SetBool("IsJumping", false);
        }
        //Gravity();
  




    }

    private void OnDrawGizmosSelected() 
    {
        //Dev Aid
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

    }


 
    public void Move(InputAction.CallbackContext context) {

        if (!inputsEnabled)
        {
            return;
        }
        //Movement Input Action
        horizontalmovement = context.ReadValue<Vector2>().x;

        //Handle Sprite direction
        if (horizontalmovement > 0)
        {
            sprite.flipX = true;
        }

        else if (horizontalmovement < 0) { 
            sprite.flipX = false;
        }
    }

    public void Jump(InputAction.CallbackContext context) {

        if (!inputsEnabled) {
            return;
        }
        if (isGrounded()){
            if (context.performed)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpforce);
                _animator.SetBool("IsJumping", true);
            }
            else if (context.canceled){
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * jumpforce/2);
           

            }
        }
    }

    private bool isGrounded() {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer)) {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Gravity() {
        //Deprecated Gravity Code
        if (rigidbody2d.velocity.y < 0)
        {
            rigidbody2d.gravityScale = gravity * fallMultiplier;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -maxfallspeed);
        }
        else {
            rigidbody2d.gravityScale = gravity;
        }
    }

    public void enableInput() {
        inputsEnabled = true;
    }
    public void disableInput() { 
        inputsEnabled = false;
    }
    
}
