using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labirynt : MonoBehaviour, ILabirynt
{

        public int rozmiarLabiryntuX { private get; set; }
        public int rozmiarLabiryntuY { private get; set; }
        public int rozmiarLabiryntuZ { private get; set; }
        public int[,,] labirynt { get; private set; }
        public int[,] drogaWyjscia { get; private set; }

    public int TworzenieLabiryntu()
    {
        labirynt = new int[rozmiarLabiryntuX, rozmiarLabiryntuY, rozmiarLabiryntuZ];

        drogaWyjscia = new int[5000, 3];

        GenerowanieLabiryntu();

        return SzukanieDrogi();
    }


    void GenerowanieLabiryntu()
    {
        System.Random liczbaLosowa = new System.Random();
        int kierunekPoszukiwanWolnejKomnaty;
        int[] polaBezKomnaty = { 0, 0, 0, 0, 0, 0 };

        int pozycjaX = liczbaLosowa.Next(0, rozmiarLabiryntuX - 1); /// wylosuj pozycje startowa rysowania labiryntu
        int pozycjaY = liczbaLosowa.Next(0, rozmiarLabiryntuY - 1);
        int pozycjaZ = liczbaLosowa.Next(0, rozmiarLabiryntuZ - 1);

        int licznikKomnat = rozmiarLabiryntuX * rozmiarLabiryntuY * rozmiarLabiryntuZ - 1;

        do
        {
            do          /// szykanie wolnego pola wokol komnaty w ktorej sie znajduje
            {
                kierunekPoszukiwanWolnejKomnaty = 0;

                if (pozycjaX > 0 && labirynt[pozycjaX - 1, pozycjaY, pozycjaZ] == 0)
                {
                    polaBezKomnaty[kierunekPoszukiwanWolnejKomnaty++] = 8;
                }/// lewo


                if (pozycjaX < rozmiarLabiryntuX - 1 && labirynt[pozycjaX + 1, pozycjaY, pozycjaZ] == 0)
                {
                    polaBezKomnaty[kierunekPoszukiwanWolnejKomnaty++] = 2;
                }/// prawo

                if (pozycjaY > 0 && labirynt[pozycjaX, pozycjaY - 1, pozycjaZ] == 0)
                {
                    polaBezKomnaty[kierunekPoszukiwanWolnejKomnaty++] = 1;
                }/// tyl

                if (pozycjaY < rozmiarLabiryntuY - 1 && labirynt[pozycjaX, pozycjaY + 1, pozycjaZ] == 0)
                {
                    polaBezKomnaty[kierunekPoszukiwanWolnejKomnaty++] = 4;
                }/// przod


                if (pozycjaZ > 0 && labirynt[pozycjaX, pozycjaY, pozycjaZ - 1] == 0
                    && (labirynt[pozycjaX, pozycjaY, pozycjaZ] & 16) == 0) /// jesli nie jest to poczatek noewgo korytarza
                {
                    polaBezKomnaty[kierunekPoszukiwanWolnejKomnaty++] = 32;
                }/// dol

                if (pozycjaZ < rozmiarLabiryntuZ - 1 && labirynt[pozycjaX, pozycjaY, pozycjaZ + 1] == 0
                    && (labirynt[pozycjaX, pozycjaY, pozycjaZ] & 32) == 0) /// jesli nie jest to poczatek noewgo korytarza
                {
                    polaBezKomnaty[kierunekPoszukiwanWolnejKomnaty++] = 16;
                }/// gora


                if (kierunekPoszukiwanWolnejKomnaty > 0)
                {
                    int wyborKierunku = liczbaLosowa.Next(0, kierunekPoszukiwanWolnejKomnaty);
                    int stworzenieKomnaty = polaBezKomnaty[wyborKierunku];
                    labirynt[pozycjaX, pozycjaY, pozycjaZ] += stworzenieKomnaty;


                    if (stworzenieKomnaty == 8) { pozycjaX--; labirynt[pozycjaX, pozycjaY, pozycjaZ] = 2; }; /// wrysuj wyjscie z komnaty
                    if (stworzenieKomnaty == 2) { pozycjaX++; labirynt[pozycjaX, pozycjaY, pozycjaZ] = 8; }; /// w ktorej jestes
                    if (stworzenieKomnaty == 1) { pozycjaY--; labirynt[pozycjaX, pozycjaY, pozycjaZ] = 4; }; /// przejdz do nastepnej
                    if (stworzenieKomnaty == 4) { pozycjaY++; labirynt[pozycjaX, pozycjaY, pozycjaZ] = 1; }; /// i wrysuj wejscie do niej
                    if (stworzenieKomnaty == 32) { pozycjaZ--; labirynt[pozycjaX, pozycjaY, pozycjaZ] = 16; };
                    if (stworzenieKomnaty == 16) { pozycjaZ++; labirynt[pozycjaX, pozycjaY, pozycjaZ] = 32; };

                    licznikKomnat--;
                }
            } while (kierunekPoszukiwanWolnejKomnaty != 0);

            do      /// gdy wokol nie ma juz wolnej komnaty przejdz do nastepnej komnaty
            {
                pozycjaX++;
                if (pozycjaX == rozmiarLabiryntuX)
                {
                    pozycjaX = 0; pozycjaY++;
                    if (pozycjaY == rozmiarLabiryntuY)
                    {
                        pozycjaY = 0; pozycjaZ++;
                        if (pozycjaZ == rozmiarLabiryntuZ)
                        {
                            pozycjaZ = 0;
                        }
                    }
                }
            } while (labirynt[pozycjaX, pozycjaY, pozycjaZ] == 0); /// szukaj az znadziesz pole z komnata
        } while (licznikKomnat > 0); /// az zapelnisz caly labirynt

        labirynt[0, 0, 0] += 1;
        labirynt[rozmiarLabiryntuX - 1, rozmiarLabiryntuY - 1, rozmiarLabiryntuZ - 1] += 4;
    }

    int SzukanieDrogi()
    {
        int sprawdzamX = 0, sprawdzamY = 0, sprawdzamZ = 0, licznikKomnat = 0;
        int[,,] mapa = new int[rozmiarLabiryntuX, rozmiarLabiryntuY, rozmiarLabiryntuZ];
        int sumaWyjsc, komnata;

        for (int z = 0; z < rozmiarLabiryntuZ; z++)
        {
            for (int y = 0; y < rozmiarLabiryntuY; y++)
            {
                for (int x = 0; x < rozmiarLabiryntuX; x++)
                {
                    mapa[x, y, z] = labirynt[x, y, z]; /// kopiowanie mapy labiryntu
                }
            }
        }
        mapa[0, 0, 0] -= 1;
        mapa[rozmiarLabiryntuX - 1, rozmiarLabiryntuY - 1, rozmiarLabiryntuZ - 1] -= 4;


        do
        {
            komnata = mapa[sprawdzamX, sprawdzamY, sprawdzamZ];
            sumaWyjsc = (komnata & 1) + (komnata & 2) / 2 + (komnata & 4) / 4 + (komnata & 8) / 8 + (komnata & 16) / 16 + (komnata & 32) / 32;

            if (sumaWyjsc == 0)
            {
                do
                {
                    licznikKomnat--;
                    sprawdzamX = drogaWyjscia[licznikKomnat, 0];
                    sprawdzamY = drogaWyjscia[licznikKomnat, 1];
                    sprawdzamZ = drogaWyjscia[licznikKomnat, 2];
                } while (mapa[sprawdzamX, sprawdzamY, sprawdzamZ] == 0);
                continue;
            }

            drogaWyjscia[licznikKomnat, 0] = sprawdzamX;
            drogaWyjscia[licznikKomnat, 1] = sprawdzamY;
            drogaWyjscia[licznikKomnat, 2] = sprawdzamZ;

            if ((komnata & 1) != 0)
            {
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ]--; sprawdzamY--;
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 4; licznikKomnat++;
                continue;
            }

            if ((komnata & 2) != 0)
            {
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 2; sprawdzamX++;
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 8; licznikKomnat++;
                continue;
            }

            if ((komnata & 4) != 0)
            {
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 4; sprawdzamY++;
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ]--; licznikKomnat++;
                continue;
            }

            if ((komnata & 8) != 0)
            {
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 8; sprawdzamX--;
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 2; licznikKomnat++;
                continue;
            }

            if ((komnata & 16) != 0)
            {
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 16; sprawdzamZ++;
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 32; licznikKomnat++;
                continue;
            }

            if ((komnata & 32) != 0)
            {
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 32; sprawdzamZ--;
                mapa[sprawdzamX, sprawdzamY, sprawdzamZ] -= 16; licznikKomnat++;
                continue;
            }
        } while (((sprawdzamX == rozmiarLabiryntuX - 1)
        && (sprawdzamY == rozmiarLabiryntuY - 1)
        && (sprawdzamZ == rozmiarLabiryntuZ - 1)) == false);

        drogaWyjscia[licznikKomnat, 0] = sprawdzamX;
        drogaWyjscia[licznikKomnat, 1] = sprawdzamY;
        drogaWyjscia[licznikKomnat, 2] = sprawdzamZ;

        return ++licznikKomnat;
    }
}

	

