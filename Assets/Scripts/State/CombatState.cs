using UnityEngine;

public class CombatState : State
{
    float gravityValue;
    Vector3 currentVelocity;
    bool grounded;
    //bool sheathWeapon;
    float playerSpeed;
    bool attack;
    bool specialA;
    bool specialB;
    bool specialC;
    bool specialD;
    Vector3 cVelocity;

    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        //sheathWeapon = false;
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;
        attack = false;
        specialA = false;
        specialB = false;
        specialC = false;
        specialD = false;

        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        //if (drawWeaponAction.triggered)
        //{
        //    sheathWeapon = true;
        //}

        if (attackAction.triggered)
        {
            attack = true;
        }
        if (specialAAction.triggered)
        {
            specialA = true; // Impostazione della variabile quando l'azione è attivata
        }
        if (specialBAction.triggered)
        {
            specialB = true; // Impostazione della variabile quando l'azione è attivata
        }
        if (specialCAction.triggered)
        {
            specialC = true; // Impostazione della variabile quando l'azione è attivata
        }
        if (specialDAction.triggered)
        {
            specialD = true; // Impostazione della variabile quando l'azione è attivata
        }

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        //if (sheathWeapon)
        //{
        //    character.animator.SetTrigger("sheathWeapon");
        //    stateMachine.ChangeState(character.standing);
        //}

        if (attack)
        {
            character.animator.SetTrigger("attack");
            stateMachine.ChangeState(character.attacking);
        }
        if (specialA)
        {
           
            character.GetComponent<ShootingBarController>().HandleSpecialA();

            specialA = false; // Reimposta la variabile
        }
        if (specialB)
        {
       
            character.GetComponent<ShootingBarController>().HandleSpecialB();

            specialB = false; // Reimposta la variabile
        }
        if (specialC)
        {
           
            character.GetComponent<ShootingBarController>().HandleSpecialC();

            specialC = false; // Reimposta la variabile
        }
        if (specialD)
        {
 
            character.GetComponent<ShootingBarController>().HandleSpecialD();

            specialD = false; // Reimposta la variabile
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
        character.animator.SetTrigger("move");
    }
}
