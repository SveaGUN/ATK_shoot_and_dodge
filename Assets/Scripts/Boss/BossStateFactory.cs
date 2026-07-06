using System;

public class BossStateFactory
{
    /// <summary>
    /// ステートを生成する
    /// </summary>
    /// <param name="action"></param>
    /// <param name="type"></param>
    /// <param name="stateTime"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public IBossState CreateState(Action action, FireSystem<BossBullet> fireSystem, BossStateData.StateType type, float stateTime) => type switch
    {
        BossStateData.StateType.Debug => new StateDebug(action, fireSystem, stateTime),

        BossStateData.StateType.Idle => new StateIdle(action, stateTime),

        BossStateData.StateType.Move => new StateIdle(action, stateTime),

        BossStateData.StateType.Special => new StateSpecial(action, fireSystem, stateTime),

        BossStateData.StateType.Attack1 => new StateSpredShotNWay(action, fireSystem, stateTime),

        BossStateData.StateType.Attack2 => new StateCrossSpinAndOmniShot(action, fireSystem, stateTime),

        BossStateData.StateType.Attack3 => new StateTripleSpinAndOmniShot(action, fireSystem, stateTime),

        BossStateData.StateType.Attack4 => new StateWaveSpreadAndRotatingWall(action, fireSystem, stateTime),

        BossStateData.StateType.Attack5 => new StateSpiralAndBurst(action, fireSystem, stateTime),

        _ => throw new ArgumentOutOfRangeException(nameof(type), $"予期されていない StateType : {type}"),
    };
}