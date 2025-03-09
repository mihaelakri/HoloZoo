public class RotateModelTablet : RotateModelBase
{
    private int old_animal_id;
    Load3DModelTablet load3DModelTablet;

    protected override void Start()
    {
        base.Start();
        load3DModelTablet = gameObject.transform.GetComponent<Load3DModelTablet>();
    }

    void Update()
    {
        if (old_animal_id != CommConstants.animal_id)
        {
            old_animal_id = CommConstants.animal_id;
            StartCoroutine(load3DModelTablet.GetModel());
        }
        RotateModel();
    }
}