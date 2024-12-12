using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUIManager : MonoBehaviour
{
    [SerializeField] private Button hostBTTN, serverBTTN, clientBTTN;

    private void Awake()
    {
        hostBTTN.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        serverBTTN.onClick.AddListener(() => { NetworkManager.Singleton.StartServer(); });
        clientBTTN.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
    }
}
