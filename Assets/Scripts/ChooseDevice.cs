using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseDevice : MonoBehaviour
{
    public GameObject panel;
    public GameObject backButton;
    public Toggle m_Toggle;
    public void chooseMobile()
    {
        storeCommMethod();
        PlayerPrefs.SetString("device", "mobile");
        panel.transform.LeanMoveLocal(new Vector2(0, -645), 0.75f).setEaseOutQuart();
    }
    public void chooseTablet()
    {
        storeCommMethod();
        PlayerPrefs.SetString("device", "tablet");
        // panel.transform.LeanMoveLocal(new Vector2(0, -645), 0.75f).setEaseOutQuart();
        SceneManager.LoadScene("HologramTablet");
    }

    public void chooseAgain()
    {
        panel.transform.LeanMoveLocal(new Vector2(0, 0), 0.75f).setEaseOutQuart();
        backButton.transform.LeanMoveLocalY(-344, 0.75f).setEaseOutQuart();
        backButton.transform.LeanRotateZ(90, 0.5f);
    }

    private void storeCommMethod()
    {
        PlayerPrefs.SetString("conn_method", "bluetooth");
        backButton.transform.LeanMoveLocalY(-272, 0.75f).setEaseOutQuart();
        backButton.transform.LeanRotateZ(-90, 0.5f);
    }
}