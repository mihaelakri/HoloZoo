using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        // happens when successfully connected to a Photon server
        PhotonNetwork.JoinLobby();
        Debug.Log("connected to master");
    }

    public override void OnJoinedLobby(){
        Debug.Log("joined lobby");
    }

}
