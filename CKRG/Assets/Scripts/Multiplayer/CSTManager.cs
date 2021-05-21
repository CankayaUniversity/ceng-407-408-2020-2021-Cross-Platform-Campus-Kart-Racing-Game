using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class CSTManager : MonoBehaviourPunCallbacks
{
    public Texture[] colors;
    public Texture[] logos;
    public MeshRenderer kartMesh;
    public GameObject partObj;

    public MeshRenderer[] logoMesh;
    // Start is called before the first frame update
    void Start()
    {

        if (photonView.Owner.CustomProperties.ContainsKey("Color"))
        {
            int color = (int)photonView.Owner.CustomProperties["Color"];
            kartMesh.material.SetTexture("_MainTex", colors[color]);
        }

        if (photonView.Owner.CustomProperties.ContainsKey("Part"))
        {
            int part = (int)photonView.Owner.CustomProperties["Part"];
            if (part == 10)
                partObj.SetActive(false);
            else
            {
                partObj.SetActive(true);
                for (int i = 0; i < logoMesh.Length; i++)
                {
                    logoMesh[i].material.SetTexture("_MainTex", logos[part]);
                }
            }
           
            
        }


    }


}
