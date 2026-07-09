using AkaneTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DebugScene : MonoBehaviour
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

        player.Init(new GameStateNotifier(GameOver, GameClear));

        if(UseBoss){ boss.Init(); }
    }

    //=================== start =====================
    private void Start()
    {
        StartCoroutine(OnStart());
        //指定した戦闘BGMを再生する
        AudioManager.Instance.PlayBGM("Battle");
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        currentState = GameState.Play;

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
                player.OnUpdate(deltaTime);

                if (UseBoss){ boss.OnUpdate(deltaTime); }

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

    public void GameClear()
    {
        if (currentState == GameState.Play)
        {
            currentState = GameState.StageClear;

            player.OnClear();
            if (UseBoss){ boss.OnGameClear(); }
            gameClearUI.AnimationPlay();
        }
    }

    public void GameOver()
    {
        if (currentState == GameState.Play)
        {
            currentState = GameState.GameOver;

            if (UseBoss){ boss.OnGameOver(); }
            gameOverUI.AnimationPlay();

            //ゲームオーバー時のBGMを再生する
            AudioManager.Instance.PlayBGM("GameOver");
        }
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


}
