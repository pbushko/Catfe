using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    // In the prefab
    //public GameObject foreground;
    //public GameObject background;
    public SpriteRenderer foodRenderer;

    //private Image m_foregroundImage;
    private Sprite m_food;

    private bool m_finished;
    private bool m_processing;

    private Recipe m_recipe;
    private float m_fill;
    private float m_time;

	// Use this for initialization
	void Start ()
    {
        //foreground.SetActive(false);
        //background.SetActive(false);

        //m_foregroundImage = foreground.GetComponent<Image>();
        m_finished = false;
        m_processing = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_processing)
        {
            if (m_fill <= 1f)
            {
                m_fill += 0.01f/m_time;
            }
            else
            {
                m_processing = false;
                //foreground.SetActive(false);
                //background.SetActive(false);
                m_finished = true;
                
                foodRenderer.sprite = m_food;
            }
            //m_foregroundImage.fillAmount = m_fill;
        }
	}

    public void Loading(float fillTime, Sprite f, Recipe r)
    {
        if (!m_finished && !m_processing)
        {
            m_food = f;
            m_recipe = r;
            m_processing = true;
            //foreground.SetActive(true);
           // background.SetActive(true);
            m_time = fillTime;
            Reset();
        }
    }

    //checks if there is a plate
    public bool HasPlate()
    {
        //Debug.Log(m_finished);
        return m_finished;
    }

    //takes the plate off of the utensil
    public Recipe PickUpPlate()
    {
        if (m_finished)
        {
            foodRenderer.sprite = PlayerScript.GetFoodSprite(null);
            m_finished = false;
            return m_recipe;
        }
        return null;
    }

    public void Reset()
    {
        m_fill = 0f;
        //m_foregroundImage.fillAmount = m_fill;
    }
}
