using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHover : Health
{
    public float y_offset = 2f;
    public Vector3 scale;
    public GameObject healthBarUIPrefab;
    public bool displayAlways = true;

    private GameObject healthBarUI;
    private Slider slider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        healthBarUI = Instantiate(healthBarUIPrefab, this.transform.position + new Vector3(0, y_offset, 0), Quaternion.identity, this.transform);
        healthBarUI.transform.localScale = scale;
        //healthBarUI.transform.position = this.transform.position + offset.position;
        if(displayAlways)
            healthBarUI.SetActive(true);
        else
            healthBarUI.SetActive(false);
        
        slider = healthBarUI.GetComponentInChildren<Slider>();
        slider.value = health / maxHealth;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(health < maxHealth)
            healthBarUI.SetActive(true);

        slider.value = health / maxHealth;
    }
}
