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
        ChangePass();
    }

    private void ChangePass()
    {
        if (newPasswordField.text.Length < 8)
        {
            toast.text = "Password too short";
            return;
        }
        if (retypepasswordField.text != newPasswordField.text)
        {
            toast.text = "Passwords do not match";
            return;
        }

        var user = GameData.Instance.GetUser(PlayerPrefs.GetInt("ID"));

        if (user == null)
        {
            toast.text = "Error updating password";
            return;
        }

        GameData.Instance.UpdateUserPassword(newPasswordField.text, user);
        toast.text = "Password updated successfully!";
    }
}