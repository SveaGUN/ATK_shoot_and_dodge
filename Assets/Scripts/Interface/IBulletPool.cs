public interface IBulletPool<T>
{
    public T GetBullet();

    public void ReleaseBullet(T obj);
}
