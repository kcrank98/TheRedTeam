using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    [Header(" ---- Gun Stats ---- ")]
    public string gunName;
    public int shootDamage;
    public int shootDistance;
    public float shootRate;
    public int ammoCurrent;
    public int ammoMax;
    public float aimFOV;
    public float aimSpeed;

    [Header(" ---- Transform Local Position ---- ")]
    public Vector3 attachmentPosition;



    [Header(" ---- Components ---- ")]

    public GameObject model;
    //public GameObject attachment;
    public ParticleSystem hitEffect;

    [Header(" ---- Audio ---- ")]

    public AudioClip reloadSound;
    [Range(0, 1)] public float reloadSoundVol;
    public AudioClip clickSound;
    [Range(0, 1)] public float clickSoundVol;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootSoundVol;
}

