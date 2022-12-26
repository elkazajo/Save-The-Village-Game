using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvasionCycleScript : MonoBehaviour
{
    private const float CycleLength = 0;
    
    public Image invasionCycleImage;
    public Text invasionCyclesAmountText;
    public Text orcsAmountInNextInvasionText;
    
    public bool invasionCycleEnded;
    [HideInInspector]
    public float invasionCycleMaxTime ;
    public float currentTime;
    private GameManager _gameManager;
    public int invasionCyclesAmount = 1;
    public int timeTillNextInvasion;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        currentTime = invasionCycleMaxTime;
        invasionCycleMaxTime = 30f;
    }
    
    void Update()
    {
        if (_gameManager.isGameStarted)
        {
            InvasionCycle();
            InvasionCycleCounter();
        }
    }

    void InvasionCycle()
    {
        invasionCycleEnded = false;
        currentTime -= Time.deltaTime;
        timeTillNextInvasion = Convert.ToInt16(currentTime);
        
        if (currentTime < CycleLength)
        {
            invasionCycleEnded = true;
            currentTime = invasionCycleMaxTime;
            _gameManager.warriorsLeft = _gameManager.warriorPopulation - _gameManager.nextInvasion;
            _gameManager.orcsLeft = _gameManager.nextInvasion - _gameManager.warriorPopulation;
            _gameManager.warriorPopulation = _gameManager.warriorsLeft;
            invasionCycleMaxTime += 5;
        }
        invasionCycleImage.fillAmount = currentTime / invasionCycleMaxTime;
    }

    void InvasionCycleCounter()
    {
        if (invasionCycleEnded)
        {
            invasionCyclesAmount++;
        }
        invasionCyclesAmountText.text = $"До {invasionCyclesAmount} набега осталось: {timeTillNextInvasion}";
        orcsAmountInNextInvasionText.text = $"Нападающих: {_gameManager.nextInvasion}";
    }
}
