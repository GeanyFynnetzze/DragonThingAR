using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField] GameObject spawnablePrefab;
    [SerializeField] GameObject joystickCanvas;
    Camera arCam;
    bool spawned = false;
    GameObject spawnedObject;
    // Start is called before the first frame update
    void Awake()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        joystickCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawned)
        {
            return;
        }
        


            RaycastHit hit;
            Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

            if (Input.touchCount == 0)
            {
                return;
            }
            if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            spawnedObject = hit.collider.gameObject;
                            
                    }
                        else
                        {
                            SpawnPrefab(m_Hits[0].pose.position);
                        }
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
                {
                    spawnedObject.transform.position = m_Hits[0].pose.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    spawnedObject = null;
                }
            }
        
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawned = true;
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
        joystickCanvas.SetActive(true);

    }
}
