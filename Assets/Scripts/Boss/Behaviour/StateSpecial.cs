using System;

public class StateSpecial : IBossState
{
    private readonly Action _action = null;
    private readonly FireSystem<BossBullet> _fireSystem = null;
    private readonly float _stateTime = 0;

    private float _currentTime = 0;
    private float _t1 = 0;
    private float _t2 = 0;
    private float _rot1 = 0;
    private float _rot2 = 0;
    private float _rot3 = 0;

    private const float _fireInterval1 = 0.05f;
    private const float _fireInterval2 = 2f;

    public StateSpecial(Action action, FireSystem<BossBullet> fireSystem, float stateTime)
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

        //前半の攻撃
        if (_fireInterval1 <= _t1)
        {
            _fireSystem.FireAtAngle(_rot1, 0f, 2f);
            _fireSystem.FireAtAngle(_rot1, 120f, 2f);
            _fireSystem.FireAtAngle(_rot1, 240f, 2f);
            _fireSystem.FireAtAngle(_rot2, 5f);
            _fireSystem.FireAtAngle(-_rot2, 5f);
            _rot1 += 7f;
            _rot2 += 5f;

            if (_rot1 >= 360) { _rot1 -= 360; }
            if (_rot2 >= 360) { _rot2 -= 360; }

            _t1 = 0f;
        }

        if (_fireInterval2 <= _t2)
        {
            _fireSystem.FireBurstCircle(5, 6, _rot3);
            _rot3 += 13f;

            if (_rot3 >= 360) { _rot3 -= 360; }

            _t2 = 0f;
        }

        //ステートの時間が経過したらステートを終了し、次のステートに遷移
        if (_currentTime > _stateTime)
        {
            _action.Invoke();
        }

        //var waita = 3f;
        //var tempa = 0f;
        //while (waita > tempa)
        //{
        //    tempa += Time.deltaTime;
        //}


        ////後半の攻撃
        //if (_fireInterval1 <= _t1)
        //{
        //    _boss.FireSystem.FireRotate(_rot1, 2f);
        //    _boss.FireSystem.FireRotate(_rot1 + 72f, 2f);
        //    _boss.FireSystem.FireRotate(_rot1 + 144f, 2f);
        //    _boss.FireSystem.FireRotate(_rot1 + 216f, 2f);
        //    _boss.FireSystem.FireRotate(_rot1 + 288f, 2f);
        //    _rot1 += 14f;

        //    if (_rot1 >= 360) { _rot1 = 0; }

        //    _t1 = 0f;
        //}

        //if (_fireInterval2 <= _t2)
        //{
        //    _boss.FireSystem.FireToPlayerSpread(7, 5f);

        //    _t2 = 0f;
        //}
    }

    public void Exit()
    {
        _currentTime = 0;
        _t1 = 0;
        _t2 = 0;
        _rot1 = 0;
        _rot2 = 0;
        _rot3 = 0;
    }
}
