using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public void logOut(){
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoadingScreen");
    }
}
