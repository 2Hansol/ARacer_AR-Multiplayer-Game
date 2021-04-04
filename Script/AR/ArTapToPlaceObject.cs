using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
//RequireComponent 속성은 요구되는 의존 컴포넌트를 자동으로 추가해줍니다.
//요구되는 컴포너트가 자동으로 해당 게임오브젝트에 추가됩니다. 
//[RequireComponent(typeof(ARRaycastManager))]-->게임오브젝트에 자동으로 ARRaycastManager가 추가 되는 것.

[RequireComponent(typeof(ARRaycastManager))]
public class ArTapToPlaceObject : MonoBehaviour
{
    public GameObject spawnedObject1;
    public Camera aRCamera;
    ARSessionOrigin m_ARSessionOrigin;
    public Slider scaleSlider;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition; //어디서 터치하고 어디에 위치시킬지
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject TextPanel;
    public GameObject checkBtn;
    public GameObject slider;

    private bool flag;
    public static bool checkBtnFlag;


    [Header("UI")]
    public GameObject ui_lobby;
    public GameObject ui_Game;
    public GameObject ui_AR;


    private void Awake()
    {
        ui_lobby.SetActive(false);
        ui_Game.SetActive(false);
        ui_Game.SetActive(true);
        flag = false;
        checkBtnFlag = true;
        checkBtn.SetActive(false);
        slider.SetActive(false);
        _arRaycastManager = GetComponent<ARRaycastManager>();
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();

    }

    void Update()
    {
        _InstantiateOnTouch();
    }
    
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void LobbySetUp()
    {
        ui_lobby.SetActive(true);
        ui_AR.SetActive(false);
    }

    public void _InstantiateOnTouch()
    {
        Touch touch;
        touch = Input.GetTouch(0);

        if (checkBtnFlag == true)
        {
            _SpawnARObject();
            //_PinchtoZoom();
            _Rotate();
        }
    }

    public void _SpawnARObject()
    {
        Touch touch;
        touch = Input.GetTouch(0);
        //AR raycast쏘기

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);
        if (_arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            if (CheckButton.checkBtnFlag)
            {
                hitPose = hits[0].pose;
                Vector3 positionToBePlaced = hitPose.position;
                spawnedObject1.transform.position = positionToBePlaced;
            }
            flag = true;
        }
        checkBtn.SetActive(flag);
        slider.SetActive(flag);
    }

    
    public void OnSliderValueChanged(float value)
    {
        if (scaleSlider != null)
        {
            m_ARSessionOrigin.transform.localScale = Vector3.one / value;
     
        }
    }

    public void _Rotate()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        if (Input.touchCount == 2)
        {
            if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                m_ARSessionOrigin.transform.Rotate(Vector3.up * 3f * Time.deltaTime * (touch1.deltaPosition.y - touch0.deltaPosition.y), Space.Self);
            }
        }
    }


    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    /*
    public void _PinchtoZoom()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.deltaPosition - touchZero.position;
            Vector2 touchOnePrevPos = touchOne.deltaPosition - touchOne.position;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            float pinchAmount = deltaMagnitudeDiff * 0.005f * Time.deltaTime;
            spawnedObject1.transform.localScale -= new Vector3(pinchAmount, pinchAmount, pinchAmount);
            spawnedObject1.transform.position -= new Vector3(0, pinchAmount, 0)*4;

            pinchAmountPlayer = pinchAmount;
        }
    }
    */



}
