using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider HealthSlider;
    private float TimeLeft = 60f;
    private float nextTime = 60f;
    bool health = true;
    bool timeflow = true;


    Ctrl ctrl;
    PlayerMovement playerMovement;
    void Start()
    {
        ctrl = FindObjectOfType<Ctrl>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        StartCoroutine(playerDie());

    }

    void Update()
    {
        if (!timeflow)
            return;
        if (Time.time > nextTime)
        {
            nextTime = Time.time + TimeLeft;
            MiusHealth();
        }

    }


    public void AddHealth(int num)
    {
        HealthSlider.value += num;
    }

    public void MiusHealth()
    {
        HealthSlider.value -= 1;
        FindObjectOfType<FoodManager>().minuFOOD(1);

    }
    IEnumerator playerDie()
    {
        yield return null;
        while (health)
        {
            //HealthSlider.value = FindObjectOfType<FoodManager>().FOOD;
            if (HealthSlider.value == 0)
            {
                ctrl.transform.Rotate(270, 0, 0);
                if(PlayerPrefs.GetInt("QUEST1_3PRGRESS",0) == 3)
                    ctrl.transform.position = new Vector3(-300f, 1, -300f);
                HealthSlider.value = 5;
                FindObjectOfType<FoodManager>().FOOD = 5;
                FindObjectOfType<FoodManager>().savefood();
                ctrl.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                ctrl.enabled = false;
                playerMovement.enabled = false;
                health = false;
                timeflow = false;
                BoolList bl = Comment.instance.CommentPrint("'한숨도 못잤더니 너무 피곤해 '");
                yield return new WaitUntil(() => bl.isDone);
                TimeManager.instance.SetTime(TimeManager.onedayTime * 19 / 24);
            }
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(playerDieElse());
    }
    IEnumerator playerDieElse()
    {
        yield return null;

        ctrl.transform.GetChild(0).GetComponent<Animator>().enabled = true;

        ctrl.enabled = true;
        playerMovement.enabled = true;
        health = true;
        timeflow = true;

        StartCoroutine(playerDie());

    }

}


