using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NungguMangsa : MonoBehaviour
{
    public SHanpe target;
    public GameObject colliding;
    public bool AlreadyHitGroundsSo = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Prey button is called when trigger is touched
    public void TombolMangsa()
    {
        GetComponent<FixedJoint2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = collision.gameObject;
        if(colliding) target = colliding.transform.parent.GetComponent<SHanpe>();
        if (!AlreadyHitGroundsSo)
        {
            if(target.gameObject == collision.transform.parent.gameObject)
            {
                target.damageMe(1000000000000000000000000f);
            }
            AlreadyHitGroundsSo = true;
        }
    }
}
