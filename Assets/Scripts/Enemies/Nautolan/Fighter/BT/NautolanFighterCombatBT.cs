using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Pathfinding;

public class NautolanFighterCombatBT : BehaviourTree.Tree
{
    public GameObject Target;

    [SerializeField]
    public LaserCannonArray LaserCannonArray;

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
        StartFireLaserCannonArrayNode startFireLaserNode = new StartFireLaserCannonArrayNode(LaserCannonArray);
        SequenceNode fireStartSequence = new SequenceNode(new List<Node>() {
            computeTargetPredictedImpactPosition,
            targetInRangeNode,
            hasLOSOnTargetPositionNode,
            isFacingPositionApproximateNode,
            startFireLaserNode
        });
        StopFireLaserCannonArrayNode stopFireLaserNode = new StopFireLaserCannonArrayNode(LaserCannonArray);
        IsWeaponActiveNode isWeaponActiveNode = new IsWeaponActiveNode(LaserCannonArray);
        SequenceNode fireStopSequence = new SequenceNode(new List<Node>() {
            isWeaponActiveNode,
            stopFireLaserNode
        });
        SelectorNode rootSelector = new SelectorNode(new List<Node>() {
            fireStartSequence,
            fireStopSequence
        });

        return rootSelector;
    }

    private void Update()
    {
        tick(Time.deltaTime);
    }

    protected override void UpdateBBVariables()
    {
        _root.SetData("targetPosition", (Vector2)Target.transform.position);
        _root.SetData("targetVelocity", _targetRb2d.velocity);
        _root.SetData("projectileSpeed", LaserCannonArray.LaserCannonConfig.LaserCannonArrayLevelConfigs[LaserCannonArray.LaserCannonConfig.CurrentWeaponLevel].ProjectileSpeed);
    }

    protected override void InitTree()
    {
        Debug.Log("Tree init " + Target);
        _targetRb2d = Target.GetComponent<Rigidbody2D>();
        _root.SetData("attackRange", AttackRange);
    }
}
