using UnityEngine;
using System.Collections;

public class GroupSelect : MonoBehaviour {

    public Transform MinionsHolder;
    
    Vector3 OnDownPoint;

    public Vector3[] PointList = new Vector3[4];

    public RectTransform RectObj;

    void Update () {
	    if(Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100000, LayerMask.GetMask("ThePlane")))
            {
                OnDownPoint = hit.point;
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100000, LayerMask.GetMask("Minions")))
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    hit.transform.GetComponent<MinionAction>().SelectMinion(hit.transform.GetComponent<MinionAction>().Selected ? false : true);
                }else
                {
                    for (int i = 0; i < MinionsHolder.childCount; i++)
                    {
                        MinionsHolder.GetChild(i).GetComponent<MinionAction>().SelectMinion(false);
                    }
                    hit.transform.GetComponent<MinionAction>().SelectMinion(true);
                }
            }else if (!Input.GetKey(KeyCode.LeftControl) &&  !Input.GetKey(KeyCode.RightControl))
            {
                for (int i = 0; i < MinionsHolder.childCount; i++)
                {
                    MinionsHolder.GetChild(i).GetComponent<MinionAction>().SelectMinion(false);
                }
            }

            RectObj.anchoredPosition = Camera.main.WorldToScreenPoint(OnDownPoint);
            RectObj.sizeDelta = Vector2.zero;
            RectObj.gameObject.SetActive(true);

        }
        else if(Input.GetMouseButtonUp(0))
        {
            RectObj.gameObject.SetActive(false);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 Start2D = Camera.main.WorldToScreenPoint(OnDownPoint);
            Rect Rectangle = new Rect(
                Input.mousePosition.x <= Start2D.x ? Input.mousePosition.x : Start2D.x,
                Input.mousePosition.y  < Start2D.y ? Input.mousePosition.y : Start2D.y,
                Mathf.Abs(Input.mousePosition.x - Start2D.x),
                Mathf.Abs(Input.mousePosition.y - Start2D.y));

            RectObj.anchoredPosition = new Vector2(Rectangle.x, Rectangle.y);
            RectObj.sizeDelta = new Vector2(Rectangle.width, Rectangle.height);

            if (Vector2.Distance(Vector2.zero, RectObj.sizeDelta) > 16)
            {
                SelectRegion();
            }

        }

    }

    void SelectRegion ()
    {
        Vector2 Start2D = Camera.main.WorldToScreenPoint(OnDownPoint);
        Rect Rectangle = new Rect(
            Input.mousePosition.x <= Start2D.x ? Input.mousePosition.x : Start2D.x,
            Input.mousePosition.y < Start2D.y ? Input.mousePosition.y : Start2D.y,
            Mathf.Abs(Input.mousePosition.x - Start2D.x),
            Mathf.Abs(Input.mousePosition.y - Start2D.y));

        PointList[0] = new Vector3(Rectangle.x, Rectangle.y, 0);
        PointList[1] = new Vector3(Rectangle.x, Rectangle.y + Rectangle.height, 0);
        PointList[2] = new Vector3(Rectangle.x + Rectangle.width, Rectangle.y + Rectangle.height, 0);
        PointList[3] = new Vector3(Rectangle.x + Rectangle.width, Rectangle.y, 0);

        Ray ray;
        RaycastHit hit;
        for (int i = 0; i<4; i++)
        {
            ray = Camera.main.ScreenPointToRay(PointList[i]);
            if (Physics.Raycast(ray, out hit, 100000, LayerMask.GetMask("ThePlane")))
            {
                PointList[i] = new Vector3(hit.point.x, 0f, hit.point.z);
            }
            else
            {
                Debug.LogError("SelectRegion Raycast Error");
            }
        }

        for(int i = 0;i<MinionsHolder.childCount;i++)
        {
            if (CrossCheckList(MinionsHolder.GetChild(i).position))
            {
                MinionsHolder.GetChild(i).GetComponent<MinionAction>().SelectMinion(true);
            }else if(Input.GetKey(KeyCode.LeftControl) == false && Input.GetKey(KeyCode.RightControl) == false)
            {
                MinionsHolder.GetChild(i).GetComponent<MinionAction>().SelectMinion(false);
            }
        }
    }

    bool CrossCheckList (Vector3 Point)
    {
        Point = new Vector3(Point.x, 0, Point.z);
        bool[] plus = new bool[4];

        for(int i =0; i<4; i++)
        {
            //Like Calculateing Normal Direction Based on 3 points
            plus[i] =       Vector3.Cross(PointList[ListLoop(i + 1)] - PointList[ListLoop(i)], Point - PointList[ListLoop(i)]).normalized.y > 0 ? true : false;
        }
        bool result;
        if(plus[0] && plus[1] && plus[2] && plus[3])
        {
            result = true;
        }else
        {
            result = false;
        }
        return result;
    }

    int ListLoop (int n)
    {
        if(n != 4)
        {
            return n;
        }else
        {
            return 0;
        }
    }
}
