using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public void logOut(){
        PlayerPrefs.DeleteAll();
        SceneManagement.Instance.ClearBackstack();
        SceneManager.LoadScene("LoadingScreen");
    }
}
