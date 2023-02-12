using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManageMent : MonoBehaviour
{
     void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
