using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelSlider : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float easing = 0.5f;

    private float swipeThreshhold = 0.2f;
    private float directionThreshold = 15.0f;
    private Vector3 panelLocation;
    private int verticalPanelPivot = 0;
    private int horizontalPanelPivot = 0;
    private bool moveDirectionVert;
    private bool moveDirectionHori;
    float Verticaldifeerence;
    float HorizontalDifference;


    void Start()
    {
        panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if( verticalPanelPivot != 0 )
        {
            Verticaldifeerence = eventData.pressPosition.y - eventData.position.y;
            if (Verticaldifeerence > directionThreshold || Verticaldifeerence < -directionThreshold)
            {
                transform.position = panelLocation - new Vector3(0, Verticaldifeerence, 0);
                moveDirectionVert = true;
                moveDirectionHori = false;

            }
        }else if ( horizontalPanelPivot != 0 )
        {
            HorizontalDifference = eventData.pressPosition.x - eventData.position.x;
            if (HorizontalDifference > directionThreshold || HorizontalDifference < -directionThreshold)
            {
                transform.position = panelLocation - new Vector3(HorizontalDifference, 0, 0);
                moveDirectionHori = true;
                moveDirectionVert = false;
            }
        }
        else
        {
            Verticaldifeerence = eventData.pressPosition.y - eventData.position.y;
            HorizontalDifference = eventData.pressPosition.x - eventData.position.x;
            if (Verticaldifeerence > directionThreshold || Verticaldifeerence < -directionThreshold)
            {
                transform.position = panelLocation - new Vector3(0, Verticaldifeerence, 0);
                moveDirectionVert = true;
                moveDirectionHori = false;

            }
            else if (HorizontalDifference > directionThreshold || HorizontalDifference < -directionThreshold)
            {
                transform.position = panelLocation - new Vector3(HorizontalDifference, 0, 0);
                moveDirectionHori = true;
                moveDirectionVert = false;
            }
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (moveDirectionVert)
        {
            VerticalSlider(eventData);
        }else if (moveDirectionHori)
        {
            HorizontalSlider(eventData);
        }
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

    void VerticalSlider(PointerEventData eventData)
    {
        float verticalSwipePercentage = (eventData.pressPosition.y - eventData.position.y) / Screen.height;
        if (Mathf.Abs(verticalSwipePercentage) >= swipeThreshhold)
        {
            Vector3 newLocation = panelLocation;
            if (verticalSwipePercentage > 0 && verticalPanelPivot < 1)
            {
                verticalPanelPivot++;
                newLocation += new Vector3(0, -Screen.height, 0);
                Debug.Log("Going down " + verticalPanelPivot);
            }
            else if (verticalSwipePercentage < 0 && verticalPanelPivot > -1 && verticalPanelPivot < 2)
            {
                verticalPanelPivot--;
                newLocation += new Vector3(0, Screen.height, 0);
                Debug.Log("Going Up " + verticalPanelPivot);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    void HorizontalSlider(PointerEventData eventData)
    {
        float horizontalSwipePercentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if (Mathf.Abs(horizontalSwipePercentage) >= swipeThreshhold)
        {
            Vector3 newLocation = panelLocation;
            if (horizontalSwipePercentage > 0 && horizontalPanelPivot < 1)
            {
                horizontalPanelPivot++;
                newLocation += new Vector3(-Screen.width, 0, 0);
                Debug.Log("Going left " + horizontalPanelPivot);
            }
            else if (horizontalSwipePercentage < 0 && horizontalPanelPivot > -1 && horizontalPanelPivot < 2)
            {
                horizontalPanelPivot--;
                newLocation += new Vector3(Screen.width, 0, 0);
                Debug.Log("Going right " + horizontalPanelPivot);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }
}
