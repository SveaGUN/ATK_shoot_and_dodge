using AkaneTools;
using UnityEngine;
using UnityEngine.UI;

//スライダーを指定することで、音量を調整できるクラス
public class VolumeUI : MonoBehaviour
{
    [Header("SettingsUI")]

    [SerializeField]
    [Tooltip("BGMスライダーを指定する")]
    private Slider bgmSlider = null;
    [SerializeField]
    [Tooltip("SEスライダーを指定する")]
    private Slider seSlider = null;

    [SerializeField]
    private DragHandler _seSliderHandler = null;

    private void Start()
    {
        var volume = AudioManager.Instance.Volume;

        //スライダーの値を指定する
        bgmSlider.value = volume.BGM_Volume;
        seSlider.value = volume.SE_Volume;

        ///イベントの登録
        //BGMの調整
        bgmSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.Volume.BGM_Volume = value; });
        //SEの調整
        seSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.Volume.SE_Volume = value; });

        //SE音量確認
        _seSliderHandler.OnEndDragEvent += () => AudioManager.Instance.PlaySE("Demo");//デモSEを再生する
    }
}