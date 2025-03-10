using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillProfile : MonoBehaviour
{
    public Text usernameTxt;
    public Text lvlTxt;
    public Text expTxt;
    public Slider expSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
            Profileinfo();
    }

    private void Profileinfo()
    {
        var user = GameData.Instance.GetUser(PlayerPrefs.GetInt("ID"));

        if (user == null)
        {
            Debug.LogError($"{nameof(FillProfile)} - User is null");
            return;
        }

        lvlTxt.text = user.level.ToString();
        usernameTxt.text = user.username;
        expTxt.text = user.experience.ToString() + "/100";
        expSlider.value = user.experience;
    }
}