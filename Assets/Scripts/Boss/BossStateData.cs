[System.Serializable]//Editor上から見えるようにする
public class BossStateData
{
    public enum StateType
    {
        Debug,
        Idle,
        Move,
        Special,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Attack5
    }

    //次に遷移するステート
    public StateType nextState = StateType.Idle;

    public float stateTime = 0f;

    //public 
}
