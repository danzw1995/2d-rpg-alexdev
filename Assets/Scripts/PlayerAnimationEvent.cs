using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

  private Player player;
  private void Awake()
  {
    player = GetComponentInParent<Player>();
  }

  private void AnimationEvent()
  {
    player.AttackOver();
  }
}
