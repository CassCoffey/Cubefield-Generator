using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public float fadetime;

    public float height;

    public bool expand;
    public bool raise;

    private Vector3 goalPos;

    private float startTime;
    private float percent = 0.0f;

    private bool fadeIn = true;
    private bool fadeOut = false;

    void Start()
    {
        if (expand)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0, height, 0), new Vector3(1, height, 1), 0);
        }
        if (raise)
        {
            goalPos = transform.position;
            transform.position = Vector3.Lerp(new Vector3(goalPos.x, 0, goalPos.z), goalPos, 0);
        }
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        if (fadeIn)
        {
            percent = (Time.time - startTime) / fadetime;
            
            if (percent >= 1)
            {
                fadeIn = false;
            }
        }

        if (fadeOut)
        {
            percent = 1 - (Time.time - startTime) / fadetime;
        }

        if (expand)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0, height, 0), new Vector3(1, height, 1), percent);
        }
        if (raise)
        {
            transform.position = Vector3.Lerp(new Vector3(goalPos.x, 0, goalPos.z), goalPos, percent);
        }
    }

    public void beginDestroy()
    {
        fadeIn = false;
        fadeOut = true;

        startTime = Time.time;
        Destroy(this.gameObject, fadetime);
    }
}
