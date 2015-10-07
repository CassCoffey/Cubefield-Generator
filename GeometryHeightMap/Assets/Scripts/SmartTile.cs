using UnityEngine;
using System.Collections;

public class SmartTile : MonoBehaviour
{
    public GameObject focus;
    public static float maxDistance;
    public static float minDistance;

    public float height;

    public bool oscilate = true;

    private Vector2 flatLoc;
    private float percent;

    // Use this for initialization
    void Start ()
    {
        flatLoc = new Vector2(transform.position.x, transform.position.z);
    }
	
	public void updateVisuals()
    {
        Vector2 focusLoc = new Vector2(focus.transform.position.x, focus.transform.position.z);

        float distance = Vector2.Distance(flatLoc, focusLoc) - minDistance;

        if (distance < 0) distance = 0;

        float goalDistance = maxDistance - minDistance;

        percent = distance / goalDistance;

        if (percent >= 1)
        {
            percent = 1;
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;

            if (oscilate && percent > 0)
            {
                percent += Mathf.Sin(Time.time) * 0.1f;
            }
        }

        transform.localScale = Vector3.Lerp(new Vector3(1, height, 1), new Vector3(0, height, 0), percent);
    }

    public void vanish()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        transform.localScale = Vector3.Lerp(new Vector3(1, height, 1), new Vector3(0, height, 0), 0);
    }
}
