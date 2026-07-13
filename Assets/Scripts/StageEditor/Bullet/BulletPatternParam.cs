
public struct BulletPatternParam
{
    public int BulletCount;
    public int DirectionCount;
    public float BaseAngle;
    public float AngleStep;
    public float AngleOffset;
    public float Speed;

    public BulletPatternParam(int bc, int dc, float ba, float ans, float ao, float spd)
    {
        BulletCount = bc;
        DirectionCount = dc;
        BaseAngle = ba;
        AngleStep = ans;
        AngleOffset = ao;
        Speed = spd;
    }
}
