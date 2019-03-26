using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PremiumMoneyTracker : MonoBehaviour
{
    private static Text premText;

    private static int premcur;

	// Use this for initialization
	void Start ()
    {
        premText = GetComponent<Text>();

        premcur = 0;

        premText.text = "" + premcur;
	}

    public static void ChangeMoneyCount(int amount)
    {
        premcur += amount;
        premText.text = "" + premcur;
    }

}
