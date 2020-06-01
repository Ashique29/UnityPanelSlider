using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float easing = 0.5f;

    private float swipeThreshhold = 0.2f;
    private Vector3 panelLocation;
    private int panelPivot = 0;

    void Start()
    {
        panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pressPosition);
        float difeerence = eventData.pressPosition.y - eventData.position.y;
        if(difeerence>15 || difeerence < -15)
        {
            Debug.Log("My differences " + difeerence);
            transform.position = panelLocation - new Vector3(0, difeerence, 0);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float swipePercentage = (eventData.pressPosition.y - eventData.position.y) / Screen.height;
        if(Mathf.Abs(swipePercentage) >= swipeThreshhold)
        {
            Vector3 newLocation = panelLocation;
            if(swipePercentage > 0 && panelPivot < 1)
            {
                panelPivot++;
                newLocation += new Vector3(0, -Screen.height, 0);
            }
            else if(swipePercentage < 0 && panelPivot > -1)
            {
                panelPivot--;
                newLocation += new Vector3(0, Screen.height, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
        Debug.Log("End position : " + eventData.position);
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
