using UnityEngine;
using UnityEngine.Events;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;

    public UnityEvent<int> OnAmmoChanged;

    private void Awake()
    {
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo); // Initialize UI
    }

    public bool TryUseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            OnAmmoChanged?.Invoke(currentAmmo);
            return true;
        }
        return false;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}

