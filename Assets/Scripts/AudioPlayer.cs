using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//BGMとSEの再生、停止を行うクラス
//BGMのフェードイン、フェードアウトができる
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;

    [Header("AudioClips")]

    [SerializeField]
    [Tooltip("BGMを格納する")]
    private AudioClip[] bgmClips = null;

    [SerializeField]
    [Tooltip("SEを格納する")]
    private AudioClip[] seClips = null;

    [Header("AudioSources\n上はBGM,下はSE")]
    [SerializeField]
    [Tooltip("BGMを再生するAudioSource")]
    private AudioSource bgmSource = null;

    [SerializeField]
    [Tooltip("SEを再生するAudioSource")]
    private AudioSource seSource = null;

    //現在フェード中かどうか
    private bool isBgmFading = false;
    
    public bool IsFading { get => isBgmFading; }

    //====================================================================
    //初期化処理
    //====================================================================

    private void Awake()
    {
        //シングルトン
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //====================================================================
    //BGM処理
    //====================================================================

    /// <summary>
    /// BGMを再生する。フェード中は再生できない
    /// </summary>
    /// <param name="bgmIndex">BGMの配列インデックス</param>
    public void PlayBGM(int bgmIndex)
    {
        if (isBgmFading)
        {
            Debug.LogError("BGMがフェード中です");
            return;
        }

        if (bgmIndex < 0 || bgmIndex >= bgmClips.Length)
        {
            Debug.LogError("BGMのインデックスが範囲外です");
            return;
        }

        //BGMが再生中なら停止する
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        //BGMを再生する
        bgmSource.clip = bgmClips[bgmIndex];
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    /// <summary>
    /// BGMを停止する。フェード中は停止不可
    /// </summary>
    public void StopBGM()
    {
        if (isBgmFading)
        {
            Debug.LogError("BGMがフェード中です");
            return;
        }

        bgmSource.Stop();
    }

    /// <summary>
    /// BGMを強制停止する。フェード中でも停止可
    /// </summary>
    public void ForcedStopBGM()
    {
        if (isBgmFading)
        {
            StopAllCoroutines(); //全てのコルーチンを停止
            isBgmFading = false; //フェード中フラグを解除
        }

        Debug.LogWarning("フェードフラグを解除し、BGMを強制停止しました");
        bgmSource.Stop();
    }

    //====================================================================
    //BGMフェードイン・フェードアウト処理
    //====================================================================

    //1は最大音量、0は最小音量

    /// <summary>
    /// BGMを再生して、フェードインさせる
    /// </summary>
    /// <remarks>
    /// <para>フェードイン時間 : 1 / fadeInAmount</para>
    /// フェードインの総フレーム数 : 1 / (fadeInAmount / FPS)
    /// </remarks>
    /// <param name="fadeInAmount">値が大きいほど、早くフェードインする</param>
    /// <param name="bgmIndex">BGMの配列インデックス</param>
    public void FadeInBGM(float fadeInAmount, int bgmIndex)
    {
        //フェードイン開始
        StartCoroutine(OnFadeInBGM(fadeInAmount, bgmIndex));
    }

    /// <summary>
    /// BGMをフェードインのみさせる
    /// </summary>
    /// <remarks>
    /// <para>フェードイン時間 : 1 / fadeInAmount</para>
    /// フェードインの総フレーム数 : 1 / (fadeInAmount / FPS)
    /// </remarks>
    /// <param name="fadeInAmount">値が大きいほど、早くフェードインする</param>
    public void FadeInBGM(float fadeInAmount)
    {
        //フェードイン開始
        StartCoroutine(OnFadeInBGM(fadeInAmount));
    }

    //BGMをフェードインさせる
    //bgmIndexが指定されている場合(-1でない場合)は再生する
    private IEnumerator OnFadeInBGM(float fadeInAmount, int bgmIndex = -1)
    {
        //音量を最小にする
        bgmSource.volume = 0;

        //bgmIndexが指定されている場合は再生する
        if (bgmIndex != -1) PlayBGM(bgmIndex);

        //フェードイン開始
        isBgmFading = true;
        Debug.Log("BGMフェードイン開始");

        //フェードイン
        while (bgmSource.volume < 1)
        {
            bgmSource.volume += fadeInAmount * Time.deltaTime;
            yield return null;
        }

        //音量を最大にする
        bgmSource.volume = 1;

        //フェードイン終了
        isBgmFading = false;
    }

    /// <summary>
    /// BGMをフェードアウトさせる
    /// </summary>
    /// <remarks>
    /// <para>フェードアウト時間 : 1 / fadeOutAmount</para>
    /// フェードアウトの総フレーム数 : 1 / (fadeOutAmount / FPS)
    /// </remarks>
    /// <param name="fadeOutAmount">値が大きいほど、早くフェードアウトする</param>
    /// <param name="isStop">フェードアウト時に、BGMを停止するか</param>
    public void FadeOutBGM(float fadeOutAmount, bool isStop = true)
    {
        //フェードアウト開始
        StartCoroutine(OnFadeOutBGM(fadeOutAmount, isStop));
    }

    //BGMをフェードアウトさせる
    private IEnumerator OnFadeOutBGM(float fadeOutAmount, bool isStop)
    {
        //フェードアウト開始
        isBgmFading = true;

        //現在の音量を取得
        float startVolume = bgmSource.volume;

        //フェードアウト
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= fadeOutAmount * Time.deltaTime;
            yield return null;
        }

        //BGMを停止する場合、音量は最小にする
        if (isStop)
        {
            bgmSource.Stop();
            bgmSource.volume = startVolume;
        }

        //フェードアウト終了
        isBgmFading = false;
        Debug.Log("BGMフェードアウト終了");
    }

    //====================================================================
    //SE処理
    //====================================================================

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="seIndex">SEの配列インデックス</param>
    public void PlaySE(int seIndex)
    {
        if (seIndex < 0 || seIndex >= seClips.Length)
        {
            Debug.LogError("SEのインデックスが範囲外です");
            return;
        }

        //SEを再生する
        seSource.PlayOneShot(seClips[seIndex]);
    }

    /// <summary>
    /// SEを停止する
    /// </summary>
    public void StopSE()
    {
        seSource.Stop();
    }
}
