using Photon.Pun;
using System.Globalization;
using UnityEngine;

public class CameraFollow : MonoBehaviourPunCallbacks
{
    private Camera m_camera;
    Vector3 m_offsetFromPlayer;
    Vector3 m_originPosition;
    void Start()
    {
        m_camera = Camera.main;
        m_originPosition = m_camera.transform.position;
        m_offsetFromPlayer = transform.position - m_camera.transform.position;
    }

}