using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using System.Linq;
using System.Runtime.CompilerServices;

public class RoomSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public TextMeshProUGUI textCurrentRoom;
    [SerializeField]
    private TextMeshProUGUI textPlayerList;
    [SerializeField]
    private Button buttonStartGame;
    [SerializeField]
    private Image panelToChangeColor;
    [SerializeField]
    private Transform pfLeaveLog;
    [SerializeField]
    private Vector3 logLocation;
    private void Start()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
            return;
        }
        textCurrentRoom.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }
    //public string GetRoomName()
    //{
    //    string roomName = PhotonNetwork.CurrentRoom.ToString();
    //    return roomName.Trim();
    //}
    private void UpdatePlayerList()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Players:");

        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {

            sb.AppendLine("- " + player.NickName);

        }
        textPlayerList.text = sb.ToString();

        if (buttonStartGame != null)
        {
            buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Transform pfLeaveLogTransform = Instantiate(pfLeaveLog, logLocation, Quaternion.identity);
        RoomSceneManager leaveLog = pfLeaveLogTransform.GetComponent<RoomSceneManager>();

        TextMeshPro textMesh;
        textMesh = transform.GetComponent<TextMeshPro>();
        textMesh.SetText(otherPlayer.NickName);

        Instantiate(leaveLog, logLocation, Quaternion.identity);
        
        UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdatePlayerList();
    }
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

}
