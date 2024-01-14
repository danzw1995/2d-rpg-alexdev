using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

  [Header("Collision info")]
  [SerializeField] protected Transform checkGroundTransform = null;
  [SerializeField] protected float checkGroundDistance = 0.5f;
  [SerializeField] protected Transform checkWallTransform = null;
  [SerializeField] protected float checkWallDistance = 0.5f;
  [SerializeField] protected LayerMask layerMask;

  protected Rigidbody2D rb;
  protected Animator anim;

  protected bool isGrounded;
  protected bool isWalled;

  protected int facingDirection = 1;
  protected bool isFacingRight = true;

  protected virtual void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();
  }

  protected virtual void Update()
  {
    CollisionChecks();
  }


  protected virtual void CollisionChecks()
  {
    isGrounded = Physics2D.Raycast(checkGroundTransform.position, Vector2.down, checkGroundDistance, layerMask);
    isWalled = Physics2D.Raycast(checkWallTransform.position, Vector2.down, checkWallDistance, layerMask);
  }



  protected virtual void OnDrawGizmos()
  {
    Gizmos.DrawLine(checkGroundTransform.position, new Vector3(checkGroundTransform.position.x, checkGroundTransform.position.y - checkGroundDistance));
    Gizmos.DrawLine(checkWallTransform.position, new Vector3(checkWallTransform.position.x + checkWallDistance * facingDirection, checkWallTransform.position.y));
  }

  protected virtual void Flip()
  {
    facingDirection *= -1;
    isFacingRight = !isFacingRight;

    transform.Rotate(0, 180, 0);
  }
}
