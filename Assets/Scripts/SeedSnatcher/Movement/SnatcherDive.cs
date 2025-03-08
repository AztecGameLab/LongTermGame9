using System.Collections.Generic;
using UnityEngine;

namespace SeedSnatcher.Movement
{
    public class SnatcherDive : SnatcherMovement
    {
        // Dive vars
        [SerializeField] private int diveStage;
        private List<Vector3> controlPoints;
        private List<Vector3> path;
        [SerializeField] private int diveCount;

        // creates the curve which the bird should dive
        // more control points may increase the dive steepness
        private void SetupDiveCurve()
        {
            var midpointTop = (startPosition + endPosition) / 2;
            midpointTop.y = startPosition.y;
            var midpointBottom = midpointTop;
            midpointBottom.y = endPosition.y;
            var midpointMidpoint = (midpointBottom + midpointTop) / 2;
            //var midpointTopMidpoint = (midpointMidpoint + midpointTop) / 2;
            //var midpointBottomMidpoint = (midpointMidpoint + midpointBottom) / 2;

            controlPoints = new List<Vector3>()
            {
                startPosition,
                midpointTop,
                //midpointTopMidpoint,
                midpointMidpoint,
                //midpointBottomMidpoint,
                midpointBottom,
                endPosition
            };
        }

        private void GeneratePath(int precision = 100)
        {
            path = new List<Vector3>();
            for (var i = 0; i < precision; i++)
            {
                var percentage = i / (precision * 1.0f);
                var targetPosition = BezierCurve.CalculateBezierPoint(controlPoints, percentage);
                path.Add(targetPosition);
                if (path.Count > 1)
                {
                    Debug.DrawLine(path[i], path[i - 1], Color.red, 90);
                }
            }
        }

        private void ReverseDiveCurve()
        {
            var originalStart = startPosition;
            startPosition = endPosition;
            originalStart.x = 2 * endPosition.x - originalStart.x;
            endPosition = originalStart;
            SetupDiveCurve();
        }

        private void Dive()
        {
            var targetPosition = path[diveStage];
            var currentPosition = transform.position;
            if (Vector3.Distance(currentPosition, targetPosition) < positionTolerance) ++diveStage;
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

            if (diveStage < path.Count - 1)
            {
                return;
            }

            ++diveCount;
            diveStage = 0;
            ReverseDiveCurve();
            GeneratePath();
            Debug.DrawLine(startPosition, endPosition, Color.green, 90);
        }

        private void ReturnToIdleState()
        {
            GetSnatcherController().SetState(SnatcherState.Idle);
        }

        public override void Init()
        {
            Init(transform.position, GetSnatcherTargeting().GetTargetPosition());
        }

        public void Init(Vector3 startPos, Vector3 endPos)
        {
            if (endPos.x > startPos.x && IsFacingLeft())
            {
                FlipSprite();
            }

            diveCount = 0;
            startPosition = startPos;
            endPosition = endPos;
            SetupDiveCurve();
            GeneratePath();
            Debug.DrawLine(startPosition, endPosition, Color.green, 90);
        }

        public override void Loop()
        {
            // return to idle when initial dive and dive back up are complete
            if (diveCount > 1)
            {
                GetSnatcherTargeting().RemoveTarget(); // ideally, the target should be nullified at the bottom of the dive instead of the end
                ReturnToIdleState();
                return;
            }

            // stop dive prematurely when the target disappears
            if (diveCount <= 1 && !GetSnatcherTargeting().HasTarget())
            {
                ReturnToIdleState();
                return;
            }

            Dive();
        }
    }
}