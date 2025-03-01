using System.Collections;
using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            StartCoroutine(GetUsername());
        }
    }
    IEnumerator GetUsername()
    {
        var user = GameData.Instance.GetCurrentUser();
        if (user != null)
            PlayerPrefs.SetString("username", user.username);

        yield break;
    }
}