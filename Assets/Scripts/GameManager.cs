using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject summonerSlot;
    [SerializeField] private GameObject scoreboard;
    private List<GameObject> summonerSlotList;
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


    public void InstantiateSummoners(int numberOfSummoners)
    {
        if(numberOfSummoners > 10)
            numberOfSummoners = 10;

        for (int i = 0; i < numberOfSummoners; i++)
        {
            summonerSlotList.Add(Instantiate(summonerSlot, scoreboard.transform));
            summonerSlotList[i].transform.Find("SummonerSpell1").GetComponentInChildren<Image>().sprite = summonerSpells[Random.Range(0, summonerSpells.Count)];
            summonerSlotList[i].transform.Find("SummonerSpell2").GetComponentInChildren<Image>().sprite = summonerSpells[Random.Range(0, summonerSpells.Count)];
            summonerSlotList[i].transform.Find("CharacterSlot").transform.Find("CharacterIcon").GetComponentInChildren<Image>().sprite = championIcon[Random.Range(0, championIcon.Count)];
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("RechargeUltimate").GetComponentInChildren<Image>().fillAmount = 0;
            summonerSlotList[i].transform.Find("UltimateSlot").transform.Find("FullUltimate").GetComponentInChildren<Image>().color = Color.clear;
            summonerSlotList[i].transform.Find("LevelSlot").GetComponentInChildren<TextMeshProUGUI>().text = "1";
            summonerSlotList[i].transform.Find("PrimaryRune").GetComponent<Image>().sprite = keyRunes[Random.Range(0, keyRunes.Count)];
            summonerSlotList[i].transform.Find("SecondaryRune").GetComponent<Image>().sprite = secondaryRunes[Random.Range(0, secondaryRunes.Count)];
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
