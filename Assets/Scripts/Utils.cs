using UnityEngine;

public static class Utils
{
    public static Vector3 ToUnity(Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Vec3 ToModel(Vector3 v)
    {
        return new Vec3(v.x, v.y, v.z);
    }

    public static Quaternion ToUnity(Rot r)
    {
        return Quaternion.Euler(0f, r.yaw, 0f);
    }

    public static Rot ToModel(Quaternion q)
    {
        return new Rot(q.eulerAngles.y);
    }
}