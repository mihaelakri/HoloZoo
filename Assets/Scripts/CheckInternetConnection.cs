using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Android;

public class CheckInternetConnection : MonoBehaviour
{
    private static volatile bool shouldCheckInternet;

    public static readonly Dictionary<string, Dictionary<string, string>> connectionMessages = new()
    {
        { "en", new Dictionary<string, string>
            {
                { "message", "Check internet connection." },
                { "button", "OK" },
            }
        },
        { "fr", new Dictionary<string, string>
            {
                { "message", "Vérifiez la connexion Internet." },
                { "button", "OK" },
            }
        },
        { "hr", new Dictionary<string, string>
            {
                { "message", "Provjerite internetsku vezu." },
                { "button", "OK" },
            }
        },
        { "es", new Dictionary<string, string>
            {
                { "message", "Verificar conexión a Internet." },
                { "button", "OK" },
            }
        },
        { "hu", new Dictionary<string, string>
            {
                { "message", "Ellenőrizze az internetkapcsolatot." },
                { "button", "OK" },
            }
        }
    };

    public static IEnumerator CheckConnection(Action<bool> reportState)
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.INTERNET"))
            Permission.RequestUserPermission("android.permission.INTERNET");

        using UnityWebRequest request = new("https://www.google.com");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Internet is connected");
            reportState(true);
        }
        else
        {
            Debug.Log("Internet is not connected");
            reportState(false);
        }
    }

    public static void ShowDialog(string message, string btnMessage, Action onOkPressed = null)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // Run on the UI thread
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject alertDialogBuilder = new("android.app.AlertDialog$Builder", activity);
                alertDialogBuilder.Call<AndroidJavaObject>("setMessage", message);

                // Prevent closing the dialog unless a button is pressed
                alertDialogBuilder.Call<AndroidJavaObject>("setCancelable", false);

                // OK button
                if (onOkPressed != null)
                {
                    alertDialogBuilder.Call<AndroidJavaObject>("setPositiveButton", btnMessage, new OnClickListener(onOkPressed));
                }

                // Create dialog
                AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject>("create");

                // Prevent dismissing when touching outside
                dialog.Call("setCanceledOnTouchOutside", false);
                dialog.Call("show");
            }));
        }
    }

    // Click listener for buttons
    private class OnClickListener : AndroidJavaProxy
    {
        private readonly Action callback;

        public OnClickListener(Action callback) : base("android.content.DialogInterface$OnClickListener")
        {
            this.callback = callback;
        }

        public void onClick(AndroidJavaObject dialog, int which)
        {
            callback?.Invoke();
        }
    }

    public static void ShowToast(string message, bool shortDuration = true)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                using AndroidJavaObject toastClass = new AndroidJavaClass("android.widget.Toast");

                using (AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext"))
                {
                    AndroidJavaObject toast = toastClass.CallStatic<AndroidJavaObject>("makeText", context, message, shortDuration ? 0 : 1);
                    toast.Call("show");
                }
            }));
        }
    }

    public static IEnumerator PromptConnectionBlocking(MonoBehaviour caller)
    {
        shouldCheckInternet = true;
        bool hasInternet = false;
        while (!hasInternet)
        {
            // Because ShowDialog is async, check if it's dismissed before checking internet again
            if (shouldCheckInternet)
            {
                shouldCheckInternet = false;
                yield return caller.StartCoroutine(CheckConnection((isConnected) => { hasInternet = isConnected; }));

                if (!hasInternet)
                {
                    ShowDialog(
                        connectionMessages[PlayerPrefs.GetString("lang", "en")]["message"],
                        connectionMessages[PlayerPrefs.GetString("lang", "en")]["button"],
                        // shouldCheckInternet is set here to wait for dialog dismissal
                        () =>
                        {
                            shouldCheckInternet = true;
                        }
                    );
                }
            }
        }
    }
}
