using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BloodEffectController : MonoBehaviour
{
    [Header("Ustawienia")]
    public PostProcessVolume postProcessVolume;
    public HealthSystem playerHealth;
    public float maxSaturation = -100f;

    private ColorGrading colorGrading;

    void Start()
    {

        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    void Update()
    {
        if (colorGrading != null && playerHealth != null)
        {

            float healthRatio = (float)playerHealth.currentHP / playerHealth.maxHP;
            colorGrading.saturation.value = Mathf.Lerp(maxSaturation, 0f, healthRatio);
        }
    }
}
