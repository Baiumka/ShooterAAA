using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class PlayerReplica : TargetReplica, ITargetable
{    
    private Player player;
    

    private Vector2 velocity;
    private Vector2 frameVelocity;
    
    private RectTransform crosshair;

    [SerializeField] private Camera headCameraToLower;    
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;

    protected override Target target { get => player; }
    public Vector3 Position { get => transform.position; }

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

    protected override Ray GetForwardRay()
    {
        return headCameraToLower.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }


    public new void TakeDamage(int hp)
    {
        Debug.Log($"{player.Name} take damage");
        base.TakeDamage(hp);

    }


    protected override void DownCamera() 
    {
        if (!defaultHeadYLocalPosition.HasValue) defaultHeadYLocalPosition = headCameraToLower.transform.localPosition.y;
        headCameraToLower.transform.localPosition = new Vector3(headCameraToLower.transform.localPosition.x, crouchYHeadPosition, headCameraToLower.transform.localPosition.z);
    }
    protected override void UpCamera()
    {
        if (headCameraToLower != null) 
        {
            headCameraToLower.transform.localPosition = new Vector3(headCameraToLower.transform.localPosition.x, defaultHeadYLocalPosition.Value, headCameraToLower.transform.localPosition.z);
        }
    }


}