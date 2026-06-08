using UnityEngine;

public class WeaponReplica : MonoBehaviour
{
    private Weapon weapon;

    public void Init(Weapon weapon)
    {
        this.weapon = weapon;
    }
}
