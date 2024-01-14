using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
  [Header("Move info")]
  [SerializeField] private float moveSpeed = 4f;

  [Header("Player detection")]
  [SerializeField] private float playerCheckDistance = 2f;
  [SerializeField] private LayerMask whatIsPlayer;

  private RaycastHit2D playerDetectHit;

  private bool isAttacking = false;

  protected override void Update()
  {
    base.Update();

    if (playerDetectHit)
    {
      if (playerDetectHit.distance > 1f)
      {
        rb.velocity = new Vector2(facingDirection * moveSpeed * 2, rb.velocity.y);

        Debug.Log("I saw player");
        isAttacking = false;
      }
      else
      {
        Debug.Log("Attack " + playerDetectHit.collider.gameObject.name);
        isAttacking = true;
      }
    }


    if (!isGrounded || isWalled)
    {
      Flip();
    }

    Movement();
  }

  private void Movement()
  {
    if (!isAttacking)
    {
      rb.velocity = new Vector2(facingDirection * moveSpeed, rb.velocity.y);

    }
  }

  protected override void CollisionChecks()
  {
    base.CollisionChecks();


    playerDetectHit = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDirection, whatIsPlayer);
  }

  protected override void OnDrawGizmos()
  {
    base.OnDrawGizmos();

    Gizmos.color = Color.blue;
    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDirection, transform.position.y));
  }
}
