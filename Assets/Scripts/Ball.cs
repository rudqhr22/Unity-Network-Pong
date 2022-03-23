using Photon.Pun;
using UnityEngine;


//바로 photon에 접근할수 있는 클래스 상속
public class Ball : MonoBehaviourPun
{
    //방장인가 ? 아니면
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    private Vector2 direction = Vector2.right;
    private readonly float speed = 10f;
    private readonly float randomRefectionIntensity = 0.1f;
    
    private void FixedUpdate()
    {
        //호스트가 아니라면
        if(!IsMasterClientLocal || PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        float distance = speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

        if(hit.collider != null)
        {
            Goalpost goalPost = hit.collider.GetComponent<Goalpost>();

            if(goalPost != null)
            {
                if(goalPost.playerNumber == 1)
                {
                    GameManager.Instance.AddScore(2, 1);
                }
                else if(goalPost.playerNumber == 2)
                {
                    GameManager.Instance.AddScore(1, 1);
                }
            }

            direction = Vector2.Reflect(direction, hit.normal);
            direction += Random.insideUnitCircle * randomRefectionIntensity;
        }
        transform.position = (Vector2)transform.position + direction * distance;
    }
}