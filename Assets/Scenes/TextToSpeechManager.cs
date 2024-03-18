using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TextToSpeechManager : MonoBehaviour
{
    private AndroidJavaObject tts;

    private void Start()
    {
        // Check if the device is running Android
        if (Application.platform == RuntimePlatform.Android)
        {
            // Initialize the TextToSpeech engine
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            tts = new AndroidJavaObject("android.speech.tts.TextToSpeech", activity, new TextToSpeechListener());
        }
        else
        {
            Debug.Log("Text-to-Speech is only supported on Android.");
        }
    }

    public void Speak(string text)
    {
        if (tts != null)
        {
            tts.Call("speak", text, 0, null, null);
        }
        else
        {
            Debug.Log("Text-to-Speech not initialized.");
        }
    }

    private class TextToSpeechListener : AndroidJavaProxy
    {
        public TextToSpeechListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        public void onInit(int status)
        {
            if (status == 0) // TextToSpeech.SUCCESS
            {
                Debug.Log("Text-to-Speech initialized successfully.");
            }
            else
            {
                Debug.Log("Text-to-Speech initialization failed with status code: " + status);
            }
        }
    }
}