using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float yOffset;
    
    private float lenght;
    private float xPosition;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;   
    }
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        
        if (distanceMoved > xPosition + lenght) xPosition += lenght;
        else if (distanceMoved < xPosition - lenght) xPosition -= lenght;
        
        transform.position = new Vector3(transform.position.x, cam.transform.position.y + yOffset, transform.position.z);
    }
}