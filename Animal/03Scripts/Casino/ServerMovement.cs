using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ServerMovement : MonoBehaviourPun
{
    public float moveSpeed = 250f;
    public float runSpeed = 400f;


    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    public float moveHorizontal;
    public float moveVertical;

    public Rigidbody bodyRB;

    [PunRPC]
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");



        //bodyRB.velocity = new Vector3(moveHorizontal * moveSpeed, bodyRB.velocity.y, moveVertical * moveSpeed);


        if (NetworkJoyStickMovement.Instance.joyVec.x != 0 || NetworkJoyStickMovement.Instance.joyVec.y != 0)
        {
            //Debug.Log(JoyStickMovement.Instance.joyVec);
            bodyRB.rotation = Quaternion.LookRotation(new Vector3(NetworkJoyStickMovement.Instance.joyVec.x, 0, NetworkJoyStickMovement.Instance.joyVec.y));
            if (Input.GetKey(KeyCode.LeftShift))
            {

                bodyRB.velocity = new Vector3(NetworkJoyStickMovement.Instance.joyVec.x, 0, NetworkJoyStickMovement.Instance.joyVec.y).normalized * Time.deltaTime * runSpeed;
            }
            else
            {
                bodyRB.velocity = new Vector3(NetworkJoyStickMovement.Instance.joyVec.x, 0, NetworkJoyStickMovement.Instance.joyVec.y).normalized * Time.deltaTime * moveSpeed;
            }

        }

        else if (moveHorizontal != 0 || moveVertical != 0)
        {
            //Debug.Log(moveHorizontal);
            //Debug.Log(moveVertical);
            Vector3 dir = new Vector3(moveHorizontal, 0, moveVertical).normalized;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                bodyRB.velocity = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.deltaTime * runSpeed;
            }
            else
            {
                bodyRB.velocity = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.deltaTime * moveSpeed;

            }
            bodyRB.rotation = Quaternion.LookRotation(dir);
        }
    }

  
}
