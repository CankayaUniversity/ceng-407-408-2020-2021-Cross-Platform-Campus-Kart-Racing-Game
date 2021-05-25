using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKRG_UIController : MonoBehaviour
{
    public GameObject mobileUI;
    public ControlMode controlMode;
    public PlayerController controller;
    public CKRG_MobileButton gasButton;
    public CKRG_MobileButton reverseButton;
    public CKRG_MobileButton leftButton;
    public CKRG_MobileButton rightButton;
    public CKRG_MobileButton brakeButton;

    private float reverseInput = 0f;
    private float brakeInput = 0f;
    private float gasInput = 0f;
    private float leftInput = 0f;
    private float rightInput = 0f;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        mobileUI.SetActive(false);
#else
    mobileUI.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller)
            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gasInput = GetInput(gasButton);
        reverseInput = GetInput(reverseButton);
        leftInput = GetInput(leftButton);
        rightInput = GetInput(rightButton);
        brakeInput = GetInput(brakeButton);
        if(controller.isMine)
        {
            controller.horizontal = -leftInput + rightInput;
            controller.vertical = -reverseInput + gasInput;
            controller.brake = brakeInput;
        }
      
    }

    float GetInput(CKRG_MobileButton button)
    {
        if (button == null)
            return 0f;

        return (button.input);

    }
}
