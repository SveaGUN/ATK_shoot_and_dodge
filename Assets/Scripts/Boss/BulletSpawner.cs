using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance { get; private set; } = null;
    private FireSystem<EnemyBullet> _fireSystem = null;

    private void Start()
    {
        if(Instance == null) {  Instance = this; }

        _fireSystem = new(transform, GameObject.FindWithTag("Player").transform, transform, EnemyBulletPool.Instance);
    }

    public void Spawn(string name, BulletPatternParam param)
    {
        switch (name)
        {
            case "a":
                _fireSystem.FireBurstCircle(param.BulletCount, param.DirectionCount, param.AngleStep, param.AngleOffset, param.Speed);
                break;

            default:
                break;
        }
    }
}
