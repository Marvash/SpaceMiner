using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TimerGateNode : Node
{
    private float _timerDuration = 1.0f;
    public string _timerDurationBBVarName = "timerDuration";

    private bool _timerStart = false;
    public string _TimerStartBBVarName = "startTimer";

    private float _startTime = 0.0f;

    public TimerGateNode() : base() { }
    public override BTState Evaluate()
    {
        object timerStartBBObj = GetData(_TimerStartBBVarName);
        if (timerStartBBObj != null)
        {
            _timerStart = (bool)timerStartBBObj;

        }
        if (_timerStart)
        {
            object timerDurationBBObj = GetData(_timerDurationBBVarName);
            if (timerDurationBBObj != null)
            {
                _timerDuration = (float)timerDurationBBObj;

            }
            _startTime = Time.time;
            _timerStart = false;
            SetDataInRoot(_TimerStartBBVarName, _timerStart);
        }
        if(Time.time - (_startTime + _timerDuration) < 0.0f)
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }
        CurrentState = BTState.SUCCESS;
        return CurrentState;
    }
}
