using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] public Profile_info profile_info;

    [Serializable]
    public class Profile_info
    {
        public string username;
        public int experience;
        public int level;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            StartCoroutine(GetUsername());
        }
    }
    IEnumerator GetUsername()
    {
        WWWForm form = new();
        form.AddField("id", PlayerPrefs.GetInt("ID"));
        form.AddField("flag", "3");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "middle_man.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("HomeScreen GetUsername error: " + www.error);
            }
            else
            {
                profile_info = JsonUtility.FromJson<Profile_info>(www.downloadHandler.text);
                PlayerPrefs.SetString("username", profile_info.username);
            }
        }
    }
}