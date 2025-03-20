using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public Transform throwPoint;
    public GameObject spinePrefab;
    private Animator anim;

    // Reference to the AmmoManager
    private AmmoManager ammoManager;

    // Time between shots
    public float timeBtwShots = 1;  
    private float timeOfLastShot;

    void Start()
    {
        // Get the AmmoManager component attached to the same GameObject or another GameObject
        ammoManager = FindFirstObjectByType<AmmoManager>();  // This assumes AmmoManager is in the scene

        // If AmmoManager is not found, log a warning
        if (ammoManager == null)
        {
            Debug.LogWarning("AmmoManager not found in the scene.");
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Call GetAmmo() to check ammo before shooting
            int currentAmmo = ammoManager.GetAmmo();

            // Only shoot if there is ammo available
            if (currentAmmo > 0 && Time.time - timeOfLastShot >= timeBtwShots)
            {
                // Decrease ammo (if you have a method for that in AmmoManager)
                ammoManager.TryUseAmmo(1);

                // Instantiate the spine prefab (or perform the attack)
                Instantiate(spinePrefab, throwPoint.position, throwPoint.rotation);
                
                //Play animation for throwing
                //TODO
                //anim.SetTrigger("Throw");
                
                // Set the time of last shot
                timeOfLastShot = Time.time;
                ammoManager.GetAmmo();
            }
            else
            {
                Debug.Log("Not enough ammo to throw.");
            }
        }
    }
}