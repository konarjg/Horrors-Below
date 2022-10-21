using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerStats
{
    public float Blood;
    public float Sanity;
    public float Stamina;
}

[Serializable]
public class PlayerMovement
{
    private GameObject Body;
    private CharacterController PhysicsEngine;
    private Vector3 Velocity;

    private float GravityValue = -9.81f;

    private InputActionReference Input;

    public void Init(GameObject body, InputActionReference input)
    {
        Body = body;
        Input = input;
        PhysicsEngine = Body.GetComponent<CharacterController>();
    }

    public void Tick(float speed)
    {
        if (PhysicsEngine.isGrounded && Velocity.y < 0f)
            Velocity.y = 0f;

        var input = Input.action.ReadValue<Vector2>();
        var move = Body.transform.forward * input.y + Body.transform.right * input.x;
        move.y = 0f;
        PhysicsEngine.Move(move * speed * Time.deltaTime);

        Velocity.y += GravityValue * Time.deltaTime;
        PhysicsEngine.Move(Velocity * Time.deltaTime);
    }
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player Stats;
    [SerializeField]
    private InputActionReference MovementInput;
    [SerializeField]
    private MouseLook MouseLook;
    [SerializeField]
    private PlayerMovement Movement;
    private bool IsSprinting = false;

    private void OnEnable()
    {
        MovementInput.action.Enable();
    }

    private void OnDisable()
    {
        MovementInput.action.Disable();
    }

    private void Start()
    {
        Movement.Init(gameObject, MovementInput);
        MouseLook.Init(transform, Camera.main.transform);
    }

    private void Update()
    {
        Movement.Tick(IsSprinting ? Stats.SprintSpeed : Stats.WalkSpeed);
        MouseLook.LookRotation(transform, Camera.main.transform);
    }
}
