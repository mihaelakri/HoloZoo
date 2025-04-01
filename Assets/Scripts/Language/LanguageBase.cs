using UnityEngine;

public abstract class LanguageBase : MonoBehaviour
{
    void OnEnable()
    {
        ApplyAccessibility.LanguageChanged += ApplyLanguageTexts;
    }

    void OnDisable()
    {
        ApplyAccessibility.LanguageChanged -= ApplyLanguageTexts;
    }

    void Start()
    {
        ApplyLanguageTexts();
    }

    protected abstract void ApplyLanguageTexts();
}