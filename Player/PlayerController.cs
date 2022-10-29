using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [SerializeField]
    private Weapon CurrentWeapon;
    private bool IsShooting;
    private bool IsReloading;
    private Vector3 CameraAngles;

    private bool IsCrouching;

    private void Hallucinate()
    {

    }

    private IEnumerator Reload() 
    {
        IsReloading = true;

        yield return new WaitForSecondsRealtime(CurrentWeapon.Stats.ReloadTime);

        if (CurrentWeapon.Ammunition >= CurrentWeapon.Stats.CapSize)
        {
            CurrentWeapon.AmmunitionLoaded = CurrentWeapon.Stats.CapSize;
            CurrentWeapon.Ammunition -= CurrentWeapon.Stats.CapSize;
        }
        else
        {
            CurrentWeapon.AmmunitionLoaded = CurrentWeapon.Ammunition;
            CurrentWeapon.Ammunition = 0;
        }

        IsReloading = false;
    }

    private IEnumerator Shoot()
    {
        IsShooting = true;

        if (CurrentWeapon.Stats.IsAutomatic)
            yield return new WaitForSecondsRealtime(1f / CurrentWeapon.Stats.FireRate);
        else
            yield return null;

        var angles = new Vector3(0f, 0f, 0f);
        var knock = CurrentWeapon.Stats.Knockback;
        var recoil = CurrentWeapon.Stats.Recoil;

        angles.x -= knock;
        angles.y += Random.Range(-recoil, recoil) + Random.Range(-0.3f, 0.3f);

        CameraAngles = angles;

        var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, CurrentWeapon.Stats.Range))
        {
            if (hit.collider.tag == "Enemy")
                Debug.LogError("ENEMY HIT");
        }

        --CurrentWeapon.AmmunitionLoaded;
        //TODO shot effects and sound

        if (!CurrentWeapon.Stats.IsAutomatic)
        {
            var angles2 = Quaternion.Euler(MouseLook.m_CameraTargetRot.eulerAngles + angles);
            MouseLook.m_CameraTargetRot = Quaternion.Slerp(MouseLook.m_CameraTargetRot, angles2, 10 * Time.smoothDeltaTime);
        }

        IsShooting = false;
    }


    private void HandleShooting()
    {
        var input = CurrentWeapon.Stats.IsAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (input && CurrentWeapon.AmmunitionLoaded != 0 && !IsShooting)
            StartCoroutine(Shoot());

        if (!input && CurrentWeapon.Stats.IsAutomatic)
            CameraAngles = new Vector3(0f, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.R) && CurrentWeapon.AmmunitionLoaded == 0 && CurrentWeapon.Ammunition != 0 && !IsReloading)
            StartCoroutine(Reload());
    }

    private void HandleSprint()
    {
        if (CurrentStats.Stamina > 0f && !SprintOnCooldown && !IsCrouching)
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

    private void HandleSanity()
    {
        CurrentStats.Sanity -= Stats.SanityLoss * Time.deltaTime;

        if (CurrentStats.Sanity < 0.5f * Stats.MaxSanity)
            Hallucinate();

        CurrentStats.Sanity = Mathf.Clamp(CurrentStats.Sanity, 0f, Stats.MaxSanity);
    }

    private void HandleCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            IsCrouching = true;
        else
            IsCrouching = false;

        if (IsCrouching)
        {
            var position = transform.position;
            var controller = GetComponent<CharacterController>();

            controller.height = 0.5f;
            position.y = 0.25f;
            transform.position = position;

            IsSprinting = false;
        }
        else
        {
            var position = transform.position;
            var controller = GetComponent<CharacterController>();

            controller.height = 2f;
            position.y = 1f;
            transform.position = position;
        }

        //TODO decrease volume of walking sounds
    }

    private void OnApplicationQuit()
    {
        if (!Directory.Exists(Application.dataPath + "/Save"))
            Directory.CreateDirectory(Application.dataPath + "/Save");

        PlayerFile.Save(CurrentStats, Application.dataPath + "/Save/Player.ini");
    }

    private void Start()
    {
        Movement.Init(gameObject);
        MouseLook.Init(transform, Camera.main.transform);

        try
        {
            PlayerFile.Load(ref CurrentStats, Application.dataPath + "/Save/Player.ini");
        }
        catch (Exception)
        {
            CurrentStats.Stamina = Stats.MaxStamina;
            CurrentStats.Sanity = Stats.MaxSanity;
        }
    }

    private void LateUpdate()
    {
        if (IsShooting && CurrentWeapon.Stats.IsAutomatic)
        {
            var angles = Quaternion.Euler(MouseLook.m_CameraTargetRot.eulerAngles + CameraAngles);
            MouseLook.m_CameraTargetRot = Quaternion.Slerp(MouseLook.m_CameraTargetRot, angles, 10 * Time.smoothDeltaTime);
        }
    }

    private void Update()
    {
        if (IsSprinting)
            Movement.Tick(Stats.SprintSpeed);
        else if (IsCrouching)
            Movement.Tick(Stats.WalkSpeed / 2f);
        else
            Movement.Tick(Stats.WalkSpeed);

        MouseLook.LookRotation(transform, Camera.main.transform);

        HandleSprint();
        HandleSanity();
        HandleShooting();
        HandleCrouching();
    }
}
