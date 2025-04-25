using UnityEngine;

namespace SeedSnatcher.Behavior.Movement
{
    public abstract class SnatcherMovement : MonoBehaviour
    {
        // Snatcher Movement vars
        protected Vector3 StartPosition;
        protected Vector3 EndPosition;
        [SerializeField] protected float speed = 1.0f;
        // make sure the endPosition is way larger than this
        [SerializeField] protected float positionTolerance = 0.5f;
        
        private SnatcherTargeting snatcherTargeting;
        private SnatcherController snatcherController;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        [SerializeField] private Sprite sprite;
        
        private void Awake()
        {
            snatcherTargeting = GetComponentInParent<SnatcherTargeting>();
            snatcherController = GetComponentInParent<SnatcherController>();
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
            animator = GetComponentInParent<Animator>();
        }

        protected void StopAnimation()
        {
            animator.enabled = false;
        }
        
        protected void StartAnimation()
        {
            animator.enabled = true;
        }
        
        protected void SetSprite()
        {
            spriteRenderer.sprite = sprite;
        }

        protected SnatcherTargeting GetSnatcherTargeting()
        {
            return snatcherTargeting;
        }

        protected SnatcherController GetSnatcherController()
        {
            return snatcherController;
        }
        
        // for flipping the bird (not *that* kind) when it's looping back
        private void FlipSprite()
        {
            // I'm using two separate objects to represent the bird
            // and I don't want to bother manually flipping both sprites.
            // gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            var localScale = transform.localScale;
            var flippedX = -localScale.x;
            transform.localScale = new Vector3(flippedX, localScale.y, localScale.z);
        }
        
        protected bool IsFacingLeft()
        {
            var localScale = transform.localScale;
            return localScale.x < 0;
        }
        
        protected void DetermineFacingDirection()
        {
            var thisPosition = transform.position;
            
            // important that we don't bother flipping
            // if its already in the correct facing
            if (IsFacingLeft())
            {
                if (thisPosition.x < EndPosition.x)
                {
                    FlipSprite();
                }
            }
            else
            {
                if (thisPosition.x > EndPosition.x)
                {
                    FlipSprite();
                }
            }
        }

        protected bool HasReachedPosition(Vector3 position)
        {
            var currentPosition = transform.position;
            return Vector3.Distance(currentPosition, position) < positionTolerance;
        }
        
        protected bool HasReachedEnd()
        {
            return HasReachedPosition(EndPosition);
        }
        
        public abstract void Init();
        public abstract void Loop();
        
    }
}