using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[RequireComponent(typeof(PhotonView))]
public class SpawnManagerRacing : MonoBehaviourPunCallbacks
{
    //여긴 경기장 오브젝트 다 받아놓고 나중에 스위치문으로 아닌거 끄고 
    //스판 포지션도 다 경기장 별로 받아오기
    //게임 캔버스 안에 게임시작 버튼, 움직임 버튼 캔버스는 여기서 관리
    //게임 맵 오브젝트 관리도 여기
    public GameObject[] playerPrefabs;
    public Transform basicPosition;
    public Transform basicPosition1;
    Vector3 spawnPositions;

    [SerializeField]
    public static bool gameStart;
    [SerializeField]
    public static bool gameOver;

    public static bool otherGameOver, overTemp;
    public static string otherName;



    public GameObject battleArenaGameobject; //중요
    public static int fieldNum; //이거필드선택하고 값해줘야해!!!

    //public bool spawnBool;
    //public static bool gameStart, gameOver,otherGameOver,overTemp;

    public static float time;
    public static float otherTime;

    [Header("UI")]
    public GameObject StartBtnCavas;
    public GameObject CountDownCavas;
    public GameObject GameCavas;
    public GameObject GameOverCanvas;
    public Text CountDownText;
    public Text TimeText;
    public Button StartButton;
    public GameObject PlayerListingMenu;
    
    [Header("DesertTrack")]
    private Vector3 instantiatePosition;


    public static bool MusicFlagOcean;
    public static bool MusicFlagCity;


    public static int[] playerScores = new int[2];
    public enum RaiseEventCodes
    {
        PlayerSpawnEventCode = 0, FlagSpawnEventCode = 1

    }

    void Awake()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        playerScores[0] = 0;
        playerScores[1] = 0;
        time = 0f;
        otherTime = 0f;
        gameStart = false;
        gameOver = false;
        otherGameOver = false;
        overTemp = false;
        GameCavas.SetActive(false);
        //GameOverCanvas.SetActive(false);


        MusicFlagOcean = false;
        MusicFlagCity = false;


        //Get Selected Track Num
        object trackSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARCarRacing.TRACK_SELECTION_NUMBER, out trackSelectionNumber))
        {
            fieldNum = (int)trackSelectionNumber;
        }

        ExitGames.Client.Photon.Hashtable playerNameHash = new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.PLAYER_NAME, AuthManager.User.Email } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerNameHash);
        //PlayerListingMenu.GetComponent<PlayerListingMenu>().GetCurrentRoomPlayers();
    }

    private void FixedUpdate()
    {
        if (gameStart && !gameOver)
        {
            time += Time.deltaTime;

            string tmp = time.ToString("N2");
            string[] times = tmp.Split('.');
            if (time < 10)
                TimeText.text = "0" + time.ToString("N2");// times[0] + " : " + times[1];
            else TimeText.text = time.ToString("N2"); //times[0] + " : " + times[1];

        }


        if (gameOver && !overTemp && otherGameOver)
        {
            GameCavas.SetActive(false);
            GameOverCanvas.SetActive(true);
            GameOver();
            overTemp = true;
        }
    }

    public void GameOver()
    {
        ExitGames.Client.Photon.Hashtable playerTimeHash = new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.PLAYER_PLAY_TIME, time } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerTimeHash);
        PlayerListingMenu.GetComponent<PlayerListingMenuRacing>().GetCurrentRoomPlayers();
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    #region Photon Callback Methods
    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];
            int receivedPlayerSelectionData = (int)data[3];
            // 처음 시작할때 플레이어들의 위치 초기화 하는 메소드 
            GameObject player = Instantiate(playerPrefabs[receivedPlayerSelectionData], receivedPosition + battleArenaGameobject.transform.position, receivedRotation);
            PhotonView _photonView = player.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];
        }

    }


    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }
    #endregion


    #region Private Methods

    public void SpawnPlayer()
    {
        
        MusicFlagOcean = true;
        MusicFlagCity = true;

        object playerSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARCarRacing.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
        {
            Debug.Log("Player selection number is " + (int)playerSelectionNumber);

            battleArenaGameobject = GameObject.Find("CityTrack");
            basicPosition = GameObject.Find("Position0").transform;
            basicPosition1 = GameObject.Find("Position1").transform;

            if (PhotonNetwork.IsMasterClient)
            {
                spawnPositions = new Vector3(basicPosition.transform.position.x, basicPosition.transform.position.y, basicPosition.transform.position.z);
            }
            else
            {
                spawnPositions = new Vector3(basicPosition1.transform.position.x, basicPosition1.transform.position.y, basicPosition1.transform.position.z);
            }

            Vector3 instantiatePosition = spawnPositions;
            GameObject playerGameobject = Instantiate(playerPrefabs[(int)playerSelectionNumber], instantiatePosition, Quaternion.identity);
            PhotonView _photonView = playerGameobject.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(_photonView)) //중요
            {
                object[] data = new object[]
                {
                    playerGameobject.transform.position - battleArenaGameobject.transform.position, playerGameobject.transform.rotation, _photonView.ViewID, playerSelectionNumber
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
                PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode, data, raiseEventOptions, sendOptions);
            }
            else
            {

                Debug.Log("Failed to allocate a viewID");
                Destroy(playerGameobject);
            }
        }
    }



    public void GameStartButton()
    {

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2)
        {
            photonView.RPC("RPCCountDown", RpcTarget.All);
            StartButton.enabled = false;
        }

    }
    public void AddScore(int playerNumber, int score)
    {
        //방장 측에서만 실행해되어야 함. 변화하는 점수 받기만 함
        /*
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        */
        playerScores[playerNumber - 1] += score;
        //마스터 측에서 모든 클라이언트 방향으로 전파하기 
        photonView.RPC("RPCUpdateScoreTEXT", RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());


    }



    [PunRPC]
    public void RPCCountDown()
    {
        StartCoroutine(CountDown());

    }

    IEnumerator CountDown()
    {
        for (int i = 5; i >= 0; i--)
        {
            CountDownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        StartBtnCavas.SetActive(false);
        CountDownCavas.SetActive(false);
        GameCavas.SetActive(true);
        gameStart = true;

    }
    #endregion
}