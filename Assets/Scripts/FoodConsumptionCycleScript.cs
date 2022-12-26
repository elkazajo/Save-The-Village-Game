using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodConsumptionCycleScript : MonoBehaviour
{
    private const float CycleLength = 6f;
    
    private Image _foodConsumptionCycleIcon;
    public Text foodToBeConsumedText;

    private GameManager _gameManager;
    
    public bool foodConsumptionCycleEnded;
    private float _foodConsumptionCycleStartingTime;
    public float currentTime;
    
    void Start()
    {
        currentTime = _foodConsumptionCycleStartingTime;
        _foodConsumptionCycleIcon = GetComponent<Image>();
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        if (_gameManager.isGameStarted)
        {
            FoodConsumptionCycle();
        }
    }

    void FoodConsumptionCycle()
    {
        foodConsumptionCycleEnded = false;
        currentTime += Time.deltaTime;
        foodToBeConsumedText.text = $"Воины съедят: {_gameManager.foodToBeConsumed}";
        if (currentTime >= CycleLength)
        {
            foodConsumptionCycleEnded = true;
            currentTime = _foodConsumptionCycleStartingTime;
        }
        _foodConsumptionCycleIcon.fillAmount = currentTime / CycleLength;
    }
}
