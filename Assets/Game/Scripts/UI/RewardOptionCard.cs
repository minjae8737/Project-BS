using TMPro;
using UnityEngine;

public class RewardOptionCard : MonoBehaviour
{
    [SerializeField] private TMP_Text displayNameText;
    [SerializeField] private TMP_Text descriptionText;

    public void SetCard(RewardOption rewardOption)
    {
        displayNameText.text = string.Format("{0} Lv.{1}", rewardOption.Data.DisplayName, rewardOption.NextLevel);
        descriptionText.text = rewardOption.Data.GetLevelBonusDescription(rewardOption.NextLevel);
    }
}