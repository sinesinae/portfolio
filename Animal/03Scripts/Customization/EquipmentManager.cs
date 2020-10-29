using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;

public enum EquipmentType // 커스터마이징 타입 선언하는 연거형 변수
{
    SkinTone = 0,
    Hairstyle = 1,
    Eye = 2,
    Nose = 3,
    Mouth = 4,
    Cheeks = 5,
    HairColor = 6
}

public class EquipmentManager : Singleton<EquipmentManager>
{
    public List<Texture> skins;
    public List<Image> hairColorImages;
    public List<GameObject> hairs, noses, mouths;
    public Material mainMat;
    public Material hairMat;
    public Material eyeMat;
    public Animator playerAnimator;

    public JsonData customJsonData;

    public int skinIndex, hairIndex, hairColorIndex, eyeIndex, noseIndex, mouthIndex, cheeksIndex;
    public List<Color> hairColors = new List<Color>();

    public NavigationManager navManager;


    private void Awake()
    {
        LoadJson();   //JsonData 들고오기
        //hairColors = new List<Color>();
        if (hairColorImages.Count != 0) // 머리색버튼의 색을 리스트에 담아둠
        {
            foreach (var img in hairColorImages)
                hairColors.Add(img.color);
        }
    }

    private void Start()
    {

        AssignSkin(Convert.ToInt32(customJsonData[0]["Idx"].ToString()));
        AssignHair(Convert.ToInt32(customJsonData[1]["Idx"].ToString()));
        AssignEye(Convert.ToInt32(customJsonData[2]["Idx"].ToString()));
        AssignNose(Convert.ToInt32(customJsonData[3]["Idx"].ToString()));
        AssignMouth(Convert.ToInt32(customJsonData[4]["Idx"].ToString()));
        AssignCheek(Convert.ToInt32(customJsonData[5]["Idx"].ToString()));
        AssignHairColor(Convert.ToInt32(customJsonData[6]["Idx"].ToString()));




    }

    public void AssignSkin(int id)
    {
        skinIndex = id;
        //mainMat.SetTexture("_BaseMap", skins[skinIndex]);
        mainMat.mainTexture = skins[skinIndex];
    }
    public void AssignHair(int id)
    {
        hairIndex = id;
        foreach (var hair in hairs)
            hair.SetActive(false);
        hairs[hairIndex].SetActive(true);
    }
    public void AssignHairColor(int id)
    {
        hairColorIndex = id;
        //hairMat.SetColor("_BaseColor", hairColors[hairColorIndex]);
        hairMat.color = hairColors[hairColorIndex];
    }
    public void AssignEye(int id)
    {
        eyeIndex = id;
        if (id == 0)
            eyeMat.SetTextureOffset("_BaseMap", new Vector2(1, 0));
        else if (id == 1)
            eyeMat.SetTextureOffset("_BaseMap", new Vector2(1.5f, 0));

    }
    public void AssignNose(int id)
    {
        noseIndex = id;
        foreach (var nose in noses)
            nose.SetActive(false);
        noses[noseIndex].SetActive(true);
    }
    public void AssignMouth(int id)
    {
        mouthIndex = id;
        foreach (var mouth in mouths)
            mouth.SetActive(false);
        mouths[mouthIndex].SetActive(true);
    }
    public void AssignCheek(int id)
    {
        cheeksIndex = id;
    }
    public int GetCurrentEquipment(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.SkinTone:
                return skinIndex;
            case EquipmentType.Hairstyle:
                return hairIndex;
            case EquipmentType.Eye:
                return eyeIndex;
            case EquipmentType.Cheeks:
                return cheeksIndex;
            case EquipmentType.Mouth:
                return mouthIndex;
            case EquipmentType.Nose:
                return noseIndex;
            case EquipmentType.HairColor:
                return hairColorIndex;
            default:
                return skinIndex;
        }
    }

    public void AssignEquipment(EquipmentType type, int id)
    {
        if (type == EquipmentType.Hairstyle || type == EquipmentType.HairColor)
            playerAnimator.SetTrigger("LookAt");

        switch (type)
        {
            case EquipmentType.SkinTone:
                AssignSkin(id);
                break;
            case EquipmentType.Hairstyle:
                AssignHair(id);
                break;
            case EquipmentType.Eye:
                AssignEye(id);
                break;
            case EquipmentType.Cheeks:
                AssignCheek(id);
                break;
            case EquipmentType.Mouth:
                AssignMouth(id);
                break;
            case EquipmentType.Nose:
                AssignNose(id);
                break;
            case EquipmentType.HairColor:
                AssignHairColor(id);
                break;
        }
    }

    public void LoadJson()  // JSON 파일을 읽어옮
    {
        if (File.Exists(Application.persistentDataPath + "/CustomData.json") == false)
        {//JSON 파일이 존재하지 않을 때 파일을 만들어주고 Idx 값을 0으로 초기화
            navManager.SaveCustomtoJsonIfNotExist();
        }

        string Jsonstring = File.ReadAllText(Application.persistentDataPath + "/CustomData.json");
        customJsonData = JsonMapper.ToObject(Jsonstring);



    }
    //private void ParsingJsonItem(JsonData customData)
    //{
    //    for (int i= 0; i< customData.Count; i++)
    //    {
    //        string tmpID = customData[i]["ID"].ToString();

    //        for (int j= 0; j<navManager.customList.Count; j++)
    //        {
    //            if(tmpID == navManager.customList[j].ID.ToString())
    //            {
    //                CustomList.Add(navManager.customList[i]);
    //            }
    //        }


    //    }
    //}
}
