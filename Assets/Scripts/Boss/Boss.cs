using UnityEngine;

public class Boss : MonoBehaviour, IDamagable
{
    private FireSystem _fireSystem = null;

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

    [SerializeField, Header("ステート遷移情報を格納する")]
    private BossStateTransitionData _transitionData = null;
    [SerializeField, Header("デバッグ用ステート遷移情報")]
    private BossStateTransitionData _debugTransitionData = null;
    [SerializeField]
    private bool _isDebug = false; //デバッグ用のフラグ
    private BossStateTransitionData _currentTransitionData = null;

    //ステート番号 BossStateTransitionData.transitionsのインデクスに相当する
    private int _stateIndex = 0;

    //ステートの遷移を管理するStateMachine
    private BossStateMachine _stateMachine = null;

    //ステートを生成するFactory
    private BossStateFactory _factory = null;

    private GameStateNotifier _stateNotifier = null;

    private System.Action _nextState = null;


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init(GameStateNotifier notifier)
    {
        _stateMachine = new BossStateMachine();
        _factory = new BossStateFactory();

        bossRend = GetComponent<SpriteRenderer>();

        currentHp = maxHp;
        _hpBar.Init();

        _fireSystem = new FireSystem(transform, GameObject.FindWithTag("Player").GetComponent<Transform>(), transform);

        //_fireSystem.Init(transform, GameObject.FindWithTag("Player").GetComponent<Transform>(), transform);

        _stateNotifier = notifier;

        //デバッグモードが有効である場合
        if (_isDebug)
        {
            //デバッグ用のステート遷移情報を使用
            _currentTransitionData = _debugTransitionData;
        }
        else
        {
            //通常のステート遷移情報を使用
            _currentTransitionData = _transitionData;
        }

        _nextState = () => { SetState(); };

        SetState();
    }

    public void SetState()
    {
        Debug.Log($"SetState! index : {_stateIndex}");

        //ステートの遷移情報を取得
        var data = _currentTransitionData.transitions[_stateIndex];

        //ステートを生成
        var state = _factory.CreateState(_nextState, _fireSystem, data.nextState, data.stateTime);

        //ステートマシンにセットし、遷移させる
        _stateMachine.TransitionTo(state);

        //インデクスを進める
        _stateIndex++;

        //インデクスがtransitions配列の長さを超えたら0に戻す
        if (_stateIndex >= _currentTransitionData.transitions.Length) { _stateIndex = 0; }
    }

    public void BossUpdate()
    {
        //ステートマシンのUpdateを呼ぶ
        _stateMachine.StateUpdate();
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
        _stateMachine.StopState();

        Instantiate(dead, transform.position, Quaternion.identity, transform);
        bossRend.enabled = false;
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    public void OnGameOver()
    {
        _stateMachine.StopState();
    }

    private void OnDestroy()
    {
        //メモリリークを防ぐために、リスナーの解除
        _stateNotifier.RemoveAllListener();
    }

}

