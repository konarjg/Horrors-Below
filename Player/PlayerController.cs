using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void Init(GameObject body)
    {
        Body = body;
        PhysicsEngine = Body.GetComponent<CharacterController>();
    }

    public void Tick(float speed)
    {
        if (PhysicsEngine.isGrounded && Velocity.y < 0f)
            Velocity.y = 0f;

        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
    private PlayerStats CurrentStats;
    [SerializeField]
    private MouseLook MouseLook;
    [SerializeField]
    private PlayerMovement Movement;
    [SerializeField]
    private bool IsSprinting = false;
    private bool SprintOnCooldown;


    private void HandleSprint()
    {
        if (CurrentStats.Stamina > 0f && !SprintOnCooldown)
            IsSprinting = Input.GetKey(KeyCode.LeftShift);
        else
        {
            SprintOnCooldown = true;
            IsSprinting = false;
        }

        if (SprintOnCooldown && CurrentStats.Stamina > Stats.StaminaLoss)
            SprintOnCooldown = false;
        

        if (IsSprinting)
            CurrentStats.Stamina -= Stats.StaminaLoss * Time.deltaTime;
        else
            CurrentStats.Stamina += Stats.StaminaRegenRate * Time.deltaTime;

        CurrentStats.Stamina = Mathf.Clamp(CurrentStats.Stamina, 0f, Stats.MaxStamina);
    }

    private void Start()
    {
        Movement.Init(gameObject);
        MouseLook.Init(transform, Camera.main.transform);

        CurrentStats.Stamina = Stats.MaxStamina;
    }

    private void Update()
    {
        Movement.Tick(IsSprinting ? Stats.SprintSpeed : Stats.WalkSpeed);
        MouseLook.LookRotation(transform, Camera.main.transform);

        HandleSprint();
    }
}
