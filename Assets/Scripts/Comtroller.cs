using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comtroller : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 dir;
    [SerializeField] private float PlayerSpeed;
    private float ChangeSpeed;
   
    
 
    
    private int lineToMove = 1;
    public float lineDistance = 4;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ChangeSpeed = PlayerSpeed;
    }

    private void Update()
    {

        if (SwipeController.swipeRight)
        { if (lineToMove < 2)
                lineToMove++;
        }

        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
        }

        if (SwipeController.swipeUp)
        {
            if (PlayerSpeed < 2 * ChangeSpeed)
            PlayerSpeed += ChangeSpeed;
       
  
        }

        if (SwipeController.swipeDown)
        {
            if (PlayerSpeed > 0)
            PlayerSpeed -= ChangeSpeed;
        }
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        transform.position = targetPosition;
    }

 
    void FixedUpdate()
    {
        dir.z = PlayerSpeed;
        controller.Move(dir * Time.fixedDeltaTime);
        
    }
}
