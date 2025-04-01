using UnityEngine;
using UnityEngine.UI;

public class BTOverlayLanguage : LanguageBase
{
    [SerializeField]
    Text connectedHeader, notConnectedHeader, cancelButton, disconnectButton;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        connectedHeader.text = translation.bluetoothOverlays.connectedHeader;
        notConnectedHeader.text = translation.bluetoothOverlays.notConnectedHeader;
        cancelButton.text = translation.buttons.btn_cancel;
        disconnectButton.text = translation.buttons.btn_disconnect;
    }
}