using Unity.VisualScripting;
using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    [SerializeField] int restoreShieldAmount;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip usePotion;
    [Range(0, 1)][SerializeField] float usePotionVol;
    //public GameObject useText;
    bool hasEnteredTrigger = false;
    MeshRenderer bottle;
    //public GameObject player;


    void Start()
    {
        bottle = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                aud.PlayOneShot(usePotion, usePotionVol);
                gameManager.instance.playerScript.updateShield(restoreShieldAmount);
                hasEnteredTrigger = false;
                bottle.enabled = false;
                Destroy(gameObject, 3);
                //useText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //useText.SetActive(true);
            hasEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //useText.SetActive(false);
            hasEnteredTrigger = false;
        }
    }
}