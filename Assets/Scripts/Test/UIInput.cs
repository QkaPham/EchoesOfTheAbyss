using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class UIInput : MonoBehaviour
{
    public Controls controls;


    public bool Pause { get; private set; }
    public bool Cancel { get; private set; }
    public bool Recycle { get; private set; }
    public bool Equip { get; private set; }
    public bool LevelUp { get; private set; }
    public bool NextRound { get; private set; }
    public bool Buy1 { get; private set; }
    public bool Buy2 { get; private set; }
    public bool Buy3 { get; private set; }
    public bool Buy4 { get; private set; }
    public bool Roll { get; private set; }


    public PlayerInput playerInput;

    public UpgradePanel upgradePanel;
    protected void Awake()
    {
        controls = new Controls();
        controls.UI.Enable();

        controls.UI.Cancel.started += OnCancel;
        controls.UI.Buy1.started += OnBuy1;
    }

    private void OnBuy1(InputAction.CallbackContext obj)
    {
        Debug.Log("buy");
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Pause)
        {
            Debug.Log("Pause");
        }
    }

    //void OnEnable()
    //{
    //    playerInput.onActionTriggered += MyEventFunction;
    //}
    //void OnDisable()
    //{
    //    playerInput.onActionTriggered -= MyEventFunction;
    //}
    //void MyEventFunction(InputAction.CallbackContext value)
    //{
    //    //Debug.Log(value.action.name + (" was triggered"));
    //}

    public void Enable()
    {
        controls.UI.Enable();
    }

    public void Disable()
    {
        controls.UI.Disable();
    }



    //public void AddListener(Action<InputAction.CallbackContext> action)
    //{
    //    controls.UI.Cancel.started += action;
    //}
}
