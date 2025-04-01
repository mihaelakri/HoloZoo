using UnityEngine;
using UnityEngine.UI;

public class PrivacyPolicyLanguage : LanguageBase
{
    [SerializeField]
    Text header, body;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        header.text = translation.privacy_scene.header;
        body.text = translation.privacy_scene.body;
    }
}