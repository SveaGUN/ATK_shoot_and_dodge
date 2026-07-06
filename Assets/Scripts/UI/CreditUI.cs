using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [SerializeField]
    private Button _closeButton = null;

    public event Action OnClose;

    private Animator _animator = null;
    private static readonly int _showId = Animator.StringToHash("Show");
    private static readonly int _hideId = Animator.StringToHash("Hide");

    public void Init()
    {
        _animator = GetComponent<Animator>();

        _closeButton.onClick.AddListener(() =>
        {
            OnClose?.Invoke();

            Hide();
        });

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        StartCoroutine(OnShow());
    }

    private IEnumerator OnShow()
    {
        _animator.SetTrigger(_showId);

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        _closeButton.Select();
    }

    public void Hide()
    {
        StartCoroutine(OnHide());
    }

    private IEnumerator OnHide()
    {
        _animator.SetTrigger(_hideId);

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();

        foreach (var listener in OnClose.GetInvocationList())
        {
            OnClose -= (Action)listener;
        }
    }
}
