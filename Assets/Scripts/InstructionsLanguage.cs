using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstuctionsLanguage : MonoBehaviour
{
    // First page
    public Text instructionOneHeading;
    public Text instructionOneMaterials;
    public Text instructionOneSupply;
    // Second page
    public Text instructionTwo;
    // Third page
    public Text instructionThree;
    public TMP_Text buttonFinish;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        instructionOneHeading.text = translation.instruction_scene.instruction_one_heading;
        instructionOneMaterials.text = translation.instruction_scene.instruction_one_materials;
        instructionOneSupply.text = translation.instruction_scene.instruction_one_supply;
        instructionTwo.text = translation.instruction_scene.instruction_two;
        instructionThree.text = translation.instruction_scene.instruction_three;
        buttonFinish.text = translation.buttons.btn_end_instruction;
    }
}