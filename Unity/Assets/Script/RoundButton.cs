using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RoundButton : MonoBehaviour
{
    public Toggle toggleButton;
    public GameObject unit;
    bool isEnable;

    public Material defaultMaterial;
    public Material blackMaterial;
    public Text PlayerRoundText;
    public void ButtonEnable()
    {
        isEnable = false;
        switch (gameObject.name)
        {
            case "PlayerRound":
                isEnable = true;
                if (Main.main.userInfo.LastPlayRound == 1) PlayerRoundText.text = "Play Round 1";
                if (Main.main.userInfo.LastPlayRound == 2) PlayerRoundText.text = "Play Round 2";
                if (Main.main.userInfo.LastPlayRound == 3) PlayerRoundText.text = "Play Round 3";
                if (Main.main.userInfo.LastPlayRound == 4) PlayerRoundText.text = "Play Round 4";
                if (Main.main.userInfo.LastPlayRound == 5) PlayerRoundText.text = "Play Round 5";
                if (Main.main.userInfo.LastPlayRound == 6) PlayerRoundText.text = "Play Round 6";
                if (Main.main.userInfo.LastPlayRound == 7) PlayerRoundText.text = "Play Round 7";
                return;
            case "Round_7":
                if (Main.main.userInfo.TopPlayRound >= 7)
                    isEnable = true;
                break;
            case "Round_6":
                if (Main.main.userInfo.TopPlayRound >= 6)
                    isEnable = true;
                break;
            case "Round_5":
                if (Main.main.userInfo.TopPlayRound >= 5)
                    isEnable = true;
                break;
            case "Round_4":
                if (Main.main.userInfo.TopPlayRound >= 4)
                    isEnable = true;
                break;
            case "Round_3":
                if (Main.main.userInfo.TopPlayRound >= 3)
                    isEnable = true;
                break;
            case "Round_2":
                if (Main.main.userInfo.TopPlayRound >= 2)
                    isEnable = true;
                break;
            case "Round_1":
                if (Main.main.userInfo.TopPlayRound >= 1)
                    isEnable = true;
                break;
        }

        toggleButton.GetComponent<Toggle>().enabled = isEnable;

        if(!isEnable)
        {
            toggleButton.isOn = false;
            unit.GetComponent<Renderer>().material = blackMaterial;
        }
        else
        {
            toggleButton.isOn = true;
            unit.GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}
