using System;

//Create by ChatGPT
public class StateSpiralAndBurst : IBossState
{
    private readonly Action _action = null;
    private readonly FireSystem<BossBullet> _fireSystem = null;
    private readonly float _stateTime = 0;

    private float _currentTime = 0;
    private float _spiralTimer = 0;
    private float _burstTimer = 0;
    private float _spiralRot = 0;

    private const float _spiralInterval = 0.03f; // スパイラル弾間隔
    private const float _burstInterval = 1.5f;   // バースト攻撃間隔

    public StateSpiralAndBurst(Action action, FireSystem<BossBullet> fireSystem, float stateTime)
    {
        _action = action;
        _fireSystem = fireSystem;
        _stateTime = stateTime;
    }

    public void Enter()
    {
        _currentTime = 0;
        _spiralTimer = 0;
        _burstTimer = 0;
        _spiralRot = 0;
    }

    public void BehaviourUpdate(float deltaTime)
    {
        _currentTime += deltaTime;
        _spiralTimer += deltaTime;
        _burstTimer += deltaTime;

        // スパイラル全方位弾（3方向 × 3セット、低速）
        if (_spiralTimer >= _spiralInterval)
        {
            _fireSystem.FireBurstCircle(2, 1, 3f, _spiralRot);
            _spiralRot += 13f;
            if (_spiralRot >= 360) { _spiralRot -= 360; }
            _spiralTimer = 0;
        }

        // 一定間隔でプレイヤーへ高速拡散バースト攻撃
        if (_burstTimer >= _burstInterval)
        {
            _fireSystem.FireSpreadToTarget(5, 3f);  // やや広い
            //_fireSystem.FireSpreadToTarget(5, 2f);  // 中程度
            //_fireSystem.FireSpreadToTarget(5, 1f);  // 狭い
            _burstTimer = 0;
        }

        if (_currentTime >= _stateTime)
        {
            _action.Invoke(); // 次のステートへ移行
        }
    }

    public void Exit()
    {
        _currentTime = 0;
        _spiralTimer = 0;
        _burstTimer = 0;
        _spiralRot = 0;
    }
}
