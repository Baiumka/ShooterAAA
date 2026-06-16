using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class PlayerReplica : TargetReplica
{
    private const float WEAPON_RAY_DISTANCE = 300;
    private Player player;
    

    private Vector2 velocity;
    private Vector2 frameVelocity;
    
    private RectTransform crosshair;
    protected override Target target { get => player; }
    [SerializeField] private Camera headCameraToLower;

    //��������� � ���������
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;
        

    public new void Awake()
    {
        base.Awake();
        RectTransform crosshair = GameObject.Find("CrosshairPoint").GetComponent<RectTransform>();
        if(crosshair != null )
        {
            this.crosshair = crosshair;
        }
        else
        {
            Debug.LogError("�rosshair not exist");

        }
    }

    public void Init(Player player)
    {
        this.player = player;
        base.Init(player);           
        this.player.onJump += MakeJump;
        this.player.onStartCrouch += StartCrouch;
        this.player.onStopCrouch += StopCrouch;
        this.player.onStartRun += StartRun;
        this.player.onStopRun += StopRun;

        Debug.Log($"Player Inited: {this.player.Name}");
    }

    private new void OnDestroy()
    {
        base.OnDestroy();      
        this.player.onJump -= MakeJump;
        this.player.onStartCrouch -= StartCrouch;
        this.player.onStopCrouch -= StopCrouch;
        this.player.onStartRun -= StartRun;
        this.player.onStopRun -= StopRun;
        this.player = null;
    }

   

    private new void Update()
    {
        base.Update();
        if (headCameraToLower != null)
        {
            Vector2 mouseDelta = new Vector2(player.MouseDelta.x, player.MouseDelta.y);   
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            headCameraToLower.transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

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
                if(weaponReplica != null)
                {
                    Vector3 screenPos = headCameraToLower.WorldToScreenPoint(weaponReplica.targetPoint);
                    crosshair.position = screenPos;

                    if (ReferenceEquals(weaponReplica.foundTarget, weaponReplica.currentTarget))
                        return;
                    weaponReplica.currentTarget?.SetHighlighted(false);
                    weaponReplica.currentTarget = weaponReplica.foundTarget;
                    weaponReplica.currentTarget?.SetHighlighted(true);
                }                
            }
        }        
    }

    private ITargetable FindTarget()
    {
        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            return hit.collider.GetComponentInParent<ITargetable>();
        }

        return null;
    }


    protected override void DownCamera() 
    {
        if (!defaultHeadYLocalPosition.HasValue) defaultHeadYLocalPosition = headCameraToLower.transform.localPosition.y;
        headCameraToLower.transform.localPosition = new Vector3(headCameraToLower.transform.localPosition.x, crouchYHeadPosition, headCameraToLower.transform.localPosition.z);
    }
    protected override void UpCamera()
    {
        if (headCameraToLower != null) //����������� ������ �������
        {
            headCameraToLower.transform.localPosition = new Vector3(headCameraToLower.transform.localPosition.x, defaultHeadYLocalPosition.Value, headCameraToLower.transform.localPosition.z);
        }
    }


}