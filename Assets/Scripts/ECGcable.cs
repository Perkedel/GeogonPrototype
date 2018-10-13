using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECGcable : MonoBehaviour {

    public SHanpe target;
    [Range(0, 100)] public float HP;
    public Slider healthBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HP = getPlayerHP();
        healthBar.value = getPlayerHP()/100f;
	}

    public float getPlayerHP()
    {
        return target.HealthPoint;
    }
}
