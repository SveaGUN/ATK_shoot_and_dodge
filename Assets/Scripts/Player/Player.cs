using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private Camera targetCamera;

    [SerializeField]
    private PlayerCollider _playerCollider = null;
    [SerializeField]
    private PlayerInputHandler _playerInputHandler = null;
    [SerializeField]
    private Bar _hpBar = null;

    [Header("移動制限")]
    [SerializeField]
    private float _boundaryX = 0.03f;
    [SerializeField]
    private float _boundaryY = 0.05f;

    [Header("ステータス")]
    [SerializeField]
    private float _playerSpeed = 1f;

    [SerializeField,Tooltip("最大HP")]
    private int _maxHp = 10;
    //現在のhp
    private int _currentHp = 0;
    private int _hitCount = 0;

    public int HitCount { get => _hitCount; }

    [Header("射撃")]
    [SerializeField, Tooltip("弾の生成地点")]
    private Transform _firePoint = null;
    [SerializeField, Tooltip("発射間隔(s)")]
    private float _fireRate = 1f;
    private float _ft;

    //入力値を受け取る
    private Vector2 _moveInput = Vector2.zero;
    private Vector2 _lookInput = Vector2.zero;

    private bool _isSleep = false;
    private bool _isDead = false;

    private string _currentControlScheme = "";

    private GameStateNotifier _stateNotifier = null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init(GameStateNotifier notifier)
    {
        targetCamera = Camera.main;

        _playerCollider.Init();
        _playerInputHandler.Init();
        SetHandler();

        _currentHp = _maxHp;
        _hpBar.Init();

        _isSleep = false;
        _isDead = false;

        _stateNotifier = notifier;
    }

    /// <summary>
    /// PlayerのUpdate処理
    /// </summary>
    public void PlayerUpdate()
    {
        if (_isSleep) return;

        Move();

        KeepInScreen();

        Fire();
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        _hitCount++;

        //プレイヤーの被弾SEを再生
        AudioPlayer.instance.PlaySE(1);

        _playerCollider.StartInvincible();
        _hpBar.UpdateFillAmount(_maxHp, _currentHp);

        if (_currentHp <= 0 && !_isDead)
        {
            //Notifierに死亡を通知して、ゲームオーバー処理の実行を依頼
            _stateNotifier.NotifyGameOver();
        }
    }

    /// <summary>
    /// プレイヤー死亡時の処理
    /// </summary>
    public void OnPlayerDead()
    {
        //死亡時のSEを再生
        AudioPlayer.instance.PlaySE(2);

        _playerCollider.EndInvincible();
        _playerCollider.DisableCollider();

        _isDead = true;
        _isSleep = true;

        //キャラクター素材を子オブジェクトから取得して非表示にする
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //Destroy(gameObject);
    }

    /// <summary>
    /// クリア時の処理
    /// </summary>
    public void OnClear()
    {
        _playerCollider.EndInvincible();
        _playerCollider.DisableCollider();

        _isSleep = true;
    }


    public void SetIsSleep()
    {
        _isSleep = true;
    }

    private void Fire()
    {
        var p = targetCamera.ScreenToWorldPoint(_lookInput);

        ////マウス、ゲームパッドに応じた処理
        //bool isMouseDevice = (_currentControlScheme == "Mouse");
        //if (isMouseDevice)
        //{   //マウスポインタの座標をワールド座標に変換
        //    ;
        //}

        _ft += Time.deltaTime;
        if (_ft >= _fireRate)
        {
            var bullet = PBulletPool.Instance.GetBullet();
            bullet.transform.position = _firePoint.position;
            //マウスポインタの方向ベクトル
            var tm = p - bullet.transform.position;
            //角度をマウスポインタに合わせる
            var r = Mathf.Atan2(tm.y, tm.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, r);
            bullet.SetDirection();

            _ft = 0f;
        }
    }
    //プレイヤーの移動
    private void Move()
    {
        if (_moveInput == Vector2.zero) return;

        var p = transform.position;
        p.x += _moveInput.x * _playerSpeed * Time.deltaTime;
        p.y += _moveInput.y * _playerSpeed * Time.deltaTime;
        transform.position = p;
    }

    //画面内にプレイヤーをとどめる
    private void KeepInScreen()
    {
        var view = targetCamera.WorldToViewportPoint(transform.position);

        view.x = Mathf.Clamp(view.x, _boundaryX, 1 - _boundaryX);
        view.y = Mathf.Clamp(view.y, _boundaryY, 1 - _boundaryY);

        transform.position = targetCamera.ViewportToWorldPoint(view);
    }

    #region Handler
    private void SetHandler()
    {
        _playerInputHandler.OnMove += MoveHandler;
        _playerInputHandler.OnLook += LookHandler;
    }

    private void MoveHandler(Vector2 input)
    {
        _moveInput = input;
    }

    private void LookHandler(Vector2 input, string name)
    {
        _lookInput = input;
        _currentControlScheme = name;
    }
    #endregion
}