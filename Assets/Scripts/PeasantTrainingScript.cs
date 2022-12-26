using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PeasantTrainingScript : MonoBehaviour
{
    private const float PeasantTrainingTime = 4f;
    
    public Image peasantTimerImage;
    public Button peasantTrainingButton;
    private GameManager _gameManager;
    public AudioClip cashOutSfx;
    private AudioManager _audioManager;
    
    public float peasantTimer = -2;
    
    void Start()
    {
        peasantTimerImage = GetComponent<Image>();
        _gameManager = FindObjectOfType<GameManager>();
        _audioManager = FindObjectOfType<AudioManager>();
    }
    
    void Update()
    {
        PeasantTrainingTimer();
    }

    private void PeasantTrainingTimer()
    {
        if (peasantTimer > 0)
        {
            peasantTimer -= Time.deltaTime;
            peasantTimerImage.fillAmount = peasantTimer / PeasantTrainingTime;
        }
        else if (peasantTimer > -1 && _gameManager.foodStockpile > _gameManager.peasantCost)
        {
            peasantTimerImage.fillAmount = 1;
            peasantTrainingButton.interactable = true;
            _gameManager.peasantPopulation += 1;
            peasantTimer = -2;
        }
    }

    public void TrainPeasant()
    {
        _audioManager.Play("CashOutSFX");
        _gameManager.foodStockpile -= _gameManager.peasantCost;
        peasantTimer = PeasantTrainingTime;
        peasantTrainingButton.interactable = false;
    }
}
