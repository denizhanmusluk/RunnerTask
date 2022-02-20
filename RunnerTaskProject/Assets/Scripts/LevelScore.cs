using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI progressLevelText, successLvlText, failLvlText;
    public Slider progressBar;
    [SerializeField] GameObject player, finishPosition;
    float maxDistance;
    float currentDistance;
    private void Start()
    {
        int lvl = PlayerPrefs.GetInt("level") + 1;
        progressLevelText.text = lvl.ToString();
        successLvlText.text = "Level " + lvl.ToString(); ;
        failLvlText.text = "Level " + lvl.ToString(); ;
        currentDistance = 0;
        maxDistance = finishPosition.transform.position.z - player.transform.position.z;
    }
    void Update()
    {
        currentDistance = 1 - (finishPosition.transform.position.z - player.transform.position.z) / maxDistance;
        //progressBar.fillAmount = currentDistance;
        progressBar.value = currentDistance;
    }
}
