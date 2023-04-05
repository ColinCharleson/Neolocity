using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Lec04
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;


public class Client : MonoBehaviour
{
    public RaceTimer race;
    public GameObject myCube;
    public GameObject otherCube;
    public Camera Cam1;
    public Camera Cam2;
    public Camera MainCam;
    public JumpBoostM jumpScirpt1;
    public JumpBoostM jumpScirpt2;

    private static byte[] outBuffer = new byte[512];
    string tempData = "";
    private static IPEndPoint remoteEP;
    private static Socket clientSoc;

    public InputField ipInput;
    public static IPAddress ip;

    public Vector3 clientTargetPos;
    public float clientTargetRot;
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
        if (race.raceEnd == true)
        {
            bool isConnected = clientSoc.Poll(1000, SelectMode.SelectRead) && (clientSoc.Available == 0);

            if (!isConnected)
            {
                // Client has disconnected, go back to main menu
                SceneManager.LoadScene("MainMenu");
                clientSoc.Close();
                return;
            }

        }
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
        // Player 2
        Cam2.enabled = false;
        Cam2.gameObject.GetComponent<AudioListener>().enabled = false;

        //First Cam turn off
        MainCam.enabled = false;

        // Get cubes position and represent it as bytes
        Vector4 dataToSend = new Vector4(myCube.transform.position.x, myCube.transform.position.y, myCube.transform.position.z, myCube.transform.rotation.eulerAngles.y);
        outBuffer = Encoding.ASCII.GetBytes(dataToSend.ToString());

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
            StringToData(data);
            otherCube.transform.position = clientTargetPos;
            otherCube.transform.rotation = Quaternion.Euler(0, clientTargetRot, 0);
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
