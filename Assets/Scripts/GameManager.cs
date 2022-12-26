using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private HarvestCycleScript _harvestCycleScript;
    private InvasionCycleScript _invasionCycleScript;
    private FoodConsumptionCycleScript _foodConsumptionCycleScript;
    private PeasantTrainingScript _peasantTrainingScript;
    private WarriorTrainingScript _warriorTrainingScript;

    public Canvas mainMenuCanvas;
    public Canvas gameCanvas;
    
    public GameObject gameOverPanel;
    public GameObject villageAttackedPanel;
    public GameObject winPanel;
    public GameObject invasionWillStartSoonPanel;

    public Button pauseButton;
    
    public Text orcCountText;
    public Text orcsWereText;
    public Text warriorsWereText;
    public Text orcsWereWinText;
    public Text warriorsWereWinText;
    public Text peasantCostText;
    public Text warriorCostText;
    public Text invasionsSurvivedText;
    public Text peasantsPopulationPanelText;
    public Text warriorsPopulationPanelText;
    public Text foodStockpilePanelText;
    public Text invasionWillStartSoonInfoText;

    private AudioManager _audioManager;
    
    public int peasantPopulation;
    public int warriorPopulation;
    public int foodStockpile;
    public int foodToCome;
    public int foodToBeConsumed;
    public int foodPerPeasant;
    public int foodPerWarrior;
    public int peasantCost;
    public int warriorCost;
    public int invasionIncrease;
    public int nextInvasion;
    public int orcsLeft;
    public int orcsWere;
    public int warriorsLeft;
    public int warriorsWere;
    private int _cyclesTillAttack = 3;

    public float startingTime;

    public bool isGameStarted;
    
    void Start()
    {
        isGameStarted = false;
        
        UpdatePopulationAndFoodInfoTexts();
        
        _harvestCycleScript = FindObjectOfType<HarvestCycleScript>();
        _invasionCycleScript = FindObjectOfType<InvasionCycleScript>();
        _foodConsumptionCycleScript = FindObjectOfType<FoodConsumptionCycleScript>();
        _peasantTrainingScript = FindObjectOfType<PeasantTrainingScript>();
        _warriorTrainingScript = FindObjectOfType<WarriorTrainingScript>();
        _audioManager = FindObjectOfType<AudioManager>();
        
        peasantCostText.text = $"Стоимость: {peasantCost} еды";
        warriorCostText.text = $"Стоимость: {warriorCost} еды";
    }

    void Update()
    {
        if (isGameStarted)
        {
            UpdateCycles();
            UpdatePopulationAndFoodInfoTexts();
            WinGame();
            GameOver();
            VillageSurvivedAttack();
            TimeLeftTillAttacksStart();
        }
    }

    private void UpdatePopulationAndFoodInfoTexts()
    {
        peasantsPopulationPanelText.text = $"Крестьян: {peasantPopulation}";
        warriorsPopulationPanelText.text = $"Воинов: {warriorPopulation}";
        foodStockpilePanelText.text = $"Еды: {foodStockpile}";
    }

    private void UpdateCycles()
    {
        foodToCome = peasantPopulation * foodPerPeasant;
        foodToBeConsumed = warriorPopulation * foodPerWarrior;
        orcsWere = nextInvasion;

        if (_harvestCycleScript.harvestCycleEnded)
        {
            _audioManager.Play("FoodHarvested");
            foodStockpile += foodToCome;
        }

        if (_foodConsumptionCycleScript.foodConsumptionCycleEnded)
        {
            _audioManager.Play("FoodCycleEnded");
            foodStockpile -= foodToBeConsumed;
        }

        if (_invasionCycleScript.invasionCycleEnded)
        {
            if (_invasionCycleScript.invasionCyclesAmount > 3)
            {
                nextInvasion += invasionIncrease;
            }
        }

        if (foodStockpile < 0)
        {
            foodStockpile = 0;
        }
    }

    private void TimeLeftTillAttacksStart()
    {
        if (_invasionCycleScript.invasionCycleEnded && _invasionCycleScript.invasionCyclesAmount < 5)
        {
            _audioManager.Play("VillageSurvivedAttack");
            
            Time.timeScale = 0;
            
            invasionWillStartSoonPanel.SetActive(true);
            
            _peasantTrainingScript.peasantTrainingButton.interactable = false;
            _warriorTrainingScript.warriorTrainingButton.interactable = false;
            
            pauseButton.interactable = false;
            
            invasionWillStartSoonInfoText.text = $"циклов \n{_cyclesTillAttack--}";
        }
    }
    
    private void VillageSurvivedAttack()
    {
        if (orcsLeft <= warriorsLeft && _invasionCycleScript.invasionCycleEnded && 
            _invasionCycleScript.invasionCyclesAmount is < 8 and > 4)
        {
            _audioManager.Play("VillageSurvivedAttack");
            
            Time.timeScale = 0;
            
            villageAttackedPanel.SetActive(true);
            
            _peasantTrainingScript.peasantTrainingButton.interactable = false;
            _warriorTrainingScript.warriorTrainingButton.interactable = false;
            
            pauseButton.interactable = false;
            
            orcsWereText.text = $"Орков было: {orcsWere}";
            warriorsWereText.text = $"Воинов было: {warriorsWere}";
        }
    }

    public void OkButton()
    {
        Time.timeScale = 1;
        
        _audioManager.Play("OkButton");
        _audioManager.Stop("VillageSurvivedAttack");
        
        villageAttackedPanel.SetActive(false);
        invasionWillStartSoonPanel.SetActive(false);
        
        _peasantTrainingScript.peasantTrainingButton.interactable = true;
        _warriorTrainingScript.warriorTrainingButton.interactable = true;
        
        pauseButton.interactable = true;
    }

    public void GameOver()
    {
        if (orcsLeft > warriorsLeft)
        {
            _audioManager.Play("GameOver");
            
            Time.timeScale = 0;
            
            gameOverPanel.SetActive(true);
            pauseButton.interactable = false;
            
            _peasantTrainingScript.peasantTrainingButton.interactable = false;
            _warriorTrainingScript.warriorTrainingButton.interactable = false;
            
            orcCountText.text = $"Орков осталось: {orcsLeft}";
        }
    }

    private void WinGame()
    {
        if (_invasionCycleScript.invasionCyclesAmount > 7)
        {
            _audioManager.Play("VillageSurvivedAttack");
            
            Time.timeScale = 0;
            
            winPanel.SetActive(true);
            pauseButton.interactable = false;
            
            orcsWereWinText.text = $"Орков было: {orcsWere}";
            warriorsWereWinText.text = $"Воинов было: {warriorsWere}";
            invasionsSurvivedText.text = $"Набегов пережито: {_invasionCycleScript.invasionCyclesAmount}";
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 0;
        gameCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        
        _audioManager.Play("OkButton");
        _audioManager.Stop("GameOver");
        
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        
        _peasantTrainingScript.peasantTrainingButton.interactable = true;
        _warriorTrainingScript.warriorTrainingButton.interactable = true;
        
        pauseButton.interactable = true;

        _harvestCycleScript.harvestCycleEnded = true;
        _harvestCycleScript.currentTime = startingTime;
        
        _invasionCycleScript.invasionCycleEnded = true;
        _invasionCycleScript.currentTime = _invasionCycleScript.invasionCycleMaxTime;
        _invasionCycleScript.invasionCyclesAmount = 1;
        _invasionCycleScript.invasionCycleMaxTime = 30f;

        _foodConsumptionCycleScript.foodConsumptionCycleEnded = true;
        _foodConsumptionCycleScript.currentTime = startingTime;
        
        _peasantTrainingScript.peasantTimer = -2;
        _peasantTrainingScript.peasantTimerImage.fillAmount = 1;
        _peasantTrainingScript.peasantTrainingButton.interactable = true;
        
        _warriorTrainingScript.warriorTimer = -2;
        _warriorTrainingScript.warriorTimerImage.fillAmount = 1;
        _warriorTrainingScript.warriorTrainingButton.interactable = true;
        
        _cyclesTillAttack = 3;
        orcsLeft = 0;
        foodToCome = 0;
        foodToBeConsumed = 0;
        orcsLeft = 0;
        warriorsLeft = 0;
        peasantPopulation = 0;
        warriorPopulation = 0;
        foodStockpile = 10;
        nextInvasion = 0;
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            _audioManager.Play("GameUnPaused");
            
            Time.timeScale = 1;
            
            _peasantTrainingScript.peasantTrainingButton.interactable = true;
            _warriorTrainingScript.warriorTrainingButton.interactable = true;
        }
        else
        {
            _audioManager.Play("GamePaused");
            
            Time.timeScale = 0;
            
            _peasantTrainingScript.peasantTrainingButton.interactable = false;
            _warriorTrainingScript.warriorTrainingButton.interactable = false;
        }
    }
    
    public void MainMenuButtonsSfx()
    {
        _audioManager.Play("OkButton");
    }

    public void StartGame()
    {
        isGameStarted = true;
    }
}
