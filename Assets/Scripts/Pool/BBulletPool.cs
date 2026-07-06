using UnityEngine;
using UnityEngine.Pool;

public class BBulletPool : MonoBehaviour
{
    //シングルトン
    private static BBulletPool instance;
    public static BBulletPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<BBulletPool>();
            }

            return instance;
        }
    }

    [SerializeField]
    [Tooltip("オブジェクトプールで管理するオブジェクト")]
    private BossBullet bullet;

    private ObjectPool<BossBullet> pool;

    private int count = 0;

    private void Awake()//オブジェクトプールの初期化
    {
        pool = new ObjectPool<BossBullet>(
            CreatePooledItem,
            TakeFromPool,
            ReturnedToPool,
            DestroyPooledObject,
            false,
            75,//通常のキャパシティ
            300//maxサイズ
            );
    }

    private BossBullet CreatePooledItem()//プールに空きがない時にオブジェクトを生成する
    {
        count++;
        return Instantiate(bullet, transform);
    }

    private void TakeFromPool(BossBullet obj)//プールに空きがあるときの処理
    {
        obj.gameObject.SetActive(true);
    }

    private void ReturnedToPool(BossBullet obj)//プールに返却するときの処理
    {
        obj.gameObject.SetActive(false);
    }

    private void DestroyPooledObject(BossBullet obj)//Maxのサイズより大きくなった時に破棄する処理
    {
        count--;
        Destroy(obj);
    }

    public BossBullet GetBullet()
    {
        return pool.Get();
    }

    public void ReleaseBullet(BossBullet obj)
    {
        pool.Release(obj);
    }

    private void FixedUpdate()
    {
        //Debug.Log("enemy bullets : " + count);
    }
}
