using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Class <c>TrackingBody</c> is used as a general representation of a Tracking-DataStream.
///     After starting the game, this script will remove itself from a gameobject and replace itself with either the according Vicon or DTrack scipt, 
///         while passing the necessary parameters along.
///     <param>host</param> IP-Adress at which the data will be expected (Vicon). 
///     <param>port</param> Port at which the data will be expected (DTRack/Vicon). 
///     <param>useVicon</param> Toggle if this object should use the Vicon Script. 
///     <param>useDTrack</param> Toggle if this object should use the DTrack Script. 
///         Note: If <param>useVicon</param> and <param>useDTrack</param> are both enabled this script will prefer the Vicon-script.
///     Author Jan Klemens
/// </summary>
public class TrackingDataStream : MonoBehaviour
{
    public string host = "localhost";
    public int port = 801;
    public bool useVicon = true;
    public bool useDTrack = false;
    void Start()
    {
        if(useVicon){
            gameObject.AddComponent<ViconDataStreamClient>();
            gameObject.GetComponent<ViconDataStreamClient>().Port = port.ToString();
            gameObject.GetComponent<ViconDataStreamClient>().HostName = host;
            Destroy(gameObject.GetComponent<TrackingDataStream>());
        }
        else if(useDTrack){
            gameObject.AddComponent<DTrack.DTrack>();
            gameObject.GetComponent<DTrack.DTrack>().listenPort = port;
            Destroy(gameObject.GetComponent<TrackingDataStream>());
        }
    }
}
