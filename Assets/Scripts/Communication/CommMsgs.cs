using MemoryPack;
using SVSBluetooth;
using UnityEngine;

namespace CommMsgs
{
    [MemoryPackable]
    public partial class RotationMsg
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        private int _animal_id = -1;
        public int animal_id
        {
            get { return _animal_id; }
            set
            {
                // Initialization edge-case
                if (_animal_id == -1)
                {
                    _animal_id = value;
                    return;
                }

                _animal_id = value;

                if (PlayerPrefs.GetString("device") == "mobile")
                {
                    BluetoothForAndroid.WriteMessage(MemoryPackSerializer.Serialize(this));
                }
            }
        }

        public RotationMsg(float x, float y, float z, int animal_id)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.animal_id = animal_id;
        }
    }
}