using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RuchGracza : MonoBehaviour
{
    public static float mnoznik;
    Vector3 opadanie = new Vector3(0, -.5f, 0);
    string trafienie;

    public Text koordynaty;
    public Text kostki;
    public Text instrukcja;
    string przerwa = " ";
    int x, y, z;
    public int liczbaKostek;

    private void Start()
    {
        mnoznik = .3f;
        kostki.text = liczbaKostek.ToString();
        instrukcja.text = "Collect all red cubes\nand you'll find the exit\nof this maze\n\nuse \'R\' for restart";
    }

    void Update ()
    {
            float przodTyl = Input.GetAxis("przod-tyl");
            float prawoLewo = Input.GetAxis("prawo-lewo");
            CharacterController player = GetComponent<CharacterController>();

        if (player.transform.position.z > -53)
        {
            Quaternion obrot = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
            Vector3 obracanyVector = new Vector3(-przodTyl, 0, prawoLewo) * mnoznik;
            Vector3 przesuniecie = obrot * obracanyVector;

            player.Move(przesuniecie);
            player.Move(opadanie);
        }

        koordynaty.text = ("x:" + ((int)(player.transform.position.x / 10 + 2)).ToString() + "  y:" 
            + ((int)-(player.transform.position.z / 10 -1)).ToString() + "  z:"
            + ((int)(player.transform.position.y / 10 + 1)).ToString());

        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(0);

        if (player.transform.position.z < -2 && player.transform.position.z > -52) instrukcja.text = "";

        if (player.transform.position.z < -52)
            instrukcja.text = "Congratulations !!!\nyou find the exit\n\nuse \'R\' for restart";
    }


    private void OnTriggerEnter(Collider zCzym)
    {
        trafienie = zCzym.gameObject.tag;
        if (trafienie == "cube")
        {
            Destroy(GameObject.Find(zCzym.gameObject.name));
            liczbaKostek--;
            if (liczbaKostek > 0)
                kostki.text = liczbaKostek.ToString();
            else
                kostki.text = "##";
        }
    }

}
