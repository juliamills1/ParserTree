using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CamController : MonoBehaviour
{
    public float Sensitivity
    {
        get {return sensitivity;}
        set {sensitivity = value;}
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";

    // current movement (stationary/walk/run)
    private float mode = 0f;
    private float previousMode = -1f;
    float speed;

    void Update()
    {
        // quit game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        // update mode and speed according to current keys pressed
        if (Walking())
        {
            speed = 6f;
            mode = 1f;
        }
        else
        {
            // start stillness timer
            if (mode == 1)
            {
                mode = 0f;
            }
        }

        Vector3 mvmt = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            mvmt = transform.forward;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {  
            mvmt = -transform.forward;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {  
            mvmt = -transform.right;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            mvmt = transform.right;
        }

        //mvmt.y = 0;
        this.transform.Translate(mvmt * Time.deltaTime * speed, Space.World);

        // map rotation to mouse position input
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        transform.localRotation = xQuat * yQuat;

        // if mode has changed
        if (mode != previousMode)
        {
            previousMode = mode;
        }
    }

    // use up/down arrow keys to move forward/backwards
    bool Walking()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
               Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) ||
               Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
    }
}