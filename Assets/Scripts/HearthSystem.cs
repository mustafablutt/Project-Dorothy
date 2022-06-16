using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearthSystem : MonoBehaviour
{
    public GameObject heart1, hearth2;
    public static int life;

    private void Start()
    {
        life = 1;
        heart1.gameObject.SetActive(true);
        hearth2.gameObject.SetActive(false);
    }
    void Update()
    {
        if (life > 2)
        {
            life = 2;
            switch (life)
            {
                case 1:
                    heart1.gameObject.SetActive(true);
                    hearth2.gameObject.SetActive(false);
                    break;
                case 2:
                    heart1.gameObject.SetActive(true);
                    hearth2.gameObject.SetActive(true);
                    break;
                case 0:
                    heart1.gameObject.SetActive(false);
                    hearth2.gameObject.SetActive(false);
                    Time.timeScale = 0;
                    break;
            }
        }
    }
}
