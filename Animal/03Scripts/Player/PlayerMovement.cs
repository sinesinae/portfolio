using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveHorizontal;
    public float moveVertical;

    public float moveSpeed = 250f;
    public float runSpeed = 400f;


    // Start is called before the first frame update
    public Rigidbody bodyRB;
    void Start()
    {
        //navManger.SaveCustomtoJson(csustomJsonData[1]["Idx"].ToString());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");


        

        //bodyRB.velocity = new Vector3(moveHorizontal * moveSpeed, bodyRB.velocity.y, moveVertical * moveSpeed);
        
        

        if (JoyStickMovement.Instance.joyVec.x != 0 || JoyStickMovement.Instance.joyVec.y != 0)
        {
            //Debug.Log(JoyStickMovement.Instance.joyVec);
            bodyRB.rotation = Quaternion.LookRotation(new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                
                bodyRB.velocity = new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y).normalized * Time.deltaTime * runSpeed;
            }
            else
            {
                bodyRB.velocity = new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y * Vector3.forward.z).normalized * Time.deltaTime * moveSpeed;
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
