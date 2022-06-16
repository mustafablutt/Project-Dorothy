using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;//Set the pos of the platform to pos of one of the points using index "startingPoint" 
    }
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,points[i].position) < 0.02f)
        {
            i++;
            if(i == points.Length)//check if the platform was on the last point after the index increase 
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);//moving platform with the index i
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
