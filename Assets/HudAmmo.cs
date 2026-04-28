using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
    public GameObject[] slots; 

    public void UpdateHUD(int currentAmmo)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(i < currentAmmo);
        }
    }
}