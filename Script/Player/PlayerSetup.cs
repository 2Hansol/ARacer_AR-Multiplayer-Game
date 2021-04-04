using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{

    public TextMeshProUGUI playerNameText;
    private int itemNumber;
    private bool goal;
    //private Canvas ui_2d;
    // Start is called before the first frame update
    void Update()
    {
        if (itemNumber == 8 && goal)
        {
            Debug.Log("win");
        }
    }
    void Start()
    {
        goal = false;
        //setPlayer();
        SetPlayerName();

    }

    void OnTriggerEnter(Collider col)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (col.gameObject.tag == "RedFlag")
            {
                itemNumber++;
                col.gameObject.SetActive(false);
            }
            else if (col.gameObject.tag == "BlueFlag")
            {
                itemNumber = 0;
                GameObject[] Red = GameObject.FindGameObjectsWithTag("RedFlag");
                foreach (GameObject red in Red)
                {
                    red.SetActive(true);
                }
               
                //GameObject.FindGameObjectWithTag("RedFlag").SetActive(true);
            }
            if (col.gameObject.tag == "RedGoal")
            {
                goal = true;
            }
        }
        else
        {
            if (col.gameObject.tag == "BlueFlag")
            {
                itemNumber++;
                col.gameObject.SetActive(false);
            }
            else if (col.gameObject.tag == "RedFlag")
            {
                itemNumber = 0;
                GameObject[] Blue = GameObject.FindGameObjectsWithTag("BlueFlag");
                for (int i = 0; i < Blue.Length; i++)
                    Blue[i].SetActive(true);

            }
            if (col.gameObject.tag == "BlueGoal")
            {
                goal = true;
            }
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
                if(PhotonNetwork.IsMasterClient)
                {
                    playerNameText.color = Color.red;
                }
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
                if (!PhotonNetwork.IsMasterClient)
                {
                    playerNameText.color = Color.red;
                }
            }

        }

    }
    /*
    void setPlayer()
    {
        if (photonView.IsMine)
        {
            //The player is local player. 
            //GetComponent<CarMovement>.enabled. = true;
            //gamGetComponent<CarMovement>().enabled = true;
            //GameObject.Find("UI_2D").SetActive(true);
            //transform.GetComponent<CarMovement>().BackButton_Up() 
            //GameObject.Find("Forward").SetActive(true);
            //GameObject.Find("Back").SetActive(true);
            //GameObject.Find("Left").SetActive(true);
            //GameObject.Find("Right").SetActive(true);
            //transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
            //transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
        }
        /*
        else
        {
            //The player is remote player
            //GetComponent<CarMovement>().enabled = false;
            //GameObject.Find("UI_2D").SetActive(false);
            //GameObject.Find("Forward").SetActive(false);
            //GameObject.Find("Back").SetActive(false);
            //GameObject.Find("Left").SetActive(false);
            //GameObject.Find("Right").SetActive(false);
            //GameObject.Find("UI_2D").SetActive(false);
            //transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }
    }
    */
}
