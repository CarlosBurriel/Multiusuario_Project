using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PHPHandler : NetworkBehaviour
{
    //Get PlayerUsername from when login
    [HideInInspector] public string PlayerUsername;

    [HideInInspector] public int Kills;

    [HideInInspector] public int Deaths;

    


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GameManager.Instance.Players.Add(this);
        UpdatePlayerCountServerRPC();
        PlayerUsername = pasableusername.instance.username;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerCountServerRPC()
    {
        GameManager.Instance.NumberOfPlayers.Value++;
    }
}
