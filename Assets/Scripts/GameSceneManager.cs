using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using JetBrains.Annotations;
using Unity.Cinemachine;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    private PhotonView _pv;

    private GameObject player;
    void Start()
    {
        _pv = gameObject.GetComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
            return;
        }
        else
        {
            InitGame();
        }
    }

    public void InitGame()
    {

        float[] optionsX = { 259.4208f, };// -307.7196f, -396.1689f, -70.1813f, 119.6863f, 398.3039f };
        float[] optionsY = { 11.34f, };// 0.45f, 2f, -0.16f, 2.51f, 0.154f };
        float[] optionsZ = { 295.2019f, };// 173.27f, -136.9375f, -348.2499f, 70.19689f, -267.3781f };
        

        //float[] optionsX = { 12f };
        //float[] optionsY = { 1f };
        //float[] optionsZ = { -1.751669f };

        int randomIndex = Random.Range(0, optionsX.Length);
        float spawnPointX = optionsX[randomIndex];
        float spawnPointY = optionsY[randomIndex];
        float spawnPointZ = optionsZ[randomIndex];
        player = PhotonNetwork.Instantiate("Chicken_001", new Vector3(spawnPointX, spawnPointY, spawnPointZ), Quaternion.identity);
        print("Player spawned at: " + spawnPointX + ", " + spawnPointY + ", " + spawnPointZ);
        //Camera.main.GetComponent<CinemachineCamera>().Follow = player.transform;
        cinemachineCamera.Follow = player.transform;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
