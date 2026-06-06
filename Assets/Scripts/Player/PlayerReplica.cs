using TMPro;
using UnityEngine;


public class PlayerReplica : MonoBehaviour
{
    private Player player;
    private Vector2 velocity;
    private Vector2 frameVelocity;   
    
    public VoidHandler onStartCrouch;
    public VoidHandler onEndCrouch;
    public VoidHandler onJump;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera headCameraToLower;
    [SerializeField] GroundCheck groundCheck;

    public bool IsRunning { get; private set; }
    public bool IsReplicaCrouched { get; private set; }

    //Перенести в настройки
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runningKey = KeyCode.LeftShift;

    //Настройки приседания    
    private float? defaultHeadYLocalPosition; //дефолтная позиция камеры, в которую надо будет вернутся после выхода из приседания
    private float crouchYHeadPosition = 1; //позиция в которую опуститься камера, при приседании
    private CapsuleCollider colliderToLower; // коллайдер, который уменьшиться в размере при преседании
    private float? defaultColliderHeight; // дефолтный размер коллайдера до приседания

    public void Init(Player player)
    {
        this.player = player;
        Debug.Log($"Player Inited: {this.player.Name}");
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

        if (Input.GetKey(crouchKey))
        {
            ApplyCrouch(true);
        }
        else
        {
            ApplyCrouch(false);
        }

        if (Input.GetKey(runningKey))
        {
            ApplyRun(true);
        }
        else
        {
            ApplyRun(false);
        }
        if (Input.GetKeyDown(jumpKey))
        {
            MakeJump();
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;   
        float targetMovingSpeed = IsReplicaCrouched ? player.croachSpeed : (IsRunning ? player.runSpeed : player.normalSpeed);
        
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        rb.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.y);
    }

    private void ApplyCrouch(bool cruched)
    {
        if (cruched)//Команда сесеть
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
        else//Команда встать
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

    private void ApplyRun(bool newValue)
    {
        IsRunning = newValue;
    }
}