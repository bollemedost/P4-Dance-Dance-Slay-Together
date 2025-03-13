using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoSerial : MonoBehaviour
{
    private SerialPort serialPort;
    private Thread serialThread;
    private bool isRunning = true;
    private Queue<string> messageQueue = new Queue<string>(); // Queue for handling messages safely
    private object lockObject = new object(); // Lock object for thread safety

    // Events to trigger input actions
    public static event Action OnLeftPressed;
    public static event Action OnDownPressed;
    public static event Action OnUpPressed;
    public static event Action OnRightPressed;

    void Start()
    {
        try
        {
            serialPort = new SerialPort("COM7", 9600);
            serialPort.Open();
            serialPort.ReadTimeout = 100;
            Debug.Log("Connected to Arduino on COM7");

            // Start a new thread for reading serial data
            serialThread = new Thread(ReadSerial);
            serialThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect: " + e.Message);
        }
    }

    void ReadSerial()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string message = serialPort.ReadLine().Trim();
                lock (lockObject)
                {
                    messageQueue.Enqueue(message); // Store messages in queue
                }
            }
            catch (TimeoutException) { }
            Thread.Sleep(10); // Prevents CPU overload
        }
    }

    void Update()
    {
        lock (lockObject)
        {
            while (messageQueue.Count > 0) // Process all messages
            {
                string message = messageQueue.Dequeue();
                Debug.Log("Received: " + message); // Display every press

                // Trigger corresponding events
                switch (message)
                {
                    case "Left":
                        OnLeftPressed?.Invoke();
                        break;
                    case "Down":
                        OnDownPressed?.Invoke();
                        break;
                    case "Up":
                        OnUpPressed?.Invoke();
                        break;
                    case "Right":
                        OnRightPressed?.Invoke();
                        break;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (serialThread != null && serialThread.IsAlive)
            serialThread.Join(); // Stop the thread safely

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}
