using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1, menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] float _speed;
    [SerializeField] float _acceleration;
    [SerializeField] float _decceleration;
    [SerializeField] float _jerk;

    public float speed => _speed;
    public float acceleration => _acceleration;
    public float decceleration => _decceleration;
    public float jerk => _jerk;

    [Header("Jump")]
    [SerializeField] float _jumpHeight;
    [SerializeField] float _jumpTime;
    [SerializeField] float _maxFallSpeed;
    [SerializeField] float _fallSpeedMultiplier;
    [SerializeField] int _inputForgivenessFrames;

    public float jumpHeight => _jumpHeight;
    public float jumpTime => _jumpTime;
    public float fallSpeedMultiplier => _fallSpeedMultiplier;
    public float maxFallSpeed => _maxFallSpeed;
    public int inputForgivenessFrames => _inputForgivenessFrames;

    [Header("Wall Cling")]
    [SerializeField] float _wallClingFallSpeed;
    [SerializeField] float _wallClingAcceleration;
    [SerializeField] Vector2 _wallJumpForce;
    [SerializeField] float _wallJumpTime;
    public float wallClingFallSpeed => _wallClingFallSpeed;
    public float wallClingAcceleration => _wallClingAcceleration;
    public Vector2 wallJumpForce => _wallJumpForce;
    public float wallJumpTime => _wallJumpTime;
    
    [Header("Dash")]
    [SerializeField] float _dashTime;
    [SerializeField] float _dashDistance;
    [SerializeField] int _maxDashes;
    [SerializeField] float _dashCD;

    public float dashTime => _dashTime;
    public float dashDistance => _dashDistance;
    public int maxDashes => _maxDashes;
    public float dashCD => _dashCD;

    [Header("Attack")]
    [SerializeField] int _damage;
    [SerializeField] float _invincibleTime;
    [SerializeField] float _sideAttackKnockback;
    [SerializeField] float _attackKnockbackTime;
    [SerializeField] Vector2 _damageKnockback;

    public int damage => _damage;
    public float invincibleTime => _invincibleTime;
    public float sideAttackKnockback => _sideAttackKnockback; //knockback from attacking
    public float attackKnockbackTime => _attackKnockbackTime;
    public Vector2 damageKnockback => _damageKnockback; //knockback dealt to others

    [Header("Drill")]
    [SerializeField] float _drillSpeed;
    [SerializeField] Vector2 _drillKnockback;
    [SerializeField] float _drillKnockbackTime;
    public float drillSpeed => _drillSpeed;
    public Vector2 drillKnockback => _drillKnockback;
    public float drillKnockbackTime => _drillKnockbackTime;

    [Header("Water")]
    [SerializeField] float _waterGravityMultiplier;
    [SerializeField] float _waterFallSpeedMultiplier;
    
    public float waterGravityMultiplier => _waterGravityMultiplier;
    public float waterFallSpeedMultiplier => _waterFallSpeedMultiplier;

}
