using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : State
{
    float gravityValue;
    Vector3 currentVelocity;
    bool grounded;
    float playerSpeed;
    Vector3 cVelocity;

    public AttackState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;

        // Disabilita tutti gli input del character controller
        DisableCharacterInputs();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // Ignora tutti gli input del personaggio
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Mantiene la gestione della gravità
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        // Riabilita gli input del character controller
        EnableCharacterInputs();
    }

    private void DisableCharacterInputs()
    {
        // Disabilita qui tutti i componenti o script del character che gestiscono gli input
        character.GetComponent<PlayerInput>().SwitchCurrentActionMap("Minigame");
        // Disabilitare altri script correlati agli input del personaggio se necessario
    }

    private void EnableCharacterInputs()
    {
        // Riabilita qui tutti i componenti o script del character che gestiscono gli input
        character.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        // Riabilitare altri script correlati agli input del personaggio se necessario
    }
}
