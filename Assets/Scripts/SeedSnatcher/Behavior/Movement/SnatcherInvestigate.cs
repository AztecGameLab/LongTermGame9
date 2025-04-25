using UnityEngine;

namespace SeedSnatcher.Behavior.Movement
{
    public class SnatcherInvestigate : SnatcherMovement
    {
        [SerializeField] private float maximumDiveRange;
        [SerializeField] private float minimumDiveHeight;

        private void PathToTarget()
        {
            var targetPosition = EndPosition;
            if (targetPosition.y - StartPosition.y < minimumDiveHeight)
            {
                targetPosition.y += minimumDiveHeight;
            }
            else
            {
                targetPosition.y = StartPosition.y;
            }
            DetermineFacingDirection();
            transform.position = Vector3.MoveTowards(StartPosition, targetPosition, speed * Time.deltaTime);
        }
        
        public override void Init()
        {
            SetSprite();
        }

        public override void Loop()
        {
            StartPosition = transform.position;
            if (GetSnatcherTargeting().HasTarget())
            {
                EndPosition = GetSnatcherTargeting().GetTargetPosition();
                var isWithinRange = Vector3.Distance(StartPosition, EndPosition) < maximumDiveRange;
                var isAtMinHeight = (StartPosition.y - (EndPosition.y + minimumDiveHeight)) <= positionTolerance;
                if (isWithinRange && isAtMinHeight)
                {
                    GetSnatcherController().SetState(SnatcherState.Diving);
                }
                else
                {
                    PathToTarget();
                }
            }
            else
            {
                GetSnatcherController().SetState(SnatcherState.Idle);
            }
        }
    }
}