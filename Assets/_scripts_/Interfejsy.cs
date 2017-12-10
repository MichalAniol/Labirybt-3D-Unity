using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ILabirynt
{
    int rozmiarLabiryntuX { set; }
    int rozmiarLabiryntuY { set; }
    int rozmiarLabiryntuZ { set; }

    int[,,] labirynt { get; }
    int[,] drogaWyjscia { get; }

    int TworzenieLabiryntu();
}
