using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBoard : MonoBehaviour
{
    public Text[] placesText;

    private void Start()
    {
        Leaderboard.Reset();
    }
    private void LateUpdate()
    {
        List<string> places = Leaderboard.GetPlaces();
        placesText[0].text = places[0];
        placesText[1].text = places[1];
        placesText[2].text = places[2];
        placesText[3].text = places[3];
    }
}
