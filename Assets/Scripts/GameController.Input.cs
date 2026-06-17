using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class GameController 
{
    private bool isEsc;
    //Перенести в настройки
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;


    private void Update()
    {
        if (player == null)
            return;

        HandleMovementInput();
        HandleActionInput();
        HandleMouseMovementInput();
    }

    private void HandleMouseMovementInput()
    {
        if (player == null) return;
        Vec2 mouseDelta = new Vec2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        player.MoveCamera(mouseDelta);
    }

    private void HandleMovementInput()
    {
        bool crouch = Input.GetKey(crouchKey);
        bool run = Input.GetKey(runKey);

        player.ApplyCrouch(crouch);
        player.ApplyRun(run);
    }

    private void HandleActionInput()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            player.MakeJump();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            player.ReloadWeapon();
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.DoShot();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEsc = !isEsc;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = isEsc;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        Vec2 input = new Vec2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        player.Move(input);
    }
    
}
