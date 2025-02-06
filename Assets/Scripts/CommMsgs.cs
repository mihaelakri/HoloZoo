using MemoryPack;
using SVSBluetooth;

namespace CommMsgs
{
    [MemoryPackable]
    public partial class RotationMsg
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        private int _animal_id;
        public int animal_id
        {
            get { return _animal_id; }
            set
            {
                _animal_id = value;
                BluetoothForAndroid.WriteMessage(MemoryPackSerializer.Serialize(this));
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