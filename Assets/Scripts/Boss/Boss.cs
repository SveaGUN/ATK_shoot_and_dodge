using UnityEngine;
using UnityEngine.Playables;

public class Boss : MonoBehaviour, IDamagable
{
    [Header("ステータス")]
    [SerializeField]
    private int maxHp = 1000;
    private int currentHp = 0;

    //撃破時のパーティクル
    [SerializeField]
    private ParticleSystem dead = null;

    [SerializeField]
    private Bar _hpBar = null;

    //レンダラー
    private SpriteRenderer bossRend;

    private GameStateNotifier _stateNotifier = null;

    private PlayableDirector _director = null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init(GameStateNotifier notifier)
    {
        bossRend = GetComponent<SpriteRenderer>();
        _director = GetComponent<PlayableDirector>();

        currentHp = maxHp;
        _hpBar.Init();

        _stateNotifier = notifier;
    }

    public void BossStart()
    {
        _director.Play();
    }

    public void BossUpdate()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        _hpBar.UpdateFillAmount(maxHp, currentHp);

        if (currentHp <= 0)
        {
            _stateNotifier.NotifyStageClear();
        }
    }

    /// <summary>
    /// 撃破時の処理
    /// </summary>
    public void OnBossDead()
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

