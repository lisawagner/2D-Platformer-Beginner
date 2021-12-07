using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class HealthBar : MonoBehaviour
{
    private RectTransform bar;
    private Image barImage;

    void Start()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
        if (Health.totalHealth < 0.3f)
        {
            barImage.color = Color.red;
        }
        SetSize(Health.totalHealth);

    }

    ///// ABSTRACTION - Health and damage functionality available
    /////               to other levels and objects
    public void Damage(float damage)
    {
        if((Health.totalHealth -= damage) >= 0f)
        {
            Health.totalHealth -= damage;
        }
        else
        {
            Health.totalHealth = 0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Health.totalHealth < 0.3f)
        {
            barImage.color = Color.red;
        }

        SetSize(Health.totalHealth);

    }

    public void SetSize(float size)
    {
        bar.localScale = new Vector3(size, 1f);
    }
}
