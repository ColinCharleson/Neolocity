using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Lec04
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;


public class Client : MonoBehaviour
{
    public GameObject myCube;
    public GameObject otherCube;
    public Camera Cam1;
    public Camera Cam2;
    private static byte[] outBuffer = new byte[512];
    string tempData = "";
    private static IPEndPoint remoteEP;
    private static Socket clientSoc;

    public InputField ipInput;
    public static IPAddress ip;
    public static void StartClient()
    {
        try
        {
            remoteEP = new IPEndPoint(ip, 8889);
            // Get IP of computer (this only works for locak server)
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

            // creates a new socket that can be used to send and recieve messages
            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        //player 1
        Cam1.enabled = false;
        // Player 2
        Cam2.enabled = true;

        // Get cubes position and represent it as bytes
        outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.ToString());

        // send cubes position to server
        clientSoc.SendTo(outBuffer, remoteEP);


        EndPoint localEP = (EndPoint)remoteEP;

        // Recieves data from client
        byte[] buffer = new byte[1024];
        int bytesReceived = clientSoc.ReceiveFrom(buffer, ref localEP);
        // Converts the buffer to a string from bytes
        string data = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

        // Makes sure the server is only updating if new data was received
        if (data != tempData)
        {
            // Updates cubes position
            otherCube.transform.position = StringToVec3(data);
            Debug.Log("Recv from: " + remoteEP + " Data: " + data);
            tempData = data;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ip = GetIP();
        //represents a network endpoint as an IP address and a port
        StartClient();
    }
    IPAddress GetIP()
    {
        if (ipInput.text == null)
        {
            return IPAddress.Parse("127.0.0.1");
        }
        else
        {

            return IPAddress.Parse(ipInput.text);
        }
    }
    private Vector3 StringToVec3(string data)
    {
        // Removes brackets
        data = data.Substring(1, data.Length - 2);
        // Assigns each value to a different position in the array
        string[] vectorValues = data.Split(',');

        return new Vector3(float.Parse(vectorValues[0]), float.Parse(vectorValues[1]), float.Parse(vectorValues[2]));
    }
}
