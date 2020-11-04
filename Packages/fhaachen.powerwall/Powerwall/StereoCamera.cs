using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Class <c>StereoCamera</c> is used to imitate a steroscopic camera in Unity.
///     The resulting output is dependant on the number of connected displays:
///         1 Screen: The left subcamera will diplay its view on display 1.
///         2 Screen: The left subcamera will diplay its view on display 1 and the right subcamera will display its view on display 2.
///         3 Screen: The left subcamera will diplay its view on display 2 and the right subcamera will display its view on display 3.
///     <param>speed</param> Factor that controlls the speed at which the <c>SteroCamera</c> moves when controlled. 
///     <param>rotationSpeed</param> Factor that controlls the speed at which the <c>SteroCamera</c> turns when controlled. 
///     <param>eyeDistance</param> Factor that controlls the distance between the subcameras. 
///     <param>leftEye</param> The left subcamera. 
///     <param>rightEye</param> The right subcamera. 
///     <param>controlKeyBoard</param> Toggle if the <c>StereoCamera</c> should be controlled by the keyboard.
///     <param>controlXbox</param> Toggle if the <c>StereoCamera</c> should be controlled by a Xbox-Controller.
///     Author Jan Klemens
/// </summary>
public class StereoCamera : MonoBehaviour
{
    public float speed = 0.02f;
    public float rotationSpeed = 1f;
    public float eyeDistance = 0.2f;
    
    private GameObject leftEye = null;
    private GameObject rightEye = null;

    public bool controlKeyBoard = true;
    public bool controlXbox  = true;
    void Start()
    {
        leftEye = GameObject.Find("Camera_Left");
        rightEye = GameObject.Find("Camera_Right");
        initScreens();
        SetEyeDistance(eyeDistance);
    }

    void Update(){
        //if(controlKeyBoard)CameraControlKeyBoard();
        //if(controlXbox)CameraControlXbox();
    }

    ///<summary>
    ///     Detects the number of connected displays and maps the subcameras to the displays as mentioned above.
    ///</summary>
    private void initScreens(){
        Debug.Log ("displays connected: " + Display.displays.Length);    
        for (int i = 1; i < Display.displays.Length; i++) Display.displays[i].Activate();
        if(Display.displays.Length == 3 ){
            leftEye.GetComponent<Camera>().targetDisplay = 1;
            rightEye.GetComponent<Camera>().targetDisplay = 2;
        }
    }

    ///<summary>
    ///     Places the subcameras acording to the given eye distance.
    ///     <param>eyeDistance</param> The new Distance between the two subcameras.
    ///</summary>
    private void SetEyeDistance(float eyeDistance){
        leftEye.transform.localPosition = new Vector3(-(eyeDistance/2), 0, 0);
        rightEye.transform.localPosition = new Vector3((eyeDistance/2), 0, 0);
    }

    ///<summary>
    /// Moves the <c>StereoCamera</c> acording to the inputs of the Keyboard.
    ///     w: z+
    ///     a: x-
    ///     s: z-
    ///     d: x+
    ///     r: y+
    ///     f: y-
    ///     e: rotate to the right
    ///     e: rotate to the left
    ///</summary>
    private void CameraControlKeyBoard(){
        Vector3 offset = new Vector3(0,0,0);
        float angle = 0;

        if(Input.GetKey(KeyCode.W)) offset += new Vector3(0f, 0f, speed); 
        if(Input.GetKey(KeyCode.A)) offset += new Vector3(-speed, 0f, 0f); 
        if(Input.GetKey(KeyCode.S)) offset += new Vector3(0f, 0f, -speed); 
        if(Input.GetKey(KeyCode.D)) offset += new Vector3(speed, 0f, 0f); 
        if(Input.GetKey(KeyCode.R)) offset += new Vector3(0f, speed, 0f); 
        if(Input.GetKey(KeyCode.F)) offset += new Vector3(0f, -speed, 0f); 

        if(Input.GetKey(KeyCode.E)) angle += rotationSpeed; 
        if(Input.GetKey(KeyCode.Q)) angle -= rotationSpeed; 

        gameObject.transform.Rotate(new Vector3(0,1,0), angle);
        gameObject.transform.localPosition += gameObject.transform.rotation *  offset;
    }

    ///<summary>
    /// Moves the <c>StereoCamera</c> acording to the inputs of a XboX-Controller.
    ///     left stick X-Axis: x
    ///     left stick Y-Axis: z
    ///     right stick X-Axis: rotation to left/right
    ///     left trigger: y+
    ///     left trigger: y-
    ///</summary>
    private void CameraControlXbox(){
        Vector3 offset = new Vector3(Input.GetAxis("LeftStick_XAxis") * speed, 0, - Input.GetAxis("LeftStick_YAxis") * speed);
        float angle = Input.GetAxis("RightStick_XAxis") * rotationSpeed;

        if(Input.GetKey(KeyCode.Joystick1Button4)) offset += new Vector3(0f, speed, 0f); 
        if(Input.GetKey(KeyCode.Joystick1Button5)) offset += new Vector3(0f, -speed, 0f); 

        gameObject.transform.Rotate(new Vector3(0,1,0), angle);
        gameObject.transform.localPosition += gameObject.transform.rotation *  offset;
    }
}
