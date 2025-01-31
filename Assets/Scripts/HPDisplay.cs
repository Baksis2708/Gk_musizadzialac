using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPDisplay : MonoBehaviour
{
    public HealthSystem healthSystem; 
    public Image[] hearts; 
    public TextMeshProUGUI hpText; 

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (hearts.Length > 0)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].enabled = i < healthSystem.currentHP;
            }
        }

        if (hpText != null)
        {
            hpText.text = $"HP: {healthSystem.currentHP}";
        }
    }

    void Update()
    {
        UpdateUI(); 
    }
}
