using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

//MonoBehaviourPunCallbacks photon callback
//어떤사건에 훅을 걸어서 사용을할떄 MonoBehaviourPunCallbacks 상속
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public Text connectionInfoText;
    public Button joinButton;
    
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        //마스터서버 접속전까지 클릭 X
        joinButton.interactable = false;

        connectionInfoText.text = "Connection To Master Server...";
    }
    
    public override void OnConnectedToMaster()
    {
        //마스터서버 접속 성공
        joinButton.interactable = true;
        connectionInfoText.text = "Success Connection";
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        //접속 종료
        joinButton.interactable = false;
        connectionInfoText.text = "False Connection : " + cause.ToString();

        //재접속시도
        PhotonNetwork.ConnectUsingSettings();
    }
    
    //수동으로 직접 버튼을 눌러 실행할 메서드
    public void Connect()
    {
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Random Room.......";
            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            connectionInfoText.text = "False Connection ";

            //재접속시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //랜덤룸 입장 실패시
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "No Empty Room, Create Room";

        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, ro);
    }
    
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room";

        //SceneManager.LoadScene() 자기만 씬이 넘어가게됨, 독자적으로 돌아감, 동기화 X

        //호스트가 포톤을 통해 다른 플레이어들도 씬을 로딩 + 동기화
        PhotonNetwork.LoadLevel("Main");    

    }
}