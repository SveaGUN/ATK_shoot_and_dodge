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
        //スライダーの値を指定する
        bgmSlider.value = VolumeSetter.instance.BGM_Volume;
        seSlider.value = VolumeSetter.instance.SE_Volume;

        ///イベントの登録
        //BGMの調整
        bgmSlider.onValueChanged.AddListener((value) => { VolumeSetter.instance.BGM_Volume = value; });
        //SEの調整
        seSlider.onValueChanged.AddListener((value) => { VolumeSetter.instance.SE_Volume = value; });

        //SE音量確認
        _seSliderHandler.OnEndDragEvent += () => AudioPlayer.instance.PlaySE(0);//デモSEを再生する
    }
}