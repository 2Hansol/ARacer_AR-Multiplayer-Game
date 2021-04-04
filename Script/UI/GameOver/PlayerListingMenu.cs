﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    private void Awake()
    {
        //GetCurrentRoomPlayers();
    }


    public void GetCurrentRoomPlayers()
    {
        /*
        var leaderboard = from p in PhotonNetwork.CurrentRoom.Players
                          orderby (float) p.CustomProperties[MultiplayerARCarRacing.PLAYER_PLAY_TIME] descending select p;



        foreach (var player in leaderboard)
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player.Value);
                _listings.Add(listing);
            }

            //AddPlayerListing(playerInfo.Value);
        }
        */


        foreach (KeyValuePair<int, Photon.Realtime.Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {

            AddPlayerListing(playerInfo.Value);
        }

    }

    private void AddPlayerListing(Photon.Realtime.Player player)
    {
        PlayerListing listing = Instantiate(_playerListing, _content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            _listings.Add(listing);
        }
    }

}