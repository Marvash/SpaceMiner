using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Pathfinding;

public class NautolanScoutMovementBT : BehaviourTree.Tree
{
    [SerializeField]
    public GameObject Target;

    [SerializeField]
    public LaserGun SourceLaserGun;

    [SerializeField]
    private float PathUpdateInterval;

    [SerializeField]
    private float SlowDownRange;

    [SerializeField]
    private float StopRange;

    [SerializeField]
    private float AccelerationFactor;

    [SerializeField]
    private float MaxSpeed;

    [SerializeField]
    private float WaypointReachDistance;

    private Rigidbody2D _targetRb2d;
    private Rigidbody2D _sourceRb2d;

    private Seeker _seeker;

    protected override Node SetupTree()
    {
        ChaseTargetToRangeSmoothNode chaseTargetToRangeSmooth = new ChaseTargetToRangeSmoothNode(gameObject);
        ComputeTargetPredictedImpactPosition computeTargetPredictedImpactPosition = new ComputeTargetPredictedImpactPosition(gameObject);
        HasLOSOnTargetPositionNode hasLOSOnTargetPositionNode = new HasLOSOnTargetPositionNode(gameObject);
        hasLOSOnTargetPositionNode.TargetPositionBBVarName = computeTargetPredictedImpactPosition.OutputPositionBBVarName;
        ChaseTargetNode chaseTarget = new ChaseTargetNode(gameObject);
        SequenceNode chaseSequence = new SequenceNode(new List<Node>()
        {
            computeTargetPredictedImpactPosition,
            hasLOSOnTargetPositionNode,
            chaseTargetToRangeSmooth
        });
        SelectorNode chaseSelector = new SelectorNode(new List<Node>() { 
            chaseSequence,
            chaseTarget
        });

        AlwaysSucceedNode alwaysSucceedChaseNode = new AlwaysSucceedNode(chaseSelector);

        HasLOSOnTargetPositionNode hasLOSOnImpactPoint = new HasLOSOnTargetPositionNode(gameObject);
        hasLOSOnImpactPoint.TargetPositionBBVarName = computeTargetPredictedImpactPosition.OutputPositionBBVarName;
        FacePositionSmoothNode faceImpactSmoothNode = new FacePositionSmoothNode(gameObject);
        faceImpactSmoothNode.PositionToFaceBBVarName = computeTargetPredictedImpactPosition.OutputPositionBBVarName;

        SequenceNode impactPointLookSequence = new SequenceNode(new List<Node>() { 
            hasLOSOnImpactPoint,
            faceImpactSmoothNode
        });

        FacePositionSmoothNode faceVelocitySmoothNode = new FacePositionSmoothNode(gameObject);

        SelectorNode lookSelector = new SelectorNode(new List<Node>() {
            impactPointLookSequence,
            faceVelocitySmoothNode
        });

        SequenceNode rootSequence = new SequenceNode(new List<Node> {
            alwaysSucceedChaseNode,
            lookSelector
        });

        return rootSequence;
    }

    private void FixedUpdate()
    {
        tick(Time.fixedDeltaTime);
    }

    protected override void UpdateBBVariables()
    {
        _root.SetData("targetPosition", (Vector2)Target.transform.position);
        _root.SetData("targetVelocity", _targetRb2d.velocity);
        _root.SetData("projectileSpeed", SourceLaserGun.LaserSpeed);
        _root.SetData("positionToFace", _sourceRb2d.worldCenterOfMass + _sourceRb2d.velocity);
    }

    protected override void InitTree()
    {
        _seeker = GetComponent<Seeker>();
        _targetRb2d = Target.GetComponent<Rigidbody2D>();
        _sourceRb2d = GetComponent<Rigidbody2D>();
        _root.SetData("slowDownRange", SlowDownRange);
        _root.SetData("stopRange", StopRange);
        _root.SetData("accelerationFactor", AccelerationFactor);
        _root.SetData("chaseMaxSpeed", MaxSpeed);
        _root.SetData("waypointReachDistance", WaypointReachDistance);
        InvokeRepeating("RequestPath", 0.0f, PathUpdateInterval);
    }

    void RequestPath()
    {
        _seeker.StartPath(transform.position, Target.transform.position, OnPathRequestComplete);
    }

    void OnPathRequestComplete(Path newPath)
    {
        if (!newPath.error)
        {
            _root.SetData("chasePath", newPath);
        }
    }
    public void SetTarget(GameObject target)
    {
        Target = target;
        _targetRb2d = Target.GetComponent<Rigidbody2D>();
    }

    public void SetSlowDownRange(float slowDownRange)
    {
        SlowDownRange = slowDownRange;
        _root.SetData("slowDownRange", SlowDownRange);

    }
    public void SetStopRange(float stopRange)
    {
        StopRange = stopRange;
        _root.SetData("stopRange", StopRange);
    }
    public void SetAccelerationFactor(float accelerationFactor)
    {
        AccelerationFactor = accelerationFactor;
        _root.SetData("accelerationFactor", AccelerationFactor);
    }
    public void SetMaxSpeed(float maxSpeed)
    {
        MaxSpeed = maxSpeed;
        _root.SetData("chaseMaxSpeed", MaxSpeed);
    }
    public void SetWaypointReachDistance(float waypointReachDistance)
    {
        WaypointReachDistance = waypointReachDistance;
        _root.SetData("waypointReachDistance", WaypointReachDistance);
    }
}
