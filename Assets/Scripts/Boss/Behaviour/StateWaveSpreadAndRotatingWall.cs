using System;

public class StateWaveSpreadAndRotatingWall : IBossState
{
    private readonly Action _action = null;
    private readonly FireSystem _fireSystem = null;
    private readonly float _stateTime = 0;

    private float _currentTime = 0;
    private float _t1 = 0;
    private float _t2 = 0;
    private float _rot1 = 0;

    private const float _fireInterval1 = 2f;
    private const float _fireInterval2 = 0.05f;

    public StateWaveSpreadAndRotatingWall(Action action, FireSystem fireSystem, float stateTime)
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
        _rot1 = 0;
    }

    public void BehaviourUpdate(float deltaTime)
    {
        _currentTime += deltaTime;
        _t1 += deltaTime;
        _t2 += deltaTime;

        if (_fireInterval1 <= _t1)
        {
            _fireSystem.FireSpreadToTarget(3, 5f, 2f);
            _fireSystem.FireSpreadToTarget(3, 5f, 3f);
            _fireSystem.FireSpreadToTarget(3, 5f, 4f);
            _fireSystem.FireSpreadToTarget(3, 5f, 5f);
            _fireSystem.FireSpreadToTarget(3, 5f, 7f);

            _t1 = 0f;
        }

        if (_fireInterval2 <= _t2)
        {
            _fireSystem.FireBurstCircle(3, 3, 5f, _rot1);
            _rot1 += 17f;

            if (_rot1 >= 360) { _rot1 -= 360; }

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
        _rot1 = 0;
    }
}
