using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 10;

    private int currentAmmo;

    private int CurrentAmmo
    {
        get => currentAmmo;
        set
        {
            currentAmmo = value;
            onAmmoChanged?.Invoke(value);
        }
    }

    public UnityEvent<int> onAmmoChanged;
    
    void Start()
    {
        CurrentAmmo = maxAmmo;
        Debug.Log("Starting ammo: " + CurrentAmmo);  // Logs the starting ammo
    }

    public bool TryUseAmmo(int amount)
    {
        if (CurrentAmmo >= amount)
        {
            CurrentAmmo -= amount;
            Debug.Log("Ammo used. Remaining ammo: " + CurrentAmmo);  // Logs remaining ammo after usage
            return true;
        }
        Debug.Log("Not enough ammo. Remaining ammo: " + CurrentAmmo);  // Logs if ammo is insufficient
        return false;
    }

    public void IncreaseAmmo(int amount)
    {
        CurrentAmmo += amount;
        if (CurrentAmmo > maxAmmo) CurrentAmmo = maxAmmo;
        Debug.Log("Ammo increased. Current ammo: " + CurrentAmmo);  // Logs the new ammo count
    }

    public int GetAmmo()
    {
        Debug.Log("Current ammo: " + CurrentAmmo);  // Logs ammo every time it's checked
        return CurrentAmmo;
    }
}