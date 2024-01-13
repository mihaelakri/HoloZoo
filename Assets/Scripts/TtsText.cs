using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using NativeTextToSpeech;


public class TtsText : MonoBehaviour
{
    // [SerializeField] private bool threadSafe;
    // private TextToSpeech _textToSpeech;
    // private bool _finishReceived; 
    // private Queue<string> errors = new Queue<string>();

    [SerializeField] public string textToSpeak;

    // void Start()
    // {
    //     _textToSpeech =  TextToSpeech.Create(OnFinish,OnError);
    //     Debug.Log("TTS - Start");
    // }

    public void doSpeak(){
        if(PlayerPrefs.GetInt("textToSpeech")==1){
            GameObject.Find("TTSHelper").GetComponent<TtsGlobal>().doSpeak(textToSpeak);
        }
        // _textToSpeech.Speak(textToSpeak, "en-US", 0.8f);
    }
    

    // private void OnFinish()
    // {
    //     if (threadSafe)
    //     {
    //         _finishReceived = true;
    //         Debug.Log("TTS - Finish");
    //     }
    //     else
    //     {
    //         // TTSFinished();
    //         Debug.Log("TTS - Finish Else");
    //     }
    // }

    // private void OnError(string msg)
    // {
    //     if (threadSafe)
    //     {
    //         errors.Enqueue(msg);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Error received in Unity main thread: " + msg);
    //     }
    // }
}

