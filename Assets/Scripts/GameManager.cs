using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//photon 이벤트 감지 가능
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public Text scoreText;
    public Transform[] spawnPositions;
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    private int[] playerScores;

    private void Start()
    {
        playerScores = new[] { 0, 0 };
        SpawnPlayer();

        if(PhotonNetwork.IsMasterClient)    //방장만 볼을 생성
        {
            SpawnBall();
        }
    }

    private void SpawnPlayer()
    {
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        //GameObject.Instantiate() 사용하면 동기화 X
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation); ;
    }

    private void SpawnBall()
    {
        PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity);

    }

    public override void OnLeftRoom()
    {
        //나 자신이 떠날떄만 실행, 포톤 메서드
        SceneManager.LoadScene("Lobby");

        //PhotonNetwork.LeaveRoom();
    }

    public void AddScore(int playerNumber, int score)
    {
        //중요한 룰이므루 방장측에서만 실행
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        playerScores[playerNumber - 1] += score;
        photonView.RPC("RPCUpdateScoreText", RpcTarget.All, 
            playerScores[0].ToString(),
            playerScores[1].ToString()
            );
    }

    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        scoreText.text = player1ScoreText + " : " + player2ScoreText;
    }
}