using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PHPHandler : NetworkBehaviour
{
    //Get PlayerUsername from when login
    [HideInInspector] public string PlayerUsername;

    public NetworkVariable<int> Deaths = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> Kills = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GameManager.Instance.Players.Add(this);
        UpdatePlayerCountServerRPC();
        PlayerUsername = PasableUsername.instance.username;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerCountServerRPC()
    {
        GameManager.Instance.NumberOfPlayers.Value++;
    }
}
