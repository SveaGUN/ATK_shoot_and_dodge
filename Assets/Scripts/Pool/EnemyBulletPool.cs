using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour, IBulletPool<EnemyBullet>
{
    public static EnemyBulletPool Instance { get; private set; } = null;

    [SerializeField, Tooltip("オブジェクトプールで管理するオブジェクト")]
    private EnemyBullet _bullet = null;

    private ObjectPool<EnemyBullet> _pool = null;

    private void Awake()//オブジェクトプールの初期化
    {
        if(Instance == null) { Instance = this; }

        _pool = new ObjectPool<EnemyBullet>(
            CreatePooledObject,
            OnGet,
            OnRelease,
            OnDestory,
            false,
            15,//通常のキャパシティ
            30//maxサイズ
            );
    }

    public EnemyBullet GetBullet() => _pool.Get();

    public void ReleaseBullet(EnemyBullet obj) => _pool.Release(obj);

    //プールに空きがない時にオブジェクトを生成する
    private EnemyBullet CreatePooledObject() => Instantiate(_bullet, transform);

    //プールに空きがあるときの処理
    private void OnGet(EnemyBullet obj) => obj.gameObject.SetActive(true);

    //プールに返却するときの処理
    private void OnRelease(EnemyBullet obj) => obj.gameObject.SetActive(false);

    //Maxのサイズより大きくなった時に破棄する処理
    private void OnDestory(EnemyBullet obj)
    {
#if UNITY_EDITOR
        if (Application.isPlaying) { Destroy(obj); }
        else { DestroyImmediate(obj); }
#else
        Destroy(obj);
#endif
        obj = null;
    }
}
