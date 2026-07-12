using UnityEngine;
using UnityEngine.UI;

//汎用的に使えるUIのインジケーター
//例えば、HPバーやMPバーなどに使える
//インジケーターの更新はfillAmountを使うため、ImageTypeはFilledにすること
public class Bar : MonoBehaviour
{
    private Image _barUI = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        _barUI = GetComponent<Image>();

        _barUI.fillAmount = 1.0f;
    }

    /// <summary>
    /// インジケーターの更新
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    public void UpdateFillAmount(float maxValue, float currentValue)
    {
        _barUI.fillAmount = currentValue / maxValue;
    }
}
