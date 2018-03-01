using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CreateCubeX(new Vector3(2, 0.5f, 2));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreateCubeX(Vector3 scale)
    {


        var cube = Instantiate(m_CubeXPrefab);
        cube.transform.localPosition = new Vector3(-5.3f, 0.5f + m_CubeAmount * 0.5f, 0);
        cube.transform.localScale = scale;
        cube.m_Camera = FindObjectOfType<CameraManager>();
        cube.m_Water = FindObjectOfType<WaterManager>();
        Debug.Log(scale);

        ++m_CubeAmount;

    }

    public void EndGame()
    {
        m_NewGameManager.gameObject.SetActive(true);
    }

    public DynamicCubeX m_CubeXPrefab;
    public NewGameManager m_NewGameManager;

    private int m_CubeAmount = 0;
}
