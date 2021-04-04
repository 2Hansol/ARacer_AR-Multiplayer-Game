using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//namespace
//pun 감지할 수 있는 PunCallbacks
public class LobbyManager : MonoBehaviourPunCallbacks
{
    //전체 큰 분류 로비냐 게임 캔버스냐는 여기서 관리 

    //gameversion이 다르면 같은 게임이라도 매칭이 안됨.
    private readonly string gameVersion = "0.0.1";

    [Header("UI")]
    public Text connectionInfoText;
    public Button joinButton;
    public GameObject ui_Lobby;
    public GameObject ui_Game;
    public GameObject ui_GameOverCanvas;

    private void Start()
    {
        //접속 시도 
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.ConnectUsingSettings();//여러가지 부가적인 게임 설정 할 수 있다. 
        ui_Game.SetActive(false);
        ui_GameOverCanvas.SetActive(false);

        joinButton.interactable = false;
        connectionInfoText.text = "connecting to master server...";
    }

    //접속 성공하면 자동 실행됨.
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server\n" + PhotonNetwork.LocalPlayer.NickName;
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    //접속 시도했는데 실패 했다거나 도중에 끊어진 경우 
    //입력으로 끊긴 이유가 들어옴. 
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - try reconnecting";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinedLobby()
    {
        connectionInfoText.text = "WELCOME! A-Racer";
        //base.OnJoinedLobby();
    }


    //이 아래 코드는 유투브에선 분리 시킴
    //포톤 아님 조인 눌렀을 때 수동으로 
    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            //자동으로 접속.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = $"Offline : Connection Disabled - try reconnecting";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //빈방 없으면 방 들어가기 실패 그 때 자동으로 실행됨. 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, creating new room";
        //네트워크 상에서 방 만들고 자동으로 참가하게 됨. 

        //일단 랜덤에서 방 없으면 안만들게
        //PhotonNetwork.CreateRoom(roomName: null, new RoomOptions { MaxPlayers = 2 });
    }

    //빈방으로 접속 성공 했을 때  자동으로 실행됨. 
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room";
        ui_Game.SetActive(true);
        ui_GameOverCanvas.SetActive(false);
        ui_Lobby.SetActive(false);
        //씬 메니저로 이동하면 안됨. SceneManager.Load->동기화가 안되게 됨. 
        //다 같이 넘어갈 수 있게끔 만들어야함.
        //호스트가 넘어가면 참가자도 넘어가게 됨. 동기화가 자동으로 됨. 
        //PhotonNetwork.LoadLevel("Race");
    }


    public void OnQuitMatchButtonClicked()
    {
        if (PhotonNetwork.InRoom)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

           // GameObject[] track = GameObject.FindGameObjectsWithTag("Track");
            for (int i = 0; i < player.Length; i++)
                Destroy(player[i]);
            
            //for (int i = 0; i < track.Length; i++) Destroy(track[i]);
            PhotonNetwork.LeaveRoom();

            ui_Game.SetActive(false);
            ui_GameOverCanvas.SetActive(false);
            ui_Lobby.SetActive(true);
        }
        else
        {
            //SceneLoader.Instance.LoadScene("Scene_Lobby");
            //SceneManager.LoadScene("LobbyTest");
        }
    }
}