using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPParticleScript : MonoBehaviour
{
    private GameObject HPLabel;
    public GameObject playerInPart;

    public playerController playerScriptC;
    public TextMesh gunDmg;

    // Set a Variable
    void Start()
    {
        playerScriptC = playerInPart.GetComponent<playerController>();
        HPLabel = gameObject.transform.Find("HPLabel").gameObject;
    }

    void Update()
    {
        //gunDmg.text = playerScriptC.dmg;
        //HPLabel.GetComponent<TextMesh>().text = gunDmg;
        updateGunDMGHitMarker();
        //gunDmg = playerScriptC.dmg;
        //HPLabel.GetComponent<TextMesh>().text = playerScriptC.gunList[playerScriptC.selectedGun].shootDamage.ToString();

    }
    public void updateGunDMGHitMarker()
    {
        gunDmg.text = playerScriptC.dmg;
        
    }
}
