public struct Vec3
{
    public float x;
    public float y;
    public float z;

    public Vec3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vec3 zero => new Vec3(0, 0, 0);

    public static Vec3 operator +(Vec3 a, Vec3 b)
        => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Vec3 operator -(Vec3 a, Vec3 b)
        => new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);

    public static Vec3 operator *(Vec3 a, float b)
        => new Vec3(a.x * b, a.y * b, a.z * b);
}