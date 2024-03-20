#if !UNITY_EDITOR && UNITY_WSA
#define IS_UWP_APP
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using CommunicationMsgs;

#if IS_UWP_APP
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth;
#endif

public class ConnectionBluetoothWindowsUWP : ConnectionBase
{
    protected bool is_BTConnected = false;
    protected string paired_BT_server;
    public override void Initialize()
    {
        base.Initialize();
#if IS_UWP_APP
        Task.Run(async () => {await Initialize_BT();});
            System.Diagnostics.Debug.WriteLine("BluetoothWindowsUWP - Initialized");
#endif
    }

    public override void SendData(CommunicationMsgs.CommunicationMsg msgType)
    {
        base.SendData(msgType);
#if IS_UWP_APP
            System.Diagnostics.Debug.WriteLine("BluetoothWindowsUWP - Send Data");
#endif
    }



#if IS_UWP_APP
    Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider _provider;
    Windows.Networking.Sockets.StreamSocket _socket;
#endif

    async Task Initialize_BT()
    {
#if IS_UWP_APP
        // Initialize the provider for the hosted RFCOMM service
        _provider =
            // await Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider.CreateAsync(
            //     RfcommServiceId.ObexObjectPush);
            await Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider.CreateAsync(
                RfcommServiceId.FromUuid(new Guid("d81a5833-37f4-460d-8a9f-347ff95474ad")));

        // Create a listener for this service and start listening
        StreamSocketListener listener = new StreamSocketListener();
        listener.ConnectionReceived += OnConnectionReceivedAsync;
        await listener.BindServiceNameAsync(
            _provider.ServiceId.AsString(),
            SocketProtectionLevel
                .BluetoothEncryptionAllowNullAuthentication);

        // Set the SDP attributes and start advertising
        InitializeServiceSdpAttributes(_provider);
        _provider.StartAdvertising(listener);
#endif
    }

    const uint SERVICE_VERSION_ATTRIBUTE_ID = 0x0300;
    const byte SERVICE_VERSION_ATTRIBUTE_TYPE = 0x0A;   // UINT32
    const uint MINIMUM_SERVICE_VERSION = 200;

#if IS_UWP_APP
    void InitializeServiceSdpAttributes(RfcommServiceProvider provider)
    {
        Windows.Storage.Streams.DataWriter writer = new Windows.Storage.Streams.DataWriter();

        // First write the attribute type
        writer.WriteByte(SERVICE_VERSION_ATTRIBUTE_TYPE);
        // Then write the data
        writer.WriteUInt32(MINIMUM_SERVICE_VERSION);

        IBuffer data = writer.DetachBuffer();
        provider.SdpRawAttributes.Add(SERVICE_VERSION_ATTRIBUTE_ID, data);
    }

    void OnConnectionReceivedAsync(
        StreamSocketListener listener,
        StreamSocketListenerConnectionReceivedEventArgs args)
    {
        // Stop advertising/listening so that we're only serving one client
        _provider.StopAdvertising();
        listener.Dispose();
        _socket = args.Socket;

        // The client socket is connected. At this point the App can wait for
        // the user to take some action, for example, click a button to receive a file
        // from the device, which could invoke the Picker and then save the
        // received file to the picked location. The transfer itself would use
        // the Sockets API and not the Rfcomm API, and so is omitted here for
        // brevity.
    }
#endif


}