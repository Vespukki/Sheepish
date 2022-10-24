using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody2D body;
    PlayerInput input; //set by onPlayerAwake

    InputAction moveHook;

    Vector2 moveInput;

    public delegate void HookEmptyDelegate();
    public static event HookEmptyDelegate OnHookSpawn;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        body.velocity = new Vector2(0, -speed);

        PlayerMovement.OnPlayerAwake += GetPlayerInfo;
        OnHookSpawn?.Invoke();
    }

    private void OnDestroy()
    {
        PlayerMovement.OnPlayerAwake -= GetPlayerInfo;
    }

    void GetPlayerInfo(PlayerMovement mover)
    {
        input = mover.GetComponent<PlayerInput>();
        moveHook = input.actions.FindAction("Move Hook");

        input.SwitchCurrentActionMap("Fishing");

        moveHook.performed += MoveHookInput;
        moveHook.canceled += MoveHookInput;
    }

    void MoveHookInput(InputAction.CallbackContext context)
    {
        //Vector2 roundInput = new Vector2(Mathf.Round(context.ReadValue<Vector2>().x), Mathf.Round(context.ReadValue<Vector2>().y));

        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        body.velocity = (moveInput * speed * 1.5f) + (Vector2.down * speed);
    }
}
