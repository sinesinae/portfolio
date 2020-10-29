using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FoodManager : MonoBehaviour
{
    [Serializable]
    public class Savefood
    {
        public int food;
    }
    public int FOOD;


    void Start()
    {
        loadfood();
        FindObjectOfType<HealthManager>().HealthSlider.value=FOOD;
    }

    void Update()
    {
        
    }
    public void addFOOD(int num)
    {
        FOOD += num;
        if (FOOD > 10)
            FOOD = 10;
        savefood();
    }
    public void minuFOOD(int num)
    {
        FOOD -= num;
        if (FOOD < 0)
            FOOD = 0;
        savefood();
    }
    public void savefood()
    {
        Savefood save = new Savefood();
        save.food = FOOD;
        string json = JsonUtility.ToJson(save);

        File.WriteAllText(Application.persistentDataPath + "/fooddata.json", json);
    }
    public void loadfood()
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/fooddata.json");
            Savefood load = new Savefood();
            load = JsonUtility.FromJson<Savefood>(json);
           
            FOOD = load.food;
        }
        catch
        {
            FOOD = 10;

        }
    }
}
