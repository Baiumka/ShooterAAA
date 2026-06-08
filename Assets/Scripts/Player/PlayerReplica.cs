using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class PlayerReplica : MonoBehaviour
{
    private const float WEAPON_RAY_DISTANCE = 300;

    private Player player;
    private Vector2 velocity;
    private Vector2 frameVelocity;   
    
    public VoidHandler onStartCrouch;
    public VoidHandler onEndCrouch;
    public VoidHandler onJump;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera headCameraToLower;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private Transform gunSlot;

    public bool IsRunning { get; private set; }
    public bool IsReplicaCrouched { get; private set; }

    //Перенести в настройки
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;

    //Настройки вращения оружия
    [SerializeField] private bool useMinDistance = true;
    [SerializeField] private float minAimDistance = 2f;
  
    [SerializeField] private RectTransform crosshair;   
    [SerializeField] private float maxDistance = 1000f;

    //Настройки приседания    
    private float? defaultHeadYLocalPosition; //дефолтная позиция камеры, в которую надо будет вернутся после выхода из приседания
    private float crouchYHeadPosition = 1; //позиция в которую опуститься камера, при приседании
    private CapsuleCollider colliderToLower; // коллайдер, который уменьшиться в размере при преседании
    private float? defaultColliderHeight; // дефолтный размер коллайдера до приседания

    public void Awake()
    {
        RectTransform crosshair = GameObject.Find("crosshair").GetComponent<RectTransform>();
        if(crosshair != null )
        {
            this.crosshair = crosshair;
        }
        else
        {
            Debug.LogError("Сrosshair not exist");

        }
    }

    public void Init(Player player)
    {
        this.player = player;
        this.player.onWeaponEquip += EquipWeapon;
        this.player.onJump += MakeJump;
        this.player.onStartCrouch += StartCrouch;
        this.player.onStopCrouch += StopCrouch;
        this.player.onStartRun += StartRun;
        this.player.onStopRun += StopRun;

        Debug.Log($"Player Inited: {this.player.Name}");
    }

    private void OnDestroy()
    {
        this.player.onWeaponEquip -= EquipWeapon;
        this.player.onJump -= MakeJump;
        this.player.onStartCrouch -= StartCrouch;
        this.player.onStopCrouch -= StopCrouch;
        this.player.onStartRun -= StartRun;
        this.player.onStopRun -= StopRun;
        this.player = null;
    }

    private void EquipWeapon(Weapon weapon)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>($"Weapon/{weapon.name}");
        GameObject weaponObj = GameObject.Instantiate(loadedPrefab, gunSlot);
        WeaponReplica weaponReplica = weaponObj.GetComponent<WeaponReplica>();
        weaponReplica.Init(weapon);
    }

    private void Update()
    {
        if (headCameraToLower != null)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            headCameraToLower.transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        }

        Ray ray = headCameraToLower.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, WEAPON_RAY_DISTANCE))
        {
            Vector3 aimPoint = hit.point;
            if (useMinDistance)
            {
                Vector3 cameraPos = headCameraToLower.transform.position;
                if (Vector3.Distance(cameraPos, aimPoint) < minAimDistance)
                {
                    aimPoint = cameraPos + headCameraToLower.transform.forward * minAimDistance;
                }
            }
            Vector3 dir = (aimPoint - gunSlot.position).normalized;
            gunSlot.rotation = Quaternion.LookRotation(dir);
        }

        if (crosshair != null)
        {
            Vector3 targetPoint;
            Ray gunRay = new Ray(gunSlot.position, gunSlot.forward);
            if (Physics.Raycast(gunRay, out RaycastHit gunHit, maxDistance))
            {
                targetPoint = gunHit.point;
            }
            else
            {
                targetPoint = gunRay.GetPoint(maxDistance);
            }
            Vector3 screenPos = headCameraToLower.WorldToScreenPoint(targetPoint);
            crosshair.position = screenPos;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;           
        Vector2 targetVelocity = new Vector2(player.moveHorz, player.moveVert);
        rb.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.y);
    }

    private void StartCrouch()
    {
        if (headCameraToLower != null)//Опускаем камеру
        {
            if (!defaultHeadYLocalPosition.HasValue) defaultHeadYLocalPosition = headCameraToLower.transform.localPosition.y;
            headCameraToLower.transform.localPosition = new Vector3(headCameraToLower.transform.localPosition.x, crouchYHeadPosition, headCameraToLower.transform.localPosition.z);
        }

        if (colliderToLower != null)//Занижаем коллайдер
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

    private void StopCrouch()
    {
        if (IsReplicaCrouched)
        {
            if (headCameraToLower != null) //вовзвращаем камеру обратно
            {
                headCameraToLower.transform.localPosition = new Vector3(headCameraToLower.transform.localPosition.x, defaultHeadYLocalPosition.Value, headCameraToLower.transform.localPosition.z);
            }
            if (colliderToLower != null) //возвращаем коллайдер обратно
            {
                colliderToLower.height = defaultColliderHeight.Value;
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }
            IsReplicaCrouched = false;
            onEndCrouch?.Invoke();
        }
    }

    private void MakeJump()
    {
        if (!groundCheck || groundCheck.isGrounded)
        {
            float str = player.jumpStrength;
            rb.AddForce(Vector3.up * 100 * str);
            onJump?.Invoke();
        }
    }

    private void StartRun()
    {
        IsRunning = true;
    }

    private void StopRun()
    {
        IsRunning = false;
    }
}