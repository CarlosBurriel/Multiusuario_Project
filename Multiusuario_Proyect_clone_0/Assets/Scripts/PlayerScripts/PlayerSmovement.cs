using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSmovement : NetworkBehaviour
{
    #region Public Variables

    public float Speed = 12;
    public float RotationSpeed;

    #endregion

    #region Private Variables
    private Vector3 MoveDirection;

    private Rigidbody rb;

    private CinemachineVirtualCamera Vcam;
    private AudioListener listener;

    #endregion

    private PlayerAnimHandler playerAnimHandler;

    #region InputHandling
    private PlayerControls ThisPlayerInputs;

    private void OnDisable() => ThisPlayerInputs.Disable();

    #endregion

  

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
      
            listener.enabled = true;
            Vcam.Priority = 100;

            #region Input Handling

            ThisPlayerInputs = new PlayerControls();
            ThisPlayerInputs.Enable();

            ThisPlayerInputs.PlayerInGame.Move.performed += OnMove;
            ThisPlayerInputs.PlayerInGame.Move.canceled += OnMove;
            #endregion
        }
        else
        {
            Vcam.Priority = 0;
            return;
        }
    }

    private void Awake()
    {
        #region Get Components
        rb = GetComponent<Rigidbody>();

        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();

        Vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        listener = GetComponentInChildren<AudioListener>();
        #endregion

    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return; }
        MoveAndRotate(MoveDirection);
    }

    #region Movement&Rotation
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector3>();
       
    }

    public void MoveAndRotate(Vector3 MoveDirection)
    {
        rb.velocity = transform.forward * MoveDirection.z * Speed;
        transform.Rotate(new Vector3(0, MoveDirection.x, 0) * RotationSpeed);

        if (MoveDirection == Vector3.zero && playerAnimHandler.state != PlayerAnimHandler.PlayerState.ATTACK) { playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.IDLE); }
        else if (playerAnimHandler.state != PlayerAnimHandler.PlayerState.ATTACK) { playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.MOVEMENT); }
    }

    #endregion
}
