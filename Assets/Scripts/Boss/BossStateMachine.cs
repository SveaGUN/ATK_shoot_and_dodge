using UnityEngine;

public class BossStateMachine
{
    //現在のステート
    private IBossState _currentState = null;

    /// <summary>
    /// ステートの初期化
    /// </summary>
    /// <param name="startState"></param>
    public void InitState(IBossState startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    /// <summary>
    /// ステートの遷移
    /// </summary>
    /// <param name="nextState">次のステート</param>
    public void TransitionTo(IBossState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    /// <summary>
    /// 現在のステートを止める
    /// </summary>
    public void StopState()
    {
        Debug.Log("現在のステートを止めました");
        _currentState = null;
    }

    /// <summary>
    /// 現在のステートのUpdate処理
    /// </summary>
    public void StateUpdate() => _currentState.BehaviourUpdate(Time.deltaTime);
}
