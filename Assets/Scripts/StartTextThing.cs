using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTextThing : MonoBehaviour
{
    void Awake()
    {
        Invoke("DisableMyself", 3f);
    }

    private void DisableMyself()
    {
        this.gameObject.SetActive(false);
    }
}
