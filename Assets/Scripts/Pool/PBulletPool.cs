using UnityEngine;
using UnityEngine.Pool;

public class PBulletPool : MonoBehaviour, IBulletPool<PlayerBullet>
{
    public static PBulletPool Instance { get; private set; } = null;

    [SerializeField, Tooltip("オブジェクトプールで管理するオブジェクト")]
    private PlayerBullet _bullet = null;

    private ObjectPool<PlayerBullet> _pool = null;

    private void Awake()//オブジェクトプールの初期化
    {
        _pool = new ObjectPool<PlayerBullet>(
            CreatePooledObject,
            OnGet,
            OnRelease,
            OnDestory,
            false,
            15,//通常のキャパシティ
            30//maxサイズ
            );
    }

    public PlayerBullet GetBullet() => _pool.Get();

    public void ReleaseBullet(PlayerBullet obj) => _pool.Release(obj);

    //プールに空きがない時にオブジェクトを生成する
    private PlayerBullet CreatePooledObject() => Instantiate(_bullet, transform);

    //プールに空きがあるときの処理
    private void OnGet(PlayerBullet obj) => obj.gameObject.SetActive(true);

    //プールに返却するときの処理
    private void OnRelease(PlayerBullet obj) => obj.gameObject.SetActive(false);

    //Maxのサイズより大きくなった時に破棄する処理
    private void OnDestory(PlayerBullet obj)
    {
        Destroy(obj);
        obj = null;
    }
}
