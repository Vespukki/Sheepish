using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;



public class PlayerMovement : MonoBehaviour
{
    #region Variables

    #region Inspector Stuff
    public PlayerStats stats;

    [SerializeField] Collider2D groundedCollider;
    [SerializeField] Collider2D leftWallClingingCollider;
    [SerializeField] Collider2D rightWallClingingCollider;
    public Collider2D downAttackCollider;
    public Collider2D drillAttackCollider;

    [SerializeField] GameObject lureObject;
    #endregion

    #region Tracking
    [HideInInspector] public static float playerGravity; 

    [HideInInspector] public PlayerState lastState;
    [HideInInspector] public PlayerState currentState;

    [HideInInspector] public float remainingDashes = 1;
    [HideInInspector] public float normalGravity = 0;

    [HideInInspector] public float lookingDir = 1;

    float moveInput = 0;
    bool grounded;
    [HideInInspector] public bool underwater;
    int wallCling = 0;

    [HideInInspector] public bool jumping = false;
    public bool canMove = true;


    //so you dont hit things multiple times in the same cast
    List<GameObject> alreadyHit = new();

    Lure currentLure = null;

    #endregion

    #region Timers and Coroutines
    //timers
    [HideInInspector] public float lastDashTimer = 100;
    [HideInInspector] public float knockbackTimer = 100;

    //coroutines
    Coroutine wallJumpCutCo;
    #endregion

    #region Startup

    Rigidbody2D body;
    PlayerInput input;
    SpriteRenderer spriter;
    Animator animator;
    PlayerInteraction inter;
    Collider2D coll;
    PlayerFishing fisher;

    InputAction move;
    InputAction jump;
    InputAction dash;
    InputAction downAttack;
    InputAction drillAttack;
    InputAction dropDown;
    InputAction interact;
    InputAction fish;
    InputAction reel;

    #endregion

    #region Events
    public delegate void PlayerEmptyDelegate();
    public delegate void PlayerMoverDelegate(PlayerMovement mover);
    
    public static event PlayerMoverDelegate OnPlayerAwake;
    public static event PlayerEmptyDelegate OnUnfish;
    #endregion

    #endregion

    #region Setup and Upkeep
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        inter = GetComponent<PlayerInteraction>();
        coll = GetComponent<Collider2D>();
        fisher = GetComponent<PlayerFishing>();

        move = input.actions.FindAction("Move");
        jump = input.actions.FindAction("Jump");
        dash = input.actions.FindAction("Dash");
        downAttack = input.actions.FindAction("Down Attack");
        drillAttack = input.actions.FindAction("Drill Attack");
        dropDown = input.actions.FindAction("Drop Down");
        interact = input.actions.FindAction("Interact");
        fish = input.actions.FindAction("Fish");
        reel = input.actions.FindAction("Reel");


        move.performed += MoveInput;
        move.canceled += MoveInput;
        jump.started += JumpInput;
        jump.canceled += CancelJumpInput;
        dash.started += DashInput;
        downAttack.started += DownAttackInput;
        drillAttack.started += DrillAttackInput;
        interact.started += InteractInput;
        fish.started += FishInput;

        CallPlayerAwake();

