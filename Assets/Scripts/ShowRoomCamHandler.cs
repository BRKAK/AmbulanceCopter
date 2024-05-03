using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoomCamHandler : MonoBehaviour
{
    public GameObject heli, blades, miniBlades;
    // Start is called before the first frame update
    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(heli.transform);
        transform.RotateAround(Vector3.zero, heli.transform.position, 15 * Time.deltaTime);
        gameObject.GetComponent<Camera>().fieldOfView = Mathf.PingPong(Time.time * 5, 70 - 30) + 30;
        AnimateHeli();
    }

    void AnimateHeli()
    {
        blades.transform.Rotate(0, 0, 200 * Time.deltaTime);
        miniBlades.transform.Rotate(0, 0, 100 * Time.deltaTime);
    }
}
