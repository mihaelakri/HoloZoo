using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class FillProfile : MonoBehaviour
{
    [SerializeField] public Profile_info profile_info;

    public Text usernameTxt;
    public Text lvlTxt;
    public Text expTxt;
    public Slider expSlider;

    [Serializable]
    public class Profile_info
    {
        public string username;
        public int experience;
        public int level;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            StartCoroutine(Profileinfo());
        }
    }

    IEnumerator Profileinfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetInt("ID"));
        form.AddField("flag", "3");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "middle_man.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                profile_info = JsonUtility.FromJson<Profile_info>(www.downloadHandler.text);

                lvlTxt.text = Convert.ToString(profile_info.level);
                usernameTxt.text = profile_info.username;
                expTxt.text = profile_info.experience.ToString() + "/100";
                expSlider.value = profile_info.experience;
            }
        }
    }
}