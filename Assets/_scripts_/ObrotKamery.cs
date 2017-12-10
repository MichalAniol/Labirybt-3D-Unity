using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObrotKamery : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smoothV;
    public float czulosc = 3.0f;
    public float gladkosc = .1f;

    GameObject mojaPostac;

    void Start ()
    {
        mojaPostac = transform.parent.gameObject;
	}

    void Update ()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(czulosc * gladkosc, czulosc * gladkosc));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / gladkosc);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / gladkosc);
        mouseLook += smoothV;

        if (mouseLook.y > 85) mouseLook.y = 85;
        if (mouseLook.y < -70) mouseLook.y = -70;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        mojaPostac.transform.localRotation = Quaternion.AngleAxis(-mouseLook.x, mojaPostac.transform.up);
    }
}
