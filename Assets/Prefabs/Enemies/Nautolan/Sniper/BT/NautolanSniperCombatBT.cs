using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class NautolanSniperCombatBT : BehaviourTree.Tree
{
    [SerializeField]
    public GameObject Target;

    [SerializeField]
    public ChargedLaserCannonArray ChargedLaserCannonArray;

    [SerializeField]
    private float AttackRange;

    [SerializeField]
    private float LaserCannonCooldown;

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
        StartFireChargedLaserCannonNode startFireChargedLaserNode = new StartFireChargedLaserCannonNode(ChargedLaserCannonArray);
        IsChargedLaserReadyNode isChargedLaserReadyNode = new IsChargedLaserReadyNode(ChargedLaserCannonArray);
        StopFireChargedLaserCannonNode stopFireChargedLaserNode = new StopFireChargedLaserCannonNode(ChargedLaserCannonArray);
        SequenceNode aimSequence = new SequenceNode(new List<Node>() {
            computeTargetPredictedImpactPosition,
            targetInRangeNode,
            hasLOSOnTargetPositionNode,
            isFacingPositionApproximateNode
        });
        ResultInverterNode resultInverterNode = new ResultInverterNode(stopFireChargedLaserNode);
        TimerGateNode timerGateNode = new TimerGateNode();
        BBVarSetter timerSetter = new BBVarSetter(timerGateNode._TimerStartBBVarName, true);
        SelectorNode aimSelector = new SelectorNode(new List<Node>() {
            aimSequence,
            resultInverterNode
        });
        SequenceNode chargeReadyShootSequence = new SequenceNode(new List<Node>() { 
            isChargedLaserReadyNode,
            stopFireChargedLaserNode,
            timerSetter
        });
        SequenceNode rootSequence = new SequenceNode(new List<Node>() {
            aimSelector,
            timerGateNode,
            startFireChargedLaserNode,
            chargeReadyShootSequence
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
        _root.SetData("projectileSpeed", ChargedLaserCannonArray.GetCurrentProjectileSpeed());
        _root.SetData("timerDuration", LaserCannonCooldown);
    }

    protected override void InitTree()
    {
        _targetRb2d = Target.GetComponent<Rigidbody2D>();
        _root.SetData("attackRange", AttackRange);
    }
}
