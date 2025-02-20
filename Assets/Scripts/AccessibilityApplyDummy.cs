using UnityEngine;

public class AccessibilityApplyDummy : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("AccessHelper").GetComponent<ApplyAccessibility>().LoadAndStyle();
    }
}