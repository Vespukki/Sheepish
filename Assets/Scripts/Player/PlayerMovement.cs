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

    [HideInInspector] public PlayerState currentState;

    [HideInInspector] public float remainingDashes = 1;

    public float lookingDir = 1;

    float moveInput = 0;
    bool grounded;
    int wallCling = 0;

    Rigidbody2D body;
    PlayerInput input;
    SpriteRenderer spriter;
    Animator animator;

    InputAction move;
    InputAction jump;
    InputAction dash;
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

        move.performed += MoveInput;
        move.canceled += MoveInput;
        jump.started += JumpInput;
        jump.canceled += CancelJumpInput;
        dash.started += DashInput;
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
        if(currentState is JumpingState)
        {
            animator.SetTriggerOneFixedFrame(this, "JumpCut");
            JumpCut();
        }
    }

    void DashInput(InputAction.CallbackContext context)
    {
        if(remainingDashes > 0)
        {
            animator.SetTriggerOneFixedFrame(this, "Dash");
        }
    }    

    private void Update()
    {
        //movement hitbox checks
        Grounded();
        WallClinging();

        SetAnimatorVars();
    }

    private void FixedUpdate()
    {
        if(currentState != null)
        {
            currentState.PhysicsUpdate(this);
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
        body.velocity = Vector2.zero;

        body.velocity += (2 * stats.jumpHeight / stats.jumpTime) * Vector2.up; //kinematics to make sure that jump is always same height
    }

    public void JumpCut()
    {
        body.velocity = new Vector2(body.velocity.x, 0);
    }

    public void WallJump()
    {
        body.velocity = new Vector2(lookingDir * stats.wallJumpForce.x, stats.wallJumpForce.y);

        StartCoroutine(WallJumpCut());
    }

    public IEnumerator WallJumpCut()
    {
        yield return new WaitForSeconds(stats.wallJumpTime);
        animator.SetTriggerOneFixedFrame(this, "WallJumpCut");
    }

    public void Dash()
    {
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

    void SetAnimatorVars()
    {
        if(wallCling != 0)
        {
            lookingDir = -wallCling;
        }
        else if (moveInput != 0)
        {
            lookingDir = moveInput;
        }


        if (!(currentState is DashState))
        {
            spriter.flipX = lookingDir == -1;
        }


        //sets animator variables
        animator.SetBool("Moving", Mathf.Abs(moveInput) > 0);
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("yVelocity", body.velocity.y);
        animator.SetInteger("WallCling", wallCling);
    }
}