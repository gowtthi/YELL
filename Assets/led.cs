using UnityEngine;                        // These are the librarys being used
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets; 

public class led : MonoBehaviour {
	int c=1;
	bool socketReady = false;                // global variables are setup here
	TcpClient mySocket;
	public NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	public String Host = "192.168.43.195";
	public Int32 Port = 80; 
	public bool lightStatus;


	void Start() {
		setupSocket ();                        // setup the server connection when the program starts
	}


	public void setupSocket() {                            // Socket setup here
		try {                
			mySocket = new TcpClient(Host, Port);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);                // catch any exceptions
		}
	}

	public void writeSocket(string theLine) {            // function to write data out
		if (!socketReady)
			return;
		String tmpString = theLine;
		theWriter.Write(tmpString);
		theWriter.Flush();


	}


	public void closeSocket() {                            // function to close the socket
		if (!socketReady)
			return;
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}

	public void maintainConnection(){                    // function to maintain the connection (not sure why! but Im sure it will become a solution to a problem at somestage)
		if(!theStream.CanRead) {
			setupSocket();
		}
	}

	void OnMouseDown (){
		setupSocket ();
		if (c % 2 == 0) {
			writeSocket ("1");
		} else {
			writeSocket ("0");
		}
		c++;
		closeSocket ();
		maintainConnection ();
	}
} // end class ClientSocket





/*using System.Collections;

using UnityEngine;
using System.IO.Ports;
public class LED : MonoBehaviour {

	public SerialPort serial =new SerialPort ("COM8",9600);

	private bool lightState = false;
	public GameObject light = null;
	public AudioClip clip;


	public void OnMouseDown(){
		if (serial.IsOpen == false)
			serial.Open ();
		if (lightState == false) {
			serial.Write ("A");

			lightState = true;
			if (light != null && light.GetComponent<Light>() != null) {
				light.GetComponent<Light>().enabled = lightState;
				light.GetComponent<AudioSource>().PlayOneShot (clip);
			}
		}else{
			serial.Write ("a");
			lightState = false;
			if (light != null && light.GetComponent<Light>() != null) {
				light.GetComponent<Light>().enabled = lightState;
				light.GetComponent<AudioSource>().PlayOneShot (clip);
			}
		}
	}
}
*/
