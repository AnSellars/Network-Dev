using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_InputField inputRoomName;
    [SerializeField]
    public TextMeshProUGUI textRoomList;
    [SerializeField]
    private TMP_InputField inputPlayerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            if (PhotonNetwork.CurrentLobby == null)
            {
                PhotonNetwork.JoinLobby();
            }
        }
    }
    public override void OnConnectedToMaster()
    {
        print("Conntected to Master!");
        PhotonNetwork.JoinLobby();
    }
    // Update is called once per frame
    public override void OnJoinedLobby()
    {
        print("Lobby Joined Successfully!");
    }

    public string GetRoomName()
    {
        string roomName = inputRoomName.text;
        return roomName.Trim();
    }

    public string GetPlayerName()
    {
        string playerName = inputPlayerName.text;
        return playerName.Trim();
    }

    public void OnClickCreateRoom()
    {
        string playerName = GetPlayerName();
        if (string .IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player Name is invalid.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;

        string roomName = GetRoomName();
        if (roomName.Length > 0)
        {
            PhotonNetwork.CreateRoom(roomName);
        }
        else
        {
            print("You entered invalid RoomName!");
        }

    }

    public void OnClickJoinRoom()
    {
        string playerName = GetPlayerName();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player Name is invalid.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;


        string roomName = GetRoomName();
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            print("Invalid Room Name!");
        }

    }

    public override void OnJoinedRoom()
    {
        print("Room Joined!");
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StringBuilder sb = new StringBuilder();
        foreach (RoomInfo roomInfo in roomList)
        {
            if(roomInfo.PlayerCount > 0)
            {

                sb.AppendLine($"RoomName: {roomInfo.Name} Player Count: {roomInfo.PlayerCount}");
            }
            textRoomList.text = sb.ToString();
        }
    }
}
