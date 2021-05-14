using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUIController : MonoBehaviour
{
    public Text playerName;
    public Text lapText;
    public Transform target;
    CanvasGroup canvasGroup;
    public Renderer carRenderer;
    CheckpointManager cpManager;

    int carReg;

    // Start is called before the first frame update
    void Start()
    {

        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        playerName = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        carReg = Leaderboard.RegisterCar(playerName.text);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (carRenderer == null) return;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool carInView = GeometryUtility.TestPlanesAABB(planes, carRenderer.bounds);
        canvasGroup.alpha = carInView ? 1 : 0;
        transform.position = Camera.main.WorldToScreenPoint(target.position+Vector3.up*1.2f);

        if (cpManager == null)
            cpManager = target.GetComponent<CheckpointManager>();

        Leaderboard.SetPosition(carReg, cpManager.lap, cpManager.checkPoint,cpManager.timeEntered);
        string position = Leaderboard.GetPosition(carReg);


        lapText.text = position;
    }
}
