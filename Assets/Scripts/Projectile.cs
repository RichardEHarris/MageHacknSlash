using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public List<Vector3> points;
    public float duration = 1f;
    public GameObject graphic;

    private bool play = false;
    private GameObject graphicsInstance;
    private float playTime;

    private void Start()
    {
        playTime = duration;
        //graphicsInstance = Instantiate(graphic, points[0], transform.rotation, transform);
    }
    //TODO handle collision
    void FixedUpdate()
    {
        if (play)
        {
            if (graphicsInstance == null)
            {
                graphicsInstance = Instantiate(graphic, points[0], transform.rotation, transform);
                graphicsInstance.transform.localScale = transform.lossyScale;
            }
            int maxIndex = points.Count-1;
            float deltaDuration = (duration-playTime) / duration;
            int index = Mathf.Clamp(Mathf.CeilToInt(maxIndex * deltaDuration), 0, maxIndex);
            //Debug.Log(maxIndex + " * " + deltaDuration + "= " + index);
            Vector3 destination = points[index];
            Vector3 startPos = new Vector3();
            if (index == 0)
            {
                startPos = points[0];
            } else
            {
                startPos = points[index - 1];
            }
            float speed = (deltaDuration * points.Count) - index;
            graphicsInstance.transform.localPosition = Vector3.Lerp(startPos, destination, speed);
            //Debug.Log(graphicsInstance.transform.localPosition);
            //Debug.Log( index + " , "+ (((duration - playTime) * points.Count) - (index * duration)));
            Debug.Log(index + " : " + deltaDuration + " : " + speed + " : " + startPos + "->" + destination);
            playTime -= Time.deltaTime;
            if (playTime <= 0)
            {
                playTime = duration;
                End();
            }
        }
    }

    public void Pause()
    {
        play = false;
    }

    public void End()
    {
        play = false;
        playTime = duration;
        graphicsInstance.GetComponent<ParticleSystem>().Stop();
        Destroy(graphicsInstance,2);
        graphicsInstance = null;
    }

    public void Play()
    {
        play = true;
        playTime = duration;
    }
}
