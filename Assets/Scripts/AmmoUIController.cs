using UnityEngine;

public class AmmoUIController : MonoBehaviour
{
    public GameObject ammoUI; // Obrazek amunicji w UI
    public PlayerController player; // Odniesienie do gracza

    void Start()
    {
        if (ammoUI != null)
            ammoUI.SetActive(false); // Na start ukrywamy ikonê amunicji
    }

    void Update()
    {
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoUI != null && player != null)
        {
            ammoUI.SetActive(player.currentAmmo != 0); // Pokazuje ikonê, jeœli ammo == 1, inaczej ukrywa
        }
    }
}
