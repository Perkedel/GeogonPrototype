using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;

public class DelayButton : MonoBehaviour
{
    public float SetTimer = 10f;
    public string ButtonText = "Next";
    [SerializeField] private float SeeTimer;
    [SerializeField] [Range(0, 100)] private float PercentTimer;

    public Button RefferButton;
    //public TextMesh TheTextMesh;
    public TextMeshProUGUI TheTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        SeeTimer = SetTimer;
        RefferButton = GetComponent<Button>();
        //TheTextMesh = GetComponent<TextMeshProUGUI>();
        TheTextMesh.color = Color.gray;
        RefferButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        PercentTimer = (SeeTimer / SetTimer) * 100;

        SeeTimer -= Time.deltaTime;

        string ViewTimer;
        if (SeeTimer > 0f) ViewTimer = "(" + (Mathf.Ceil(SeeTimer)) + ")"; else ViewTimer = "";
        TheTextMesh.text = ButtonText + " " + ViewTimer;

        if (SeeTimer < 0f) {
            TheTextMesh.color = Color.white;
            RefferButton.interactable = true;
            SeeTimer = 0;
        }
    }
}
