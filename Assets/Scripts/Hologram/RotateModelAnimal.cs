using UnityEngine.UI;

public class RotateModelAnimal : RotateModelBase
{
    public Slider bottomSlider;
    public Slider sideSlider;

    protected override void Start()
    {
        base.Start();
        UpdateRotationCallback();
    }

    void Update()
    {
        RotateModel();
    }

    public void UpdateRotationCallback()
    {
        UpdateRotationMessage(sideSlider.value, bottomSlider.value, 0f);
    }
}