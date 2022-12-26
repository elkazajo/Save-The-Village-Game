using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestCycleScript : MonoBehaviour
{
    private const float CycleLength = 5f;
    
    private Image _foodCycleIcon;
    public Text expectedFoodAmountText;

    private GameManager _gameManager;

    public bool harvestCycleEnded;
    private float _harvestCycleMaxLength;
    public float currentTime;

    void Start()
    {
        currentTime = _harvestCycleMaxLength;
        _foodCycleIcon = GetComponent<Image>();
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        if (_gameManager.isGameStarted)
        {
            HarvestCycle();
        }
    }

    void HarvestCycle()
    {
        harvestCycleEnded = false;
        currentTime += Time.deltaTime;
        expectedFoodAmountText.text = $"Еды ожидается: {_gameManager.foodToCome}";
        if (currentTime >= CycleLength)
        {
            harvestCycleEnded = true;
            currentTime = _harvestCycleMaxLength;
        }
        _foodCycleIcon.fillAmount = currentTime / CycleLength;
    }
}
