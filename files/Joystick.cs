using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;

namespace Main{
	public class Joystick{
		public static String data;
		public static TcpListener server;  
		public static TcpClient client;
		public static int buffSize;
		public static byte[] inStream;
		public static NetworkStream stream;

		public static Dictionary<string, bool> Button = new Dictionary<string, bool>();
		
		public static string GetLocalIPAddress(){
			IPHostEntry host;
			string localIP = "?";
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
				if(ip.AddressFamily == AddressFamily.InterNetwork)
					localIP = ip.ToString();
			return localIP; 
		}

		public static void Start(){
	        Int32 port = 8888;
	        IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());

	        server = new TcpListener(localAddr, port);
	        server.Start();

	        client = server.AcceptTcpClient();
	        JoystickState.connected = true;

			stream = client.GetStream();
	     	buffSize = client.ReceiveBufferSize;
	        inStream = new byte[buffSize];
	    }

	    public static void Stop(){
	    	server.Stop();
	    	JoystickState.connected 	= false;
	    	JoystickState.delay 		= false;
	    }

	    public static void Run(){ 
	    	while(JoystickState.connected){
		        data = System.Text.Encoding.ASCII.GetString(inStream, 0, stream.Read(inStream, 0, buffSize));
		        if(data == "")
		        	Stop();

		        string[] data1 = data.Split('|');

		        foreach(string d in data1){
		        	if(d != ""){
			        	string[] data2 = d.Split(':'); 
			        	if(data2[1] == "true" || data2[1] == "false")
			        		Button[data2[0]] = Convert.ToBoolean(data2[1]);
			        }
		        }
		    }
	    }   
	}
}