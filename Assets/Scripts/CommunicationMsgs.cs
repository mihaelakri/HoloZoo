namespace CommunicationMsgs
{
    public struct RotationMsg
    {
        public string x;
        public string y;
        public string z;
        public int player_id;
        public string animal_id;

        public RotationMsg(string x, string y, string z, int player_id, string animal_id)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }
}