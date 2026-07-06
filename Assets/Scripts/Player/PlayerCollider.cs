using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    //プレイヤーのコライダー
    private CircleCollider2D playerCollider = null;

    [SerializeField]
    [Tooltip("無敵状態のシールド")]
    private SpriteRenderer invincibleShield = null;

    [SerializeField]
    [Tooltip("無敵時間")]
    private float invincibleTime = 1f;

    private WaitForSeconds waitInvincible = null;

    Coroutine invincible = null;

    //初期化処理
    public void Init()
    {
        playerCollider = GetComponent<CircleCollider2D>();
        playerCollider.enabled = true;

        waitInvincible = new(invincibleTime);

        invincibleShield.enabled = false;
    }

    /// <summary>
    /// プレイヤーのコライダーを無効にする
    /// </summary>
    public void DisableCollider()
    {
        playerCollider.enabled = false;
    }

    /// <summary>
    /// 無敵時間を開始する。自動で終了する
    /// </summary>
    public void StartInvincible()
    {
        if (playerCollider.enabled)
        {
            invincible = StartCoroutine(Invincible());
        }
    }

    /// <summary>
    /// 無敵時間を止める
    /// </summary>
    public void EndInvincible()
    {
        if(invincible == null) return;

        StopCoroutine(invincible);

        invincible = null;
    }

    private IEnumerator Invincible()
    {
        playerCollider.enabled = false;
        invincibleShield.enabled = true;

        yield return waitInvincible;

        invincibleShield.enabled = false;
        playerCollider.enabled = true;

        invincible = null;
    }
}
