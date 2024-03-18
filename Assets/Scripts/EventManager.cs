using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CommunicationMsgs;

public class EventManager : MonoBehaviour
{
    private static EventManager instance = null;
    private readonly List<CommunicationMsg> CommMsgs;

    void Start(){
       List<CommunicationMsg> CommMsgs = new List<CommunicationMsg> {
        CommConstants.state,
        CommConstants.rotation,
        CommConstants.animalid
       };

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
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        foreach (CommunicationMsg msg in CommMsgs)
        {
            msg.ObserveUpdate();
        }
    }

    public void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}