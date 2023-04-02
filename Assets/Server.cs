using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour
{
    private Socket socket;
    IPAddress ip;

    public GameObject myCube;
    public GameObject otherCube;
    public Camera Cam1;
    public Camera Cam2;
    private static byte[] outBuffer = new byte[512];
    string tempData = "";

    public void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 8889));

        //Get the IP of the current computer
        IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
        ip = IPAddress.Parse("127.0.0.1");

        //Display host name and IP of server
        Debug.Log("Server name: " + hostInfo.HostName + " IP: " + ip);
    }

    public void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        //player 1
        Cam1.enabled = true;
        //player 2
        Cam2.enabled = false;

        // Represents a network endpoint as an IP address and a port
        IPEndPoint localEP = new IPEndPoint(ip, 0);
        EndPoint remoteEP = (EndPoint)localEP;

        // Recieves data from client
        byte[] buffer = new byte[1024];
        int bytesReceived = socket.ReceiveFrom(buffer, ref remoteEP);
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

        // Get cubes position and represent it as bytes
        outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.ToString());

        // send cubes position to server
        socket.SendTo(outBuffer, remoteEP);

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