using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceStarter : MonoBehaviour
{
    public GameObject[] countDownItems;
    CheckpointManager[] carsCPM;
    public static bool raceStart = false;
    public static int totalLaps = 1;

    public GameObject gameOverPanel;
    public GameObject HUD;

    private void Awake()
    {
        foreach (GameObject item in countDownItems)
        {
            item.SetActive(false);
        }
        StartCoroutine(PlayCountDown());

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        carsCPM = new CheckpointManager[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            carsCPM[i] = cars[i].GetComponent<CheckpointManager>();
        }
    }

    IEnumerator PlayCountDown()
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject item in countDownItems)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(1);
            item.SetActive(false);
        }
        raceStart = true;
    }

    private void LateUpdate()
    {
        int finishedCount = 0;
        foreach (CheckpointManager cpm in carsCPM)
        {
            if (cpm.lap == totalLaps + 1)
                finishedCount++;

        }
        if(finishedCount == carsCPM.Length)
        {
            HUD.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }

    public void MenuButtons(int index)
    {
        if (index == 0)
        {
            raceStart = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
            SceneManager.LoadScene("Menu");
    }
}
