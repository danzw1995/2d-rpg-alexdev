using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 4f;

  [SerializeField] private float jumpSpeed = 6f;

  [Header("Dash info")]
  [SerializeField] private float dashDuration = 1f;
  [SerializeField] private float dashSpeed = 5f;

  [SerializeField] private float dashCooldown = 1f;
  private float dashCooldownTimer = -1f;
  private float dashTime = 0;

  [Header("Attack info")]
  [SerializeField] private float comboTimeWindow = 0.5f;
  private float comboTimeCounter = 0;
  private bool isAttacking = false;
  private int comboCounter = 0;

  [Header("Collision info")]
  [SerializeField] private float checkGroundDistance = 0f;
  [SerializeField] private LayerMask layerMask;
  private bool isGrounded;
  private Rigidbody2D rb;

  private Animator anim;
  private float xInput;

  private bool isFacingRight = true;

  private bool isMoving;
  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();
  }

  private void Update()
  {

    Movement();

    HandleInputs();

    CollisionChecks();

    FlipController();
    AnimationControllers();
  }

  private void HandleInputs()
  {
    xInput = Input.GetAxisRaw("Horizontal");

    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      Attack();
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      Jump();
    }

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      DashAbility();
    }
    dashCooldownTimer -= Time.deltaTime;
    dashTime -= Time.deltaTime;
    comboTimeCounter -= Time.deltaTime;

  }

  private void Attack()
  {
    if (!isGrounded) return;

    if (comboTimeCounter < 0)
    {
      comboCounter = 0;
    }
    comboTimeCounter = comboTimeWindow;
    isAttacking = true;
  }

  private void DashAbility()
  {
    if (dashCooldownTimer < 0)
    {
      dashCooldownTimer = dashCooldown;
      dashTime = dashDuration;
    }
  }

  private void Movement()
  {

    if (isAttacking)
    {
      rb.velocity = new Vector2(0, 0);
    }
    else if (dashTime > 0)
    {
      rb.velocity = new Vector2(xInput * dashSpeed, 0);

    }
    else
    {
      rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }


  }

  private void AnimationControllers()
  {
    isMoving = xInput != 0;

    anim.SetFloat("yVelocity", rb.velocity.y);
    anim.SetBool("isMoving", isMoving);
    anim.SetBool("isGrounded", isGrounded);
    anim.SetBool("isDashing", dashTime > 0);
    anim.SetBool("isAttacking", isAttacking);
    anim.SetInteger("comboCounter", comboCounter);
  }

  private void Jump()
  {
    if (isGrounded)
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
  }


  private void CollisionChecks()
  {
    isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkGroundDistance, layerMask);
  }

  private void FlipController()
  {
    if (rb.velocity.x > 0 && !isFacingRight)
    {
      Flip();
    }
    else if (rb.velocity.x < 0 && isFacingRight)
    {
      Flip();
    }
  }

  private void Flip()
  {
    isFacingRight = !isFacingRight;

    transform.Rotate(0, 180, 0);
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - checkGroundDistance));
  }

  public void AttackOver()
  {
    isAttacking = false;
    comboCounter++;

    if (comboCounter > 2)
    {
      comboCounter = 0;
    }
  }
}
