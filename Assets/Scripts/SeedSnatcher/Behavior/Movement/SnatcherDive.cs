using System.Collections.Generic;
using SeedSnatcher.Utils;
using UnityEngine;

namespace SeedSnatcher.Behavior.Movement
{
    internal class BezierPathing
    {
        private readonly List<Vector3> controlPoints;
        private readonly AnimationCurve animeCurve;
        public float Length { get; }

        public BezierPathing(List<Vector3> controlPoints, AnimationCurve animeCurve)
        {
            this.animeCurve = animeCurve;
            this.controlPoints = controlPoints;
            Length = CalculateLength();
        }
        
        public BezierPathing(Vector3 startPosition, Vector3 endPosition, AnimationCurve animeCurve)
        {
            this.animeCurve = animeCurve;
            controlPoints = SetupControlPoints(startPosition, endPosition);
            Length = CalculateLength();
        }
        
        /**
         * Computes appropriate control points for the
         * dive's Bézier curve.
         */
        private List<Vector3> SetupControlPoints(Vector3 startPosition, Vector3 endPosition)
        {
            var midpointTop = (startPosition + endPosition) / 2;
            midpointTop.y = startPosition.y;
            var midpointBottom = midpointTop;
            midpointBottom.y = endPosition.y;
            /*var midpointMidpoint = (midpointBottom + midpointTop) / 2;
            var startLow = new Vector3(startPosition.x, endPosition.y, 0.0f);
            var midpoint = (startPosition + endPosition) / 2;
            var midpointHigh = new Vector3(midpoint.x, startPosition.y, 0.0f);
            var midpointLow = new Vector3(midpoint.x, endPosition.y, 0.0f);*/
            
            return new List<Vector3>()
            {
                startPosition,
                midpointTop,
                midpointBottom,
                endPosition
            };
        }
        
        public Vector3 CalculateNextPosition(float normalizedTime)
        {
            var t = animeCurve.Evaluate(normalizedTime);
            return BezierCurve.CalculateBezierPoint(controlPoints, t);
        }
        
        private float CalculateLength(int precision = 100)
        {
            var length = 0f;
            var prev = controlPoints[0];
            for (var i = 0; i < precision; i++)
            {
                var percentage = i / (precision * 1.0f);
                var targetPosition = CalculateNextPosition(percentage);
                var distance = Vector3.Distance(prev, targetPosition);
                length += distance;
                Debug.DrawLine(targetPosition, prev, Color.red, 90);
                prev = targetPosition;
            }

            return length;
        }
    }
    
    internal enum DiveStage {
        Dive,
        Recovery,
        Glide,
        Complete
    }
    
    public class SnatcherDive : SnatcherMovement
    {
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
        private DiveStage diveStage;
        private BezierPathing bezierPathing;

        [SerializeField] private AnimationCurve diveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private AnimationCurve recoveryCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float recoveryDistance = 10.0f;
        private float diveDuration = 5f;
        private float elapsedTime;
        private Vector3 targetPosition;
        private AnimationCurve animeCurve;

        private void ExitDive()
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            SnatcherController.SetState(SnatcherState.Idle);
        }

        public override void Init()
        {
            StopAnimation();
            SetSprite();
            // reset values to defaults in case this was previously used
            diveStage = 0;
            isNewStage = true;
        }
        
        public override void Loop()
        {
            // Cancel when the target disappears (i.e. seed picked up)
            if ((int)diveStage < 1 && !SnatcherTargeting.HasTarget())
            {
                ExitDive();
                return;
            }

            if (isNewStage)
            {
                switch (diveStage)
                {
                    case DiveStage.Dive:
                        SnatcherSfx.PlayBeginDive();
                        var thisPosition = transform.position;
                        targetPosition = SnatcherTargeting.GetTargetPosition();
                        (StartPosition, EndPosition) = (thisPosition, targetPosition);
                        animeCurve = diveCurve;
                        bezierPathing = new BezierPathing(StartPosition, EndPosition, animeCurve);
                        break;
                    case DiveStage.Recovery:
                        SnatcherTargeting.DestroyTarget();
                        var originalHeight = StartPosition.y;
                        StartPosition = EndPosition;
                        var recoveryDirection = IsFacingLeft() ? Vector3.left : Vector3.right;
                        var recoveryXComp = recoveryDirection * recoveryDistance;
                        EndPosition = StartPosition + recoveryXComp;
                        EndPosition.y = originalHeight;
                        animeCurve = recoveryCurve;
                        bezierPathing = new BezierPathing(StartPosition, EndPosition, animeCurve);
                        break;
                    case DiveStage.Glide:
                        /*StartPosition = EndPosition;
                        EndPosition.x *= 1.1f;
                        var ctrlPts = new List<Vector3>()
                        {
                            StartPosition,
                            EndPosition
                        };
                        animeCurve = AnimationCurve.Linear(0, 0, 1, 1);
                        bezierPathing = new BezierPathing(ctrlPts, animeCurve);
                        break;*/
                    case DiveStage.Complete:
                    default:
                        ExitDive();
                        return;
                }
                
                DetermineFacingDirection();
                diveDuration = bezierPathing.Length / speed;
                isNewStage = false;
            }
            
            elapsedTime += Time.deltaTime;
            var normalizedTime = elapsedTime / diveDuration;
            var pos = bezierPathing.CalculateNextPosition(normalizedTime);
            
            transform.position = pos;
            
            var lookAheadTime = Mathf.Max(elapsedTime + 0.3f, 1f);
            var lookAheadPos = bezierPathing.CalculateNextPosition(lookAheadTime);
            
            // Orient bird to next point
            Vector2 target = lookAheadPos - pos;
            float angle = Vector2.SignedAngle(IsFacingLeft() ? Vector2.left : Vector2.right, target);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

            if (elapsedTime >= diveDuration || (diveStage == DiveStage.Recovery && (lookAheadPos - pos).magnitude < 0.5f))
            {
                diveStage += 1;
                isNewStage = true;
                elapsedTime = 0;
            }
        }
    }
}