using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.ComponentModel;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Text;
using System;



public class ChannelScript : MonoBehaviour
{

    
    private SerialPort sp;
    private Thread recvThread; // Thread
    private string s = "1,1,0,0";
    public int A, B, C, D;
    private bool isRead = false;


    // Use this for initialization   
    void Start()
    {
        try
        {
            // Get available ports
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                // Use the first available port instead of hardcoding COM7
                string portName = ports[0];
                Debug.Log("Using port: " + portName);
                
                sp = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                // Serial port initialization  
                if (!sp.IsOpen)
                {
                    sp.Open();
                }
                recvThread = new Thread(ReceiveData); // This thread is used to receive serial port data  
                recvThread.Start();
            }
            else
            {
                Debug.LogWarning("No COM ports available. Running without serial connection.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing serial port: " + ex.Message);
            Debug.LogWarning("Game will run without serial connection.");
        }
    }
    void Update()
    {
        if (isRead)
        {
            string[] sArry = Regex.
            Split(s, ",", RegexOptions.IgnoreCase);
            A = int.Parse(sArry[0]);
            B = int.Parse(sArry[1]);
            C = int.Parse(sArry[2]);
            D = int.Parse(sArry[3]);
            s = sArry[0]+","+sArry[1]+",0"+ ",0";
        }
    }

    private void ReceiveData()
    {
        try
        {
             s = "";
            // Read serial port data in line mode
            while ((s = sp.ReadLine()) != null)
            {
                print(s); // Print each line of data read
                isRead = true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OnApplicationQuit()
    {
        sp.Close(); // Close serial port
    }
    public void ClosePort()         // Close serial port
    {
        try
        {
            sp.Close();           
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
