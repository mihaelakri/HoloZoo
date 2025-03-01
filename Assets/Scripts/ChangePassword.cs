using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangePassword : MonoBehaviour
{
    public InputField newPasswordField;
    public InputField retypepasswordField;
    public Text toast;

    public void CallChangePassword()
    {
        StartCoroutine(ChangePass());
    }

    IEnumerator ChangePass()
    {
        if (newPasswordField.text.Length < 8)
        {
            toast.text = "Password too short";
            yield break;
        }
        if (retypepasswordField.text != newPasswordField.text)
        {
            toast.text = "Passwords do not match";
            yield break;
        }

        var user = GameData.Instance.GetUser(PlayerPrefs.GetInt("ID"));

        if (user == null)
        {
            toast.text = "Error updating password";
            yield break;
        }

        GameData.Instance.UpdateUserPassword(newPasswordField.text, user);
        toast.text = "Password updated successfully!";
    }
}