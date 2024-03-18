using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class cutsceneScript : MonoBehaviour
{
    [SerializeField] PlayableDirector timeLine;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        timeLine = GetComponent<PlayableDirector>();
        count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && count == 0)
        {
            timeLine.Play();
            count++;
        }
    }
}
