using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawLine : MonoBehaviour
{
    //reference to LineRenderer component
    private LineRenderer line;

    //car to store mouse position on the screen
    private Vector3 mousePos;

    //assign a material to the Line Renderer in the Inspector
    public Material material;

    //number of lines drawn
    public static int currLines = 0;

    private Vector3 mouseStartPos;

    void Update()
    {
        if (currLines < 1)
        {
            //Create new Line on left mouse click(down)
            if (Input.GetMouseButtonDown(0))
            {
                //check if there is no line renderer created
                if (line == null)
                {
                    //create the line
                    createLine();
                    mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                //get the mouse position
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //set the z co ordinate to 0 as we are only interested in the xy axes
                mousePos.z = 0;
                //set the start point and end point of the line renderer
                line.SetPosition(0, mousePos);
                line.SetPosition(1, mousePos);
            }
            //if line renderer exists and left mouse button is click exited (up)
            else if (Input.GetMouseButtonUp(0) && line)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                //set the end point of the line renderer to current mouse position
                line.SetPosition(1, mousePos);
                mouseStartPos.z = 0;
                AddColliderToLine(line, mouseStartPos, mousePos);
                //set line as null once the line is created
                line = null;
                currLines++;
            }
            //if mouse button is held clicked and line exists
            else if (Input.GetMouseButton(0) && line)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                //set the end position as current position but dont set line as null as the mouse click is not exited
                line.SetPosition(1, mousePos);
            }
        }

        if (Target.LevelDone)
        {
            var line0 = GameObject.Find("Line0").GetComponent<LineRenderer>();
            line0.useWorldSpace = false;
            line0.transform.position += new Vector3(0, transform.position.y - 0.1f, 0);
        }
    }

    //method to create line
    private void createLine()
    {
        //create a new empty gameobject and line renderer component
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        //assign the material to the line
        line.material = material;
        //set the number of points to the line
        line.SetVertexCount(2);
        //set the width
        line.SetWidth(0.15f, 0.15f);
        //render line to the world origin and not to the object's position
        line.useWorldSpace = true;
        line.numCapVertices = 50;
        line.shadowCastingMode = ShadowCastingMode.On;
    }

    private void AddColliderToLine(LineRenderer line, Vector3 startPoint, Vector3 endPoint)
    {
        //create the collider for the line
        BoxCollider lineCollider = new GameObject("LineCollider").AddComponent<BoxCollider>();
        //set the collider as a child of your line
        lineCollider.transform.parent = line.transform;
        // get width of collider from line 
        float lineWidth = line.endWidth;
        // get the length of the line using the Distance method
        float lineLength = Vector3.Distance(startPoint, endPoint);
        // size of collider is set where X is length of line, Y is width of line
        //z will be how far the collider reaches to the sky
        lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
        // get the midPoint
        Vector3 midPoint = (startPoint + endPoint) / 2;
        // move the created collider to the midPoint
        lineCollider.transform.position = midPoint;

        //heres the beef of the function, Mathf.Atan2 wants the slope, be careful however because it wants it in a weird form
        //it will divide for you so just plug in your (y2-y1),(x2,x1)
        float angle = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));

        // angle now holds our answer but it's in radians, we want degrees
        // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
        angle *= Mathf.Rad2Deg;

        //were interested in the inverse so multiply by -1
        //angle *= -1;
        if ((endPoint.y - startPoint.y) / (endPoint.x - startPoint.x) < 0)
        {
            angle = 180 + angle;
        }

        // now apply the rotation to the collider's transform, carful where you put the angle variable
        // in 3d space you don't wan't to rotate on your y axis
        //lineCollider.transform.Rotate(0, angle, 0);
        lineCollider.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}