using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Pathfinding;

public class NautolanScoutCombatBT : BehaviourTree.Tree
{
    [SerializeField]
    public GameObject Target;

    [SerializeField]
    public LaserGun SourceLaserGun;

    [SerializeField]
    private float LaserCooldown;

    [SerializeField]
    private float AttackRange;

    private Rigidbody2D _targetRb2d;

    protected override Node SetupTree()
    {
        ComputeTargetPredictedImpactPosition computeTargetPredictedImpactPosition = new ComputeTargetPredictedImpactPosition(gameObject);
        HasLOSOnTargetPositionNode hasLOSOnTargetPositionNode = new HasLOSOnTargetPositionNode(gameObject);
        TargetInRangeNode targetInRangeNode = new TargetInRangeNode(gameObject);
        targetInRangeNode.TargetBBVarName = computeTargetPredictedImpactPosition.TargetPositionBBVarName;
        IsFacingPositionApproximateNode isFacingPositionApproximateNode = new IsFacingPositionApproximateNode(gameObject.transform);
        hasLOSOnTargetPositionNode.TargetPositionBBVarName = computeTargetPredictedImpactPosition.OutputPositionBBVarName;
        isFacingPositionApproximateNode.PositionToFaceBBVarName = computeTargetPredictedImpactPosition.OutputPositionBBVarName;
        FireLaserWithCooldownNode fireLaserNode = new FireLaserWithCooldownNode(SourceLaserGun);
        SequenceNode rootSequence = new SequenceNode(new List<Node>() {
            computeTargetPredictedImpactPosition,
            targetInRangeNode,
            hasLOSOnTargetPositionNode,
            isFacingPositionApproximateNode,
            fireLaserNode
        });
        return rootSequence;
    }

    private void Update()
    {
        tick(Time.deltaTime);
    }

    protected override void UpdateBBVariables()
    {
        _root.SetData("targetPosition", (Vector2)Target.transform.position);
        _root.SetData("targetVelocity", _targetRb2d.velocity);
        _root.SetData("projectileSpeed", SourceLaserGun.LaserSpeed);
    }

    protected override void InitTree()
    {
        _targetRb2d = Target.GetComponent<Rigidbody2D>();
        _root.SetData("laserCooldown", LaserCooldown);
        _root.SetData("attackRange", AttackRange);
    }

    public void SetLaserCooldown(float laserCooldown)
    {
        LaserCooldown = laserCooldown;
        _root.SetData("laserCooldown", LaserCooldown);
    }

    public void SetAttackRange(float laserCooldown)
    {
        LaserCooldown = laserCooldown;
        _root.SetData("laserCooldown", LaserCooldown);
    }
}
