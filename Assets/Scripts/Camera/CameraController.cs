using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PublicUtility;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public GameObject UI;
    UIManager m_UIManager;

    public Transform followTransform;
    public Camera CurrentCam;

    public float normalSpeed;
    public float fasterSpeed;

    public float movementTime;
    public float rotationAmount;
    public float zoomAmount;

    public Vector2 zoomMinMax;

    float movementSpeed = 1;

    Vector3 newPos;
    Quaternion newRot;
    float newZoom;

    Vector3 dragStartPos;
    Vector3 dragCurrentPos;
    Vector3 rotStartPos;
    Vector3 rotCurrentPos;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        m_UIManager = UIManager.Instance;
        if(!m_UIManager)
        {
            Debug.LogError("UIManager was null on run time");
        }

        newPos = transform.position; 
        newRot = transform.rotation;
        if (CurrentCam)
            newZoom = CurrentCam.orthographicSize;
        else
        {
            Debug.LogError("Camera has not been set to CameraRig");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, movementTime * Time.deltaTime);
        }
        else
        {
            HandleMovement();
            HandleMouse();
        }

        HandleZoom();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
            m_UIManager.OnDeselect();
        }
    }

    void HandleMovement()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fasterSpeed;
        }
        else 
        {
            movementSpeed = normalSpeed;
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPos += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPos += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPos += (transform.right * -movementSpeed );
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPos += (transform.right * movementSpeed);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            newRot *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if(Input.GetKey(KeyCode.E))
        {
            newRot *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        newZoom = Mathf.Clamp(newZoom, zoomMinMax.x, zoomMinMax.y);

        transform.position = Vector3.Lerp(transform.position, newPos, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, movementTime * Time.deltaTime);

    }

    void HandleMouse()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPos = ray.GetPoint(entry);
            }
        }

        if(Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPos = ray.GetPoint(entry);

                newPos = transform.position + dragStartPos - dragCurrentPos; 
            }
        }    

        if(Input.GetMouseButtonDown(2))
        {
            rotStartPos = Input.mousePosition;
        }
        if(Input.GetMouseButton(2))
        {
            rotCurrentPos = Input.mousePosition;
            Vector3 differ = rotStartPos - rotCurrentPos;
            rotStartPos = rotCurrentPos;

            newRot *= Quaternion.Euler(Vector3.up * (-differ.x / 5));
        }
    }

    void HandleZoom()
    {
        //if (Input.GetKey(KeyCode.R))
        //{
        //    newZoom -= zoomAmount;
        //}
        //if (Input.GetKey(KeyCode.F))
        //{
        //    newZoom += zoomAmount;
        //}

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom -= Input.mouseScrollDelta.y * (zoomAmount * 100);
        }

        newZoom = Mathf.Clamp(newZoom, zoomMinMax.x, zoomMinMax.y);

        CurrentCam.orthographicSize = Mathf.Lerp(CurrentCam.orthographicSize, newZoom, movementTime * Time.deltaTime);
    }
}
