using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject summonerSlot;
    [SerializeField] private GameObject scoreboard;
    private List<GameObject> summonerSlotList = new();
    [SerializeField] private List<Sprite> summonerSpells;
    [SerializeField] private List<Sprite> keyRunes;
    [SerializeField] private List<Sprite> secondaryRunes;
    [SerializeField] private List<Sprite> championIcon;
    private TextMeshProUGUI minionCounter;
    private TextMeshProUGUI kda;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InstantiateSummoners(string numberOfSummoners)
    {
        if (summonerSlotList.Count > 0)
        {
            foreach (var item in summonerSlotList)
            {
                Destroy(item.gameObject);
            }

            summonerSlotList.Clear();
        }

        int summoners = Convert.ToInt32(numberOfSummoners);

        if(summoners > 10)
            summoners = 10;

        for (int i = 0; i < summoners; i++)
        {
            summonerSlotList.Add(Instantiate(summonerSlot, scoreboard.transform));

            summonerSlotList[i].transform.Find("SummonerSpell1").transform.Find("Image").GetComponent<Image>().sprite = summonerSpells[UnityEngine.Random.Range(0, summonerSpells.Count)];
            summonerSlotList[i].transform.Find("SummonerSpell2").transform.Find("Image").GetComponent<Image>().sprite = summonerSpells[UnityEngine.Random.Range(0, summonerSpells.Count)];
            summonerSlotList[i].transform.Find("CharacterSlot").transform.Find("CharacterIcon").transform.Find("Image").GetComponentInChildren<Image>().sprite = championIcon[UnityEngine.Random.Range(0, championIcon.Count)];
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponent<Image>().fillAmount = 0;
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("FullUltimate").GetComponent<Image>().color = Color.clear;
            summonerSlotList[i].transform.Find("LevelSlot").GetComponentInChildren<TextMeshProUGUI>().text = "1";
            summonerSlotList[i].transform.Find("PrimaryRune").GetComponent<Image>().sprite = keyRunes[UnityEngine.Random.Range(0, keyRunes.Count)];
            summonerSlotList[i].transform.Find("SecondaryRune").GetComponent<Image>().sprite = secondaryRunes[UnityEngine.Random.Range(0, secondaryRunes.Count)];
            summonerSlotList[i].transform.Find("MinionCounter").GetComponent<TextMeshProUGUI>().text = "0";
            summonerSlotList[i].transform.Find("KDA").GetComponent<TextMeshProUGUI>().text = "0/0/0";

            InstantiateZeroItems(summonerSlotList[i].transform.Find("ItemSlots").gameObject);
        }
    }

    public void InstantiateZeroItems(GameObject itemSlots)
    {
        for(int i = 0; i < itemSlots.transform.childCount; i++)
        {
            itemSlots.transform.GetChild(i).GetComponentInChildren<Image>().color = Color.clear;
        }
    }

}
