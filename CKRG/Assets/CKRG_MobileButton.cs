using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CKRG_MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private Button button;

    internal float input;
    public bool pressing;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        pressing = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        pressing = false;

    }

    void OnPress(bool isPressed)
    {

        if (isPressed)
            pressing = true;
        else
            pressing = false;

    }

    void OnDisable()
    {

        input = 0f;
        pressing = false;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(input);
        if (pressing)
				input += Time.deltaTime * 3;
			else
				input -= Time.deltaTime * 5;
        if (input < 0f)
            input = 0f;

        if (input > 1f)
            input = 1f;
    }
}
