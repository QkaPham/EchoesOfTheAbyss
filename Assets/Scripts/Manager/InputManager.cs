using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private Controls controls;
    [field: SerializeField] public Vector2 MousePosition { get; private set; }
    [field: SerializeField] public Vector2 MouseDelta { get; private set; }
    public Vector2 MouseOnScreen => Camera.main.ScreenToViewportPoint(MousePosition);
    public Vector2 MouseOnWorld => Camera.main.ScreenToWorldPoint(MousePosition);
    [field: SerializeField] public Vector2 MoveDir { get; private set; }
    [field: SerializeField] public Vector2 DashDir { get; private set; }
    public bool Move => MoveDir != Vector2.zero;
    [field: SerializeField] public bool Attack { get; private set; }
    [field: SerializeField] public bool RangeAttack { get; private set; }
    [field: SerializeField] public bool Run { get; private set; }
    [field: SerializeField] public bool Dash { get; private set; }
    public bool Pause { get; private set; }
    public bool Cancel { get; private set; }
    public bool Recycle { get; private set; }
    public bool Equip { get; private set; }
    public bool LevelUp { get; private set; }
    public bool NextRound { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        controls = new Controls();
        controls.Player.Enable();
        controls.UI.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        controls.Player.Run.performed += OnRun;
        controls.Player.Run.canceled += OnRun;

        controls.Player.MouseMove.performed += MouseMove;
        controls.Player.MouseMove.canceled += MouseMove;

        controls.Player.MousePosition.performed += MousePos;
        controls.Player.MousePosition.canceled += MousePos;

        controls.Player.RangeAttack.performed += OnRangeAttack;
        controls.Player.RangeAttack.canceled += OnRangeAttack;
        
        controls.Player.Attack.performed += OnAttack;
        controls.Player.Attack.canceled += OnAttack;
    }


    public void EnablePlayerInput(bool enable)
    {
        if (enable) controls.Player.Enable();
        else controls.Player.Disable();
    }

    public void EnableUIInput(bool enable)
    {
        if (enable) controls.UI.Enable();
        else controls.UI.Disable();
    }

    private void Update()
    {
        Dash = controls.Player.Dash.triggered;
        Pause = controls.Player.Pause.triggered;
        Cancel = controls.UI.Cancel.triggered;
        Recycle = controls.UI.Recycle.triggered;
        Equip = controls.UI.Equip.triggered;
        LevelUp = controls.UI.LevelUp.triggered;
        NextRound = controls.UI.NextRound.triggered;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveDir = ctx.ReadValue<Vector2>();
        DashDir = ctx.performed ? ctx.ReadValue<Vector2>() : DashDir;
    }

    private void OnRun(InputAction.CallbackContext ctx)
    {
        Run = ctx.ReadValue<float>() != 0;
    }

    private void MousePos(InputAction.CallbackContext ctx)
    {
        MousePosition = ctx.ReadValue<Vector2>();
    }
    private void MouseMove(InputAction.CallbackContext ctx)
    {
        MouseDelta = ctx.ReadValue<Vector2>();
    }

    private void OnRangeAttack(InputAction.CallbackContext ctx)
    {
        RangeAttack = ctx.ReadValue<float>() != 0;
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        Attack = controls.Player.Attack.ReadValue<float>() != 0;
    }
}

