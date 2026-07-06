using System;

public class StateDebug : IBossState
{
    private readonly Action _action = null;
    private readonly FireSystem _fireSystem = null;
    private readonly float _stateTime = 0;

    private float _currentTime = 0;

    private float _t1 = 0;
    private float _t2= 0;
    private float _t3 = 0;

    private const float _fireInterval1 = 0.5f;
    private const float _fireInterval2 = 1f;
    private const float _fireInterval3 = 1.5f;

    public StateDebug(Action action, FireSystem fireSystem, float stateTime)
    {
        _action = action;
        _fireSystem = fireSystem;
        _stateTime = stateTime;
    }

    public void Enter()
    {
        _currentTime = 0;
        _t1 = 0;
        _t2 = 0;
        _t3 = 0;
    }

    public void BehaviourUpdate(float deltaTime)
    {
        _currentTime += deltaTime;
        _t1 += deltaTime;
        _t2 += deltaTime;
        _t3 += deltaTime;

        if (_fireInterval2 <= _t2)
        {
            _fireSystem.FireBurstCircle(2, 6);
            _t2 = 0f;
        }

        //ステートの時間が経過したらステートを終了し、次のステートに遷移
        if (_currentTime > _stateTime)
        {
            _action.Invoke();
        }
    }

    public void Exit()
    {
        _currentTime = 0;
        _t1 = 0;
        _t2 = 0;
        _t3 = 0;
    }
}
