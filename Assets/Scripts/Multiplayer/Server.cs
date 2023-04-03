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
    public JumpBoostM jumpScirpt1;
    public JumpBoostM jumpScirpt2;
    private static byte[] outBuffer = new byte[512];
    string tempData = "";

    public Vector3 clientTargetPos;
    public float clientTargetRot;
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
        jumpScirpt1.enabled = true;
        //player 2
        jumpScirpt1.enabled = false;

        //player 1
        Cam1.enabled = true;
        //player 2
        Cam2.enabled = false;
        Cam2.gameObject.GetComponent<AudioListener>().enabled = false;

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
            StringToData(data);
            otherCube.transform.position = clientTargetPos;
            otherCube.transform.rotation = Quaternion.Euler(0, clientTargetRot, 0);
            tempData = data;
        }

        // Get cubes position and represent it as bytes
        Vector4 dataToSend = new Vector4(myCube.transform.position.x, myCube.transform.position.y, myCube.transform.position.z, myCube.transform.rotation.eulerAngles.y);
        outBuffer = Encoding.ASCII.GetBytes(dataToSend.ToString());

        // send cubes position to server
        socket.SendTo(outBuffer, remoteEP);

    }

    private void StringToData(string data)
    {
        // Removes brackets
        data = data.Substring(1, data.Length - 2);
        // Assigns each value to a different position in the array
        string[] vectorValues = data.Split(',');

        clientTargetPos = new Vector3(float.Parse(vectorValues[0]), float.Parse(vectorValues[1]), float.Parse(vectorValues[2]));
        clientTargetRot = float.Parse(vectorValues[3]);
    }
}