using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro; //TextMashPro

public class PlayerSelectionManager : MonoBehaviour
{

    public Transform playerSwitcherTransform;
    public GameObject[] Cars;


    // start method
    public int playerSelectionNumber;

    [Header("UI")]
    //public TextMeshProUGUI playerModelType_Text;
    public Button next_Button;
    public Button previous_Button;

    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;
    public GameObject TrackSelection;


    #region UNITY Methods

    // Start is called before the first frame update
    void Start()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
        TrackSelection.SetActive(false);
        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        playerSelectionNumber += 1; // plater 수 증가시키기 

        if (playerSelectionNumber >= Cars.Length)
        {
            playerSelectionNumber = 0;
        }
        Debug.Log(playerSelectionNumber);


        next_Button.enabled = false; // 버그 없애기 위해서 enable/able 필요하다 
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

    }

    public void PreviousPlayer()
    {

        playerSelectionNumber -= 1; // player 숫자 감소 
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = Cars.Length - 1;
        }
        Debug.Log(playerSelectionNumber);

        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));
    }



    public void OnSelectButtonClicked()
    {

        uI_Selection.SetActive(false);
        uI_AfterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };
        //Hashtable
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReSelectButtonClicked()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        //SceneLoader.Instance.LoadScene("LobbyTest"); 로딩씬을 위해
        //SceneManager.LoadScene("LobbyTest");
        uI_AfterSelection.SetActive(false);
        TrackSelection.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        //SceneLoader.Instance.LoadScene("SignIn"); 로딩씬을 위해
        SceneManager.LoadScene("SignIn");
    }
    public void Track0()
    {
        //SceneLoader.Instance.LoadScene("LobbyTest"); 로딩씬을 위해
        ExitGames.Client.Photon.Hashtable trackSelectionProp =
            new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.TRACK_SELECTION_NUMBER, 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(trackSelectionProp);
        SceneManager.LoadScene("DesertTrack");
    }

    public void Track1()
    {
        //SceneLoader.Instance.LoadScene("LobbyTest"); 로딩씬을 위해
        ExitGames.Client.Photon.Hashtable trackSelectionProp =
    new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.TRACK_SELECTION_NUMBER, 1 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(trackSelectionProp);
        SceneManager.LoadScene("CityTrack");
    }

    public void Track2()
    {
        ExitGames.Client.Photon.Hashtable trackSelectionProp =
    new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.TRACK_SELECTION_NUMBER, 2 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(trackSelectionProp);
        SceneManager.LoadScene("OceanTrack");
    }

    #endregion


    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {

        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        next_Button.enabled = true;
        previous_Button.enabled = true;


    }

    #endregion


}