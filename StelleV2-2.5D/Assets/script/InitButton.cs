using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InitButton : MonoBehaviour
{
    private GameObject lastSelectt;

    // Start is called before the first frame update

    void Start()
    {
        lastSelectt = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectt);
        }
        else
        {
            lastSelectt = EventSystem.current.currentSelectedGameObject;
        }
    }
}
