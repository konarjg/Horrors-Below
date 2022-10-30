using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    [SerializeField]
    private Material LightOn;
    [SerializeField]
    private Material LightOff;
    [SerializeField]
    private Renderer Renderer;

    [SerializeField]
    private bool BlinkReady;
    [SerializeField]
    private bool IsLightOn;
    [SerializeField]
    private float TimeToBlink;

    private void Start()
    {
        TimeToBlink = Random.Range(0.1f, 0.25f);
        IsLightOn = false;
        BlinkReady = false;
    }

    private void Update()
    {
        if (BlinkReady)
        {
            var materials = Renderer.materials;

            if (IsLightOn)
                materials[1] = LightOff;
            else
                materials[1] = LightOn;

            Renderer.materials = materials;

            TimeToBlink = Random.Range(0.4f, 0.6f);
            IsLightOn = !IsLightOn;
            BlinkReady = false;
        }

        if (TimeToBlink > 0f)
            TimeToBlink -= Time.deltaTime;
        else
        {
            TimeToBlink = 0f;
            BlinkReady = true;
        }
    }
}
