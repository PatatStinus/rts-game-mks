using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropdown : MonoBehaviour
{
    public Animator panelDropdown;
    private bool panelOpen = false;

    private bool buttonActive = true;
    public void panelButton()
    {
        if (buttonActive)
        {
            Invoke("ButtonInactive", 1f);
            buttonActive = false;
            if (panelOpen)
            {
                panelDropdown.Play("closePanel");
                panelOpen = false;
            }
            else
            {
                panelDropdown.Play("openPanel");
                panelOpen = true;
            }
        }
    }
    private void ButtonInactive()
    {
        buttonActive = true;
    }
}
