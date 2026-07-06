public interface IBossState
{
    //このステートに入った時に行う処理
    public void Enter();

    //このステートで毎フレーム行う処理
    public void BehaviourUpdate(float deltaTime);

    //このステートを出た時に行う処理
    public void Exit();
}
