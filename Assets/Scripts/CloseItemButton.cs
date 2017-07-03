using UnityEngine;
using System.Collections;

public class CloseItemButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Ended)
            {
                Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(touchRay, out hit))
                {
                    if (hit.collider.tag == "CloseItemButton")
                    {
                        GameObject.Destroy(hit.collider.transform.gameObject);
                    }
                }
            }
        }
    }
}
