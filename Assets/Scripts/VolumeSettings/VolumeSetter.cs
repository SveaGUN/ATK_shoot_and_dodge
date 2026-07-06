using UnityEngine;
using UnityEngine.Audio;

//BGMやSEの音量を管理するためのクラス
//AudioMixerを使用して音量を調整する
//
//UIから音量調整を行う場合は、VolumeUI.csを使用すること
//スクリプトから音量調整を行う場合、
//VolumeSetter.instance.BGM_Volume , VolumeSetter.instance.SE_Volumeを呼び出すこと
public class VolumeSetter : MonoBehaviour
{
    public static VolumeSetter instance;

    [SerializeField]
    [Tooltip("AudioMixerを指定する")]
    private AudioMixer audioMixer;

    public AudioMixer AudioMixer { get { return audioMixer; } }

    [SerializeField]
    private float bgmDecibel = -10f;

    //BGMのボリュームを取得または設定する
    public float BGM_Volume
    {
        get
        {
            //デシベルをボリュームに変換する(10に decibelBGM / 20f 乗する)
            return Mathf.Pow(10f, bgmDecibel / 20f);
        }
        set
        {
            //ボリュームをデシベルに変換する(log10をとって20をかける)
            float decibel = 20f * Mathf.Log10(value);
            //-80から0にClampする
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            bgmDecibel = decibel;
            audioMixer.SetFloat("BGMParam", decibel);
        }
    }

    [SerializeField]
    private float seDecibel = -10f;

    // SEのボリュームを取得または設定する
    public float SE_Volume
    {
        get
        {
            return Mathf.Pow(10f, seDecibel / 20f);
        }
        set
        {
            //ボリュームをデシベルに変換する(log10をとって20をかける)
            float decibel = 20f * Mathf.Log10(value);
            //-80から0にClampする
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            seDecibel = decibel;
            audioMixer.SetFloat("SEParam", decibel);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //音量を初期の状態に設定する
            AudioMixer.SetFloat("BGMParam", bgmDecibel);
            AudioMixer.SetFloat("SEParam", seDecibel);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
