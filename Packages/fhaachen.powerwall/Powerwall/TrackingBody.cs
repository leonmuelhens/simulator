using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Class <c>TrackingBody</c> is used as a general representation of a tracked object.
///     After starting the game, this script will remove itself from a gameobject and replace itself with either the according Vicon or DTrack scipt, 
///         while passing the necessary parameters along.
///     <param>bodyName</param> Name of the Tracked object (Vicon). 
///     <param>bodyID</param> ID of the Tracked object (DTRack). 
///     <param>DataStream</param> GameObject that carries an Tracking Datastream. 
///     <param>useVicon</param> Toggle if this object should use the Vicon Script. 
///     <param>useDTrack</param> Toggle if this object should use the DTrack Script. 
///         Note: If <param>useVicon</param> and <param>useDTrack</param> are both enabled this script will prefer the Vicon-script.
///     Author Jan Klemens
/// </summary>
public class TrackingBody : MonoBehaviour
{
    public string bodyName = "Object";
    public int bodyID = 0;
    private GameObject DataStream;
    public bool useVicon = true;
    public bool useDTrack = false;
    void Start()
    {                    
        StartCoroutine(ScriptSetup());
    }

    ///<summary>
    ///     Coroutine that will replace this script on an gameobject with the according Dtrack/Vicon counterpart (if possible).
    ///</summary>
    IEnumerator ScriptSetup(){
        //prevents script from initializing before Datastream
        yield return new WaitForSeconds(1);

        DataStream = GameObject.Find("TrackingDataStream");
        if(DataStream == null)
            Debug.Log("No DataStream Object found.");
        else{
            if(useVicon){
                ViconDataStreamClient viconClient = DataStream.GetComponent<ViconDataStreamClient>();
                if(viconClient == null)
                    Debug.Log("DataStream is not set up to be Vicon DataStream.");
                else{
                    gameObject.AddComponent<UnityVicon.RBScript>();
                    gameObject.GetComponent<UnityVicon.RBScript>().ObjectName = bodyName;
                    gameObject.GetComponent<UnityVicon.RBScript>().Client = viconClient;
                    Destroy(gameObject.GetComponent<TrackingBody>());
                }
            }
            else if(useDTrack){
                DTrack.DTrack DTrackClient = DataStream.GetComponent<DTrack.DTrack>();
                if(DTrackClient == null)
                    Debug.Log("DataStream is not set up to be DTrack DataStream.");
                else{
                    gameObject.AddComponent<DTrack.DTrackReceiver6Dof>();
                    gameObject.GetComponent<DTrack.DTrackReceiver6Dof>().bodyId = bodyID;
                    Destroy(gameObject.GetComponent<TrackingBody>());
                }
            }                
        }
    }
}
