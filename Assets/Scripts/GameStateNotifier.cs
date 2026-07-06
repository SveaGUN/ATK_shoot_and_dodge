using System;

//メモリリークを防ぐため、オブジェクトが破棄されるときはRemoveAllListenerを呼び出すこと!!!

public class GameStateNotifier
{
    //ゲームオーバー時に呼び出されるイベント
    private event Action _gameOverListener = null;
    //ステージクリア時に呼び出されるイベント
    private event Action _stageClearListener = null;

    public GameStateNotifier(Action gameOver, Action stageClear)
    {
        _gameOverListener += gameOver;
        _stageClearListener += stageClear;
    }

    ~GameStateNotifier()
    {
        if (_gameOverListener != null)
        {
            foreach (var d in _gameOverListener.GetInvocationList())
            {
                _gameOverListener -= (Action)d;
            }
        }

        if (_stageClearListener != null)
        {
            foreach (var d in _stageClearListener.GetInvocationList())
            {
                _stageClearListener -= (Action)d;
            }
        }
    }

    /// <summary>
    /// ゲームオーバーの通知を行う
    /// </summary>
    public void NotifyGameOver()
    {
        _gameOverListener?.Invoke();
    }

    /// <summary>
    /// ステージクリアの通知を行う
    /// </summary>
    public void NotifyStageClear()
    {
        _stageClearListener?.Invoke();
    }
}
