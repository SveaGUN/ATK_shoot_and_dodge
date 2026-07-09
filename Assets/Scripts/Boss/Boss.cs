using UnityEngine;
using UnityEngine.Playables;

public class Boss : MonoBehaviour, IUpdatable
{
    //撃破時のパーティクル
    [SerializeField]
    private ParticleSystem dead = null;

    //レンダラー
    private SpriteRenderer bossRend;

    private PlayableDirector _director = null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        bossRend = GetComponent<SpriteRenderer>();
        _director = GetComponent<PlayableDirector>();
    }

    public void OnStart()
    {
        _director.Play();
    }

    public void OnUpdate(float deltaTime)
    {
        _director.time += deltaTime;
        _director.Evaluate();
    }

    /// <summary>
    /// ゲームクリア時の処理
    /// </summary>
    public void OnGameClear()
    {
        _director.Stop();
        
        Instantiate(dead, transform.position, Quaternion.identity, transform);
        bossRend.enabled = false;
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    public void OnGameOver()
    {
        _director.Stop();
    }
}

