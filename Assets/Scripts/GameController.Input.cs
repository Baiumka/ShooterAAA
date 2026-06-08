using UnityEngine;
using static Unity.Collections.Unicode;

public partial class GameController 
{
    //Перенести в настройки
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    private void Update()
    {
        if (player == null)
            return;

        HandleMovementInput();
        HandleActionInput();
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
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        player.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
