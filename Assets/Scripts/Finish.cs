using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;


public class Finish : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camFin;
    protected Rigidbody[] childrenClothes;
    public int scoreMagnitude = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    public Image progressBar;
    public GameObject finScoreBar;
    public GameObject star1,star2,star3;
    public GameObject star1Particle, star2Particle, star3Particle;
    public GameObject nextButton;
    public int maXScore;
    public static Finish Instance;
    GameObject player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        finScoreBar.SetActive(false);
        //cam = GameObject.Find("CameraMain").GetComponent<CinemachineVirtualCamera>();
    }
    public void diamondCount()
    {
        Globals.maxScore = maXScore;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "followobject")
    //    {
    //        Globals.isGameActive = false;
    //        other.GetComponentInParent<PathFollow>().speed = 120;
    //        //cam.Follow = other.gameObject.transform;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageble>() != null)
        {
            this.GetComponent<Collider>().enabled = false;
            Destroy(this.GetComponent<Rigidbody>());
            other.GetComponent<IDamageble>().finish();
            StartCoroutine(checkFinish());
            player = other.gameObject;
        }
    }

    IEnumerator checkFinish()
    {
        bool checkActive = true;
        int preScoreMagnitude = -1;
        while (checkActive)
        {

            if(preScoreMagnitude == scoreMagnitude)
            {
                checkActive = false;
                player.GetComponent<Player>().FlyEffect.SetActive(false);
                finScoreBar.SetActive(true);
                GameManager.Instance.Notify_WinObservers();
                camFin.Priority = 20;
                StartCoroutine(scoreBar());
                StartCoroutine(scoreMult());
            }
            preScoreMagnitude = scoreMagnitude;
            yield return new WaitForSeconds(2);
        }
        //GameManager.Instance.Notify_WinObservers();
        //finScoreBar.SetActive(true);
        
    }
    IEnumerator scoreBar()
    {
        float currentBar = 0;
        int score = Globals.score;
        bool finActive = true;
        //int newScore = scoreMagnitude * score;
        while (currentBar <= score && finActive)
        {
            currentBar += 30 * Time.deltaTime;
            float scoreRatio = (float)currentBar / (float)Globals.maxScore;
           
            progressBar.fillAmount = scoreRatio;

            if (scoreRatio >= 0.95f)
            {
                finActive = false;
                star1.SetActive(false);
                star2.SetActive(false);
                star3.SetActive(false);
                star3Particle.SetActive(true);
            }
            else if (scoreRatio >= 0.75f)
            {
                star1.SetActive(false);
                star2.SetActive(false);
                star2Particle.SetActive(true);

            }
            else if (scoreRatio >= 0.5f)
            {
                star1.SetActive(false);
                star1Particle.SetActive(true);
            }
            yield return null;
        }
        nextButton.SetActive(true);
    }
    IEnumerator scoreMult()
    {
        int score = Globals.score;
        int newScore = scoreMagnitude * score;
        int sign = -1;
        int magn = -1;

        while (score <= newScore)
        {
            magn = sign * magn;
            score += scoreMagnitude;
            Globals.totalCost += scoreMagnitude;

            scoreText.text = Globals.totalCost.ToString();
            scoreText.rectTransform.localScale = new Vector3(1 + magn / 2, 1 + magn / 2, 1);
            yield return null;
        }

        PlayerPrefs.SetInt("cost", Globals.totalCost);
    }
}
