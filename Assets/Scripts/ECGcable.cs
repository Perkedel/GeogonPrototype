using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECGcable : MonoBehaviour {

    public SHanpe target;
    [Range(0, 100)] public float HP;
    public Slider healthBar;
    public Image HPfillColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HP = getPlayerHP();
        healthBar.value = getPlayerHP();

        if(HP >= 100)
        {
            HPfillColor.color = Color.blue;
        } else if(HP < 100 && HP >= 50)
        {
            HPfillColor.color = Color.green;
        } else if(HP < 50 && HP >= 25)
        {
            HPfillColor.color = Color.yellow;
        } else if(HP < 25 && HP > 0)
        {
            HPfillColor.color = Color.red;
        } else if(HP <= 0)
        {
            HPfillColor.color = Color.grey;
        }
	}

    public float getPlayerHP()
    {
        return target.HealthPoint;
    }
}
