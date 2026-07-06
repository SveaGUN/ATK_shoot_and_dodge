using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameOverUI : MonoBehaviour
{
    //ボタンの指定
    [SerializeField]
    private Button retryButton = null;
    [SerializeField]
    private Button titleButton = null;

    //UnityEventの登録
    [SerializeField]
    private UnityEvent onRetryButtonClick = null;
    [SerializeField]
    private UnityEvent onTitleButtonClick = null;

    public UnityEvent OnRetryButtonClick => onRetryButtonClick;
    public UnityEvent OnTitleButtonClick => onTitleButtonClick;

    Animator animator;

    static readonly int introId = Animator.StringToHash("Intro");
    static readonly int outroId = Animator.StringToHash("Outro");


    private void Awake()
    {
        animator = GetComponent<Animator>();

        retryButton.onClick.AddListener(() =>
        {
            OnRetryButtonClick.Invoke();
            animator.SetTrigger(outroId);

        });

        titleButton.onClick.AddListener(() =>
        {
            OnTitleButtonClick.Invoke();
            animator.SetTrigger(outroId);

        });

        Hide();
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    public void AnimationPlay()
    {
        Show();
        animator.SetTrigger(introId);
        retryButton.Select();
    }

    /// <summary>
    /// UIを表示する
    /// </summary>
    private void Show()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// UIを非表示にする
    /// </summary>
    private void Hide()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
