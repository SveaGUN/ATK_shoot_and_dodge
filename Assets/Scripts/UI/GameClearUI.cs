using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    [SerializeField]
    private Sprite[] numbers = null;

    [SerializeField]
    private Image hitNum = null;

    [SerializeField]
    private Button titleButton = null;

    [SerializeField]
    private UnityEvent onTitleButtonClick = null;
    public UnityEvent OnTitleButtonClick => onTitleButtonClick;

    Animator animator;

    static readonly int introId = Animator.StringToHash("Intro");
    static readonly int outroId = Animator.StringToHash("Outro");

    private void Awake()
    {
        animator = GetComponent<Animator>();

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
        SetHitCount();
        animator.SetTrigger(introId);
        titleButton.Select();
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

    /// <summary>
    /// 被弾回数欄に該当する数字スプライトを割り当てる
    /// </summary>
    private void SetHitCount()
    {
        //ここでPlayerのHitCountを取得すればok!
        //hitNum.sprite = numbers[hitcount];
    }

}
