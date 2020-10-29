using System;
using UnityEngine;

public class JoyStickManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static JoyStickManager Instance { get; set; }
    public GameObject joystickCanvas;

    public Action pressA;
    public Action pressB;
    public Action pressX;
    public Action pressY;

    private void Awake()
    {
        Instance = this;
        pressA = testA;
        pressB = testB;
        pressX = testX;
        pressY = testY;
        

    }
    


    public void buttonA()
    {
        pressA();
    }

    public void buttonB()
    {
        pressB();

    }
    public void buttonX()
    {
        pressX();

    }
    public void buttonY()
    {
        pressY();

    }
    public void CanvasDisable()
    {
        joystickCanvas.SetActive(false);
    }
    public void CanvasAble()
    {
        joystickCanvas.SetActive(true);
    }


    public void testA()
    {
        Debug.Log("A");
    }
    public void testB()
    {
        Debug.Log("B");
    }
    public void testX()
    {
        Debug.Log("X");
    }
    public void testY()
    {
        Debug.Log("Y");
    }
}
