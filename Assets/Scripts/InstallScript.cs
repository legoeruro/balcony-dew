using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallScript : MonoBehaviour
{
    public GameObject mblock;
    public GameObject wframe;
    public GameObject canvas;

    private dragger drag;
    private hover hov;
    

    private float animTime = 0;
    private int anim = 20;



    private Vector3 defScale = new Vector3(0.67f, 0.5f, 1);

    private string state = "wemp";
    //Go1234

    //drill & nail
    private GameObject[] nails = new GameObject[3];
    private GameObject[] subBoard = new GameObject[3];
    private Vector3[] nailDrillCoord = new Vector3[3];
    private int nailCount = 0;

    //wait
    private float lastTime = 0;

    //dirt filling
    private bool dirty = false;
    public GameObject bag;
    private BoxCollider2D bagcol;
    private BoxCollider2D botlcol;

    //bottle placement
    private GameObject[] botl = new GameObject[6];
    private BoxCollider[] bottelsPlacement = new BoxCollider[6];
    int bottleCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        drag = mblock.GetComponent<dragger>();
        hov = wframe.GetComponent<hover>();
        bagcol = bag.GetComponent<BoxCollider2D>();
        botlcol = wframe.GetComponent<BoxCollider2D>();
        canvas.SetActive(false);
        for (int i = 0; i<6; ++i)
        {
            print("didthis " + i);
            bottelsPlacement[i] = wframe.transform.GetChild(i).gameObject.GetComponent<BoxCollider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!drag.isDown) 
        {
            switch (state)
            {
                case "Go1":
                    if (mblock.transform.position.x >= -1.06 &&
                        mblock.transform.position.x <= -0.5 &&
                        mblock.transform.position.y >= -1 &&
                        mblock.transform.position.y <= -0.1)
                    {
                        state = "Go2";
                        loadState(Resources.Load<Sprite>(state), defScale);
                    }

                    break;

                case "Go2":
                    if (mblock.transform.position.x >= 0.25 &&
                        mblock.transform.position.x <= 1 &&
                        mblock.transform.position.y >= -1 &&
                        mblock.transform.position.y <= -0.1)
                    {
                        state = "Go3";
                        loadState(Resources.Load<Sprite>(state), defScale);
                    }

                    break;

                case "Go3":
                    if (mblock.transform.position.x >= 2.25 &&
                        mblock.transform.position.x <= 2.75 &&
                        mblock.transform.position.y >= -1 &&
                        mblock.transform.position.y <= -0.1)
                    {
                        state = "Go4";
                        loadState(Resources.Load<Sprite>(state), defScale);
                    }

                    break;

                case "Go4":
                    if (mblock.transform.position.x >= 4.11 &&
                        mblock.transform.position.x <= 4.61 &&
                        mblock.transform.position.y >= -1 &&
                        mblock.transform.position.y <= -0.1)
                    {
                        loadState(Resources.Load<Sprite>("Khung"), new Vector3(0.67f, 0.67f, 1));
                        state = "driu";
                    }
                    break;

                case "driu":
                    mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Drill_Animation_");
                    loadState(Resources.Load<Sprite>("Wool_Hori_90"), new Vector3(0.67f, 0.67f, 1));
                    wframe.transform.position = new Vector3(-4.39f, -0.88f);
                    wframe.transform.localScale = new Vector3(0.6f, 0.6f, 1);


                    nails[0] = Instantiate(wframe);
                    nails[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Nail");
                    subBoard[0] = Instantiate(wframe);
                    subBoard[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Wool_Hori_90");
                    makeNewSboards(subBoard[0], 2.06f);
                    makeNewNails(nails[0], 3.03f);
                    nailDrillCoord[0] = new Vector3(-1.86f, 0.98f);


                    nails[1] = Instantiate(nails[0]);
                    subBoard[1] = Instantiate(wframe);
                    subBoard[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Wool_Hori_90");
                    makeNewSboards(subBoard[1], -0.12f);
                    makeNewNails(nails[1], 0.77f);
                    nailDrillCoord[1] = new Vector3(-1.86f, -1.27f);

                    nails[2] = Instantiate(nails[0]);
                    subBoard[2] = Instantiate(wframe);
                    subBoard[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Wool_Hori_90");
                    makeNewSboards(subBoard[0], -2.39f);
                    makeNewNails(nails[2], -1.5f);
                    nailDrillCoord[2] = new Vector3(-1.86f, -3.54f);

                    state = "drili";
                    break;
                case "drili":
                    Destroy(subBoard[2]);
                    for (int i = 0; i<3; ++i)
                    {
                        if (checkRange(mblock.transform.position, nailDrillCoord[i]))
                        {
                            float temp = nails[i].transform.position.y;
                            nails[i].transform.position = new Vector3(-6.62f, temp);
                            temp = mblock.transform.position.y;
                            mblock.transform.position = new Vector3(-2.90f, temp);
                            nailCount++;

                        }
                    }
                    if (nailCount >= 3)
                    {
                        state = "wremove";
                        for (int i = 0; i < 3; ++i)
                        {
                            Destroy(nails[i]);
                            Destroy(subBoard[i]);
                        }

                        mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Water_Bottle");
                        wframe.GetComponent<SpriteRenderer>().sprite = null;
                        mblock.transform.position = new Vector3(0, 0, 0);
                        mblock.transform.eulerAngles = new Vector3(0, 0, 0);
                        mblock.transform.localScale = new Vector3(1, 1, 1);
                        drag = mblock.GetComponent<dragger>();

                        drag.setDraggable(false);
                    }
                    break;

                case "wremove":

                    checkMizu();
                    if (drag.getState())
                    {
                        print("mizu");
                        mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Empty_Bottle");
                        state = "wait";
                        lastTime = Time.time;
                    }

                    break;

                case "wait":
                    float dt = Time.time - lastTime;
                    if (dt >= 3) state = "wemp";
                    break;
                case "wemp":
                    mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Knife");
                    mblock.transform.localScale = new Vector3(1.7f, 1.27f, 2.54f);
                    loadState(Resources.Load<Sprite>("CutBottle"), new Vector3(1, 1, 1));
                    wframe.transform.position = new Vector3(0, 0, 0);
                    wframe.transform.localScale = new Vector3(1, 1, 1);
                    drag.setDraggable(true);
                    state = "wcut";
                    break;

                case "wcut":
                    if (checkRange(mblock.transform.position, new Vector3(3.8f, 1.74f, 0)))
                    {
                        mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Trovel");
                        loadState(Resources.Load<Sprite>("cutmtbot"), new Vector3(1, 1, 1));
                        wframe.transform.position = new Vector3(-6, 0, 0);
                        wframe.transform.eulerAngles = new Vector3(0, 0, 90);
                        wframe.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
                        bag.transform.position = new Vector3(5.22f, -2.25f, 0);
                        state = "wdirt";
                    }
                    break;

                case "wdirt":
                    if (!dirty && bagcol.bounds.Contains(mblock.transform.position))
                    {
                        dirty = true;
                        mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("DirtTrovel");
                    }
                    if (dirty && botlcol.bounds.Contains(mblock.transform.position))
                    {
                        bag.GetComponent<SpriteRenderer>().sprite = null;
                        wframe.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Treo1");
                        mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dirtbt");
                        mblock.GetComponent<BoxCollider2D>().offset = new Vector2(0.1426861f, -0.3920171f);
                        mblock.GetComponent<BoxCollider2D>().size = new Vector2(2.750995f, 3.540207f);
                        wframe.transform.position = new Vector3(-0.74f, -0.14f);
                        wframe.transform.eulerAngles = new Vector3(0, 0, 0);
                        state = "battlepass";
                        int count = 0;
                        for(int i = 0; i<2; ++i)
                        {
                            for(int j = 0; j<3; ++j)
                            {
                                GameObject temp = Instantiate(mblock);
                                temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dirtbt");
                                temp.transform.position = new Vector3(j + 5.94f, i * 2 - 1.44f);
                                temp.transform.eulerAngles = new Vector3(0, 0, 0);
                                temp.transform.localScale = new Vector3(1, 1, 1);
                                botl[count] = temp;
                                count++;
                            }
                        }

                        state = "battlepass";
                        Destroy(mblock);
                    }
                    break;

                case "battlepass":
                    print(bottleCount);
                    for(int i = 0; i<6; ++i)
                    {
                        if (botl[i] != null &&
                            bottelsPlacement[bottleCount].bounds.Contains(botl[i].transform.position))
                        {
                            Destroy(botl[i]);
                            botl[i] = null;
                            ++bottleCount;
                            wframe.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Treo" + (bottleCount + 1));
                        }
                    }
                    if (bottleCount == 6)
                    {
                        state = "good2";
                        wframe.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Treo1");
                        mblock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dirtbt");
                    }
                    break;

                case "good2":
                    canvas.SetActive(true);
                    GameObject text2 = canvas.transform.GetChild(3).gameObject;
                    text2.GetComponent<TMPro.TextMeshProUGUI>().text = "<s>" + text2.GetComponent<TMPro.TextMeshProUGUI>().text;
                    state = "wait2";
                    break;

                case "wait2":
                    //dt = Time.time - lastTime;
                    //if (dt >= 3) state = "wemp";
                    break;
            }
        }
  
    }

    void checkMizu()
    {

    }

    bool checkRange(Vector3 pos, Vector3 range)
    {
        if (range.x - 0.5 <= pos.x &&
            pos.x <= range.x + 0.5 &&
            range.y - 0.5 <= pos.y &&
            pos.y <= range.y + 0.5) return true;
        return false;
    }

    void makeNewNails(GameObject nail, float yCoord)
    {
        nail.transform.position = new Vector3(-4.89f, yCoord);
        nail.transform.eulerAngles = new Vector3(0, 0, 172);
        nail.transform.localScale = new Vector3(0.5424f, 0.5424f, 0.5424f);
            
    }

    void makeNewSboards(GameObject obj, float yCoord)
    {
        obj.transform.position = new Vector3(-5.79f, yCoord);
        obj.transform.eulerAngles = new Vector3(0, 0, 3.24f);
        obj.transform.localScale = new Vector3(0.39f, 0.14f, 0.5424f);

    }

    void loadState(Sprite frameNextSprite, Vector3 blockScale)
    {
        wframe.GetComponent<SpriteRenderer>().sprite = frameNextSprite;

        mblock.transform.position = new Vector3(13.49f, 0.05f);
        mblock.transform.eulerAngles = new Vector3(0, 0, -6.92f);
        mblock.transform.localScale = blockScale;

        GameObject newblock = Instantiate(mblock);
        Destroy(mblock);
        mblock = newblock;
        drag = mblock.GetComponent<dragger>();
    }



    void mbPressed()
    {

    }


}
