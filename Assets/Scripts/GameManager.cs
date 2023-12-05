using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPun{
    public string playerPrefabPath;
    public List<Transform> spawnPoints;
    public PlayerController[] players;
    public static GameManager instance;
    private int playersInGame;

    void Awake(){
        instance = this;
    }

    void Start(){
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ImInGame(){
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length){
            SpawnPlayer();
        }
    }

    void SpawnPlayer(){
        int spawnedAt = Random.Range(0,spawnPoints.Count);
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[spawnedAt].position, Quaternion.identity);
        spawnPoints.Remove(spawnPoints[spawnedAt]);
        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
