using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventorySortingScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        
	}

    public static void SortAZ(List<GameObject> toSort)
    {
        List<GameObject> sorted = toSort.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
    }

    public static void SortLowHigh(List<GameObject> toSort)
    {
        
    }

    public static void SortHighLow(List<GameObject> toSort)
    {
        
    }

    public static void SortType(List<GameObject> toSort)
    {
        if (toSort.Count != 0) {
            toSort[0].GetComponent<Decoration>();
        }
    }
}
