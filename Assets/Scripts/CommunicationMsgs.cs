using UnityEngine;
using Newtonsoft.Json;
namespace CommunicationMsgs
{
    public class CommunicationMsg
    {
        [JsonIgnore]
        public bool isUpdated = false;
        [JsonIgnore]
        public CommunicationMsg newMsg;
        protected virtual void UpdateData(CommunicationMsg msg) { }
        public void UpdateMsgFromOtherThread(CommunicationMsg msg)
        {
            newMsg = msg;
            isUpdated = true;
        }
        public void ObserveUpdate()
        {
            if (isUpdated)
            {
                UpdateData(newMsg);
                TriggerOnUpdatedEvent();
                isUpdated = false;
                System.Diagnostics.Debug.WriteLine(GetType().Name + " ObservedUpdate: " + JsonConvert.SerializeObject(this));
            }
        }
        protected void TriggerOnUpdatedEvent()
        {
            OnUpdated?.Invoke();
        }

        public delegate void OnUpdatedDelegate();
        public event OnUpdatedDelegate OnUpdated;
    }

    [System.Serializable]
    public class StateMsg : CommunicationMsg
    {
        [JsonProperty("a")]
        public int player_id = 0;
        [JsonProperty("b")]
        public int control_type = 0;
        [JsonProperty("c")]
        public float initial_size = 1f;
        [JsonProperty("d")]
        public float finish_size = 0f;
        [JsonProperty("e")]
        public float initial_rotation_speed = 1f;
        [JsonProperty("f")]
        public int start_quiz_flag = 0;
        [JsonProperty("g")]
        public int background_color = 0;

        public StateMsg(int player_id, int control_type, float initial_size, float finish_size, float initial_rotation_speed, 
                int start_quiz_flag, int background_color, bool isUpdated)
        {
            this.player_id = player_id;
            this.control_type = control_type;
            this.initial_size = initial_size;
            this.finish_size = finish_size;
            this.initial_rotation_speed = initial_rotation_speed;
            this.start_quiz_flag = start_quiz_flag;
            this.background_color = background_color;
            base.isUpdated = isUpdated;
        }

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is StateMsg)
            {
                StateMsg stmsg = msg as StateMsg;
                player_id = stmsg.player_id;
                control_type = stmsg.control_type;
                initial_size = stmsg.initial_size;
                finish_size = stmsg.finish_size;
                initial_rotation_speed = stmsg.initial_rotation_speed;
                start_quiz_flag = stmsg.start_quiz_flag;
                background_color = stmsg.background_color;
            }
        }
    }

    [System.Serializable]
    public class RotationMsg : CommunicationMsg
    {
        public float x = 0f;
        public float y = 0f;
        public float z = 0f;

        public RotationMsg(float x, float y, float z, bool isUpdated)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            base.isUpdated = isUpdated;
        }

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is RotationMsg)
            {
                RotationMsg rotationMsg = msg as RotationMsg;

                x = rotationMsg.x;
                y = rotationMsg.y;
                z = rotationMsg.z;
            }
        }
    }

    [System.Serializable]
    public class AnimalIdMsg : CommunicationMsg
    {
        public string animal_id = "";

        public AnimalIdMsg(string animal_id, bool isUpdated)
        {
            this.animal_id = animal_id;
            base.isUpdated = isUpdated;
        }

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is AnimalIdMsg)
            {
                AnimalIdMsg animMsg = msg as AnimalIdMsg;
                animal_id = animMsg.animal_id;
            }
        }

    }

    [System.Serializable]
    public class LeapTimeMsg : CommunicationMsg
    {
        public float leap_time = -1f;

        public LeapTimeMsg(float leap_time, bool isUpdated)
        {
            this.leap_time = leap_time;
            base.isUpdated = isUpdated;
        }

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is LeapTimeMsg)
            {
                LeapTimeMsg leapMsg = msg as LeapTimeMsg;
                leap_time = leapMsg.leap_time;
            }
        }
    }

    [System.Serializable]
    public class RequestLeapTimeMsg : CommunicationMsg
    {
        public RequestLeapTimeMsg(bool isUpdated)
        {
            base.isUpdated = isUpdated;
        }

        protected override void UpdateData(CommunicationMsg msg)
        {
        }
    }
}