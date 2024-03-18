using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int restoreHealthAmount;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip usePotion;
    [Range(0, 1)][SerializeField] float usePotionVol;
    //public GameObject useText;
    bool hasEnteredTrigger = false;
    SpriteRenderer bottle;
    //public GameObject player;


    void Start()
    {
        bottle = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                aud.PlayOneShot(usePotion, usePotionVol);
                gameManager.instance.playerScript.updateHealth(restoreHealthAmount);
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