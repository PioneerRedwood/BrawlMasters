using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance = null;

    public string identifier = "Chris";
    public string IPAddress = "127.0.0.1";
    public int Port = 9000;

    private TcpClient Client;
    private Thread ClientThread;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Send("Space key down");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Send("Q key down");
        }
    }

    private void OnApplicationQuit()
    {
        if (ClientThread != null)
        {
            ClientThread.Abort();
        }
    }

    public bool IsConnected()
    {
        return Client.Connected;
    }

    public void SendData(string data)
    {
        Send(data);
    }

    private void ConnectToServer()
    {
        try
        {
            ClientThread = new Thread(Communicate);
            ClientThread.IsBackground = true;
            ClientThread.Start();
        }
        catch (SocketException e)
        {
            CatchSocketError(e);
        }
    }

    void Communicate()
    {
        try
        {
            Client = new TcpClient(IPAddress, Port);
            Send(identifier);
        }
        catch (SocketException e)
        {
            CatchSocketError(e);
        }

        Recv();
    }

    void Recv()
    {
        try
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                using (NetworkStream stream = Client.GetStream())
                {
                    int length;

                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        byte[] data = new byte[length];
                        Array.Copy(bytes, 0, data, 0, length);

                        string serverMsg = Encoding.Default.GetString(data);
                        Debug.Log(serverMsg);
                    }
                }
            }
        }
        catch (SocketException e)
        {
            CatchSocketError(e);
        }
    }

    void Send(string data)
    {
        if(Client == null)
        {
            return;
        }

        try
        {
            NetworkStream stream = Client.GetStream();

            if (stream.CanWrite)
            {
                byte[] clientMsgByteArray = Encoding.Default.GetBytes(data);
                stream.Write(clientMsgByteArray, 0, clientMsgByteArray.Length);
            }
        }
        catch(SocketException e)
        {
            CatchSocketError(e);
        }
    }

    void CatchSocketError(SocketException e)
    {
        Debug.Log(e.ToString());
        if (e.SocketErrorCode == SocketError.ConnectionRefused)
        {
            Debug.Log("Connection refused .. tring to connect server again\n");
            if (ClientThread.IsAlive)
            {
                Thread.Sleep(1000);
                ClientThread.Abort();
            }
            ConnectToServer();
        }
    }
}
