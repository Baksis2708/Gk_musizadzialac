using UnityEngine;

public class BloodySpikes : MonoBehaviour
{
    [SerializeField] private Renderer spikesRenderer;
    [Range(0, 1)] public float maxBloodIntensity = 0.8f;

    private Material materialInstance;
    private static readonly int BloodIntensity = Shader.PropertyToID("_BloodIntensity");

    void Start()
    {
        // Tworzymy instancjê materia³u
        materialInstance = new Material(spikesRenderer.material);
        spikesRenderer.material = materialInstance;
    }

    public void ApplyBloodEffect()
    {
        float current = materialInstance.GetFloat(BloodIntensity);
        float newValue = Mathf.Min(current + 0.5f, maxBloodIntensity);
        materialInstance.SetFloat(BloodIntensity, newValue);
    }
}