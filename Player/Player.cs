using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Horrors Below/Player/New Player Stats", fileName = "New Player Stats")]
public class Player : ScriptableObject
{
    public float MaxBlood;
    public float MaxSanity;
    public float MaxStamina;

    [Space()]
    public float WalkSpeed;
    public float SprintSpeed;

    [Space()]
    public float StaminaLoss;
    public float StaminaRegenRate;
    public float SanityLoss;
}
