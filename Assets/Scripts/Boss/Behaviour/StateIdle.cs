using System;

public class StateIdle : IBossState
{
    private Action _action = null;
    private float _currentTime = 0;

    private float _stateTime = 0;

    public StateIdle(Action action, float waitTime)
    {
        _action = action;
        _stateTime = waitTime;
    }

    public void Enter()
    {
        _currentTime = 0;
    }

    public void BehaviourUpdate(float deltaTime)
    {
        _currentTime += deltaTime;

        //ステートの時間が経過したらステートを終了し、次のステートに遷移
        if (_currentTime > _stateTime)
        {
            _action.Invoke();
        }
    }

    public void Exit()
    {
        _currentTime = 0;
    }
}
