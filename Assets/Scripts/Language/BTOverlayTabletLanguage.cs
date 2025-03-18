using UnityEngine;
using UnityEngine.UI;

public class BTOverlayTabletLanguage : MonoBehaviour
{
    [SerializeField]
    Text unpairedHeader, pairedHeader, connectedHeader;
    [SerializeField]
    Text unpairedBody1, unpairedBody2;
    [SerializeField]
    Text pairButton, connectButton, disconnectButton, forgetButton1, forgetButton2;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        unpairedHeader.text = translation.bluetoothOverlays.tabletUnpairedHeader;
        pairedHeader.text = translation.bluetoothOverlays.tabletPairedHeader;
        connectedHeader.text = translation.bluetoothOverlays.tabletConnectedHeader;

        unpairedBody1.text = translation.bluetoothOverlays.tabletUnpairedBody1;
        unpairedBody2.text = translation.bluetoothOverlays.tabletUnpairedBody2;

        pairButton.text = translation.buttons.btn_pair;
        connectButton.text = translation.buttons.btn_connect;
        forgetButton1.text = translation.buttons.btn_forget;
        disconnectButton.text = translation.buttons.btn_disconnect;
        forgetButton2.text = translation.buttons.btn_forget;
    }
}