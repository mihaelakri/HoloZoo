#define LIGHTSPEED

using UnityEngine;

namespace WPM
{
    public class RotateModelGlobe : RotateModelBase
    {
        protected override void Start()
        {
            base.Start();
            RotateGlobeModel(transform.eulerAngles);
        }

        void Update()
        {
            if (transform.hasChanged)
            {
                // print("The transform has changed!");
                transform.hasChanged = false;

                RotateGlobeModel(transform.eulerAngles);
            }
        }

        public void RotateGlobeModel(Vector3 spherePosition)
        {
            UpdateRotationMessage(spherePosition.x,spherePosition.y,spherePosition.z);
        }
    }
}