using UnityEngine;

namespace CommunicationMsgs
{
    [System.Serializable]
    public struct RotationMsg
    {
        public float x;
        public float y;
        public float z;
        public int player_id;
        public string animal_id;

        public RotationMsg(float x, float y, float z, int player_id, string animal_id)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }
}