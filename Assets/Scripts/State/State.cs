using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    public Character character;
    public StateMachine stateMachine;

    protected Vector3 gravityVelocity;
    protected Vector3 velocity;
    protected Vector2 input;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction crouchAction;
    public InputAction sprintAction;
    //public InputAction drawWeaponAction;
    public InputAction attackAction;
    public InputAction specialAAction;
    public InputAction specialBAction;
    public InputAction specialCAction;
    public InputAction specialDAction;

    public State(Character _character, StateMachine _stateMachine)
	{
        character = _character;
        stateMachine = _stateMachine;

        moveAction = character.playerInput.actions["Move"];
        lookAction = character.playerInput.actions["Look"];
        jumpAction = character.playerInput.actions["Jump"];
        crouchAction = character.playerInput.actions["Crouch"];
        sprintAction = character.playerInput.actions["Sprint"];
        //drawWeaponAction = character.playerInput.actions["DrawWeapon"];
        attackAction = character.playerInput.actions["Attack"];
        specialAAction = character.playerInput.actions["SpecialA"];
        specialBAction = character.playerInput.actions["SpecialB"];
        specialCAction = character.playerInput.actions["SpecialC"];
        specialDAction = character.playerInput.actions["SpecialD"];

    }

    public virtual void Enter()
    {
 
    }

    public virtual void HandleInput()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Exit()
    {
    }
}

