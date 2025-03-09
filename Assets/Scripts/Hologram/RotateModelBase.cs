using System;
using System.Collections;
using CommMsgs;
using MemoryPack;
using SVSBluetooth;
using UnityEngine;

public class RotateModelBase : MonoBehaviour
{
    public GameObject model;
    private bool isRotationNew = false;
    protected bool isSendingRotation = false;

    protected virtual void Start()
    {
    }

    protected void RotateModel()
    {
        if (CommConstants.animal_id == 0)
        {
            model.transform.GetChild(0).transform.rotation = Quaternion.Euler(CommConstants.x, CommConstants.y, CommConstants.z);
            return;
        }

        try
        {
            float interpolationFactor = 10f;

            if (CommConstants.animal_id == 0)   // Globe
            {
                interpolationFactor = 10f;
            }

            Quaternion currentRotation = model.transform.GetChild(0).transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(CommConstants.x, CommConstants.y + 180, CommConstants.z);
            Quaternion smoothedRotation;

            if (Quaternion.Dot(currentRotation, targetRotation) < 0f)
            {
                smoothedRotation = Quaternion.RotateTowards(currentRotation, targetRotation, -30f);
            }
            else
            {
                smoothedRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * interpolationFactor);
            }

            model.transform.GetChild(0).transform.rotation = smoothedRotation;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    protected IEnumerator BTSendModelRotationLoop()
    {
        isSendingRotation = true;

        while (isSendingRotation)
        {
            yield return new WaitUntil(() => isRotationNew);

            byte[] serializedMsg = MemoryPackSerializer.Serialize(CommConstants.rotationMsg);
            // Debug.Log("Bluetooth - BTSendRotate3DModel: " + serializedMsg);
            BluetoothForAndroid.WriteMessage(serializedMsg);
            isRotationNew = false;

            yield return new WaitForSecondsRealtime(0.025f);    // 40 Hz
        }
    }

    protected void UpdateRotationMessage(float x, float y, float z)
    {
        CommConstants.x = x;
        CommConstants.y = y;
        CommConstants.z = z;

        CommConstants.rotationMsg = new RotationMsg(x, y, z, CommConstants.animal_id);
        isRotationNew = true;
        // BTSendModelRotation();
    }
}