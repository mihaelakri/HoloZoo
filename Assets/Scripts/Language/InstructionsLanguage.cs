using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionsLanguage : LanguageBase
{
    [SerializeField]
    Text instructionOneHeading, instructionOneMaterials, instructionOneSupply, instructionTwo, instructionThree;
    [SerializeField]
    TMP_Text buttonFinish;

    protected override void ApplyLanguageTexts()
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