using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

namespace SeedSnatcher.Movement
{
    public class SnatcherDive : SnatcherMovement
    {
        /** for creating the Bézier curve */
        private List<Vector3> controlPoints;
        /** the actual points along the curve */
        private List<Vector3> path;
        /** the position in the dive */
        [SerializeField] private int diveStep;
        /**
         * Whether a stage has just changed.
         * Can't use diveStep == 0 since we can't be sure
         * when the bird reaches diveStep 1.
         */
        private bool isNewStage = true;
        
        /**
         * Dives have three stages: start, bottom, and end.
         * The start stage creates the curve down to the bottom
         * position. The bottom stage creates the curve to the
         * end position (on top of handling the target). The end
         * stage switches back to the idle mode.
         */
        private int diveStage;

        [SerializeField] private List<Sprite> diveAnimation;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer renderer;
        
        /**
         * Computes appropriate control points for the
         * dive's Bézier curve.
         */
        private void SetupControlPoints()
        {
            var midpointTop = (startPosition + endPosition) / 2;
            midpointTop.y = startPosition.y;
            var midpointBottom = midpointTop;
            midpointBottom.y = endPosition.y;
            var midpointMidpoint = (midpointBottom + midpointTop) / 2;

            controlPoints = new List<Vector3>()
            {
                startPosition,
                midpointTop,
                midpointMidpoint,
                midpointBottom,
                endPosition
            };
        }
        
        /**
         * Generates a Bézier curve that the bird should fly along,
         * using `controlPoints` for De Casteljau's algorithm.
         */
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
        
        /**
         * Generates the curve back up from the bottom.
         * The end point will be the same Y position as
         * the original start point. Its X position will
         * be the distance from the original target position,
         * times two.
         */
        private void CalculateReverseDive()
        {
            var newEndPosition = startPosition;
            startPosition = endPosition;
            newEndPosition.x = 2 * endPosition.x - newEndPosition.x;
            endPosition = newEndPosition;
        }

        /**
         * Wrapper function for all the helper
         * functions to set up the curve.
         *
         * Kinda useless. Maybe I should just
         * make the actual functions do more.
         */
        private void SetupDive()
        {
            SetupControlPoints();
            GeneratePath();
            DetermineFacingDirection();
        }

        private void ExitDive()
        {
            GetSnatcherController().SetState(SnatcherState.Idle);
            animator.enabled = true;
        }

        public override void Init()
        {
            // reset values to defaults in case this was previously used
            diveStage = 0;
            diveStep = 0;
            controlPoints = null;
            path = null;
            animator.enabled = false;
        }

        public override void Loop()
        {
            // Cancel when the target disappears (i.e. seed picked up)
            if (diveStage < 1 && !GetSnatcherTargeting().HasTarget())
            {
                ExitDive();
            }
            
            // Setup for dive stages
            if (isNewStage)
            {
                isNewStage = false;
                switch (diveStage)
                {
                    case 0:
                        var thisPosition = transform.position;
                        var targetPosition = GetSnatcherTargeting().GetTargetPosition();
                        (startPosition, endPosition) = (thisPosition, targetPosition);
                        SetupDive();
                        Debug.DrawLine(startPosition, endPosition, Color.green, 90);
                        break;
                    case 1:
                        GetSnatcherTargeting().DestroyTarget();
                        CalculateReverseDive();
                        SetupDive();
                        break;
                    case 2:
                        ExitDive();
                        break;
                }
            }

            if (path == null) return;
            
            // Move to next point in curve
            var nextPosition = path[diveStep];
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
            if (HasReachedPosition(nextPosition))
            {
                diveStep++;
            }

            switch (diveStage)
            {
                case 0 when Mathf.Approximately(diveStep, 0):
                    renderer.sprite = diveAnimation[0];
                    break;
                case 0 when Mathf.Approximately(diveStep, path.Count * 0.2f):
                    renderer.sprite = diveAnimation[1];
                    break;
                case 0 when Mathf.Approximately(diveStep, path.Count * 0.35f):
                    renderer.sprite = diveAnimation[2];
                    break;
                case 0 when Mathf.Approximately(diveStep, path.Count * 0.5f):
                    renderer.sprite = diveAnimation[3];
                    break;
                case 0 when Mathf.Approximately(diveStep, path.Count * 0.65f):
                    renderer.sprite = diveAnimation[4];
                    break;
                case 0:
                {
                    if(Mathf.Approximately(diveStep, path.Count * 0.8f))
                    {
                        renderer.sprite = diveAnimation[5];
                    }

                    break;
                }
                case 1 when Mathf.Approximately(diveStep, path.Count * 0.2f):
                    renderer.sprite = diveAnimation[6];
                    break;
                case 1 when Mathf.Approximately(diveStep, path.Count * 0.5f):
                    renderer.sprite = diveAnimation[7];
                    break;
                case 1 when Mathf.Approximately(diveStep, path.Count * 0.8f):
                    renderer.sprite = diveAnimation[8];
                    break;
            }
            
            // Move to next stage when end of curve reached
            if (diveStep < path.Count) return;
            diveStep = 0;
            diveStage++;
            isNewStage = true;
        }
    }
}