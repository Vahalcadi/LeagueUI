using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region class variables
    [SerializeField] private GameObject summonerSlot;
    [SerializeField] private GameObject scoreboard;
    private List<GameObject> summonerSlotList = new();
    [SerializeField] private List<Sprite> summonerSpells;
    [SerializeField] private List<Sprite> items;
    [SerializeField] private List<Sprite> wards;

    [SerializeField] private List<FunctioningDictionary> runes;

    [SerializeField] private List<Sprite> championIcons;

    int deathCounter;
    int minionCounter;
    int level;

    private int numberOfSummoners;

    private bool isDead;
    private bool canUseUlti;
    private bool canUseSummoner1;
    private bool canUseSummoner2;
    private bool isEnabled;

    private int random;
    private List<int> extractedChampions = new();
    private List<int> extractedSummonerSpells = new();
    private List<int> extractedRunes = new();
    private List<int> extractedItems = new();
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            scoreboard.SetActive(!scoreboard.activeSelf);

        if (Input.GetKeyDown(KeyCode.P) && numberOfSummoners >= 1 && isEnabled)
        {
            for (int i = 0; i < numberOfSummoners; i++)
            {
                GenerateRandomItem(summonerSlotList[i].transform.Find("ItemSlots").gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && canUseUlti && !isDead && isEnabled)
        {
            canUseUlti = false;
            summonerSlotList[0].transform.Find("UltimateSlot").transform.Find("FullUltimate").GetComponent<Image>().color = Color.clear;
            summonerSlotList[0].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponent<Image>().fillAmount = 0;

            StartCoroutine(RechargeUltimate());
        }

        if (Input.GetKeyDown(KeyCode.D) && canUseSummoner1 && !isDead && isEnabled)
        {
            canUseSummoner1 = false;
            summonerSlotList[0].transform.Find("SummonerSpell1").transform.Find("RechargeSummoner").GetComponent<Image>().fillAmount = 1;
            StartCoroutine(RechargeSummonerSpellOnD());
        }

        if (Input.GetKeyDown(KeyCode.F) && canUseSummoner2 && !isDead && isEnabled)
        {
            canUseSummoner2 = false;
            summonerSlotList[0].transform.Find("SummonerSpell2").transform.Find("RechargeSummoner").GetComponent<Image>().fillAmount = 1;
            StartCoroutine(RechargeSummonerSpellOnF());
        }
        if (Input.GetKeyDown(KeyCode.K) && !isDead && isEnabled)
        {
            isDead = true;
            string death = summonerSlotList[0].transform.Find("KDA").GetComponent<TextMeshProUGUI>().text.Split("/")[1];
            deathCounter = Convert.ToInt32(death) + 1;
            summonerSlotList[0].transform.Find("KDA").GetComponent<TextMeshProUGUI>().text = $"0/{deathCounter}/0";
            StartCoroutine(DeathTimer());
        }
        if (Input.GetKeyDown(KeyCode.G) && !isDead && isEnabled)
        {
            minionCounter = Convert.ToInt32(summonerSlotList[0].transform.Find("MinionCounter").GetComponent<TextMeshProUGUI>().text) + 1;
            summonerSlotList[0].transform.Find("MinionCounter").GetComponent<TextMeshProUGUI>().text = $"{minionCounter}";
        }

        if (Input.GetKeyDown(KeyCode.J) && !isDead && isEnabled)
        {
            level = Convert.ToInt32(summonerSlotList[0].transform.Find("LevelSlot").GetComponentInChildren<TextMeshProUGUI>().text) + 1;
            if (level > 18)
                return; 

            summonerSlotList[0].transform.Find("LevelSlot").GetComponentInChildren<TextMeshProUGUI>().text = $"{level}";
        }
    }

    #region abilities
    public IEnumerator RechargeSummonerSpellOnD()
    {
        do
        {
            summonerSlotList[0].transform.Find("SummonerSpell1").transform.Find("RechargeSummoner").GetComponent<Image>().fillAmount -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(.5f);
        } while (summonerSlotList[0].transform.Find("SummonerSpell1").transform.Find("RechargeSummoner").GetComponent<Image>().fillAmount > 0);
        canUseSummoner1 = true;
    }

    public IEnumerator RechargeSummonerSpellOnF()
    {
        do
        {
            summonerSlotList[0].transform.Find("SummonerSpell2").transform.Find("RechargeSummoner").GetComponent<Image>().fillAmount -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(.5f);
        } while (summonerSlotList[0].transform.Find("SummonerSpell2").transform.Find("RechargeSummoner").GetComponent<Image>().fillAmount > 0);
        canUseSummoner2 = true;
    }

    public IEnumerator RechargeUltimate()
    {
        do
        {
            summonerSlotList[0].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponent<Image>().fillAmount += Time.fixedDeltaTime;
            yield return new WaitForSeconds(.5f);

        }
        while (summonerSlotList[0].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponent<Image>().fillAmount < 1);

        canUseUlti = true;
        summonerSlotList[0].transform.Find("UltimateSlot").transform.Find("FullUltimate").GetComponent<Image>().color = Color.green;
    }

    public IEnumerator DeathTimer()
    {
        int deathCounter = 15;
        summonerSlotList[0].transform.Find("CharacterSlot").transform.Find("CharacterIcon").transform.Find("Image").GetComponentInChildren<Image>().color = Color.grey;
        summonerSlotList[0].transform.Find("CharacterSlot").transform.Find("DeathCounter").GetComponent<TextMeshProUGUI>().color = Color.red;
        summonerSlotList[0].transform.Find("CharacterSlot").transform.Find("DeathCounter").GetComponent<TextMeshProUGUI>().text = $"{deathCounter}";

        do
        {
            yield return new WaitForSeconds(1);
            deathCounter--;
            summonerSlotList[0].transform.Find("CharacterSlot").transform.Find("DeathCounter").GetComponent<TextMeshProUGUI>().text = $"{deathCounter}";
        }
        while (deathCounter > 0);

        summonerSlotList[0].transform.Find("CharacterSlot").transform.Find("CharacterIcon").transform.Find("Image").GetComponentInChildren<Image>().color = Color.white;
        summonerSlotList[0].transform.Find("CharacterSlot").transform.Find("DeathCounter").GetComponent<TextMeshProUGUI>().color = Color.clear;

        isDead = false;
    }
    #endregion

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
        else if (this.numberOfSummoners < 1)
            return;

        for (int i = 0; i < this.numberOfSummoners; i++)
        {
            summonerSlotList.Add(Instantiate(summonerSlot, scoreboard.transform));

            GenerateRandomSummonerSpells
                (
                    summonerSlotList[i].transform.Find("SummonerSpell1").transform.Find("Image").GetComponent<Image>(),
                    summonerSlotList[i].transform.Find("SummonerSpell2").transform.Find("Image").GetComponent<Image>()
                );
            canUseSummoner1 = true;
            canUseSummoner2 = true;

            GenerateRandomRunes
                (
                    summonerSlotList[i].transform.Find("PrimaryRune").GetComponent<Image>(),
                    summonerSlotList[i].transform.Find("SecondaryRune").GetComponent<Image>()
                );

            GenerateRandomChampions
                (
                    summonerSlotList[i].transform.Find("CharacterSlot").transform.Find("CharacterIcon").transform.Find("Image").GetComponentInChildren<Image>()
                );
            summonerSlotList[i].transform.Find("CharacterSlot").transform.Find("DeathCounter").GetComponent<TextMeshProUGUI>().color = Color.clear;
            summonerSlotList[i].transform.Find("CharacterSlot").transform.Find("DeathCounter").GetComponent<TextMeshProUGUI>().text = "0";


            summonerSlotList[i].transform.Find("LevelSlot").GetComponentInChildren<TextMeshProUGUI>().text = "6";

            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponent<Image>().fillAmount = 0;
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("FullUltimate").GetComponent<Image>().color = Color.green;
            canUseUlti = true;

            summonerSlotList[i].transform.Find("MinionCounter").GetComponent<TextMeshProUGUI>().text = "0";
            summonerSlotList[i].transform.Find("KDA").GetComponent<TextMeshProUGUI>().text = "0/0/0";

            InstantiateZeroItems(summonerSlotList[i].transform.Find("ItemSlots").gameObject);
        }

        extractedChampions.Clear();
        isEnabled = true;
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