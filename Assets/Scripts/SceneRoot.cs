using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private Boss boss = null;

    [Header("UI")]
    [SerializeField]
    private GameOverUI gameOverUI = null;
    [SerializeField]
    private GameClearUI gameClearUI = null;

    [Header("Audio")]
    [Tooltip("戦闘中に流すBGMインデックスを指定する")]
    [SerializeField]
    private int battleBgmIndex = 4;

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

    private void Awake()
    {
        currentState = GameState.Intro;

        animator = GetComponent<Animator>();

        //コールバックの登録
        gameOverUI.OnRetryButtonClick.AddListener(Retry);
        gameOverUI.OnTitleButtonClick.AddListener(Title);
        gameClearUI.OnTitleButtonClick.AddListener(Title);

        GameStateNotifier gameStateNotifier = new GameStateNotifier(gameOver: GameOver, stageClear: GameClear);

        player.Init(gameStateNotifier);
        boss.Init(gameStateNotifier);
    }

    private void Start()
    {
        StartCoroutine(OnStart());
        //指定した戦闘BGMを再生する
        AudioPlayer.instance.PlayBGM(battleBgmIndex);
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Intro:
                break;

            case GameState.Play:
                player.PlayerUpdate();
                boss.BossUpdate();
                break;

            case GameState.StageClear:
                break;

            case GameState.GameOver:
                break;

            default:
                break;
        }
    }

    public void GameClear()
    {
        if (currentState == GameState.Play)
        {
            currentState = GameState.StageClear;

            player.OnClear();
            boss.OnBossDead();
            gameClearUI.AnimationPlay();
        }
    }

    public void GameOver()
    {
        if (currentState == GameState.Play)
        {
            currentState = GameState.GameOver;

            player.OnPlayerDead();
            boss.OnGameOver();
            gameOverUI.AnimationPlay();

            //ゲームオーバー時のBGMを再生する
            AudioPlayer.instance.PlayBGM(3);
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

        AudioPlayer.instance.FadeOutBGM(1 / animationLength);
        //現在再生されているアニメーションの長さに応じて待機する
        yield return new WaitForSeconds(animationLength);

        SceneManager.LoadScene(name);
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        currentState = GameState.Play;
    }
}
