using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSmovement : MonoBehaviour
{
    #region Public Variables

    public float Speed;
    public float RotationSpeed;

    #endregion

    #region Private Variables
    private Vector3 MoveDirection;

    private Rigidbody rb;
    

    #endregion

    #region InputHandling
    private PlayerControls ThisPlayerInputs;

    private void Awake() => ThisPlayerInputs = new PlayerControls();

    private void OnEnable()
    {
        ThisPlayerInputs.Enable();

        ThisPlayerInputs.PlayerInGame.Move.performed += OnMove;
        ThisPlayerInputs.PlayerInGame.Move.canceled += OnMove;
    }

    private void OnDisable() => ThisPlayerInputs.Disable();

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveAndRotate();
    }

    #region Movement&Rotation
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector3>();
    }

    public void MoveAndRotate()
    {
        rb.velocity = transform.forward * MoveDirection.z * Speed;
        transform.Rotate(new Vector3(0, MoveDirection.x, 0) * RotationSpeed);
    }
    #endregion
}