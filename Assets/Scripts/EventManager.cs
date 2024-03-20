using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CommunicationMsgs;

public class EventManager : MonoBehaviour
{
    private static EventManager instance = null;
    private List<CommunicationMsg> CommMsgs;

    void Start(){


    }
    public static void initUnityThread(bool visible = false)
    {
        if (instance != null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            // add an invisible game object to the scene
            GameObject obj = new GameObject("MainThreadExecuter");
            if (!visible)
            {
                obj.hideFlags = HideFlags.HideAndDontSave;
            }

            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<EventManager>();
        }
        System.Diagnostics.Debug.WriteLine("EventManager init");
    }

    void Awake()
    {
        CommMsgs = new List<CommunicationMsg> {
        CommConstants.state,
        CommConstants.rotation,
        CommConstants.animalid,
        CommConstants.leapTimeMsg,
        CommConstants.requestLeapTimeMsg
        };
        DontDestroyOnLoad(gameObject);
        System.Diagnostics.Debug.WriteLine("EventManager awake");
    }

    void Update()
    {
        foreach (CommunicationMsg msg in CommMsgs)
        {
            msg.ObserveUpdate();
        }
    }

    public void OnDisable()
    {
        System.Diagnostics.Debug.WriteLine("EventManager ondisable");
        if (instance == this)
        {
            instance = null;
        }
    }
}