using System.Collections;
using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
           GetUsername();
        }
    }
    private void GetUsername()
    {
        var user = GameData.Instance.GetCurrentUser();
        if (user != null)
            PlayerPrefs.SetString("username", user.username);

        return;
    }
}