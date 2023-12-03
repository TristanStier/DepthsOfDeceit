using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime; 

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public void CreateRoom()
    {
        // Check if the room name is not empty
        if (!string.IsNullOrEmpty(createInput.text))
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 }; // You can customize this based on your requirements
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            Debug.LogError("Room name is empty!");
        }
    }

    public void JoinRoom()
    {
        // Check if the room name is not empty
        if (!string.IsNullOrEmpty(joinInput.text))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            Debug.LogError("Room name is empty!");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("box");
    }
}