using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MAthTest : MonoBehaviour
{
    [SerializeField] Transform transPoint1;
    [SerializeField] Transform transPoint2;
    [SerializeField] Transform transPoint3;
    [SerializeField] Transform between1;
    [SerializeField] Transform between2;
    [SerializeField] Transform intersection;

    [SerializeField] Transform Circle;

    [SerializeField] LineRenderer lineRenderer1;
    [SerializeField] LineRenderer lineRenderer2;
    LineEquation LineEquation1;
    LineEquation LineEquation2;

    //[SerializeField] float slope1;
    //[SerializeField] float slope2;


    private void Awake()
    {
        //initializing the two line equations
        LineEquation1 = new LineEquation(transPoint1.position, transPoint2.position);
        LineEquation2 = new LineEquation(transPoint2.position, transPoint3.position);
    }

    private void Update()
    {
        //getting the points between the three points
        between1.position = PointBetween(transPoint1.position, transPoint2.position);
        between2.position = PointBetween(transPoint2.position, transPoint3.position);

        //making a line out of the between points, rotated 90 degrees from the two main points
        LineEquation1.NewEquation(between1.position, 
            WholeAngleBetweenPoints(transPoint1.position, transPoint2.position)+90);
        LineEquation2.NewEquation(between2.position,
            WholeAngleBetweenPoints(transPoint2.position, transPoint3.position) + 90);

        //the line renderers are 2000 units long
        float x1 = -1000;
        float x2 = 1000;
        //line renderer 1 points
        Vector3 pos1 = new Vector3(x1, LineEquation1.Evaluate(x1), 0);
        Vector3 pos2 = new Vector3(x2, LineEquation1.Evaluate(x2), 0);

        //line renderer 2 points
        Vector3 pos3 = new Vector3(x1, LineEquation2.Evaluate(x1), 0);
        Vector3 pos4 = new Vector3(x2, LineEquation2.Evaluate(x2), 0);
        lineRenderer1.SetPositions(new Vector3[] { pos1, pos2});
        lineRenderer2.SetPositions(new Vector3[] { pos3, pos4 });

        //if the lines are parrallel, no circle can be created
        if (LineEquation1.slope != LineEquation2.slope)
        {
            //float circleX = (LineEquation2.b - LineEquation1.b) / (LineEquation1.slope - LineEquation2.slope);
            float circleX = (-LineEquation1.b + LineEquation2.b + (LineEquation1.slope * LineEquation1.xOffset) - (LineEquation2.slope * LineEquation2.xOffset))/(LineEquation1.slope - LineEquation2.slope);
            float circleY = LineEquation1.slope * (circleX- LineEquation1.xOffset) + LineEquation1.b;
            Circle.position = new Vector3(circleX, circleY, 0);
            float circleSize = Vector2.Distance(transPoint2.position, Circle.position) * 2;
            Circle.localScale = new Vector3(circleSize, circleSize, 1);

            intersection.transform.position = new Vector3(circleX, circleY, 0);
        }

        //troubleshooting
        //slope1 = LineEquation1.slope;
        //slope2 = LineEquation2.slope;
    }

    //OBSOLETE converted to a class instead
    /*
    Func<float, float> GetEquation(Vector2 p1, Vector2 p2)
    {
        float m = (p2.y - p1.y) / (p2.x - p1.x);
        return (x) => (m * (x - p1.x) + p1.y);
    }

    Func<float, float> GetEquation(Vector2 p1, float angle)
    {
        float m;
        switch (angle)
        {
            case 90:
            case 270:
                m = 9999;
                break;
            default:
                m = Mathf.Tan(angle * Mathf.Deg2Rad);
                break;
        }
        return (x) => (m * (x - p1.x) + p1.y);
    }*/

    Vector2 PointBetween(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }

    public float WholeAngleBetweenPoints(Vector3 center, Vector3 to)
    {
        Vector2 direction = to - center;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }


    //Toggles
    public void ToggleIntersectionPoint(bool toggle)
    {
        intersection.GetComponent<Renderer>().enabled = toggle;
    }

    public void ToggleInbetweenPoint(bool toggle)
    {
        between1.GetComponent<Renderer>().enabled = toggle;
        between2.GetComponent<Renderer>().enabled = toggle;
    }
    public void ToggleInbetweenLines(bool toggle)
    {
        lineRenderer1.enabled = toggle;
        lineRenderer2.enabled = toggle;
    }

}

public class LineEquation
{
    //every line has a y=mx+b formula.
    public float slope { get; private set; } //m
    public float b { get; private set; }    //b.  You can think of this as a Y offset
    public float xOffset { get; private set; } //our x has an offset

    Func<float, float> equation;//The equation stored as a function

    //we can create a line from iether 2 points, or a point and an angle
    public LineEquation(Vector2 p1, Vector2 p2)
    {
        NewEquation(p1, p2);
    }

    public LineEquation(Vector2 p1, float angle)
    {
        NewEquation(p1, angle);
    }

    public void NewEquation(Vector2 p1, Vector2 p2)
    {
        slope = (p2.y - p1.y) / (p2.x - p1.x);
        b = p1.y;
        xOffset = p1.x;
        equation = (x) => (slope * (x - p1.x) + p1.y);
    }

    public void NewEquation(Vector2 p1, float angle)
    {
        switch (angle)
        {
            case 90:
            case 270:
                slope = 9999;
                break;
            default:
                slope = Mathf.Tan(angle * Mathf.Deg2Rad);
                break;
        }
        b = p1.y;
        xOffset = p1.x;
        equation = (x) => (slope * (x - p1.x) + p1.y);
    }

    public float Evaluate(float x) => equation(x);
}
