using UnityEngine;
using UnityEngine.UI;

public class SetDifficulty : MonoBehaviour
{
    public Scrollbar difficulty;
    public Button submitButton;

    public void SetDiff()
    {
        float diff = difficulty.value;
        string keyName = "diff";

        if (diff < 0.5f)
        {
            PlayerPrefs.SetInt("diff", 1);
        }
        else if (diff >= 0.5f && diff < 1)
        {
            PlayerPrefs.SetInt("diff", 2);
        }
        else
        {
            PlayerPrefs.SetInt("diff", 3);
        }
        Debug.Log(PlayerPrefs.GetInt(keyName));
    }
}