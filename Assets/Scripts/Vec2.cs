public struct Vec2
{
    public float x;
    public float y;

    public Vec2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vec2 zero => new Vec2(0, 0);

    public static Vec2 operator +(Vec2 a, Vec2 b)
        => new Vec2(a.x + b.x, a.y + b.y);

    public static Vec2 operator -(Vec2 a, Vec2 b)
        => new Vec2(a.x - b.x, a.y - b.y);

    public static Vec2 operator *(Vec2 a, float b)
        => new Vec2(a.x * b, a.y * b);
}