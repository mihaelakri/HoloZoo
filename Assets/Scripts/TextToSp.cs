using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSp : MonoBehaviour
{
    public AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource> ();
        StartCoroutine (DownloadTheAudio());
    }

    IEnumerator DownloadTheAudio(){
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=SampleText&tl=En-gb";
        WWW www = new WWW (url);
        yield return www;

        _audio.clip = www.GetAudioClip (false, true, AudioType.MPEG);
        _audio.Play();

    }

    public void ButtonClick(){
        StartCoroutine (DownloadTheAudio());
    }
}
