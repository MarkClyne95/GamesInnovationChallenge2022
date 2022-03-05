using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCollection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject[] trash;

    private void Awake()
    {
        trash = GameObject.FindGameObjectsWithTag("Trash");
    }

    private void Start()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Up");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject != null)
                {
                    Debug.Log($"{clickedObject.name} hit");
                }
            }
        }
    }
}
