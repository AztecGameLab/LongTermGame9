using UnityEngine;

namespace SeedSnatcher.Movement
{
    public class SnatcherIdle : SnatcherMovement
    {
        [SerializeField] private Vector3 patrolStartPosition;
        [SerializeField] private Vector3 patrolEndPosition;
        private bool isReturningToStart;
        private bool firstInit = true;
        
        private bool HasReachedTarget()
        {
            var currentPosition = transform.position;
            return Vector3.Distance(currentPosition, endPosition) < positionTolerance;
        }

        private void SwapPositions()
        {
            (startPosition, endPosition) = (endPosition, startPosition);
        }
        
        public override void Init()
        {
            if (firstInit)
            {
                patrolStartPosition = patrolStartPosition != Vector3.zero ? patrolStartPosition : transform.position;
                patrolEndPosition = patrolEndPosition != Vector3.zero ? patrolEndPosition : transform.position;
                firstInit = false;
            }
            
            isReturningToStart = true;
            Init(transform.position, patrolStartPosition);
        }

        public void Init(Vector3 startPos, Vector3 endPos)
        {
            if (endPos.x > startPos.x && IsFacingLeft())
            {
                FlipSprite();
            }

            startPosition = startPos;
            endPosition = endPos;
        }

        private void SearchForTarget()
        {
            var snatcherTargeting = GetSnatcherTargeting();
            snatcherTargeting.FindTarget();
            if (snatcherTargeting.HasTarget())
            {
                GetSnatcherController().SetState(SnatcherState.Diving);
            }
        }

        public override void Loop()
        {
            if (HasReachedTarget())
            {
                if (isReturningToStart)
                {
                    isReturningToStart = false;
                    startPosition = patrolStartPosition;
                    endPosition = patrolEndPosition;
                }
                SwapPositions();
                FlipSprite();
            }

            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

            SearchForTarget();
        }
    }
}