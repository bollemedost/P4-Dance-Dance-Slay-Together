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
    private Queue<string> messageQueue = new Queue<string>();
    private object lockObject = new object();

    // Player 1 Events
    public static event Action OnLeft1Pressed;
    public static event Action OnDown1Pressed;
    public static event Action OnUp1Pressed;
    public static event Action OnRight1Pressed;

    // Player 2 Events
    public static event Action OnLeft2Pressed;
    public static event Action OnDown2Pressed;
    public static event Action OnUp2Pressed;
    public static event Action OnRight2Pressed;

    void Start()
    {
        try
        {
            string portName;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            portName = "COM7"; // Replace with your Windows COM port
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            portName = "/dev/cu.usbmodem1101"; // Replace with your Mac port
#else
            portName = "COM7"; // Fallback default
#endif

            serialPort = new SerialPort(portName, 9600);
            serialPort.Open();
            serialPort.ReadTimeout = 100;
            Debug.Log($"✅ Connected to Arduino on port: {portName}");

            serialThread = new Thread(ReadSerial);
            serialThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Failed to connect: " + e.Message);
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
                    messageQueue.Enqueue(message);
                }
            }
            catch (TimeoutException) { }
            Thread.Sleep(10);
        }
    }

    void Update()
    {
        lock (lockObject)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                Debug.Log("Received: " + message);

                // Player 1
                if (message == "Left1") OnLeft1Pressed?.Invoke();
                if (message == "Down1") OnDown1Pressed?.Invoke();
                if (message == "Up1") OnUp1Pressed?.Invoke();
                if (message == "Right1") OnRight1Pressed?.Invoke();

                // Player 2
                if (message == "Left2") OnLeft2Pressed?.Invoke();
                if (message == "Down2") OnDown2Pressed?.Invoke();
                if (message == "Up2") OnUp2Pressed?.Invoke();
                if (message == "Right2") OnRight2Pressed?.Invoke();
            }
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (serialThread != null && serialThread.IsAlive)
            serialThread.Join();

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}
