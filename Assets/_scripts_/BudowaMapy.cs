using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudowaMapy : MonoBehaviour
{
    public GameObject podlogaOtwarta;
    public GameObject podlogaZamknieta;
    public GameObject scianaOtwartaBok;
    public GameObject scianaZamknietaBok;
    public GameObject scianaOtwartaPrzod;
    public GameObject scianaZamknietaPrzod;

    public GameObject drabinaPrzod;
    public GameObject drabinaLewa;
    public GameObject drabinaPrawa;
    public GameObject drabinaTyl;

    public GameObject kostka;
    public GameObject player;

    public int rozmiarLabiryntuX = 3;
    public int rozmiarLabiryntuY = 3;
    public int rozmiarLabiryntuZ = 1;

    Vector3 przesuniecie;

    Labirynt labirynt;


    void Start()
    {
        Labirynt mapa = new Labirynt();

        mapa.rozmiarLabiryntuX = 5;
        mapa.rozmiarLabiryntuY = 5;
        mapa.rozmiarLabiryntuZ = 5;


        mapa.TworzenieLabiryntu();
        

        int droga = mapa.TworzenieLabiryntu();

        BudowaLabiryntu(mapa);

        RysowanieDrogi(droga, mapa);

        player.GetComponent<RuchGracza>().liczbaKostek = droga;
    }

    void BudowaLabiryntu(Labirynt mapa)
    {
        GameObject wstaw;

        int komnata;

        for (int z = 0; z < rozmiarLabiryntuZ; z++) /// budowa korytarzy labiryntu
        {
            for (int y = 0; y < rozmiarLabiryntuY; y++)
            {
                for (int x = 0; x < rozmiarLabiryntuX; x++)
                {
                    przesuniecie = new Vector3(10 * x, 10 * z, -10 * y);
                    komnata = mapa.labirynt[x, y, z];

                    if ((komnata & 1) == 1)
                        wstaw = (GameObject)Instantiate(scianaOtwartaPrzod, scianaOtwartaPrzod.transform.position + przesuniecie, scianaOtwartaPrzod.transform.rotation);
                    else
                        wstaw = (GameObject)Instantiate(scianaZamknietaPrzod, scianaZamknietaPrzod.transform.position + przesuniecie, scianaZamknietaPrzod.transform.rotation);

                    if ((komnata & 2) == 2)
                        wstaw = (GameObject)Instantiate(scianaOtwartaBok, scianaOtwartaBok.transform.position + przesuniecie, scianaOtwartaBok.transform.rotation);
                    else
                        wstaw = (GameObject)Instantiate(scianaZamknietaBok, scianaZamknietaBok.transform.position + przesuniecie, scianaZamknietaBok.transform.rotation);

                    if ((komnata & 32) == 32)
                    {
                        wstaw = (GameObject)Instantiate(podlogaOtwarta, podlogaOtwarta.transform.position + przesuniecie, podlogaOtwarta.transform.rotation);

                        if ((komnata & 1) == 1) wstaw = (GameObject)Instantiate(drabinaPrzod, drabinaPrzod.transform.position + przesuniecie, drabinaPrzod.transform.rotation);
                        if ((komnata & 2) == 2) wstaw = (GameObject)Instantiate(drabinaPrawa, drabinaPrawa.transform.position + przesuniecie, drabinaPrawa.transform.rotation);
                        if ((komnata & 4) == 4) wstaw = (GameObject)Instantiate(drabinaTyl, drabinaTyl.transform.position + przesuniecie, drabinaTyl.transform.rotation);
                        if ((komnata & 8) == 8) wstaw = (GameObject)Instantiate(drabinaLewa, drabinaLewa.transform.position + przesuniecie, drabinaLewa.transform.rotation);
                    }
                    else
                        wstaw = (GameObject)Instantiate(podlogaZamknieta, podlogaZamknieta.transform.position + przesuniecie, podlogaZamknieta.transform.rotation);
                }
            }
        }

        int xxx = 0; /// budowa bocznej sciany
        for (int z = 0; z < rozmiarLabiryntuZ; z++)
        {
            for (int y = 0; y < rozmiarLabiryntuY; y++)
            {
                przesuniecie = new Vector3(-10, 10 * z, -10 * y);
                komnata = mapa.labirynt[xxx, y, z];

                if ((komnata & 8) == 8)
                    wstaw = (GameObject)Instantiate(scianaOtwartaBok, scianaOtwartaBok.transform.position + przesuniecie, scianaOtwartaBok.transform.rotation);
                else
                    wstaw = (GameObject)Instantiate(scianaZamknietaBok, scianaZamknietaBok.transform.position + przesuniecie, scianaZamknietaBok.transform.rotation);
            }
        }

        int yyy = rozmiarLabiryntuY - 1; /// budowa przedniej scianki
        for (int z = 0; z < rozmiarLabiryntuZ; z++)
        {
            for (int x = 0; x < rozmiarLabiryntuX; x++)
            {
                przesuniecie = new Vector3(x * 10, 10 * z, -10 * (yyy + 1));
                komnata = mapa.labirynt[x, yyy, z];

                if ((komnata & 4) == 4)
                    wstaw = (GameObject)Instantiate(scianaOtwartaPrzod, scianaOtwartaPrzod.transform.position + przesuniecie, scianaOtwartaPrzod.transform.rotation);
                else
                    wstaw = (GameObject)Instantiate(scianaZamknietaPrzod, scianaZamknietaPrzod.transform.position + przesuniecie, scianaZamknietaPrzod.transform.rotation);
            }
        }

        int zzz = rozmiarLabiryntuZ - 1; /// budowa sufitu
        for (int y = 0; y < rozmiarLabiryntuY; y++)
        {
            for (int x = 0; x < rozmiarLabiryntuX; x++)
            {
                przesuniecie = new Vector3(x * 10, 10 * (zzz + 1), -10 * y);
                komnata = mapa.labirynt[x, y, zzz];

                wstaw = (GameObject)Instantiate(podlogaZamknieta, podlogaZamknieta.transform.position + przesuniecie, podlogaZamknieta.transform.rotation);
            }
        }
    }

    void RysowanieDrogi(int dlugoscDrogi, Labirynt mapa)
    {
        int x, y, z;
        GameObject wstaw;

        for (int i = 0; i < dlugoscDrogi; i++)
        {
            x = mapa.drogaWyjscia[i, 0];
            y = mapa.drogaWyjscia[i, 1];
            z = mapa.drogaWyjscia[i, 2];

            przesuniecie = new Vector3(10 * x, 10 * z, -10 * y);

            wstaw = (GameObject)Instantiate(kostka, kostka.transform.position + przesuniecie, kostka.transform.rotation);
            wstaw.name = i.ToString();
        }
    }
}
