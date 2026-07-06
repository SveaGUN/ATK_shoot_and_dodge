using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private Animator _animator = null;
    private static readonly int s_showId = Animator.StringToHash("Show");
    private static readonly int s_hideId = Animator.StringToHash("Hide");

    //Showの処理が終わった後に呼ばれるイベント
    public event Action OnAfterShowAnimation;
    //Hideの処理が終わった後に呼ばれるイベント
    public event Action OnAfterHideAnimation;

    [SerializeField]
    private Button _closeButton = null;

    public void Init()
    {
        _animator = GetComponent<Animator>();

        _closeButton.onClick.AddListener(Hide);

        //非表示
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(OnShow());
    }

    public void Hide()
    {
        StartCoroutine(OnHide());
    }

    private IEnumerator OnShow()
    {
        _animator.SetTrigger(s_showId);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        _closeButton.Select();

        OnAfterShowAnimation?.Invoke();
    }

    private IEnumerator OnHide()
    {
        _animator.SetTrigger(s_hideId);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        OnAfterHideAnimation?.Invoke();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (OnAfterShowAnimation != null)
        {
            foreach (var action in OnAfterShowAnimation.GetInvocationList())
            {
                OnAfterShowAnimation -= (Action)action;
            }
        }

        if (OnAfterHideAnimation != null)
        {
            foreach (var action in OnAfterHideAnimation.GetInvocationList())
            {
                OnAfterHideAnimation -= (Action)action;
            }
        }
    }
}