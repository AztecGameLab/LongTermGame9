using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 10;
    private int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
        Debug.Log("Starting ammo: " + currentAmmo);  // Logs the starting ammo
    }

    public bool TryUseAmmo(int amount)
    {
        if (currentAmmo >= amount)
        {
            currentAmmo -= amount;
            Debug.Log("Ammo used. Remaining ammo: " + currentAmmo);  // Logs remaining ammo after usage
            return true;
        }
        Debug.Log("Not enough ammo. Remaining ammo: " + currentAmmo);  // Logs if ammo is insufficient
        return false;
    }

    public void IncreaseAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        Debug.Log("Ammo increased. Current ammo: " + currentAmmo);  // Logs the new ammo count
    }

    public int GetAmmo()
    {
        Debug.Log("Current ammo: " + currentAmmo);  // Logs ammo every time it's checked
        return currentAmmo;
    }
}