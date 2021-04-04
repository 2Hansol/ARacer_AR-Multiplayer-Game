using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// This example demonstrates how to toggle plane detection,
/// and also hide or show the existing planes.
/// </summary>
[RequireComponent(typeof(ARPlaneManager))]
public class CheckButton : MonoBehaviour
{
    public Text infoText;
    public GameObject checkBtnPanel;
    public static bool checkBtnFlag;
    public static bool TestTest;
    public static bool TestTest1;

    private int count;

    void Start()
    {
        count = 0;
        checkBtnFlag = true;
        TestTest = false;
        infoText.text = "바닥을 인식해주세요.";
    }

    private void Update()
    {
        if (m_ARPlaneManager.trackables.count > 0)
        {
            infoText.text = "놓고싶은 위치를 터치하세요.";
        }
    }

    public void TogglePlaneDetection()
    {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
        checkBtnFlag = false;
        // string planeDetectionMessage = "";
        if (m_ARPlaneManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else//체크 버튼 눌러서 해당 위치에서 게임 시작
        {
            SetAllPlanesActive(false);
        }
        //ArTapToPlaceObject.spawnedObject1.SetActive(false);
        //ArTapToPlaceObject.spawnedObject2.SetActive(true);

        TestTest = true;
        ArTapToPlaceObject arTapToPlaceObject = GetComponent<ArTapToPlaceObject>();
        arTapToPlaceObject.LobbySetUp();
        //GameObject.Find("AR Session Origin").GetComponent<ArTapToPlaceObject>().LobbySetUp();
    }

    void SetAllPlanesActive(bool value)
    {
        ArTapToPlaceObject.checkBtnFlag = value;
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);
        if (!value)
        {
            checkBtnPanel.SetActive(false);
        }
    }

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    ARPlaneManager m_ARPlaneManager;
}