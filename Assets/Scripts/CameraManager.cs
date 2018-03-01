using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (m_Promote)
        {
            Debug.Log("test");
            transform.position = Vector3.SmoothDamp(transform.position, m_Target, ref m_Velocity, m_Smoothness);
        }
    }

    public void Promote()
    {
        m_Promote = true;
        m_Target = new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z);

    }

    public float m_Smoothness = 0.3f;

    private bool m_Promote = false;
    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_Target;
}
