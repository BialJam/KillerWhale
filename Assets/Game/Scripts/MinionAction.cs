using UnityEngine;
using System.Collections;

public class MinionAction : MonoBehaviour {

    public bool Selected;
    NavMeshAgent agent;

    public Material MatOn;
    public Material MatOff;
    void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
    }
	void Update () {
        if (Input.GetMouseButtonDown(1) && Selected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("ThePlane")))
            {
                agent.SetDestination(hit.point);
                agent.Resume();
            }
        }
    }

    public void SelectMinion (bool select)
    {
        GetComponent<MeshRenderer>().sharedMaterial = select ? MatOn : MatOff;
        Selected = select;
    }
}
