using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Networkplayer : MonoBehaviourPunCallbacks
{

    public static GameObject LocalPlayerInstance;
    public GameObject playerNamePrefab;
    public Rigidbody rb;
    public Renderer kartMesh;


    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
            if (GetComponent<PlayerController>())
                GetComponent<PlayerController>().isMine = true;
        }
        else
        {
            if (GetComponent<PlayerController>())
                GetComponent<PlayerController>().isMine = false;

            GameObject playerName = Instantiate(playerNamePrefab);
            playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;
            string sentName = null;
            if (photonView.InstantiationData != null)
                sentName = (string)photonView.InstantiationData[0];

            if (sentName != null)
                playerName.GetComponent<Text>().text = sentName;
            else
                playerName.GetComponent<Text>().text = photonView.Owner.NickName;

            playerName.GetComponent<NameUIController>().carRenderer = kartMesh;
        }
    }

}
