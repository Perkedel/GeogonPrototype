using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0169
//https://stackoverflow.com/questions/3820985/suppressing-is-never-used-and-is-never-assigned-to-warnings-in-c-sharp
//suppress warning "is never used"!

#pragma warning disable 0414
//https://answers.unity.com/questions/367829/c-variable-is-assigned-but-its-value-is-never-used.html
//suppress also for Unity engine
//but this is not recomended!

//excercise file. this coding is dirty. sorry for inconveniences

public class HeroPlayer_SHanpe : MonoBehaviour {

#pragma warning disable 0169
#pragma warning disable 0414

    private enum Shaping{Circle, Line, Triangle, Square, Pentagon, Hexagon, Septagon, Octagon};
    [SerializeField] Shaping skin;
    

    public Sprite Circle;
    public Sprite Line;
    public Sprite Triangle;
    public Sprite Square;
    public Sprite Pentagon;
    public Sprite Hexagon;
    public Sprite Septagon;
    public Sprite Octagon;
    public Sprite Nonagon;
    public Sprite Dekagon;
    public Sprite MonokaiDekagon;
    public Sprite DikaiDekagon;
    public Sprite TrikaiDekagon;
    public Sprite TetrakaiDekagon;

    public float speed = 10.0f;
    public float roll = 10.0f;
    public float gravity = 10.0f;

    private GameObject thisColRigid;
    [SerializeField] private GameObject PentaColRigid;
    private Rigidbody2D newRb;
    private Rigidbody2D prevRb;
    

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer drawing;
    [SerializeField] private CircleCollider2D cirCollide;
    [SerializeField] private BoxCollider2D boxCollide;
    [SerializeField] private PolygonCollider2D polCollide;
    //[SerializeField] private PolygonCollider2D PentaCollide;
    [SerializeField] private PolygonCollider2D customCollide;
    [SerializeField] private Collider2D jstCollide;

    // Use this for initialization
    void Start () {
#pragma warning disable 0169
#pragma warning disable 0414
        rb = GetComponent<Rigidbody2D>();
        prevRb = rb;
        newRb = PentaColRigid.GetComponent<Rigidbody2D>();
        PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
        drawing = GetComponent<SpriteRenderer>();

        //colliders
        cirCollide = GetComponent<CircleCollider2D>();
        boxCollide = GetComponent<BoxCollider2D>();
        polCollide = GetComponent<PolygonCollider2D>();
        //PentaCollide = GetComponent<PolygonCollider2D>();

        //Custom Game Object
        thisColRigid = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
#pragma warning disable 0169
#pragma warning disable 0414
        float translation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float translotion = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float pushroller = Input.GetAxis("Horizontal") * roll;
        float pushriller = Input.GetAxis("Vertical") * roll;


        float transgravity = -1 * gravity * Time.deltaTime;

        /*if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                //transform.Rotate(0, 0, -(Input.GetAxis("Horizontal")) * roll * Time.deltaTime);
                transform.Translate(translation, 0, 0, Space.World);
            } else if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                //transform.Rotate(0, 0, -(Input.GetAxis("Horizontal")) * roll * Time.deltaTime);
                transform.Translate(translation, 0, 0, Space.World);
            }
        }*/
        //transform.Rotate(0, 0, -(Input.GetAxis("Horizontal")) * roll * Time.deltaTime);
        //transform.Translate(translation, 0, 0, Space.World);
        rb.AddTorque(pushroller * -1);

        if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Alpha1))
        {
            skin--;
        }
        if(Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Alpha3))
        {
            skin++;
        }

        switch (skin)
        {
            case Shaping.Circle:
                cirCollide.enabled = true;
                boxCollide.enabled = false;
                polCollide.enabled = false;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Circle;
                break;
            case Shaping.Triangle:
                cirCollide.enabled = false;
                boxCollide.enabled = false;
                polCollide.enabled = true;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Triangle;
                break;
            case Shaping.Square:
                cirCollide.enabled = false;
                boxCollide.enabled = true;
                polCollide.enabled = false;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Square;
                break;
            case Shaping.Pentagon:
                cirCollide.enabled = false;
                boxCollide.enabled = false;
                polCollide.enabled = false;
                rb = newRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = true;
                //above method works! use external rigidbody and collider!
                //PentaCollide.enabled = true;

                drawing.sprite = Pentagon;
                break;
            case Shaping.Hexagon:
                cirCollide.enabled = true;
                boxCollide.enabled = false;
                polCollide.enabled = false;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Hexagon;
                break;
            case Shaping.Septagon:
                cirCollide.enabled = true;
                boxCollide.enabled = false;
                polCollide.enabled = false;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Septagon;
                break;
            case Shaping.Octagon:
                cirCollide.enabled = true;
                boxCollide.enabled = false;
                polCollide.enabled = false;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Octagon;
                break;
            default:
                cirCollide.enabled = true;
                boxCollide.enabled = false;
                polCollide.enabled = false;
                rb = prevRb;
                PentaColRigid.GetComponent<PolygonCollider2D>().enabled = false;
                //PentaCollide.enabled = false;
                drawing.sprite = Circle;
                break;

                //Tiring!!!
        }
    }
}

//#pragma warning restore 0169
