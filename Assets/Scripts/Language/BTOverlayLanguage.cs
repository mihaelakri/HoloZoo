using UnityEngine;
using UnityEngine.UI;

public class BTOverlayLanguage : MonoBehaviour
{
    [SerializeField]
    Text connectedHeader, notConnectedHeader, cancelButton, disconnectButton;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        connectedHeader.text = translation.bluetoothOverlays.connectedHeader;
        notConnectedHeader.text = translation.bluetoothOverlays.notConnectedHeader;
        cancelButton.text = translation.buttons.btn_cancel;
        disconnectButton.text = translation.buttons.btn_disconnect;
    }
}