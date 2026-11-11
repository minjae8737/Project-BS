using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpRewardPanel : MonoBehaviour
{
    [SerializeField] private List<RewardOptionCard> optionCards;
    public event Action<int> RewardSelected;

    private void Awake()
    {
        for (int i = 0; i < optionCards.Count; i++)
        {
            int index = i;
            optionCards[index].GetComponent<Button>().onClick.AddListener(() => OnCardClicked(index));
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowRewardOption(List<RewardOption> rewardOptions)
    {
        HideCards();
        
        for (int i = 0; i < rewardOptions.Count; i++)
        {
            optionCards[i].SetCard(rewardOptions[i]);
            optionCards[i].gameObject.SetActive(true);
        }
    }

    private void HideCards()
    {
        foreach (RewardOptionCard card in optionCards)
        {
           card.gameObject.SetActive(false); 
        }
    }
    
    private void OnCardClicked(int cardIndex)
    {
        RewardSelected?.Invoke(cardIndex);
    }
}