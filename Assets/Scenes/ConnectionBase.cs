using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;
using CommunicationMsgs;
using Newtonsoft.Json;

public class ConnectionBase : MonoBehaviour
{
    protected int player_id;
    protected float lastUpdateTime = 0f;
    protected string msg = "";

    public void Start()
    {
        player_id = PlayerPrefs.GetInt("ID");
    }

    public virtual void Initialize()
    {
    }

    public virtual void SendData(CommunicationMsgs.CommunicationMsg msgType)
    {
        switch(msgType)
        {
            case CommunicationMsgs.StateMsg a:
            msg = "s";
            msg += JsonConvert.SerializeObject(CommConstants.state, Formatting.None);
            break;

            case CommunicationMsgs.RotationMsg b:
            msg = "r";
            msg += JsonConvert.SerializeObject(CommConstants.rotation, Formatting.None);
            break;

            case CommunicationMsgs.AnimalIdMsg c:
            msg = "a";
            msg += JsonConvert.SerializeObject(CommConstants.animalid, Formatting.None);
            break;

            default:
            break;
        }
    }

    public void ParseAndStoreMsg(string msg)
    {
        string header = msg[0].ToString();

        switch (header)
        {
            case "s":
                StateMsg stateMsg = JsonConvert.DeserializeObject<StateMsg>(msg[1..]);
                CommConstants.state.UpdateMsgFromOtherThread(stateMsg);
                System.Diagnostics.Debug.WriteLine("New State: "+JsonConvert.SerializeObject(CommConstants.state));
                break;

            case "r":
                RotationMsg rotationMsg = JsonConvert.DeserializeObject<RotationMsg>(msg[1..]);
                CommConstants.rotation.UpdateMsgFromOtherThread(rotationMsg);
                System.Diagnostics.Debug.WriteLine("New Rotation: "+JsonConvert.SerializeObject(CommConstants.rotation));
                break;
            case "a":
                AnimalIdMsg animalIdMsg = JsonConvert.DeserializeObject<AnimalIdMsg>(msg[1..]);
                CommConstants.animalid.UpdateMsgFromOtherThread(animalIdMsg);
                System.Diagnostics.Debug.WriteLine("New animal: "+JsonConvert.SerializeObject(CommConstants.animalid));
            break;

            default:
                break;
        }
    }


}