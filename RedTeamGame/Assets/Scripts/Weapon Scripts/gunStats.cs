using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    [Header(" ---- Gun Stats ---- ")]
    public string gunSize;
    public string gunName;
    public int shootDamage;
    public int shootDistance;
    public float shootRate;
    public float reloadRate;
    public float aimFOV;
    public float aimSpeed;


    [Header(" ---- Transform Local Position ---- ")]
    public Vector3 attachmentPosition;

    [Header(" ---- Ammo ---- ")]
    public int magazineMax;
    public int reservesMax;
    public int magazine;
    public int reserves;

    [Header(" ---- Components ---- ")]
    public Sprite uiImage;
    public GameObject model;
    //public Animator gunAnimator;
    //public RuntimeAnimatorController runtimeAnimatorController;
    public Sprite sprite;
    public ParticleSystem hitEffectPS;
    [SerializeField] GameObject hitEffectIMG;

    

    [Header(" ---- Audio ---- ")]

    public AudioClip reloadSound;
    [Range(0, 1)] public float reloadSoundVol;
    public AudioClip clickSound;
    [Range(0, 1)] public float clickSoundVol;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootSoundVol;
}

