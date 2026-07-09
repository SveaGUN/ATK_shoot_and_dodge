public struct BulletPatternParam
{
    public int BulletCount;
    public int DirectionCount;
    public float AngleStep;
    public float AngleOffset;
    public float Speed;

    public BulletPatternParam(int bc, int dc, float ans, float ao, float spd)
    {
        BulletCount = bc;
        DirectionCount = dc;
        AngleStep = ans;
        AngleOffset = ao;
        Speed = spd;
    }
}
