
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dropdown : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown dropdown;

    public void AddOption(string text)
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData(text, null));

        dropdown.RefreshShownValue();
    }

    public void RemoveOption()
    {
        int indexToRemove = 0;

        if (dropdown.value == indexToRemove)
            dropdown.value = 0;

        dropdown.options.RemoveAt(indexToRemove);
        dropdown.RefreshShownValue();
    }
}
