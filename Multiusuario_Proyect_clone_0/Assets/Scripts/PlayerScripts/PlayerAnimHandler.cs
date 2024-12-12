using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimHandler : NetworkBehaviour
{
    [HideInInspector]public PlayerState state;

    private Animator myanim;

    private Rigidbody rb;

    private void Start()
    {
        state = PlayerState.IDLE;

        rb = GetComponentInParent<Rigidbody>();
        myanim = GetComponent<Animator>();
    }

   public void UpdateState(PlayerState CurrentState)
    {
        state = CurrentState;

        BoolChangeMethod();

        switch(state)
        {
            case PlayerState.IDLE:
                myanim.SetBool("Idling", true);
                break;
            case PlayerState.MOVEMENT:
                myanim.SetBool("Running", true);
                break;
            case PlayerState.ATTACK:
                myanim.SetBool("Attacking", true);
                break;
            case PlayerState.DEATH:
                myanim.SetBool("Dead", true);
                break;
        }
    }

    public void BoolChangeMethod()
    {
        foreach(AnimatorControllerParameter parameter in myanim.parameters)
        {
            myanim.SetBool(parameter.name, false);
        }
    }


    public void AttackToIdle()
    {
        if (rb.velocity == Vector3.zero) { UpdateState(PlayerState.IDLE); } else {UpdateState(PlayerState.MOVEMENT); }
    }
    

    public enum PlayerState { IDLE, MOVEMENT, ATTACK, DEATH }
}
