using System;
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
    [SerializeField] private List<Sprite> items;
    [SerializeField] private List<Sprite> wards;

    [SerializeField] private List<FunctioningDictionary> runes;

    [SerializeField] private List<Sprite> championIcons;

    private TextMeshProUGUI minionCounter;
    private TextMeshProUGUI kda;

    private int numberOfSummoners;

    private int random;
    private List<int> extractedChampions = new();
    private List<int> extractedSummonerSpells = new();
    private List<int> extractedRunes = new();
    private List<int> extractedItems = new();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && numberOfSummoners >= 1)
        {
            for (int i = 0; i < numberOfSummoners; i++)
            {
                GenerateRandomItem(summonerSlotList[i].transform.Find("ItemSlots").gameObject);
            }
        }
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

        this.numberOfSummoners = Convert.ToInt32(numberOfSummoners);

        if (this.numberOfSummoners > 10)
            this.numberOfSummoners = 10;

        for (int i = 0; i < this.numberOfSummoners; i++)
        {
            summonerSlotList.Add(Instantiate(summonerSlot, scoreboard.transform));

            GenerateRandomSummonerSpells
                (
                    summonerSlotList[i].transform.Find("SummonerSpell1").transform.Find("Image").GetComponent<Image>(),
                    summonerSlotList[i].transform.Find("SummonerSpell2").transform.Find("Image").GetComponent<Image>()
                );

            GenerateRandomRunes
                (
                    summonerSlotList[i].transform.Find("PrimaryRune").GetComponent<Image>(),
                    summonerSlotList[i].transform.Find("SecondaryRune").GetComponent<Image>()
                );

            GenerateRandomChampions
                (
                    summonerSlotList[i].transform.Find("CharacterSlot").transform.Find("CharacterIcon").transform.Find("Image").GetComponentInChildren<Image>()
                );

            summonerSlotList[i].transform.Find("LevelSlot").GetComponentInChildren<TextMeshProUGUI>().text = "6";
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponent<Image>().fillAmount = 0;
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("FullUltimate").GetComponent<Image>().color = Color.green;
            summonerSlotList[i].transform.Find("MinionCounter").GetComponent<TextMeshProUGUI>().text = "0";
            summonerSlotList[i].transform.Find("KDA").GetComponent<TextMeshProUGUI>().text = "0/0/0";

            InstantiateZeroItems(summonerSlotList[i].transform.Find("ItemSlots").gameObject);
        }

        extractedChampions.Clear();
    }



    private void GenerateRandomRunes(Image keyRune, Image secondaryRune)
    {
        List<Sprite> tempList = new();

        for (int i = 0; i < runes.Count; i++)
        {
            tempList.Add(runes[i].key);
        }

        CheckExtractedElement(tempList, extractedRunes);
        keyRune.sprite = runes[random].value[UnityEngine.Random.Range(0, runes[random].value.Count)];

        CheckExtractedElement(tempList, extractedRunes);
        secondaryRune.sprite = runes[random].key;

        extractedRunes.Clear();
    }

    private void GenerateRandomChampions(Image championIcon)
    {
        CheckExtractedElement(championIcons, extractedChampions);
        championIcon.sprite = championIcons[random];

    }

    private void GenerateRandomSummonerSpells(Image firstSpell, Image SecondSpell)
    {
        CheckExtractedElement(summonerSpells, extractedSummonerSpells);
        firstSpell.sprite = summonerSpells[random];
        CheckExtractedElement(summonerSpells, extractedSummonerSpells);
        SecondSpell.sprite = summonerSpells[random];

        extractedSummonerSpells.Clear();
    }

    private void CheckExtractedElement(List<Sprite> generalList, List<int> extractedInts)
    {
        do
        {
            random = UnityEngine.Random.Range(0, generalList.Count);
        }
        while (extractedInts.Contains(random));

        extractedInts.Add(random);
    }
    public void InstantiateZeroItems(GameObject itemSlots)
    {
        for (int i = 0; i < itemSlots.transform.childCount; i++)
        {
            itemSlots.transform.GetChild(i).GetComponentInChildren<Image>().color = Color.clear;
        }
    }
    private void GenerateRandomItem(GameObject itemSlots)
    {
        InstantiateZeroItems(itemSlots);

        for (int i = 0; i < itemSlots.transform.childCount - 1; i++)
        {
            CheckExtractedElement(items, extractedItems);
            itemSlots.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = items[random];
            itemSlots.transform.GetChild(i).GetComponentInChildren<Image>().color = Color.white;
        }
        itemSlots.transform.GetChild(6).GetChild(0).GetComponent<Image>().sprite = wards[UnityEngine.Random.Range(0, 2)];
        itemSlots.transform.GetChild(6).GetComponentInChildren<Image>().color = Color.white;

        extractedItems.Clear();
    }

}

[Serializable]
public class FunctioningDictionary
{
    public Sprite key;
    public List<Sprite> value;
}