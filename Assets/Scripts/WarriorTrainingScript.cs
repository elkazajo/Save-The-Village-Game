using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorTrainingScript : MonoBehaviour
{
    private const float WarriorTrainingTime = 5f;
    
    public Image warriorTimerImage;
    public float warriorTimer = -2;
    public Button warriorTrainingButton;
    private GameManager _gameManager;
    public AudioClip cashOutSfx;
    private AudioSource _audioSource;
    
    void Start()
    {
        warriorTimerImage = GetComponent<Image>();
        _gameManager = FindObjectOfType<GameManager>();
        _audioSource = FindObjectOfType<AudioSource>();
    }
    
    void Update()
    {
        WarriorTrainingTimer();
    }

    private void WarriorTrainingTimer()
    {
        if (warriorTimer > 0)
        {
            warriorTimer -= Time.deltaTime;
            warriorTimerImage.fillAmount = warriorTimer / WarriorTrainingTime;
        }
        else if (warriorTimer > -1 && _gameManager.foodStockpile > _gameManager.warriorCost)
        {
            warriorTimerImage.fillAmount = 1;
            warriorTrainingButton.interactable = true;
            _gameManager.warriorPopulation += 1;
            _gameManager.warriorsWere = _gameManager.warriorPopulation;
            warriorTimer = -2;
        }
    }

    public void TrainWarrior()
    {
        _audioSource.PlayOneShot(cashOutSfx);
        _gameManager.foodStockpile -= _gameManager.warriorCost;
        warriorTimer = WarriorTrainingTime;
        warriorTrainingButton.interactable = false;
    }
}
