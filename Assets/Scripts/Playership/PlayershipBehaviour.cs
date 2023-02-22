using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipBehaviour : MonoBehaviour
{
    public PlayershipManagerSO PlayershipManagerSO;

    // Start is called before the first frame update
    void Awake()
    {
        PlayershipManagerSO.RegisterPlayer(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
