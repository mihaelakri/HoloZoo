using UnityEngine;

public class AccessibilityApplyDummy : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("AccessibilityManager").GetComponent<ApplyAccessibility>().LoadAndStyle();
    }
}