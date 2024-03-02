using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChangePassword : MonoBehaviour
{

    public InputField newPasswordField;
    public InputField retypepasswordField;
    public Text toast;


    public void CallChangePassword(){
    StartCoroutine(ChangePass());
 }

 IEnumerator ChangePass(){
        if (newPasswordField.text.Length < 8){
          toast.text = "Password too short";
        } else if (retypepasswordField.text!=newPasswordField.text){
          toast.text = "Passwords do not match";
        } else{
            WWWForm form = new WWWForm();
            form.AddField("password", newPasswordField.text);
            form.AddField("flag", "4");
            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/middle_man.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    if(www.downloadHandler.text == "400"){
                        Debug.Log("Bad Request");
                    }
                    else if(www.downloadHandler.text == "404"){
                        Debug.Log("Flag not found");
                    }
                    else if(www.downloadHandler.text != "0"){
                        Debug.Log(www.downloadHandler.text);
                        Debug.Log("Password updated successfully!");
                        toast.text = "Password updated successfully!";
                    }
                    else{
                        Debug.Log("Error updating password!");
                        toast.text = "Error updating password";
                    }
                }
            }
    }
 }
}
