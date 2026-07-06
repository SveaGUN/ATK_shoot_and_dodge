using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PBulletPool : MonoBehaviour
{
    //シングルトン
    private static PBulletPool instance;
    public static PBulletPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<PBulletPool>();
            }

            return instance;
        }
    }

    [SerializeField]
    [Tooltip("オブジェクトプールで管理するオブジェクト")]
    private PlayerBullet bullet;

    private ObjectPool<PlayerBullet> pool;

    private void Awake()//オブジェクトプールの初期化
    {
        pool = new ObjectPool<PlayerBullet>(
            CreatePooledItem,
            TakeFromPool,
            ReturnedToPool,
            DestroyPooledObject,
            false,
            15,//通常のキャパシティ
            30//maxサイズ
            );
    }

    private PlayerBullet CreatePooledItem()//プールに空きがない時にオブジェクトを生成する
    {
        return Instantiate(bullet, transform);
    }

    private void TakeFromPool(PlayerBullet obj)//プールに空きがあるときの処理
    {
        obj.gameObject.SetActive(true);
    }

    private void ReturnedToPool(PlayerBullet obj)//プールに返却するときの処理
    {
        obj.gameObject.SetActive(false);
    }

    private void DestroyPooledObject(PlayerBullet obj)//Maxのサイズより大きくなった時に破棄する処理
    {
        Destroy(obj);
    }

    public PlayerBullet GetBullet()
    {
        return pool.Get();
    }

    public void ReleaseBullet(PlayerBullet obj)
    {
        pool.Release(obj);
    }
}
