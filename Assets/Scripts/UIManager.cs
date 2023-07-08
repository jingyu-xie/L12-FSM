using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerHealthUI;
    private int heart_cnt;

    public void SetHealth(int health)
    {
        heart_cnt = 0;

        foreach (Transform heart in playerHealthUI.transform)
        {
            if (heart_cnt < health)
                heart.gameObject.SetActive(true);
            else
                heart.gameObject.SetActive(false);

            heart_cnt++;
        }

        
    }
}
