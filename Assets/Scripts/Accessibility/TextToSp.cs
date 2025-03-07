using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSp : MonoBehaviour
{
    public AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(DownloadTheAudio());
    }

    IEnumerator DownloadTheAudio()
    {
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=SampleText&tl=En-gb";
        using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"{nameof(TextToSp)} - Request failed: {request.result}");
            yield break;
        }

        _audio.clip = DownloadHandlerAudioClip.GetContent(request);
        _audio.Play();
    }

    public void ButtonClick()
    {
        StartCoroutine(DownloadTheAudio());
    }
}