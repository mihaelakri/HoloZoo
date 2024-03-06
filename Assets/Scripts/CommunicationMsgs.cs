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
        public int control_type;
        public float initial_size;
        public float finish_size;
        public float initial_rotation_speed;
        public int start_quiz_flag;

        public RotationMsg(float x, float y, float z, int player_id, string animal_id, 
            int control_type, float initial_size, float finish_size, float initial_rotation_speed, int start_quiz_flag)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
            this.control_type = control_type;
            this.initial_size = initial_size;
            this.finish_size = finish_size;
            this.initial_rotation_speed = initial_rotation_speed;
            this.start_quiz_flag = start_quiz_flag;
        }
    }
}