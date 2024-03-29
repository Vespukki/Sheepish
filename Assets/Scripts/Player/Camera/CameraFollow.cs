using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float xOffset;

    Rigidbody2D body;

    [HideInInspector] public CamZone zone;
    CamZone lastzone = null;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        zone = GetZone();
        ChangeCam();
    }

    private void FixedUpdate()
    {
        lastzone = zone;
        zone = GetZone();

        if(zone != null)
        {
            zone.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x = Mathf.Sign(xOffset * body.velocity.x);
        }


        if (zone != lastzone)
        {
            ChangeCam();
            
        }
    }

    void ChangeCam()
    {
        if (zone != null)
        {
            zone.vCam.Priority = 1;
            if (zone.vCam.Follow == null)
            {
                zone.vCam.Follow = transform;
            }
        }
        if (lastzone != null)
        {
            lastzone.vCam.Priority = 0;
        }
    }

    CamZone GetZone()
    {
        foreach (var hit in Physics2D.GetRayIntersectionAll(new Ray(new Vector3(transform.position.x, transform.position.y, 10), new Vector3(0, 0, -1))))
        {
            if (hit.collider.TryGetComponent<CamZone>(out CamZone zone))
            {
                return zone;
            }
        }

        return null;
    }
}
