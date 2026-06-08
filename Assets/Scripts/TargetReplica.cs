using UnityEditor;
using UnityEngine;

public abstract class TargetReplica : MonoBehaviour
{
    public VoidHandler onStartCrouch;
    public VoidHandler onEndCrouch;
    public VoidHandler onJump;

    protected virtual Target target { get; }

    [SerializeField] protected Rigidbody rb;    
    [SerializeField] protected GroundCheck groundCheck;
    [SerializeField] protected Transform gunSlot;

    //Ќастройки вращени€ оружи€
    [SerializeField] protected bool useMinDistance = true;
    [SerializeField] protected float minAimDistance = 2f;
    [SerializeField] protected float maxDistance = 1000f;


    //Ќастройки приседани€    
    protected float? defaultHeadYLocalPosition; //дефолтна€ позици€ камеры, в которую надо будет вернутс€ после выхода из приседани€
    protected float crouchYHeadPosition = 1; //позици€ в которую опуститьс€ камера, при приседании
    protected CapsuleCollider colliderToLower; // коллайдер, который уменьшитьс€ в размере при преседании
    protected float? defaultColliderHeight; // дефолтный размер коллайдера до приседани€

    public bool IsRunning { get; protected set; }
    public bool IsReplicaCrouched { get; protected set; }

    protected void EquipWeapon(Weapon weapon)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>($"Weapon/{weapon.name}");
        GameObject weaponObj = GameObject.Instantiate(loadedPrefab, gunSlot);
        WeaponReplica weaponReplica = weaponObj.GetComponent<WeaponReplica>();
        weaponReplica.Init(weapon);
    }

    protected void Update()
    {
        //
    }

    private void FixedUpdate()
    {
        if (target == null) return;
        if (target.canWalk)
        {
            float targetMovingSpeed = target.isCrouch ? target.croachSpeed : (target.isRun ? target.runSpeed : target.normalSpeed);
            Vector2 targetVelocity = new Vector2(target.InputMove.x * targetMovingSpeed, target.InputMove.y * targetMovingSpeed);
            rb.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.y);
        }

    }

    protected void StartCrouch()
    {
        if (target != null)//ќпускаем камеру
        {
            DownCamera();
        }

        if (colliderToLower != null)//«анижаем коллайдер
        {
            if (!defaultColliderHeight.HasValue) defaultColliderHeight = colliderToLower.height;
            float loweringAmount;
            if (defaultHeadYLocalPosition.HasValue)
            {
                loweringAmount = defaultHeadYLocalPosition.Value - crouchYHeadPosition;
            }
            else
            {
                loweringAmount = defaultColliderHeight.Value * .5f;
            }
            colliderToLower.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
            colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
        }
        if (!IsReplicaCrouched)
        {
            IsReplicaCrouched = true;
            onStartCrouch?.Invoke();
        }
    }

    protected void StopCrouch()
    {
        if (IsReplicaCrouched)
        {
            UpCamera();
            if (colliderToLower != null) //возвращаем коллайдер обратно
            {
                colliderToLower.height = defaultColliderHeight.Value;
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }
            IsReplicaCrouched = false;
            onEndCrouch?.Invoke();
        }
    }

    protected virtual void UpCamera() { }
    protected virtual void DownCamera() { }

    protected void MakeJump()
    {
        if (!groundCheck || groundCheck.isGrounded)
        {
            float str = target.jumpStrength;
            rb.AddForce(Vector3.up * 100 * str);
            onJump?.Invoke();
        }
    }

    protected void StartRun()
    {
        IsRunning = true;
    }

    protected void StopRun()
    {
        IsRunning = false;
    }
}
