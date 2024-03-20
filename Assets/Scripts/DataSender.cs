using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DataSender : MonoBehaviour
{
    public IEnumerator SendDataToServer(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", data);
        Debug.Log("datasender aplikacija "+data);
        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"receive_data.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Data successfully sent to the server.");
            }
        }
    }

    public IEnumerator CheckForDataFromServer()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"receive_data.php"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Process the received data
                string receivedData = www.downloadHandler.text;
                Debug.Log("Received data: " + receivedData);
            }
        }
    }
}
