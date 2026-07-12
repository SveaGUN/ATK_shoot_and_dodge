using AkaneTools;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private bool UseBoss = false;
    [SerializeField]
    private Boss boss = null;

    [Header("UI")]
    [SerializeField]
    private GameOverUI gameOverUI = null;
    [SerializeField]
    private GameClearUI gameClearUI = null;

    Animator animator;
    static readonly int outroId = Animator.StringToHash("Outro");

    [SerializeField]
    private PlayableDirector _stage = null;
    [SerializeField]
    private Bar _timeBar = null;

    private float _stageTime = 0f;

    enum GameState
    {
        //イントロ中
        Intro,
        //戦闘中
        Play,
        //ステージクリア
        StageClear,
        //ゲームオーバー
        GameOver
    }

    GameState currentState = GameState.Intro;

    //=================== awake =====================

    private void Awake()
    {
        currentState = GameState.Intro;

        animator = GetComponent<Animator>();

        //コールバックの登録
        gameOverUI.OnRetryButtonClick.AddListener(Retry);
        gameOverUI.OnTitleButtonClick.AddListener(Title);
        gameClearUI.OnTitleButtonClick.AddListener(Title);

        player.Init(new GameStateNotifier(GameOver));

        if (UseBoss) { boss.Init(); }
    }

    //=================== start =====================
    private void Start()
    {
        _stage.Stop();
        _timeBar.Init();
        _stageTime = (float)_stage.duration;

        _stage.stopped += GameClear;

        StartCoroutine(OnStart());
        //指定した戦闘BGMを再生する
        AudioManager.Instance.PlayBGM("Battle");
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        currentState = GameState.Play;

        _stage.Play();

        if (UseBoss) { boss.OnStart(); }
    }

    //=================== update =====================
    private void Update()
    {
        var deltaTime = Time.deltaTime;

        switch (currentState)
        {
            case GameState.Intro:
                break;

            case GameState.Play:
                _timeBar.UpdateFillAmount(_stageTime, _stageTime - (float)_stage.time);

                player.OnUpdate(deltaTime);

                if (UseBoss) { boss.OnUpdate(deltaTime); }

                break;

            case GameState.StageClear:
                break;

            case GameState.GameOver:
                break;

            default:
                break;
        }
    }

    //=================== other =====================

    public void GameClear(PlayableDirector director)
    {
        if (currentState != GameState.Play) { return; }

        currentState = GameState.StageClear;
        gameClearUI.AnimationPlay();

        _timeBar.UpdateFillAmount(1f, 0f);

        player.OnClear();
        if (UseBoss) { boss.OnGameClear(); }

    }

    public void GameOver()
    {
        if (currentState != GameState.Play) { return; }

        currentState = GameState.GameOver;

        if (UseBoss) { boss.OnGameOver(); }
        gameOverUI.AnimationPlay();

        //ゲームオーバー時のBGMを再生する
        AudioManager.Instance.PlayBGM("GameOver");

    }

    private void Retry()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }

    private void Title()
    {
        StartCoroutine(LoadScene("TitleScene"));
    }

    IEnumerator LoadScene(string name)
    {
        var animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        animator.SetTrigger(outroId);

        AudioManager.Instance.FadeOutBGM(1 / animationLength);
        //現在再生されているアニメーションの長さに応じて待機する
        yield return new WaitForSeconds(animationLength);

        SceneManager.LoadScene(name);
    }

    private void OnDestroy()
    {
        _stage.stopped -= GameClear;
    }
}
