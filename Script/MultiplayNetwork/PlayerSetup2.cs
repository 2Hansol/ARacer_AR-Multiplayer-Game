using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup2 : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;
    private bool goal, IsOver;


    //private Canvas ui_2d;
    // Start is called before the first frame update
    void FixedUpdate()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (SpawnManager.gameStart && !SpawnManager.gameOver) // 게임 시작
            {
                if (photonView.IsMine)
                {
                    //The player is local player. 
                    transform.GetComponent<CarMovement>().enabled = true;
                    transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(true);
                    transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(true);
                }
                else
                {
                    //The player is remote player
                    transform.GetComponent<CarMovement>().enabled = false;
                    transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
                    transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
                }
            }

            if (SpawnManager.gameOver) //GameOver인 경우
            {
                transform.GetComponent<CarMovement>().inputValue1 = 0;
                //transform.GetComponent<CarMovement>().enabled = false;
                transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
                transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
            }


        }
        else
        {

            if (!SpawnManager.gameOver && SpawnManager.gameStart) //게임 시작
            {
                if (photonView.IsMine)
                {
                    //The player is local player. 
                    transform.GetComponent<CarMovement>().enabled = true;
                    transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(true);
                    transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(true);
                }
                else
                {
                    //The player is remote player

                    transform.GetComponent<CarMovement>().inputValue1 = 0;
                    //transform.GetComponent<CarMovement>().enabled = false;
                    transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
                    transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
                }
            }
            if (SpawnManager.gameOver)
            {
                transform.GetComponent<CarMovement>().inputValue1 = 0;
                //transform.GetComponent<CarMovement>().enabled = false;
                transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
                transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        IsOver = false;

        if (!SpawnManager.gameStart)
        {
            transform.GetComponent<CarMovement>().enabled = false;
            transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
            transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
        }
        goal = false;
        SetPlayerName();

    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "FinishLine" && photonView.IsMine && !SpawnManager.gameOver && !IsOver )
        {
            if (PhotonNetwork.IsMasterClient && SpawnManager.deserTrack_item_num[0] >= 4)
            {
                SpawnManager.gameOver = true;
                IsOver = true;
            }
            else if(!PhotonNetwork.IsMasterClient && SpawnManager.deserTrack_item_num[1] >= 4)
            {
                SpawnManager.gameOver = true;
                IsOver = true;
            }
        
        }

        if(PhotonNetwork.IsMasterClient)
        {
            if (col.gameObject.tag == "RedFlag")
            {
               
                col.gameObject.SetActive(false);
     
                    GameObject.Find("SpawnManager").GetComponent<SpawnManager>().AddScore(0,1);
            }
        }
        else
        {
            if (col.gameObject.tag == "RedFlag")
            {
                col.gameObject.SetActive(false);
          
                    GameObject.Find("SpawnManager").GetComponent<SpawnManager>().AddScore(1,1);
            }
        }
        
        /*
        //깃발
        if (PhotonNetwork.IsMasterClient)
        {
            if(photonView.IsMine)
            {
                if (col.gameObject.tag == "RedFlag")
                {
                    SpawnManager.deserTrack_item_num++;
                    col.gameObject.SetActive(false);
                    GameObject.Find("SpawnManager").GetComponent<SpawnManager>().AddScore(0, 1);
                }
                else if (col.gameObject.tag == "BlueFlag")
                {
                    StartCoroutine("Stop");
                }               
            }
            else
            {
                if (col.gameObject.tag == "RedFlag")
                {
                    col.gameObject.SetActive(false);
                }
                else if (col.gameObject.tag == "BlueFlag")
                {
                    StartCoroutine("Stop");
                }
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                if (col.gameObject.tag == "BlueFlag")
                {
                    SpawnManager.deserTrack_item_num++;
                    col.gameObject.SetActive(false);
                    GameObject.Find("SpawnManager").GetComponent<SpawnManager>().AddScore(1, 1);
                }
                else if (col.gameObject.tag == "RedFlag")
                {
                    StartCoroutine("Stop");
                }
            }
            else
            {
                if (col.gameObject.tag == "BlueFlag")
                {
                    col.gameObject.SetActive(false);                   
                }
                else if (col.gameObject.tag == "RedFlag")
                {
                    StartCoroutine("Stop");
                }
            }
        }*/
    }

    IEnumerator Stop()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<CarMovement>().enabled = false;
            transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
            transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
            yield return new WaitForSeconds(5); //WaitForSeconds객체를 생성해서 반환
            transform.GetComponent<CarMovement>().enabled = true;
            transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(true);
            transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(true);
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
                playerNameText.text = "";

            }

        }

    }
}