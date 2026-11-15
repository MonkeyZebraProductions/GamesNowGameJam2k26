using TMPro;
using UnityEngine;

public class MoneyDisplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private Resource money;

    [SerializeField]
    private int startAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        money.SetAmount(startAmount);
        money.OnAmountChanged += UpdateText;
        UpdateText(money.Amount);
    }

    private void UpdateText(int Amount)
    {
        moneyText.text = "€" + Amount;
    }

    private void OnDestroy()
    {
        money.OnAmountChanged -= UpdateText;
    }
}
