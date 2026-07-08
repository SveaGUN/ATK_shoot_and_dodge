using System;
using UnityEngine;

public class StateDebug : IBossState
{
    private readonly Action _action = null;
    private readonly FireSystem<BossBullet> _fireSystem = null;
    private readonly float _stateTime = 0;

    private float _currentTime = 0;

    private float _t1 = 0;
    
    private float _t3 = 0;

    private const float _fireInterval1 = 0.5f;

    private const float _fireInterval3 = 1.5f;

    private float _offset1 = 0;
    private float _offset3 = 0;

    public StateDebug(Action action, FireSystem<BossBullet> fireSystem, float stateTime)
    {
        _action = action;
        _fireSystem = fireSystem;
        _stateTime = stateTime;
    }

    public void Enter()
    {
        Debug.Log("Enter");
        _currentTime = 0;
        _t1 = 0;
        _t2 = 0;
        _t3 = 0;
    }

    private float _t2 = 0;
    private const float _fireInterval2 = 1f;
    private float _offset2 = 0;

    public void BehaviourUpdate(float deltaTime)
    {
        _currentTime += deltaTime;
        _t1 += deltaTime;
        _t2 += deltaTime;
        _t3 += deltaTime;

        if (_t2 >= _fireInterval2)
        {
            _offset2 += 5f;
            _t2 = 0f;

            //_fireSystem.FireBurstCircle(2, 6, 5, _offset2);
            //_fireSystem.FireBurstCircle(5, 6);
            _fireSystem.FireBurstCircle(5, 1, _offset2);
        }

        //ステートの時間が経過したらステートを終了し、次のステートに遷移
        if (_currentTime > _stateTime)
        {
            _action.Invoke();
        }
    }

    public void Exit()
    {
        Debug.Log("Exit");
        _currentTime = 0;
        _t1 = 0;
        _t2 = 0;
        _t3 = 0;
    }
}
