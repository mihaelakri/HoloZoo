#define LIGHTSPEED

using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace WPM {
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