using UnityEngine;

public class HUDController : MonoBehaviour {

    CanvasGroup canvasGroup;   

    void Start() {

        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update() {

        if (RaceStarter.raceStart) {

            canvasGroup.alpha = 1f;
        }

  
    }
}
