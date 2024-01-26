using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startTime;
    private float elapsedTime; // To store the elapsed time
    private bool isTimerRunning = false;

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime = Time.time - startTime;

            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);
            int milliseconds = (int)((elapsedTime * 1000) % 1000);

            string timerString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            //Debug.Log(timerString);

        }
    }

    public void StartTimer()
    {
        startTime = Time.time; // Start from the current time
        elapsedTime = 0f; // Reset elapsed time to 0
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        elapsedTime = Time.time - startTime; // Save the elapsed time when stopping
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
