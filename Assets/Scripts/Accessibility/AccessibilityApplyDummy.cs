using UnityEngine;

public class AccessibilityApplyDummy : MonoBehaviour
{
    void Start()
    {
        ApplyAccessibility.Instance.LoadAndStyle();
    }
}