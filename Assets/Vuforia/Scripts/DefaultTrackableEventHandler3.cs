/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/
using System.Collections;
using UnityEngine;
using System.IO.Ports;
using System;
using System.IO;
using System.Net.Sockets; 


namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler3 : MonoBehaviour,
                                                ITrackableEventHandler
    {
		//public SerialPort serial =new SerialPort ("COM8",9600);

		/*private bool lightState = false;
		public GameObject light = null;
		public AudioClip clip;*/

		int c=0;
		bool socketReady = false;                // global variables are setup here
		TcpClient mySocket;
		public NetworkStream theStream;
		StreamWriter theWriter;
		StreamReader theReader;
		public String Host = "192.168.43.195";
		public Int32 Port = 80; 
		public bool lightStatus;




        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
			setupSocket (); 
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





        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
		/// 


        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
		
            }
            else
            {
                OnTrackingLost();
		            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

		/*	if (serial.IsOpen == false)
				serial.Open ();
			if (lightState == false) {
				serial.Write ("A");
				lightState = true;
				if (GetComponent<Light>() != null && GetComponent<Light>().GetComponent<Light>() != null) {
					GetComponent<Light>().GetComponent<Light>().enabled = lightState;
					GetComponent<Light>().GetComponent<AudioSource>().PlayOneShot (clip);
				}
			}*/
			setupSocket ();
			if (c % 2 == 0) {
				writeSocket ("1");

			} else {
				writeSocket ("0");

			}
			c++;
			closeSocket ();
			maintainConnection ();

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }

		/*public void gowon(){
			

		}*/

		/*public void gowoff(){
			

		}*/
        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

		/*	if (serial.IsOpen == true) {
				
				if (lightState = false) {
					serial.Write ("a");
					serial.Close ();
					if (GetComponent<Light> () != null && GetComponent<Light> ().GetComponent<Light> () != null) {
						GetComponent<Light> ().GetComponent<Light> ().enabled = lightState;
						GetComponent<Light> ().GetComponent<AudioSource> ().PlayOneShot (clip);
					}
				}
			}
			*/
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
