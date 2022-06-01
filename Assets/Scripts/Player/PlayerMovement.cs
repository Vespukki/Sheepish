using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;



public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public PlayerStats stats;

    [SerializeField] Collider2D groundedCollider;
    [SerializeField] Collider2D leftWallClingingCollider;
    [SerializeField] Collider2D rightWallClingingCollider;
    public Collider2D downAttackCollider;
    public Collider2D drillAttackCollider;

    [HideInInspector] public PlayerState currentState;

    [HideInInspector] public float remainingDashes = 1;

    public float lookingDir = 1;

    float moveInput = 0;
    bool grounded;
    int wallCling = 0;

    [HideInInspector] public bool jumping = false;

    //timers
    [HideInInspector] public float lastDashTimer = 100;
    [HideInInspector] public float knockbackTimer = 100;

    //coroutines
    Coroutine wallJumpCutCo;

    //so you dont hit things multiple times in the same cast
    List<GameObject> alreadyHit = new List<GameObject>();

    Rigidbody2D body;
    PlayerInput input;
    SpriteRenderer spriter;
    Animator animator;

    InputAction move;
    InputAction jump;
    InputAction dash;
    InputAction downAttack;
    InputAction drillAttack;
    #endregion

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        move = input.actions.FindAction("Move");
        jump = input.actions.FindAction("Jump");
        dash = input.actions.FindAction("Dash");
        downAttack = input.actions.FindAction("Down Attack");
        drillAttack = input.actions.FindAction("Drill Attack");

        move.performed += MoveInput;
        move.canceled += MoveInput;
        jump.started += JumpInput;
        jump.canceled += CancelJumpInput;
        dash.started += DashInput;
        downAttack.started += DownAttackInput;
        drillAttack.started += DrillAttackInput;
    }

    void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    void JumpInput(InputAction.CallbackContext context)
    {
        animator.SetTriggerXFixedFrames(stats.inputForgivenessFrames, this, "Jump");
    }

    void CancelJumpInput(InputAction.CallbackContext context)
    {
        if (currentState is JumpingState || (currentState is SideAttackState && jumping == true))
        {
            animator.SetTriggerOneFixedFrame(this, "JumpCut");
            JumpCut();
        }
    }

    void DashInput(InputAction.CallbackContext context)
    {
        if(remainingDashes > 0 && lastDashTimer >= stats.dashCD)
        {
            animator.SetTriggerOneFixedFrame(this, "Dash");
        }
    }    

    void DownAttackInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() <= 0) return;
        animator.SetTriggerOneFixedFrame(this, "SideAttack");
    }

    void DrillAttackInput(InputAction.CallbackContext context)
    {
        animator.SetTriggerOneFixedFrame(this, "DrillAttack");
    }

    private void Update()
    {

        //movement hitbox checks
        Grounded();
        WallClinging();
        SideAttack();
        DrillAttack();

        SetAnimatorVars();
    }

    private void FixedUpdate()
    {
        UpdateTimers();

        if (currentState != null)
        {
            currentState.PhysicsUpdate(this);
        }

        if(body.velocity.y < 0)
        {
            jumping = false;
        }
    }

    public void Movement()
    {
        float targetSpeed = moveInput * stats.speed; //dir to move in at speed
        float speedDiff = targetSpeed - body.velocity.x; //diff between current and desired

        float accelRate = (Mathf.Abs(targetSpeed) > .01) ? stats.acceleration : stats.decceleration; //choose to accelerate or decelerate

        //applies acceleration to speedDiff, then sets to a power to set jerk. then reapplies direction
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, stats.jerk) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.right);

        //no small numbers!
        if(Mathf.Abs(body.velocity.x) < 0.05)
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }    
    }

    public void Jump()
    {
        jumping = true;
        body.velocity = new Vector2(body.velocity.x, 0);

        body.velocity += (2 * stats.jumpHeight / stats.jumpTime) * Vector2.up; //kinematics to make sure that jump is always same height
    }

    public void JumpCut()
    {
        body.velocity = new Vector2(body.velocity.x, 0);
    }

    public void WallJump()
    {
        body.velocity = new Vector2(lookingDir * stats.wallJumpForce.x, stats.wallJumpForce.y);

        wallJumpCutCo = StartCoroutine(WallJumpCut());
    }

    public IEnumerator WallJumpCut()
    {
        yield return new WaitForSeconds(stats.wallJumpTime);
        animator.SetTriggerOneFixedFrame(this, "WallJumpCut");
    }

    public void StopWallJumpCut()
    {
        if (wallJumpCutCo != null)
        {
            StopCoroutine(wallJumpCutCo);
        }
    }

    public void Dash()
    {
        spriter.flipX = lookingDir == -1;
        StopCoroutine(DashCut());
        animator.ResetTrigger("DashCut");
        body.velocity = new Vector2(stats.dashDistance / stats.dashTime * lookingDir, 0);
        StartCoroutine(DashCut());
    }

    public IEnumerator DashCut()
    {
        yield return new WaitForSeconds(stats.dashTime);
        animator.SetTriggerOneFixedFrame(this, "DashCut");
    }

    public void LimitFallSpeed()
    {
        if(body.velocity.y < stats.maxFallSpeed)
        {
            body.velocity = new Vector2(body.velocity.x, stats.maxFallSpeed);
        }
    }

    public void StartAttack(Collider2D collider)
    {
        collider.gameObject.SetActive(true);
        collider.offset = new Vector2(lookingDir * collider.offset.x, collider.offset.y);
    }
    public void EndAttack(Collider2D collider)
    {
        collider.offset = new Vector2(lookingDir * collider.offset.x, collider.offset.y);
        collider.gameObject.SetActive(false);
        alreadyHit.Clear();
    }

    void Grounded()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        groundedCollider.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.IsTouching(GetComponent<Collider2D>()) && hit.CompareTag("Ground"))
            {
                grounded = true;
                return;
            }
        }
        grounded = false;
    }
    void WallClinging()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        //first check left
        leftWallClingingCollider.GetContacts(colliders);
        foreach (var hit in colliders)
        {
            if (hit.IsTouching(GetComponent<Collider2D>()) && hit.CompareTag("Ground"))
            {
                wallCling = -1;
                return;
            }
        }

        //then right
        rightWallClingingCollider.GetContacts(colliders);
        foreach (var hit in colliders)
        {
            if (hit.IsTouching(GetComponent<Collider2D>()) && hit.CompareTag("Ground"))
            {
                wallCling = 1;
                return;
            }
        }

        //can only reach here not touching either wall
        wallCling = 0;
    }
    void SideAttack()
    {
        if(!downAttackCollider.gameObject.activeSelf) return;


        List<Collider2D> colliders = new List<Collider2D>();

        downAttackCollider.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent<IHittable>(out IHittable iHit) && !alreadyHit.Contains(hit.gameObject) && hit.isTrigger)
            {
                iHit.OnHit(this);
                if(alreadyHit.Count == 0)
                {
                    jumping = false;
                    body.velocity = new Vector2(stats.knockback * lookingDir, body.velocity.y);
                }
                alreadyHit.Add(hit.gameObject);
                return;
            }
        }
    }
    void DrillAttack()
    {
        if (!drillAttackCollider.gameObject.activeSelf) return;


        List<Collider2D> colliders = new List<Collider2D>();

        drillAttackCollider.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent<IHittable>(out IHittable iHit) && !alreadyHit.Contains(hit.gameObject) && hit.isTrigger)
            {
                iHit.OnHit(this);
                if (alreadyHit.Count == 0)
                {
                    jumping = false;
                    body.velocity = new Vector2(stats.drillKnockback.x * lookingDir, stats.drillKnockback.y);
                    knockbackTimer = 0;
                    animator.SetTrigger("DrillAttackCut");
                }
                alreadyHit.Add(hit.gameObject);
                return;
            }
        }
    }

    void SetAnimatorVars()
    {
        if(wallCling != 0 && (currentState is WallClingState))
        {
            lookingDir = -wallCling;
        }
        else if(currentState.canChangeDir && moveInput != 0)
        {
            lookingDir = moveInput;
        }

        spriter.flipX = lookingDir == -1;


        //sets animator variables
        animator.SetBool("Moving", Mathf.Abs(moveInput) > 0);
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("yVelocity", body.velocity.y);
        animator.SetInteger("WallCling", wallCling);
    }

    void UpdateTimers()
    {
        lastDashTimer += Time.fixedDeltaTime;
        knockbackTimer += Time.fixedDeltaTime;
    }




}