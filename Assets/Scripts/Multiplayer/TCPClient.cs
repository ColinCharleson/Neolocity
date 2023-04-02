using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class TCPClient : MonoBehaviour
{
    public InputField inputField;
    public Text textObject;

    private Socket clientSoc;
    private byte[] inBuffer = new byte[512];

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // Create a new TCP socket and connect to the server
            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSoc.Connect(new IPEndPoint(IPAddress.Loopback, 8888));

            Debug.Log("Connected to server: " + clientSoc.RemoteEndPoint.ToString());

            // Start receiving messages from the server
            clientSoc.BeginReceive(inBuffer, 0, inBuffer.Length, SocketFlags.None, ReceiveCallback, clientSoc);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }

    // Send a message to the server when the "Send" button is clicked
    public void SendMessage()
    {
        string message = inputField.text;

        // Convert the message string to a byte array
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);

        // Send the message to the server
        clientSoc.Send(messageBytes);

        textObject.text += "You: " + message + "\n";
        inputField.text = null;
    }

    // Callback function that is called when a message is received from the server
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            // Get the socket that received the message
            Socket clientSoc = (Socket)result.AsyncState;

            // Get the number of bytes received
            int bytesReceived = clientSoc.EndReceive(result);

            if (bytesReceived > 0)
            {
                // Convert the received bytes to a string and display it on the Text object
                string messageString = Encoding.ASCII.GetString(inBuffer, 0, bytesReceived);
                textObject.text += "Client: " + messageString + "\n";

                Debug.Log("Received " + bytesReceived.ToString() + " bytes from server: " + clientSoc.RemoteEndPoint.ToString());

                // Start receiving the next message
                clientSoc.BeginReceive(inBuffer, 0, inBuffer.Length, SocketFlags.None, ReceiveCallback, clientSoc);
            }
            else
            {
                // If the connection was closed by the server, close the socket
                clientSoc.Close();
                Debug.Log("Connection closed by server: " + clientSoc.RemoteEndPoint.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }
    void OnApplicationQuit()
    {
        // Close the TCP socket when the application quits
        clientSoc.Close();
    }
}