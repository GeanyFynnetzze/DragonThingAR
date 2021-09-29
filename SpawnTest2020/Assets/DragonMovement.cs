using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragonMovement : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] RectTransform gamePad;
    [SerializeField] float moveSpeed = 0.5f;

    GameObject arObject;
    Vector3 move;
    

    
    void Start()
    {
        arObject = GameObject.FindGameObjectWithTag("Player");
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.localPosition = Vector2.ClampMagnitude(eventData.position - (Vector2)gamePad.position, gamePad.rect.width * 0.5f);

        move = new Vector3(transform.localPosition.x, 0f, transform.localPosition.y).normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // do the movement when touched down
        StartCoroutine(PlayerMovement());


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero; // joystick returns to mean pos when not touched
        move = Vector3.zero;
        StopCoroutine(PlayerMovement());

    }

    IEnumerator PlayerMovement()
    {
        while (true)
        {
            arObject.transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            if (move != Vector3.zero)
                arObject.transform.rotation = Quaternion.Slerp
                    (arObject.transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 5.0f);

            yield return null;


        }
    }
}


