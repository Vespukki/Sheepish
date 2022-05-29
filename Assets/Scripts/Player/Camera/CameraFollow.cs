using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float xOffset;

    PlayerMovement mover;

    CamZone zone = null;
    CamZone lastzone;

    float CamYBound;
    float CamXBound;

    private void Awake()
    {
        mover = GetComponent<PlayerMovement>();

        CamYBound = (Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)) - Camera.main.ViewportToWorldPoint(Vector3.zero)).y;
        CamXBound = (Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)) - Camera.main.ViewportToWorldPoint(Vector3.zero)).x;
    }

    private void FixedUpdate()
    {
        lastzone = zone;
        zone = GetZone();

        if(zone != null)
        {
            zone.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x = xOffset * mover.lookingDir;
        }


        if (zone != lastzone)
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
