using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;

public class ConnectionBase : MonoBehaviour
{
    protected int player_id;
    
    protected class RotationMsg {
        public string x;
        public string y;
        public string z;
        public int player_id;
        public string animal_id;
        protected float lastUpdateTime = 0f;

        public RotationMsg(string x, string y, string z, int player_id, string animal_id){
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }

    public ConnectionBase() 
    {
        StartCoroutine(mockAuth());
        player_id = PlayerPrefs.GetInt("ID");
    }

    public virtual async Task Initialize()
    {
    }

    public virtual void SendData()
    {        
    }


    IEnumerator mockAuth(){
        string id_animal = "0";

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"animal/model/"+id_animal)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    Debug.Log(www.downloadHandler.text);
                }
        }
    }

}
