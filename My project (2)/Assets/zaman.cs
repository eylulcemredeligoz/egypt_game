using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sure : MonoBehaviour
{
    public float zaman;
    public Text zaman_T;
    private bool oyunDevamEdiyor = true;

    void Start()
    {
        zaman = 60;
        zaman_T.text = "" + (int)zaman;
    }

    void Update()
    {
        if (oyunDevamEdiyor)
        {
            zaman -= Time.deltaTime;
            zaman_T.text = "" + Mathf.RoundToInt(zaman); // Zaman? yuvarla ve TextMeshProUGUI'da göster

            if (zaman <= 0)
            {
                zaman = 0;
                oyunDevamEdiyor = false; // Oyunu durdur
            }
        }
    }

    // Karakterin ölümü durumunda oyunu durdur
}
