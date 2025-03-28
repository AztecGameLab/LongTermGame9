using UnityEngine;

namespace SeedSnatcher.Behavior.Movement
{
    public class SnatcherIdle : SnatcherMovement
    {
        [SerializeField] private Vector3 patrolStartPosition;
        [SerializeField] private Vector3 patrolEndPosition;
        /**
         * Primarily used for tracking next patrol cycle
         * when the bird starts from a non-patrol state.
         */
        private bool reachedPatrolEnd;

        /**
         * Upon initialization, have the bird figure out the
         * nearest patrol point and move there.
         * 
         * This is also useful for restarting the idle patrol
         * after exiting a dive.
         */
        private void MoveToPatrolPoint()
        {
            var thisPosition = transform.position;
            var distanceToPatrolStart = Vector3.Distance(thisPosition, patrolStartPosition);
            var distanceToPatrolEnd = Vector3.Distance(thisPosition, patrolEndPosition);

            StartPosition = transform.position;
            if (distanceToPatrolStart < distanceToPatrolEnd)
            {
                EndPosition = patrolStartPosition;
                reachedPatrolEnd = false;
            }
            else
            {
                EndPosition = patrolEndPosition;
                reachedPatrolEnd = true;
            }

            DetermineFacingDirection();
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
        
        public override void Init()
        {
            MoveToPatrolPoint();
        }

        public override void Loop()
        {
            if (HasReachedEnd())
            {
                // assuming we've reached the patrol start, the
                // bird should return to the end, and vice versa
                if (!reachedPatrolEnd)
                {
                    (StartPosition, EndPosition) = (patrolEndPosition, patrolStartPosition);
                    reachedPatrolEnd = true;
                }
                else
                {
                    (StartPosition, EndPosition) = (patrolStartPosition, patrolEndPosition);
                    reachedPatrolEnd = false;
                }

                DetermineFacingDirection();
            }
            
            transform.position = Vector3.MoveTowards(transform.position, EndPosition, speed * Time.deltaTime);

            SearchForTarget();
        }
    }
}