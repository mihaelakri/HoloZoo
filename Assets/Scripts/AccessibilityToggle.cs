using UnityEngine;

public class AccessibilityToggle : MonoBehaviour
{
    public GameObject panelPrefab;
    GameObject panel;

    void Start()
    {
        panel = Instantiate(panelPrefab, transform.parent.transform);
        GameObject.Find("AccessHelper").GetComponent<ApplyAccessibility>().ApplyAccessibilitySettings();
    }

    public void OpenPanel()
    {
        panel.transform.LeanMoveLocal(new Vector2(0, 0), 1).setEaseOutQuart();
        panel.transform.GetComponent<Accessibility>().showAccesibility();
    }
}
