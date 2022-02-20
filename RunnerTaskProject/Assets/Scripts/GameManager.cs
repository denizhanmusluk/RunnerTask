using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using TMPro;


public class GameManager : MonoBehaviour, IWinObserver, ILoseObserver, IEndGameObserver
{
    public bool gameActive;
    public static GameManager Instance;
    [SerializeField] public GameObject startButton;
    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject ProgressBar;
    [SerializeField] GameObject diamondUpgradeButton;
    [SerializeField] TextMeshProUGUI diamondUpgradeText;
    [SerializeField] GameObject costText;

    [SerializeField] GameObject heartUpgradeButton;
    [SerializeField] TextMeshProUGUI heartUpgradeText;

    [SerializeField] GameObject heartsParent;
    [SerializeField] GameObject heartPrefab;
    //[SerializeField] GameObject confetti;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        endGameObservers = new List<IEndGameObserver>();
        winObservers = new List<IWinObserver>();
        loseObservers = new List<ILoseObserver>();
        startObservers = new List<IStartGameObserver>();
        finishObservers = new List<IFinish>();
    }

    void Start()
    {
        startButton.SetActive(true);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        ProgressBar.SetActive(true);
        Add_WinObserver(this);
        Add_LoseObserver(this);
        Add_EndObserver(this);

        diamondUpgradeButton.SetActive(true);
        heartUpgradeButton.SetActive(true);
        costText.SetActive(true);
        if (PlayerPrefs.GetInt("diamond") >= 1)
        {
            Globals.diamondMultiple = PlayerPrefs.GetInt("diamond");
        }
        diamondUpgradeText.text = "x " + (Globals.diamondMultiple + 1).ToString();


        if (PlayerPrefs.GetInt("cost") >= 1)
        {
            Globals.totalCost = PlayerPrefs.GetInt("cost");
        }
        costText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.totalCost.ToString();

        if (PlayerPrefs.GetInt("heart") >= 1)
        {
            Globals.heartCount = PlayerPrefs.GetInt("heart");
        }
        heartUpgradeText.text = "x " + (Globals.heartCount + 1).ToString();

        
        for (int i = 0; i < Globals.heartCount; i++)
        {
            var heart = Instantiate(heartPrefab, transform.position, Quaternion.identity);
            heart.transform.parent = heartsParent.transform;
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 50, 0, 0);
        }
    }

    private void Update()
    {

    }


    public void StartButton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(startDelay());
            diamondUpgradeButton.SetActive(false);
            costText.SetActive(false);
            heartUpgradeButton.SetActive(false);

        }
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Globals.isGameActive = true;
        Globals.finish = false;
        startButton.SetActive(false);
        Notify_GameStartObservers();
    }
    public void RestartButton()
    {
        PlayerPrefs.SetInt("cost", 0);
        PlayerPrefs.SetInt("diamond", 0);
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("heart", 0);
        Globals.currentLevel = 0;
        Globals.diamondMultiple = 1;
        Globals.heartCount = 3;
        Globals.totalCost = 0;
        Globals.isGameActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void NextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Globals.isGameActive = true;
    }

    public void diamondUpgrade()
    {
        Globals.diamondMultiple++;
        PlayerPrefs.SetInt("diamond", Globals.diamondMultiple);
        diamondUpgradeText.text = "x " + (Globals.diamondMultiple + 1).ToString();

        if (Globals.totalCost > 10)
        {
            Globals.totalCost -= 10;
            PlayerPrefs.SetInt("cost", Globals.totalCost);
            costText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.totalCost.ToString();
        }
    }
    public void healthUpgrade()
    {
        Globals.heartCount++;
        PlayerPrefs.SetInt("heart", Globals.heartCount);
        heartUpgradeText.text = "x " + (Globals.heartCount + 1).ToString();
        if (Globals.totalCost > 1000)
        {
            Globals.totalCost -= 1000;
            PlayerPrefs.SetInt("cost", Globals.totalCost);
            costText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.totalCost.ToString();
        }

        var heart = Instantiate(heartPrefab, transform.position, Quaternion.identity);
        heart.transform.parent = heartsParent.transform;
        heart.GetComponent<RectTransform>().anchoredPosition = new Vector3((Globals.heartCount - 1) * 50, 0, 0);
    }
    public void LoseScenario()
    {
        //GameEvents.characterEvent.RemoveAllListeners();
        Globals.isGameActive = false;
        ProgressBar.SetActive(false);


        StartCoroutine(Fail_Delay());
    }
    IEnumerator Fail_Delay()
    {
        yield return new WaitForSeconds(2);

        failPanel.SetActive(true);

    }
    public void WinScenario()
    {
        //GameEvents.characterEvent.RemoveAllListeners();
        

        ProgressBar.SetActive(false);

        Globals.isGameActive = false;

        StartCoroutine(win_Delay());

        Globals.currentLevel++;
        PlayerPrefs.SetInt("level", Globals.currentLevel);

    }
    IEnumerator win_Delay()
    {
        yield return new WaitForSeconds(0.1f);

        successPanel.SetActive(true);

    }
    public void GameEnd()
    {

    }





    #region Observer Funcs

    private List<IEndGameObserver> endGameObservers;
    private List<IWinObserver> winObservers;
    private List<ILoseObserver> loseObservers;
    private List<IStartGameObserver> startObservers;
    private List<IFinish> finishObservers;
    #region Finish Observer
    public void Add_FinishObserver(IFinish observer)
    {
        finishObservers.Add(observer);
    }

    public void Remove_FinishObserver(IFinish observer)
    {
        finishObservers.Remove(observer);
    }

    public void Notify_GameFinishObservers()
    {
        foreach (IFinish observer in finishObservers.ToArray())
        {
            if (finishObservers.Contains(observer))
                observer.humansRagdoll(true);
        }
    }
    #endregion

    #region Start Observer
    public void Add_StartObserver(IStartGameObserver observer)
    {
        startObservers.Add(observer);
    }

    public void Remove_StartObserver(IStartGameObserver observer)
    {
        startObservers.Remove(observer);
    }

    public void Notify_GameStartObservers()
    {
        foreach (IStartGameObserver observer in startObservers.ToArray())
        {
            if (startObservers.Contains(observer))
                observer.StartGame();
        }
    }
    #endregion

    #region End Observer
    public void Add_EndObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void Remove_EndObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void Notify_GameEndObservers()
    {
        foreach (IEndGameObserver observer in endGameObservers.ToArray())
        {
            if (endGameObservers.Contains(observer))
                observer.GameEnd();
        }
    }
    #endregion

    #region Win Observer
    public void Add_WinObserver(IWinObserver observer)
    {
        winObservers.Add(observer);
    }

    public void Remove_WinObserver(IWinObserver observer)
    {
        winObservers.Remove(observer);
    }

    public void Notify_WinObservers()
    {
        foreach (IWinObserver observer in winObservers.ToArray())
        {
            if (winObservers.Contains(observer))
                observer.WinScenario();
        }
    }
    #endregion

    #region Lose Observer
    public void Add_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Add(observer);
    }

    public void Remove_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Remove(observer);
    }

    public void Notify_LoseObservers()
    {
        foreach (ILoseObserver observer in loseObservers.ToArray())
        {
            if (loseObservers.Contains(observer))
                observer.LoseScenario();
        }
    }
    #endregion
    #endregion
}
