using UnityEngine;
using UnityEngine.UI;

public class PrivacyPolicyLanguage : MonoBehaviour
{
    public Text header;
    public Text body;

    void Start()
    {
        header.text = GameData.Instance.translations.privacy_scene.header;
        body.text = GameData.Instance.translations.privacy_scene.body;
    }
}