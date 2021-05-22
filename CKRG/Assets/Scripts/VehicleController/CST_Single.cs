using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CST_Single : MonoBehaviour
{
    public Texture[] colors;
    public Texture[] logos;
    public MeshRenderer kartMesh;
    public GameObject partObj;

    public MeshRenderer[] logoMesh;
    public bool isMine = false;
    // Start is called before the first frame update

    void Start()
    {
        if(isMine)
        {
            if (PlayerPrefs.HasKey("SelectedColor"))
            {
                int color = PlayerPrefs.GetInt("SelectedColor");
                kartMesh.material.SetTexture("_MainTex", colors[color]);
            }
            else
            {                
                kartMesh.material.SetTexture("_MainTex", colors[0]);                
            }

            if (PlayerPrefs.HasKey("SelectedLogo"))
            {
                int logo = PlayerPrefs.GetInt("SelectedLogo");
                if (logo == 10)
                    partObj.SetActive(false);
                else
                {
                    partObj.SetActive(true);
                    for (int i = 0; i < logoMesh.Length; i++)
                    {
                        logoMesh[i].material.SetTexture("_MainTex", logos[logo]);
                    }
                }
            }           
        }
        else
        {
            int randomColor = Random.Range(0, colors.Length);
            kartMesh.material.SetTexture("_MainTex", colors[randomColor]);

            partObj.SetActive(true);
            int randomLogo = Random.Range(0, logos.Length);
            for (int i = 0; i < logoMesh.Length; i++)
            {
                logoMesh[i].material.SetTexture("_MainTex", logos[randomLogo]);
            }
        }
    }

 
}
