using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guti_16_Game : MonoBehaviour
{
    GameObject[] grid_board = new GameObject[37];

    public Sprite[] gutis = new Sprite[2];
    public GameObject mainParent;
    public GameObject parentObject;
    public GameObject bground;

    // Start is called before the first frame update
    void Start()
    {
        resizeScreen();

        string name = "16 Guti1_0";
        for (int i = 0; i < 18; i++)
        {
            string temp = name + " (" + i + ")";
            Transform childTransform = mainParent.transform.Find(temp);
            grid_board[i] = childTransform.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;
                SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = gutis[0];

            }
        }
    }

    void resizeScreen()
    {
        Renderer renderer = parentObject.GetComponent<Renderer>();
        Bounds bounds = renderer.bounds;
        Bounds bounds1 = bground.GetComponent<Renderer>().bounds;
        Vector3 sizeInScreenSpace = Camera.main.WorldToScreenPoint(bounds.size) - Camera.main.WorldToScreenPoint(Vector3.zero);
        Vector3 sizeInScreenSpace1 = Camera.main.WorldToScreenPoint(bounds1.size) - Camera.main.WorldToScreenPoint(Vector3.zero);
        float sizeInScreenUnitswidth = Screen.width / sizeInScreenSpace.x;
        float sizeInScreenUnitsheight = Screen.height / sizeInScreenSpace.y;
        float sizeInScreenUnitswidth1 = Screen.width / sizeInScreenSpace1.x;
        float sizeInScreenUnitsheight1 = Screen.height / sizeInScreenSpace1.y;

        if (sizeInScreenUnitswidth < sizeInScreenUnitsheight)
        {
            sizeInScreenUnitsheight = sizeInScreenUnitswidth;
        }
        else
        {
            sizeInScreenUnitswidth = sizeInScreenUnitsheight;
        }
        parentObject.transform.localScale = new Vector3(sizeInScreenUnitswidth, sizeInScreenUnitsheight, 1);
        bground.transform.localScale = new Vector3(sizeInScreenUnitswidth1, sizeInScreenUnitsheight1, 1);
    }

}
