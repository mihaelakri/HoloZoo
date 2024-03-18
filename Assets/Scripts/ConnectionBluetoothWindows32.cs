using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using CommunicationMsgs;

public class ConnectionBluetoothWindows32 : ConnectionBase
{
    private NetworkStream stream;
    private Thread clientThread;
    private float updateFrequency = 1f/20f; 
    public override void Initialize()
    {
        base.Initialize();
        Task.Run(async () => { await Initialize_BT(); });
    }

    public override void SendData(CommunicationMsgs.CommunicationMsg msgType)
    {
        float timeSinceLastUpdate = Time.time - lastUpdateTime;
        
        if (timeSinceLastUpdate >= updateFrequency)
        {
            base.SendData(msgType);

            byte[] array = Encoding.UTF8.GetBytes(base.msg);
            array = ModAddArray(array, 2);
            sbyte[] sBytes = ByteToSByteArray(array);

            stream.Write(array, 0, array.Length);
            System.Diagnostics.Debug.WriteLine("BluetoothWindows - Send Data");
        }
    }

    public async Task Initialize_BT()
    {
        System.Diagnostics.Debug.WriteLine("BluetoothWindows - Initialize_BT");

        var lsnr = new BluetoothListener(new Guid("d81a5833-37f4-460d-8a9f-347ff95474ad"));
        lsnr.Start();

        BluetoothClient client = lsnr.AcceptBluetoothClient();
        System.Diagnostics.Debug.WriteLine("BluetoothWindows - Accepted BT Client");

        // Handle the client in a separate thread
        clientThread = new Thread(() => HandleClient(client));
        clientThread.Start();
    }

    protected void HandleClient(BluetoothClient client)
    {
        try
        {
            stream = client.GetStream();
            System.Diagnostics.Debug.WriteLine("BluetoothWindows - Got Stream");

            while (stream.CanRead)
            {
                // Read header
                byte[] msgHeader = new byte[5];
                if (stream.Read(msgHeader, 0, msgHeader.Length) <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("BluetoothWindows - Failed reading msg header");
                    break;
                }
                byte[] msgHeaderLength = new byte[4];
                Array.Copy(msgHeader, 0, msgHeaderLength, 0, 4);
                int msgType = msgHeader[4];

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(msgHeaderLength, 0, sizeof(int));
                    System.Diagnostics.Debug.WriteLine("Reversed msgHeaderLength");
                }
                int msgLength = BitConverter.ToInt32(msgHeaderLength, 0);
                System.Diagnostics.Debug.WriteLine("Msg Length: " + msgLength + " Msg Type: " + msgType);

                // Read message
                byte[] buffer = new byte[msgLength];
                int bytesRead;

                if ((bytesRead = stream.Read(buffer, 0, buffer.Length)) <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("BluetoothWindows - Failed reading msg body");
                    break;
                }

                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                System.Diagnostics.Debug.WriteLine("BluetoothWindows - Received data: " + data);


                //StateMsg stateMsg = JsonUtility.FromJson<StateMsg>(data);
                // StateMsg stateMsg = JsonConvert.DeserializeObject<StateMsg>(data);
                // CommConstants.state = stateMsg;
                base.ParseAndStoreMsg(data);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("BluetoothWindows - Error handling Bluetooth client: " + ex.Message);
        }
        finally
        {
            client.Close();
            System.Diagnostics.Debug.WriteLine("BluetoothWindows - Client disconnected.");
        }
    }

    private byte[] ModAddArray(byte[] array, int typeData)
    {
        byte[] lengthArray = BitConverter.GetBytes(array.Length + 1);
        byte[] newArray = new byte[array.Length + 5];
        newArray[0] = lengthArray[3];
        newArray[1] = lengthArray[2];
        newArray[2] = lengthArray[1];
        newArray[3] = lengthArray[0];
        newArray[4] = (byte)typeData;
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i + 5] = array[i];
        }
        return newArray;
    }

    private sbyte[] ByteToSByteArray(byte[] array)
    {
        sbyte[] sBytes = new sbyte[array.Length];
        Buffer.BlockCopy(array, 0, sBytes, 0, array.Length);
        return sBytes;
    }

}