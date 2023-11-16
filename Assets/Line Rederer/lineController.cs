using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lineController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject point1, point2;
    LineRenderer line;
    public GameObject canvas;
    public GameObject panel;
    public GameObject text;
    public GameObject camera;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, point1.transform.position);
        line.SetPosition(1, point2.transform.position);

        canvas.transform.position = ((point1.transform.position + point2.transform.position) / 2);

        canvas.transform.LookAt(line.transform);

        Vector3 targetDir = point2.transform.position - canvas.transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);


        //Vector3 newRotation1 = new Vector3(canvas.transform.rotation.x, canvas.transform.rotation.y-90, canvas.transform.rotation.z+ angle);
        //canvas.transform.eulerAngles = newRotation1;

        

        if (canvas.transform.position.y >= 0)
        {
            Vector3 newRotation1 = new Vector3(canvas.transform.rotation.x, canvas.transform.rotation.y - 90, canvas.transform.rotation.z + angle);
            canvas.transform.eulerAngles = newRotation1;
        }
        else
        {
            Vector3 newRotation1 = new Vector3(canvas.transform.rotation.x, canvas.transform.rotation.y - 90, canvas.transform.rotation.z - angle);
            canvas.transform.eulerAngles = newRotation1;
        }

        if (canvas.transform.position.z >= 0)
        {
            //text.transform.eulerAngles = new Vector3(text.transform.eulerAngles.x, text.transform.eulerAngles.y, text.transform.eulerAngles.z );
            text.transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            text.transform.localScale = new Vector3(-1, -1, 0);
        }
 /*       if (canvas.transform.position.x >= 0)
        {
            Vector3 newRotation1 = new Vector3(canvas.transform.rotation.x, canvas.transform.rotation.y - 90, canvas.transform.rotation.z + angle);
            canvas.transform.eulerAngles = newRotation1;
        }
        else
        {
            Vector3 newRotation1 = new Vector3(canvas.transform.rotation.x, canvas.transform.rotation.y - 90, angle -canvas.transform.rotation.z  );
            canvas.transform.eulerAngles = newRotation1;
        }*/


        /*

        panel.transform.LookAt(point2.transform);
        Vector3 newRotation = new Vector3(panel.transform.rotation.x, panel.transform.rotation.y - 90, panel.transform.rotation.z);
        panel.transform.eulerAngles = newRotation;

        canvas.transform.LookAt(camera.transform);
        Vector3 newRotation1 = new Vector3(canvas.transform.rotation.x, canvas.transform.rotation.y, canvas.transform.rotation.z );
        canvas.transform.eulerAngles = newRotation1;*/

        //Transform canvasLookingCamera = canvas.transform;


        //canvas.transform.LookAt(point2.transform);
        //Vector3 newRotation = new Vector3(canvas.transform.eulerAngles.x, canvas.transform.eulerAngles.y, point1.transform.eulerAngles.z + point2.transform.eulerAngles.z);
        //canvas.transform.eulerAngles = newRotation;

        //text.transform.LookAt(camera.transform);

        // This will make arrow look directly toward target
        //canvas.transform.LookAt(camera.transform);

        //Vector3 newRotation = new Vector3(canvas.transform.eulerAngles.x, canvas.transform.eulerAngles.y, point1.transform.eulerAngles.z + point2.transform.eulerAngles.z);
        //canvas.transform.eulerAngles = newRotation;

        /*
                if (point2.transform.position.z >= 0)
                {
                    Vector3 newRotation1 = new Vector3(text.transform.eulerAngles.x, text.transform.eulerAngles.y, -90);
                    text.transform.eulerAngles = newRotation1;
                }
                else
                {
                    Vector3 newRotation1 = new Vector3(text.transform.eulerAngles.x, text.transform.eulerAngles.y, 90);
                    text.transform.eulerAngles = newRotation1;
                }*/


        // To keep arrow on floor instead of bending into floor.
        //Vector3 newRotation = new Vector3(canvas.transform.eulerAngles.x, canvas.transform.eulerAngles.y, canvas.transform.eulerAngles.z);
        //canvas.transform.eulerAngles = newRotation;


    }
}
