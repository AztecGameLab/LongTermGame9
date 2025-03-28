using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    private static readonly int ThrowSpine = Animator.StringToHash("ThrowSpine");

    [Header("Throwing")]
    public Transform throwPoint;
    public GameObject spinePrefab;

    [Header("Ammo")]
    public float timeBtwShots = 1f;
    private float timeOfLastShot;
    private AmmoManager ammoManager;
    
    [Header("Animation")]
    [SerializeField] private Animator anim;

    private PlayerMovementControl movementControl;
    private PlayerFlip playerFlip;

    void Start()
    {
        ammoManager = FindFirstObjectByType<AmmoManager>();
        if (ammoManager == null)
        {
            Debug.LogWarning("AmmoManager not found in the scene.");
        }

        movementControl = GetComponent<PlayerMovementControl>();
        if (movementControl == null)
        {
            Debug.LogWarning("PlayerMovementControl not found on the GameObject.");
        }

        playerFlip = GetComponent<PlayerFlip>();
        if (playerFlip == null)
        {
            Debug.LogWarning("PlayerFlip not found on the GameObject.");
        }
    }

    void Update()
    {
        if (playerFlip != null)
        {
            float direction = playerFlip.ForwardVector2.x;

            // Offset the throw point in front of the player
            throwPoint.localPosition = new Vector3(
                Mathf.Abs(throwPoint.localPosition.x) * Mathf.Sign(direction),
                throwPoint.localPosition.y,
                throwPoint.localPosition.z
            );

            // Flip throw point's rotation so the projectile launches the right way
            throwPoint.localEulerAngles = direction > 0 ? Vector3.zero : new Vector3(0, 180, 0);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int currentAmmo = ammoManager.GetAmmo();

            if (currentAmmo > 0 && (timeOfLastShot == 0.0 || Time.time - timeOfLastShot >= timeBtwShots))
            {
                ammoManager.TryUseAmmo(1);

                GameObject spine = Instantiate(spinePrefab, throwPoint.position, throwPoint.rotation);
                
                if (anim != null)
                {
                    anim.SetTrigger(ThrowSpine);
                }

                int direction = playerFlip != null && playerFlip.ForwardVector2.x < 0 ? -1 : 1;

                Debug.Log("Throw direction: " + direction);

                // Optionally flip the projectile visually (sprite-wise)
                if (direction == -1)
                {
                    spine.transform.localScale = new Vector3(
                        -spine.transform.localScale.x,
                        spine.transform.localScale.y,
                        spine.transform.localScale.z
                    );
                }

                // Set direction on spine
                Spine spineScript = spine.GetComponent<Spine>();
                if (spineScript != null)
                {
                    spineScript.SetDirection(direction);
                }

                // Stop player movement if grounded
                if (movementControl != null)
                {
                    Debug.Log("IsGrounded: " + movementControl.IsGrounded());

                    if (movementControl.IsGrounded())
                    {
                        movementControl.AllowMovement = false;
                        Invoke(nameof(EnableMovement), 0.3f);
                    }
                }

                timeOfLastShot = Time.time;
            }
            else
            {
                Debug.Log("Not enough ammo to throw.");
            }
        }
    }

    private void EnableMovement()
    {
        if (movementControl != null)
        {
            movementControl.AllowMovement = true;
        }
    }
}
