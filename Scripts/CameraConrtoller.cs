using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraConrtoller : MonoBehaviour
{

    public Vector3 Distance;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int dir;
    private float rotationSpeed = 10;
    private float scroll;
    private Camera cameraa;
    public float zoomspeed;
    public float maxsize = 6;
    public float minsize = 2;



    public void CameraZoom()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            cameraa.orthographicSize -= scroll * zoomspeed;
            cameraa.orthographicSize = Mathf.Clamp(cameraa.orthographicSize, minsize,maxsize);
        }

    }
 
    public void mousemove()
    {
        if (Input.GetMouseButton(1))
        {
           float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

            Quaternion rotation = Quaternion.Euler(0, horizontal, 0);
            Distance = rotation * Distance;

            transform.position = player.transform.position + Distance;
            transform.LookAt(player.transform.position); 
        }
    }

    public void playerfacecamera()
    {
        player.transform.LookAt(transform);
    }
    
    void Start()
    {
        transform.position = player.transform.position + Distance;
        cameraa = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + Distance;
        mousemove();
        playerfacecamera();
        CameraZoom();
    }
}
