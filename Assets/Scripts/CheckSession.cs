using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CheckSession : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Session());
    }

    IEnumerator Session()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            WWWForm form = new();
            form.AddField("id", PlayerPrefs.GetInt("ID"));
            form.AddField("flag", "5");

            using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "middle_man.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("CheckSession error: " + www.downloadHandler.text);
                }
            }
        }
    }
}