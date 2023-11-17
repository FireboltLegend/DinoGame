using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class FlappyBirdSocket : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    
    void Start()
    {
        // Receive on a separate thread so Unity doesn't freeze waiting for data
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
    }

    void Update()
    {
        //Listening
        Connection();
    }
    
    void GetData()
    {
        // Create the server
        server = new TcpListener(IPAddress.Any, connectionPort); //might need a server on python backend
        server.Start();

        // Create a client to get the data stream
        client = server.AcceptTcpClient();

        //check if the server stops when scene ends or not in play mode
        server.Stop();
    }

    void Connection()
    {
        // Read data from the network stream
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        // Decode the bytes into a string
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        // Write Lines
        /*
        if (playing) {
            byte[] WriteBuffer = Encoding.ASCII.GetBytes("");
        }
        else if (gameover) {
            byte[] WriteBuffer = Encoding.ASCII.GetBytes("");
        }
        nwStream.Write(WriteBuffer, 0, WriteBuffer.Length);
        */
    }
}