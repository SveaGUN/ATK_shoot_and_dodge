using AkaneTools;
using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "GameScene";

    [Header("タイトルボタン")]
    [SerializeField]
    private Button _startButton = null;
    [SerializeField]
    private Button _settingsButton = null;
    [SerializeField]
    private Button _creditButton = null;
    [SerializeField]
    private Button _exitButton = null;

    [Header("UI")]
    [SerializeField]
    private CreditUI _creditUI = null;
    [SerializeField]
    private SettingsUI _settingsUI = null;


    Animator _animator;
    static readonly int s_outroId = Animator.StringToHash("Outro");

    private void Awake()
    {
        var platform = Application.platform;

        _animator = GetComponent<Animator>();

        _creditUI.Init();
        _settingsUI.Init();

        //イベントの登録
        _startButton.onClick.AddListener(() => LoadScene());

        //WebGLPlayerの時はExitボタンを非表示にする
        if (platform == RuntimePlatform.WebGLPlayer) { _exitButton.gameObject.SetActive(false); }
        else { _exitButton.onClick.AddListener(() => Quit()); }

        _creditButton.onClick.AddListener(() =>
        {
            _creditUI.Show();
            ButtonInteractable(false);
        });
        _creditUI.OnClose += () =>
        {
            ButtonInteractable(true);
            _creditUI.Hide();
            _creditButton.Select();
        };

        _settingsButton.onClick.AddListener(() =>
        {
            _settingsUI.Show();
            ButtonInteractable(false);
        });
        //設定UIの非表示処理が終わった後に呼ばれるイベントを登録
        _settingsUI.OnAfterHideAnimation += () =>
        {
            ButtonInteractable(true);
            _settingsButton.Select();
        };


        _startButton.Select();
    }


    /// <summary>
    /// ボタンのインタラクションを切り替える
    /// </summary>
    /// <param name="interactable"></param>
    private void ButtonInteractable(bool interactable)
    {
        _startButton.interactable = interactable;
        _settingsButton.interactable = interactable;
        _exitButton.interactable = interactable;
        _creditButton.interactable = interactable;
    }

    private void Start()
    {
        //タイトルシーンのBGMを再生する
        AudioManager.Instance.PlayBGM("Title");
    }

    /// <summary>
    /// 指定したシーンを読み込む
    /// </summary>
    public void LoadScene()
    {
        StartCoroutine(OnLoadScene(sceneName));
    }

    IEnumerator OnLoadScene(string name)
    {
        var animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        _animator.SetTrigger(s_outroId);

        //BGMをフェードアウトさせる。フェードアウトの時間は、アニメーションの長さに応じて変化する
        AudioManager.Instance.FadeOutBGM(1 / animationLength);

        //現在再生されているアニメーションの長さに応じて待機する
        yield return new WaitForSeconds(animationLength);

        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
