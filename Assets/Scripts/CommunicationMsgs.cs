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
        protected virtual void UpdateData(CommunicationMsg msg){}
        public virtual void UpdateMsgFromOtherThread(CommunicationMsg msg){}
        public virtual void ObserveUpdate() {}
    }

    [System.Serializable]
    public class StateMsg : CommunicationMsg
    {
        [JsonIgnore]
        public int _player_id = 0;
        [JsonProperty("a")]
        public int player_id { get { return _player_id; } set { _player_id = value; } }

        [JsonIgnore]
        public int _control_type = 0;
        [JsonProperty("b")]
        public int control_type { get { return _control_type; } set { _control_type = value; } }

        [JsonIgnore]
        public float _initial_size = 1f;
        [JsonProperty("c")]
        public float initial_size { get { return _initial_size; } set { _initial_size = value; } }

        [JsonIgnore]
        public float _finish_size = 0f;
        [JsonProperty("d")]
        public float finish_size { get { return _finish_size; } set { _finish_size = value; } }

        [JsonIgnore]
        public float _initial_rotation_speed = 1f;
        [JsonProperty("e")]
        public float initial_rotation_speed { get { return _initial_rotation_speed; } set { _initial_rotation_speed = value;} }

        [JsonIgnore]
        public int _start_quiz_flag = 0;
        [JsonProperty("f")]
        public int start_quiz_flag { get { return _start_quiz_flag; } set { _start_quiz_flag = value;} }

        [JsonIgnore]
        public int _background_color =0;
        [JsonProperty("g")]
        public int background_color { get { return _background_color; } set { _background_color = value;} }

        public StateMsg(int playerId, int controlType, float initialSize, float finishSize, float initialRotationSpeed, int startQuizFlag, int backgroundColor, bool isUpdated)
        {
            this._player_id = playerId;
            this._control_type = controlType;
            this._initial_size = initialSize;
            this._finish_size = finishSize;
            this._initial_rotation_speed = initialRotationSpeed;
            this._start_quiz_flag = startQuizFlag;
            this._background_color=backgroundColor;
            base.isUpdated = isUpdated;
        }

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is StateMsg)
            {
                StateMsg stmsg = msg as StateMsg;
                this._player_id = stmsg.player_id;
                this._control_type = stmsg.control_type;
                this._initial_size = stmsg.initial_size;
                this._finish_size = stmsg.finish_size;
                this._initial_rotation_speed = stmsg.initial_rotation_speed;
                this._start_quiz_flag = stmsg.start_quiz_flag;
                this._background_color= stmsg.background_color;
            }
        }

        public override void UpdateMsgFromOtherThread(CommunicationMsg msg)
        {
            base.newMsg = msg;
            base.isUpdated = true;
        }

        public override void ObserveUpdate()
        {
            if (base.isUpdated)
            {
                this.UpdateData(base.newMsg);
                OnStateUpdated?.Invoke();
                base.isUpdated = false;
                System.Diagnostics.Debug.WriteLine("StateMsg ObservedUpdate: " + JsonConvert.SerializeObject(CommConstants.state));
            }
        }

        public delegate void OnStateUpdatedDelegate();
        public event OnStateUpdatedDelegate OnStateUpdated;
    }

    [System.Serializable]
    public class RotationMsg : CommunicationMsg
    {
        [JsonIgnore]
        public float _x = 0f;
        public float x { get { return _x; } set { _x = value; } }

        [JsonIgnore]
        public float _y = 0f;
        public float y { get { return _y; } set { _y = value; } }

        [JsonIgnore]
        public float _z = 0f;
        public float z { get { return _z; } set { _z = value; } }

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is RotationMsg)
            {
                RotationMsg rotationMsg = msg as RotationMsg;

                this._x = rotationMsg.x;
                this._y = rotationMsg.y;
                this._z = rotationMsg.z;
            }
        }

        public RotationMsg(float x, float y, float z, bool isUpdated)
        {
            this._x = x;
            this._y = y;
            this._z = z;
            base.isUpdated = isUpdated;
        }

        public override void UpdateMsgFromOtherThread(CommunicationMsg msg)
        {
                base.newMsg = msg;
                base.isUpdated = true;
                //System.Diagnostics.Debug.WriteLine("RotationMsg UpdateMsgFromOtherThread, isupdated: " + base.isUpdated.ToString());
        }

        public override void ObserveUpdate()
        {
            //System.Diagnostics.Debug.WriteLine("RotationMsg ObserveUpdate, isupdated: " + base.isUpdated.ToString());
            if (base.isUpdated)
            {
                this.UpdateData(base.newMsg);
                OnRotationUpdated?.Invoke();
                base.isUpdated = false;
                System.Diagnostics.Debug.WriteLine("RotationMsg ObservedUpdate: " + JsonConvert.SerializeObject(CommConstants.rotation));
            }
        }

        public delegate void OnRotationUpdatedDelegate();
        public event OnRotationUpdatedDelegate OnRotationUpdated;
    }

    [System.Serializable]
    public class AnimalIdMsg : CommunicationMsg
    {
        [JsonIgnore]
        public string _animal_id = "";
        public string animal_id { get { return _animal_id; } set { _animal_id = value; }}

        protected override void UpdateData(CommunicationMsg msg)
        {
            if (msg is AnimalIdMsg)
            {
                AnimalIdMsg animMsg = msg as AnimalIdMsg;
                this._animal_id = animMsg.animal_id;
            }
        }
    
        public AnimalIdMsg(string animalId, bool isUpdated)
        {
            this._animal_id = animalId;
            base.isUpdated = isUpdated;
        }
        public override void UpdateMsgFromOtherThread(CommunicationMsg msg)
        {
            base.newMsg = msg;
            base.isUpdated = true;
        }

        public override void ObserveUpdate()
        {
            if (base.isUpdated)
            {
                this.UpdateData(base.newMsg);
                OnAnimalIdUpdated?.Invoke();
                base.isUpdated = false;
                System.Diagnostics.Debug.WriteLine("AnimalIdMsg ObservedUpdate: " + JsonConvert.SerializeObject(CommConstants.animalid));
            }
        }
        public delegate void OnAnimalIdUpdatedDelegate();
        public event OnAnimalIdUpdatedDelegate OnAnimalIdUpdated;
    }
}