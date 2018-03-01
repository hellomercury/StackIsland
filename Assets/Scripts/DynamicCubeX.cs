using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCubeX : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        m_Water.Promote();
        m_Camera.Promote();

        if (m_GameOver)
            return;

        if (tag == "Invalid")
            return;

        foreach (var contact in collision.contacts)
        {
            Debug.Log($"({contact.point.x},{contact.point.z})");
            m_X.Add(contact.point.x);
        }

        float x_min = m_X.Min();
        float x_max = m_X.Max();
        Debug.Log($"x_min:{x_min},x_max:{x_max},x:{transform.position.x}");

        // remain:
        var remain_positon = new Vector3((x_max + x_min) / 2, transform.position.y, transform.position.z);
        float remain_scale_value = (x_max - x_min) / transform.localScale.x;
        var remain_scale = new Vector3(transform.localScale.x * remain_scale_value, transform.localScale.y, transform.localScale.z);


        if (!Mathf.Approximately(remain_scale_value, 0))
        {
            var remain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            remain.transform.localPosition = remain_positon;
            remain.transform.localScale = remain_scale;
            remain.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;

            var scroe_manager = FindObjectOfType<ScoreManager>();
            scroe_manager.SetScore((int)(remain_scale_value * 10));

        }


        // to_fall
        var bottom_transform = collision.collider.transform;
        float bottom_x_max = bottom_transform.position.x + bottom_transform.localScale.x / 2;
        float bottom_x_min = bottom_transform.position.x - bottom_transform.localScale.x / 2;
        Debug.Log($"bottom_x_min:{bottom_x_min},bottom_x_max:{bottom_x_max}");


        var to_fall_scale = new Vector3(transform.localScale.x * (1 - remain_scale_value), transform.localScale.y, transform.localScale.z);
        var to_fall_position = Vector3.zero;

        if (Mathf.Approximately(bottom_x_min, x_min))
        {
            to_fall_position = new Vector3(remain_positon.x - transform.localScale.x / 2, transform.position.y, transform.position.z);
        }
        else if (Mathf.Approximately(bottom_x_max, x_max))
        {
            to_fall_position = new Vector3(remain_positon.x + transform.localScale.x / 2, transform.position.y, transform.position.z);
        }
        else
        {
            tag = "Invalid";
            CreateNewCubeX(remain_scale);
            return;
        }

        if (!Mathf.Approximately(1 - remain_scale_value, 0))
        {
            GameObject.FindGameObjectWithTag("Fracture").GetComponent<AudioSource>().Play();
            var to_fall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            to_fall.transform.localPosition = to_fall_position;
            to_fall.transform.localScale = to_fall_scale;
            to_fall.AddComponent<Rigidbody>();
            to_fall.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
            to_fall.GetComponent<Rigidbody>().drag = 3.5f;
        }

        CreateNewCubeX(remain_scale);
        Destroy(gameObject);

        Debug.Log(collision.collider.transform.position.y);
        Debug.Log(m_Water.transform.position.y);


        return;
    }

    private void Start()
    {

    }

    private void CreateNewCubeX(Vector3 scale)
    {
        if (Mathf.Approximately(scale.x, 0))
        {
            //FindObjectOfType<GameManager>().CreateCubeX(new Vector3(0.2f, scale.y, scale.z));
            m_GameOver = true;
            FindObjectOfType<GameManager>().EndGame();
            Debug.Log("game over");

        }
        else
        {
            FindObjectOfType<GameManager>().CreateCubeX(scale);
        }
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < m_Water.transform.position.y)
        {
            gameObject.GetComponent<DynamicCubeX>().enabled = false;
            Debug.Log("game over");
            m_Rigidbody.drag = 3.5f;
            m_GameOver = true;
            FindObjectOfType<GameManager>().EndGame();
        }

        if (m_GameOver)
            return;

        if (Input.GetMouseButton(0))
        {
            m_Throw = true;
            Throw();
        }

        if (!m_Throw)
        {
            m_Radian += m_PerRadian;

            Debug.Log(m_MoveToPositiveX);
            m_MoveToPositiveX = (Mathf.Sin(m_Radian) > 0);

            float dx = Mathf.Sin(m_Radian) * m_Radius;
            transform.position += new Vector3(dx * Time.deltaTime, 0, 0);
        }


    }

    private void Throw()
    {
        m_Rigidbody.useGravity = true;
    }
    public WaterManager m_Water;
    public CameraManager m_Camera;

    private List<float> m_X = new List<float>();

    //
    private float m_Radian = 0;
    private float m_PerRadian = 0.03f;
    private float m_Radius = 8f;

    private bool m_MoveToPositiveX = true;
    private bool m_Throw = false;

    //
    private Rigidbody m_Rigidbody;
    private bool m_GameOver = false;
}
