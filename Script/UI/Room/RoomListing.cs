using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;


    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name + " (" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
    }

    public void OnClick_Button()
    {
        //PhotonNetwork.PlayerList.Length   CountOfPlayersInRooms
        //리스팅 저기 오류나면 여기서 커스텀과 비교 해서 들어가도 됨~~ 랜덤에도 넣어야함!!!
        
        /*
        int num;
        object trackSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARCarRacing.TRACK_SELECTION_NUMBER, out trackSelectionNumber))
            num = (int)trackSelectionNumber;
        else num = 0;
        ExitGames.Client.Photon.Hashtable cp = RoomInfo.CustomProperties;

    */
        if ( RoomInfo.PlayerCount < 2)//num == (int)cp[MultiplayerARCarRacing.ROOM_CUSTOM_PROTERTIES] &&
            PhotonNetwork.JoinRoom(RoomInfo.Name);

    }

}