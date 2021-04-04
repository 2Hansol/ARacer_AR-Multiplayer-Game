using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSetup1 : MonoBehaviourPun
{
    //public Text playerNameText;
    public TextMeshProUGUI playerNameText;
    //TextMeshProUGUI 
    private bool goal, IsOver;

    //private Canvas ui_2d;
    // Start is called before the first frame update
    void FixedUpdate()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (SpawnManagerRacing.gameStart && !SpawnManagerRacing.gameOver) // 게임 시작
            {
                if (photonView.IsMine)
                {
                    //The player is local player. 
                    transform.GetComponent<CarMovement1>().enabled = true;
                    transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(true);
                    transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(true);
                }
                else
                {
                    //The player is remote player
                    transform.GetComponent<CarMovement1>().enabled = false;
                    transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(false);
                    transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(false);
                }
            }

            if (SpawnManagerRacing.gameOver) //GameOver인 경우
            {
                transform.GetComponent<CarMovement1>().inputValue1 = 0;
                //transform.GetComponent<CarMovement>().enabled = false;
                transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(false);
                transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(false);
            }


        }
        else
        {

            if (!SpawnManagerRacing.gameOver && SpawnManagerRacing.gameStart) //게임 시작
            {
                if (photonView.IsMine)
                {
                    //The player is local player. 
                    transform.GetComponent<CarMovement1>().enabled = true;
                    transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(true);
                    transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(true);
                }
                else
                {
                    //The player is remote player

                    transform.GetComponent<CarMovement1>().inputValue1 = 0;
                    //transform.GetComponent<CarMovement>().enabled = false;
                    transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(false);
                    transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(false);
                }
            }
            if (SpawnManagerRacing.gameOver)
            {
                transform.GetComponent<CarMovement1>().inputValue1 = 0;
                //transform.GetComponent<CarMovement>().enabled = false;
                transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(false);
                transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        IsOver = false;

        if (!SpawnManagerRacing.gameStart)
        {
            transform.GetComponent<CarMovement1>().enabled = false;
            transform.GetComponent<CarMovement1>().joystick1.gameObject.SetActive(false);
            transform.GetComponent<CarMovement1>().joystick2.gameObject.SetActive(false);
        }
        goal = false;
        SetPlayerName();
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "FinishLine" && photonView.IsMine && !SpawnManagerRacing.gameOver && !IsOver)
        {
            SpawnManagerRacing.gameOver = true;
            IsOver = true;
        }

    }


    // 플레이어 이름 위에 뜨도록 하기 //포톤 뷰에서 뜬다 
    void SetPlayerName()
    {

        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";

            }
            else
            {
                playerNameText.text = "";//$"{ AuthManager.User.Email}";
            }

        }
    }
}