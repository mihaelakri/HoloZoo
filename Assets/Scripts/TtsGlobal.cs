using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using NativeTextToSpeech;

public class TtsGlobal : MonoBehaviour
{
    [SerializeField] private bool threadSafe;
    private TextToSpeech _textToSpeech;
    private bool _finishReceived; 
    private Queue<string> errors = new Queue<string>();
    public TtsGlobal instance = null;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        _textToSpeech =  TextToSpeech.Create(OnFinish,OnError);
        Debug.Log("TTS - Start");
    }

    public void doSpeak(string textToSpeak){
        _textToSpeech.Speak(textToSpeak, "en-US", 0.8f);
    }

    public void readHome() {
        _textToSpeech.Speak("Profile, Learn, Quiz, Accessibility", "en-US", 0.8f);
    }

    public void readQuiz(string q, string a, string b, string c) {
        _textToSpeech.Speak(q+ ", " + a+ ", " + b+ ", " + c, "en-US", 0.8f);
    }

    public void readScore(string a) {
        _textToSpeech.Speak(a, "en-US", 0.8f);
    }
    private void OnFinish()
    {
        if (threadSafe)
        {
            _finishReceived = true;
            Debug.Log("TTS - Finish");
        }
        else
        {
            // TTSFinished();
            Debug.Log("TTS - Finish Else");
        }
    }

    private void OnError(string msg)
    {
        if (threadSafe)
        {
            errors.Enqueue(msg);
        }
        else
        {
            Debug.LogWarning("Error received in Unity main thread: " + msg);
        }
    }
}