        normalGravity = (-2 * stats.jumpHeight) / (stats.jumpTime * stats.jumpTime) / -10;
        playerGravity = normalGravity;
    }

    public void CallPlayerAwake()
    {
        OnPlayerAwake?.Invoke(this);
    }

    private void FixedUpdate()
    {


        UpdateTimers();

        if (body.velocity.y < 0)
        {
            jumping = false;
        }

        if (dropDown.IsPressed())
        {
            Physics2D.IgnoreLayerCollision(8, 6, true);
        }
        else if (underwater == false)
        {
            Physics2D.IgnoreLayerCollision(8, 6, false);
        }

        if(reel.IsPressed())
        {
            Reel(-(int)Mathf.Sign(reel.ReadValue<float>()));
        }

        //hitbox checks
        Grounded();
        Underwater();
        WallClinging();
        SideAttack();
        DrillAttack();

        SetAnimatorVars();


    }

    void SetAnimatorVars()
    {
        if (wallCling != 0 && (currentState is WallClingState))
        {
            lookingDir = -wallCling;
        }
        else if (currentState != null && currentState.canChangeDir && moveInput != 0)
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

    public void GroundReset()
    {
        remainingDashes = stats.maxDashes;
    }
    void UpdateTimers()
    {
        lastDashTimer += Time.fixedDeltaTime;
        knockbackTimer += Time.fixedDeltaTime;
    }

    #endregion

    #region Input

    void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    void JumpInput(InputAction.CallbackContext context)
    {
        animator.SetTriggerXFixedFrames(stats.inputForgivenessFrames, this, "Jump");

        if(currentState is SideAttackState && grounded)
        {
            Jump();
        }
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
            animator.SetTriggerXFixedFrames(stats.inputForgivenessFrames, this, "Dash");
        }
    }    

    void DownAttackInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() <= 0) return;
        animator.SetTriggerXFixedFrames(stats.inputForgivenessFrames, this, "SideAttack");
    }

    void DrillAttackInput(InputAction.CallbackContext context)
    {
        animator.SetTriggerXFixedFrames(stats.inputForgivenessFrames, this, "DrillAttack");
    }

    void InteractInput(InputAction.CallbackContext context)
    {
        inter.TryInteract();
    }

    public void FishInput(InputAction.CallbackContext context)
    {
        if (currentLure != null)
        {
            Unfish();
        }
        else
        {
            Fish();
        }
    }

    #endregion

    #region Movement


    public void Movement()
    {
        if (!canMove) return;
        

        if (Mathf.Sign(body.velocity.x) == Mathf.Sign(moveInput) && Mathf.Abs(body.velocity.x + .01f) > Mathf.Abs(stats.speed) && moveInput != 0 && currentState is AirState)
        {
            //no deceleration
        }
        else
        {
            Move();
        }
        //no small numbers!
        if (Mathf.Abs(body.velocity.x) < 0.1)
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }

    }

    void Move()
    {
        float targetSpeed = moveInput * stats.speed; //dir to move in at speed

        float speedDiff = targetSpeed - body.velocity.x; //diff between current and desired

        bool accel = Mathf.Abs(targetSpeed) > .01; //choose to accelerate or decelerate
        float accelRate = accel ? stats.acceleration : stats.decceleration;

        //applies acceleration to speedDiff, then sets to a power to set jerk. then reapplies direction
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, stats.jerk) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.right);

        
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
        if(moveInput != 0)
        {
            lookingDir = moveInput;
        }
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

    

    public void LimitFallSpeed(float multiplier)
    {
        float waterMultiplier = underwater ? stats.waterFallSpeedMultiplier : 1;

        float maxFallSpeed = stats.maxFallSpeed * multiplier * waterMultiplier;
        if (body.velocity.y < maxFallSpeed)
        {
            body.velocity = new Vector2(body.velocity.x, stats.maxFallSpeed * multiplier * waterMultiplier);
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

    IEnumerator CantMove(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void Knockback(Vector2 force, float direction)
    {
        jumping = false;
        body.velocity = new Vector2(force.x * direction, force.y);
        knockbackTimer = 0;
    }

    void Unfish()
    {
        OnUnfish?.Invoke();
        DeleteLure();
    }

    void Fish()
    {
        if (currentLure != null)
        {
            DeleteLure();
        }


        currentLure = Instantiate(lureObject, transform.position, transform.rotation).GetComponent<Lure>();


        currentLure.gameObject.GetComponent<Rigidbody2D>().velocity = (currentState is AirState ? new Vector2(lookingDir * stats.airCastSpeed.x, stats.airCastSpeed.y) :
            new Vector2(lookingDir * stats.castSpeed.x, stats.castSpeed.y)) + new Vector2(body.velocity.x, body.velocity.y > 0 ? body.velocity.y : 0);

        currentLure.mover = this;
        currentLure.fisher = fisher;
    }

    public void DeleteLure()
    {
        if (currentLure != null)
        {
            Destroy(currentLure.gameObject);
            currentLure = null;
        }
        
        animator.SetTriggerOneFixedFrame(this, "GrappleCut");
    }

    public void GrappleSwing()
    {
        body.AddForce(moveInput * stats.swingSpeed * Vector2.right);
    }

    public void Reel(float direction)
    {
        if (currentLure == null) return;

        if (currentLure.state == lureState.grappling)
        {
            currentLure.distJoint.distance += stats.reelSpeed * direction;
        }
        else if (currentLure.state == lureState.fishing)
        {
            if (direction < 0)
            {
                currentLure.body.AddForce(new Vector2(Mathf.Sign(transform.position.x - currentLure.body.position.x), 0) * stats.fishingReelSpeed);
            }
        }
    }

    #endregion

    #region Hitbox Checks
    //also checks if on water surface
    void Grounded()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        groundedCollider.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.IsTouching(coll) && hit.CompareTag("Ground"))
            {
                grounded = true;
                return;
            }
            else if (hit.IsTouching(coll) && hit.CompareTag("Water Surface"))
            {
                grounded = true;
                return;
            }
        }
        grounded = false;
    }
    void Underwater()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        coll.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.IsTouching(coll) && hit.CompareTag("Water"))
            {
                underwater = true;
                return;
            }
        }
        underwater = false;
    }
    void WallClinging()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        //first check left
        leftWallClingingCollider.GetContacts(colliders);
        foreach (var hit in colliders)
        {
            if (hit.IsTouching(coll) && hit.CompareTag("Ground"))
            {
                wallCling = -1;
                return;
            }
        }

        //then right
        rightWallClingingCollider.GetContacts(colliders);
        foreach (var hit in colliders)
        {
            if (hit.IsTouching(coll) && hit.CompareTag("Ground"))
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
                iHit.OnHit(stats.damage, gameObject);
                if(alreadyHit.Count == 0)
                {
                    jumping = false;
                    body.velocity = new Vector2(stats.sideAttackKnockback * lookingDir, body.velocity.y);
                    StartCoroutine(CantMove(stats.attackKnockbackTime));
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
                iHit.OnHit(stats.damage, gameObject);
                if (alreadyHit.Count == 0)
                {
                    Knockback(stats.drillKnockback, lookingDir);
                    GroundReset();
                    animator.SetTrigger("DrillAttackCut");
                }
                alreadyHit.Add(hit.gameObject);
                return;
            }
        }
    }

    #endregion
}