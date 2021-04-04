using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class FlagGame : MonoBehaviourPun
{
    public GameObject[] flagPrefabs;
    private Vector3 instantiatePosition;
    private GameObject Track;
    [Header("Flag Psosition")]
    [SerializeField] private Transform[] BluePos;//0하고  2번만 있으면 됨
    [SerializeField] private Transform[] RedPos;


    public enum RaiseEventCodes
    {
        FlagSpawnEventCode = 1

    }
    public void MakeFlag()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                SpawnFlags(0, i);
                SpawnFlags(1, i);
            }
        }
    }
    void Awake()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        Track = GameObject.Find("CityTrack");


    }
    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCodes.FlagSpawnEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];
            int color = (int)data[3];
            // 처음 시작할때 플레이어들의 위치 초기화 하는 메소드 
            GameObject flag = Instantiate(flagPrefabs[color], receivedPosition + Track.transform.position, receivedRotation);
            PhotonView _photonView = flag.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];
        }
    }

    void SpawnFlags(int color, int range)
    {
        float randomIntX;
        float randomIntZ;
        if (range == 0)//blue진영
        {
            randomIntX = Random.Range(BluePos[0].position.x, BluePos[1].position.x);
            randomIntZ = Random.Range(BluePos[1].position.z, BluePos[0].position.z);
        }
        else // red 진영
        {
            randomIntX = Random.Range(RedPos[0].position.x, RedPos[1].position.x);
            randomIntZ = Random.Range(RedPos[1].position.x, RedPos[0].position.x);
        }


        instantiatePosition = new Vector3(randomIntX, BluePos[0].position.y, randomIntZ);
        GameObject flag;
        if (color == 0)
        {
            flag = Instantiate(flagPrefabs[0], instantiatePosition, Quaternion.identity);
        }
        else
        {
            flag = Instantiate(flagPrefabs[1], instantiatePosition, Quaternion.identity);
        }
        PhotonView _photonView = flag.GetComponent<PhotonView>();



        if (PhotonNetwork.AllocateViewID(_photonView)) //중요
        {
            object[] data = new object[]
            {
                    flag.transform.position - Track.transform.position, flag.transform.rotation, _photonView.ViewID, color
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache

            };
            SendOptions sendOptions = new SendOptions
            {
                Reliability = true
            };

            //Raise Events!
            PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.FlagSpawnEventCode, data, raiseEventOptions, sendOptions);


        }



    }
    /*
    void SpawnFlags(int color, int range)
    {
        float randomIntX;
        float randomIntZ;
        if (range == 0)
        {
            randomIntX = Random.Range(0, 4);
            randomIntZ = Random.Range(0, 4);
        }
        else if (range == 1)
        {
            randomIntX = Random.Range(0, 4);
            randomIntZ = Random.Range(-4, 0);
        }
        else if (range == 2)
        {
            randomIntX = Random.Range(-4, 0);
            randomIntZ = Random.Range(0, 4);
        }
        else
        {
            randomIntX = Random.Range(-4, 0);
            randomIntZ = Random.Range(-4, 0);
        }
        instantiatePosition = new Vector3(planePos.position.x + randomIntX, planePos.position.y, planePos.position.z + randomIntZ);


        if (color == 0)
        {
            Instantiate(flags[0], instantiatePosition, Quaternion.identity);
        }
        else
        {
            Instantiate(flags[1], instantiatePosition, Quaternion.identity);
        }
    }
    */

}