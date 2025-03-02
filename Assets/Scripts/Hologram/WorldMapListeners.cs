#define LIGHTSPEED

using UnityEngine;

namespace WPM
{
    public class WorldMapListeners : MonoBehaviour
    {
        WorldMapGlobe map; // the globe
        public GameObject canvas;
        public RotateModel rotateModel;

        void Update()
        {
            if (transform.hasChanged)
            {
                // print("The transform has changed!");
                transform.hasChanged = false;

                rotateModel.rotateGlobeModel(transform.eulerAngles);
            }
        }
    }
}