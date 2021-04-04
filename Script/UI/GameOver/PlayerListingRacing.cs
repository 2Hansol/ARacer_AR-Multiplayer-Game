using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingRacing : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text TimeText;

    private Photon.Realtime.Player _player;

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        _player = player;
        //string name = AuthManager.User.Email;
        object playerName;

        if (player.CustomProperties.TryGetValue(MultiplayerARCarRacing.PLAYER_NAME, out playerName))
        {
            string[] name = ((string)playerName).Split('@');
            nameText.text = name[0];
        }
        else
        {
            nameText.text = ((SpawnManagerRacing.otherName).Split('@'))[0];

        }

        object playerPlayTime;
        if (player.CustomProperties.TryGetValue(MultiplayerARCarRacing.PLAYER_PLAY_TIME, out playerPlayTime))
        {
            float time = (float)playerPlayTime;
            if (time < 10)
                TimeText.text = "0" + time.ToString("N2");
            else TimeText.text = time.ToString("N2");
        }
        else
        {
            float time = SpawnManagerRacing.otherTime;
            if (time < 10)
                TimeText.text = "0" + time.ToString("N2");
            else TimeText.text = time.ToString("N2");

        }
    }
}