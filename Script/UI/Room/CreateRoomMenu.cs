using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;

public class CreateRoomMenu : MonoBehaviour
{
    [SerializeField]
    private Text _roomName;
    private Text RoomName { get { return _roomName; } }
    public int mapSelectionNumber;


    public void OnClick_CreateRoom()
    {
        int num = 0;
        object trackSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARCarRacing.TRACK_SELECTION_NUMBER, out trackSelectionNumber))
            num = (int)trackSelectionNumber+1;
        if (!PhotonNetwork.IsConnected) return;


        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { MultiplayerARCarRacing.ROOM_CUSTOM_PROTERTIES, num } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { MultiplayerARCarRacing.ROOM_CUSTOM_PROTERTIES };

        if (PhotonNetwork.CreateRoom("Track" + num.ToString() + " " + RoomName.text, roomOptions, TypedLobby.Default))
            print("create room successfully sent.");
        else
            print("create room failed to sent");
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed : " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        print("Room created successfully!");

    }
}