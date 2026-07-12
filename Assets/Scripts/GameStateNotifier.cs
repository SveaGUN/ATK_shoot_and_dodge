using System;

public class GameStateNotifier
{
    //ゲームオーバー時に呼び出されるイベント
    private event Action _gameOverListener = null;

    public GameStateNotifier(Action gameOver)
    {
        _gameOverListener += gameOver;
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
    }

    /// <summary>
    /// ゲームオーバーの通知を行う
    /// </summary>
    public void NotifyGameOver()
    {
        _gameOverListener?.Invoke();
    }
}
