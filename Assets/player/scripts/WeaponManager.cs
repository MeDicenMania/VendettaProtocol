using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Sistemas de arma")]
    public MeleeAttack meleeAttack;
    public PistolaSimple pistola;

    public bool usaMelee;

    void Start()
    {
        usaMelee = PlayerPrefs.GetInt("UsaMelee", 0) == 1;

        if (meleeAttack != null)
            meleeAttack.enabled = usaMelee;

        if (pistola != null)
            pistola.enabled = !usaMelee;
    }
}