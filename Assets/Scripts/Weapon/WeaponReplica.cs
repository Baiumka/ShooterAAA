using System;
using UnityEngine;

public class WeaponReplica : MonoBehaviour
{
    private Weapon weapon;
    public Vector3 targetPoint;
    public ITargetable currentTarget;
    public ITargetable foundTarget;
    [SerializeField] private Transform weaponMuzle;
    [SerializeField] private LayerMask hitMask;

    public void Init(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.onShot += Fire;
    }
    private void OnDestroy()
    {
        this.weapon.onShot -= Fire;
    }

    private void Fire()
    {
        GameObject loadedPrefab = Resources.Load<GameObject>($"Bullet");
        Vector3 muzlePosition = weaponMuzle.transform.position;
        GameObject bulletObject = GameObject.Instantiate(loadedPrefab, muzlePosition, new Quaternion());        
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        
        bullet.Init(targetPoint, weapon.buleltSpeed);
    }

    private void Update()
    {
        if (weapon != null)
        {            
            Ray gunRay = new Ray(weaponMuzle.position, weaponMuzle.forward);
            if (Physics.Raycast(gunRay, out RaycastHit gunHit, weapon.maxDistance, hitMask))
            {
                targetPoint = gunHit.point;
                foundTarget = gunHit.collider.GetComponentInParent<ITargetable>();
            }
            else
            {
                targetPoint = gunRay.GetPoint(weapon.maxDistance);
                foundTarget = null;
            }
            
        }
    }


}
