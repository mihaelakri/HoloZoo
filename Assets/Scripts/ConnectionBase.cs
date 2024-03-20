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
    protected float updateFrequency = 1f / 20f;
    protected float timeSinceLastUpdate;
    protected float lastUpdateTime = 0f;
    protected bool skipSending = false;
    protected int player_id;
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
        switch (msgType)
        {
            case CommunicationMsgs.StateMsg a:
                reliableDelay(1);
                msg = "s";
                msg += JsonConvert.SerializeObject(CommConstants.state, Formatting.None);
                break;

            case CommunicationMsgs.RotationMsg b:
                unreliableThrottle();
                msg = "r";
                msg += JsonConvert.SerializeObject(CommConstants.rotation, Formatting.None);
                break;

            case CommunicationMsgs.AnimalIdMsg c:
                reliableDelay(50);
                msg = "a";
                msg += JsonConvert.SerializeObject(CommConstants.animalid, Formatting.None);
                break;

            case CommunicationMsgs.LeapTimeMsg l:
                reliableDelay(1);
                msg = "l";
                msg += JsonConvert.SerializeObject(CommConstants.leapTimeMsg, Formatting.None);
                break;

            case CommunicationMsgs.RequestLeapTimeMsg q:
                reliableDelay(100);
                msg = "q";
                msg += JsonConvert.SerializeObject(CommConstants.requestLeapTimeMsg, Formatting.None);
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
                // System.Diagnostics.Debug.WriteLine("New State: " + JsonConvert.SerializeObject(CommConstants.state));
                break;
            case "r":
                RotationMsg rotationMsg = JsonConvert.DeserializeObject<RotationMsg>(msg[1..]);
                CommConstants.rotation.UpdateMsgFromOtherThread(rotationMsg);
                // System.Diagnostics.Debug.WriteLine("New Rotation: " + JsonConvert.SerializeObject(CommConstants.rotation));
                break;
            case "a":
                AnimalIdMsg animalIdMsg = JsonConvert.DeserializeObject<AnimalIdMsg>(msg[1..]);
                CommConstants.animalid.UpdateMsgFromOtherThread(animalIdMsg);
                // System.Diagnostics.Debug.WriteLine("New animal: " + JsonConvert.SerializeObject(CommConstants.animalid));
                break;
            case "l":
                LeapTimeMsg leapTimeMsg = JsonConvert.DeserializeObject<LeapTimeMsg>(msg[1..]);
                CommConstants.leapTimeMsg.UpdateMsgFromOtherThread(leapTimeMsg);
                // System.Diagnostics.Debug.WriteLine("New leap time: " + JsonConvert.SerializeObject(CommConstants.leapTimeMsg));
                break;
            case "q":
                RequestLeapTimeMsg requestLeapTimeMsg = JsonConvert.DeserializeObject<RequestLeapTimeMsg>(msg[1..]);
                CommConstants.requestLeapTimeMsg.UpdateMsgFromOtherThread(requestLeapTimeMsg);
                // System.Diagnostics.Debug.WriteLine("New RequestLeapTimeMsg: " + JsonConvert.SerializeObject(CommConstants.requestLeapTimeMsg));
                break;
            default:
                break;
        }
    }

    // Essentially creates a separate throttled stream
    private void unreliableThrottle()
    {
        timeSinceLastUpdate = Time.time - lastUpdateTime;

        if (timeSinceLastUpdate >= updateFrequency)
        {
            skipSending = false;
            lastUpdateTime = Time.time;
        }
        else
        {
            skipSending = true;
        }
    }

    private void reliableDelay(int delay)
    {
        Thread.Sleep(delay);
        skipSending = false;
    }

}