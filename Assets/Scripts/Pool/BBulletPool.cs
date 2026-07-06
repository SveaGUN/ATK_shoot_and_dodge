using UnityEngine;
using UnityEngine.Pool;

public class BBulletPool : MonoBehaviour, IBulletPool<BossBullet>
{
    public static BBulletPool Instance { get; private set; } = null;

    [SerializeField, Tooltip("オブジェクトプールで管理するオブジェクト")]
    private BossBullet _bullet = null;

    private ObjectPool<BossBullet> _pool = null;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }

        //オブジェクトプールの初期化
        _pool = new ObjectPool<BossBullet>(
            CreatePooledObject,
            OnGet,
            OnRelease,
            OnDestory,
            false,
            75,//通常のキャパシティ
            300//maxサイズ
            );
    }

    public BossBullet GetBullet() => _pool.Get();

    public void ReleaseBullet(BossBullet obj) => _pool.Release(obj);

    //プールに空きがない時にオブジェクトを生成する
    private BossBullet CreatePooledObject() => Instantiate(_bullet, transform);

    //プールに空きがあるときの処理
    private void OnGet(BossBullet obj) => obj.gameObject.SetActive(true);

    //プールに返却するときの処理
    private void OnRelease(BossBullet obj)=> obj.gameObject.SetActive(false);

    //Maxのサイズより大きくなった時に破棄する処理
    private void OnDestory(BossBullet obj)
    {
        Destroy(obj);
        obj = null;
    }
}
