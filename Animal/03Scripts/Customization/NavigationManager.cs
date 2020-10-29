using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using System;
using LitJson;
//using UnityEngine.InputSystem;
[Serializable]
public class CustomItem
{
    public int ID;
    public string Name;
    public int Idx;
    public CustomItem(int id, string name, int idx)
    {
        ID = id;
        Name = name;
        Idx = idx;
    }
}
public class NavigationManager : MonoBehaviour
{

    //public InputMaster input;
    public TopBarManager topBarMng;
    public GameObject confirmPanel, confirmButton;
    public List<GenericButton> confirmPanelButtons;
    public PhaseCtrl phaseCtrl;
    public EquipmentManager equipManager;

    public List<CustomItem> customList = new List<CustomItem>();

    //void Awake()
    //{
    //    input = new InputMaster();
    //}

    //void OnSecondaryMovement(InputValue iv)
    //{
    //    var actionValue = iv.Get<float>();
    //    if (actionValue == 1)
    //        topBarMng.MoveOnTopBar(false);
    //    else if (actionValue == -1)
    //        topBarMng.MoveOnTopBar(true);
    //}

    public void Confirm(bool wantActive)
    {
        confirmPanel.SetActive(wantActive);

        if (wantActive)
            EventSystem.current.SetSelectedGameObject(confirmPanelButtons[0].gameObject);
        else
        {
            EventSystem.current.SetSelectedGameObject(confirmButton);
            foreach (var btn in confirmPanelButtons)
                btn.HandleState(false);
        }
    }
    public void GetOK()
    {
        phaseCtrl.Customization.SetActive(false);
        phaseCtrl.Tutorial.SetActive(true);
        phaseCtrl.GoNextScript();

        SaveCustomtoJson();
    }


    public void SaveCustomtoJson()
    {
        customList.Clear();
        customList.Add(new CustomItem(0, "SkinTone", equipManager.skinIndex));
        customList.Add(new CustomItem(1, "Hairstyle", equipManager.hairIndex));
        customList.Add(new CustomItem(2, "Eye", equipManager.eyeIndex));
        customList.Add(new CustomItem(3, "Nose", equipManager.noseIndex));
        customList.Add(new CustomItem(4, "Mouth", equipManager.mouthIndex));
        customList.Add(new CustomItem(5, "Cheeks", equipManager.cheeksIndex));
        customList.Add(new CustomItem(6, "HairColor", equipManager.hairColorIndex));


        JsonData CustomJson = JsonMapper.ToJson(customList);


        //string JsonData = JsonUtility.ToJson(customList);
        string path = Application.persistentDataPath + "/CustomData.json";
        File.WriteAllText(path, CustomJson.ToString());

    }

    public void SaveCustomtoJsonIfNotExist()
    {
        customList.Clear();
        customList.Add(new CustomItem(0, "SkinTone", 0));
        customList.Add(new CustomItem(1, "Hairstyle", 0));
        customList.Add(new CustomItem(2, "Eye", 0));
        customList.Add(new CustomItem(3, "Nose", 0));
        customList.Add(new CustomItem(4, "Mouth", 0));
        customList.Add(new CustomItem(5, "Cheeks", 0));
        customList.Add(new CustomItem(6, "HairColor", 0));


        JsonData CustomJson = JsonMapper.ToJson(customList);


        //string JsonData = JsonUtility.ToJson(customList);
        string path = Application.persistentDataPath + "/CustomData.json";
        File.WriteAllText(path, CustomJson.ToString());

    }
}
