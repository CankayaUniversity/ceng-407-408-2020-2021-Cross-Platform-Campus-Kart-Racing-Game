using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RaceStarter : MonoBehaviourPunCallbacks
{
    public GameObject[] countDownItems;
    CheckpointManager[] carsCPM;
    public static bool raceStart = false;
    public static int totalLaps = 1;

    public GameObject kartPrefabMulti;
    public GameObject kartPrefabSingle;
    public GameObject kartPrefabAI;
    public GameObject kartPrefabAIMulti;
    public Transform[] spawnPos;

    public GameObject gameOverPanel;
    public GameObject HUD;
    public GameObject startButton;

    public GameObject waitingText;

    private void Start()
    {
        foreach (GameObject item in countDownItems)
        {
            item.SetActive(false);
        }
        startButton.SetActive(false);
        waitingText.SetActive(false);       
        gameOverPanel.SetActive(false);

        int randomStartPos = Random.Range(0, spawnPos.Length);
        Vector3 startPos = spawnPos[randomStartPos].position;
        Quaternion startRot = spawnPos[randomStartPos].rotation;

        GameObject pcar = null;

        if (PhotonNetwork.IsConnected)
        {
            startPos = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
            startRot = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1].rotation;

            if (Networkplayer.LocalPlayerInstance == null)
            {
                pcar = PhotonNetwork.Instantiate(kartPrefabMulti.name, startPos, startRot, 0);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            else
            {
                waitingText.SetActive(true);
            }
        }
        else
        {
            pcar = Instantiate(kartPrefabSingle, startPos, startRot);

            foreach (Transform item in spawnPos)
            {
                if (item == spawnPos[randomStartPos]) continue;
                GameObject car = Instantiate(kartPrefabAI);
                car.transform.position = item.position;
                car.transform.rotation = item.rotation;
            }
            StartGame();
        }

        SmoothFollow.playerKart = pcar.gameObject.GetComponent<KartController>().rb.transform;
        pcar.GetComponent<KartController>().enabled = true;
        pcar.GetComponent<PlayerController>().enabled = true;
       
    }

    public void BeginGame()
    {
        string[] aiNames = { "Buğra", "Okan", "Cem", "Esra", "Murat", "Ahmet", "Mehmet", "Veli" };
        int numAIPlayer = PhotonNetwork.CurrentRoom.MaxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;

        for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < PhotonNetwork.CurrentRoom.MaxPlayers; ++i)
        {
            Vector3 startPos = spawnPos[i].position;
            Quaternion startRot = spawnPos[i].rotation;

            object[] instanceData = new object[1];
            instanceData[0] = (string)aiNames[Random.Range(0, aiNames.Length)];


            GameObject AIcar = PhotonNetwork.Instantiate(kartPrefabAIMulti.name, startPos, startRot, 0);
            AIcar.GetComponent<AIController>().enabled = true;
            AIcar.GetComponent<KartController>().networkName = (string)instanceData[0];
            AIcar.GetComponent<KartController>().enabled = true;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.All, null);
        }
    }
    [PunRPC]
    public void StartGame()
    {
        StartCoroutine(PlayCountDown());
        startButton.SetActive(false);
        waitingText.SetActive(false);
       
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
        if (!raceStart) return;

        int finishedCount = 0;

        foreach (CheckpointManager cpm in carsCPM)
        {
            if (cpm.lap == totalLaps + 1)
                finishedCount++;

        }
        if (finishedCount == carsCPM.Length)
        {
            HUD.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void RestartLevel()
    {
        raceStart = false;
        if (PhotonNetwork.IsConnected)
            photonView.RPC("RestartGame", RpcTarget.All, null);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
