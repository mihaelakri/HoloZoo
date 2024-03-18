using UnityEngine;

namespace CommunicationMsgs
{
    public class CommunicationMsg
    {
        private bool isUpdated = false;
        private CommunicationMsg newMsg;
        private virtual void UpdateData(CommunicationMsg msg);
        public virtual void UpdateMsgFromOtherThread(CommunicationMsg msg);
        public virtual void ObserveUpdate();
    }

    [System.Serializable]
    public class StateMsg : CommunicationMsg
    {
        private int _player_id = 0;
        public int player_id { get { return _player_id; } set { _player_id = value; OnStateUpdated?.Invoke(); } }
        private int _control_type = 0;
        public int control_type { get { return _control_type; } set { _control_type = value; OnStateUpdated?.Invoke(); } }
        private float _initial_size = 1f;
        public float initial_size { get { return _initial_size; } set { _initial_size = value; OnStateUpdated?.Invoke(); } }
        private float _finish_size = 0f;
        public float finish_size { get { return _finish_size; } set { _finish_size = value; OnStateUpdated?.Invoke(); } }
        private float _initial_rotation_speed = 1f;
        public float initial_rotation_speed { get { return _initial_rotation_speed; } set { _initial_rotation_speed = value; OnStateUpdated?.Invoke(); } }
        private int _start_quiz_flag = 0;
        public int start_quiz_flag { get { return _start_quiz_flag; } set { _start_quiz_flag = value; OnStateUpdated?.Invoke(); } }

        private override void UpdateData(StateMsg msg)
        {
            this._player_id = msg.player_id;
            this._control_type = msg.control_type;
            this._initial_size = msg.initial_size;
            this._finish_size = msg.finish_size;
            this._initial_rotation_speed = msg.initial_rotation_speed;
            this._start_quiz_flag = msg.start_quiz_flag;
        }

        public override void UpdateMsgFromOtherThread(StateMsg msg)
        {
            newMsg = msg;
            isUpdated = true;
        }

        public void ObserveUpdate()
        {
            if (isUpdated)
            {
                this.UpdateData(newMsg);
                OnStateUpdated?.Invoke();
                isUpdated = false;
            }
        }

        public delegate void OnStateUpdatedDelegate();
        public event OnStateUpdatedDelegate OnStateUpdated;
    }

    [System.Serializable]
    public class RotationMsg : CommunicationMsg
    {
        private float _x = 0f;
        public float x { get { return _x; } set { _x = value; OnRotationUpdated?.Invoke(); } }
        private float _y = 0f;
        public float y { get { return _y; } set { _y = value; OnRotationUpdated?.Invoke(); } }
        private float _z = 0f;
        public float z { get { return _z; } set { _z = value; OnRotationUpdated?.Invoke(); } }

        private void UpdateData(RotationMsg msg)
        {
            this._x = msg.x;
            this._y = msg.y;
            this._z = msg.z;
        }

        public override void UpdateMsgFromOtherThread(RotationMsg msg)
        {
            newMsg = msg;
            isUpdated = true;
        }

        public void ObserveUpdate()
        {
            if (isUpdated)
            {
                this.UpdateData(newMsg);
                OnRotationUpdated?.Invoke();
                isUpdated = false;
            }
        }

        public delegate void OnRotationUpdatedDelegate();
        public event OnRotationUpdatedDelegate OnRotationUpdated;
    }

    [System.Serializable]
    public class AnimalIdMsg : CommunicationMsg
    {
        private string _animal_id = "";
        public string animal_id { get { return _animal_id; } set { _animal_id = value; OnAnimalIdUpdated?.Invoke();}}

        private void UpdateData(AnimalIdMsg msg)
        {
            this._animal_id = msg.animal_id;
        }

        public override void UpdateMsgFromOtherThread(AnimalIdMsg msg)
        {
            newMsg = msg;
            isUpdated = true;
        }

        public void ObserveUpdate()
        {
            if (isUpdated)
            {
                this.UpdateData(newMsg);
                OnAnimalIdUpdated?.Invoke();
                isUpdated = false;
            }
        }
        public delegate void OnAnimalIdUpdatedDelegate();
        public event OnAnimalIdUpdatedDelegate OnAnimalIdUpdated;
    }
}