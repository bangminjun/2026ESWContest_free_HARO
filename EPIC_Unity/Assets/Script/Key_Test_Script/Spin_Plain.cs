using UnityEngine;

public class Spin_Plain : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
           transform.Rotate(Vector3.up * 100f * Time.deltaTime, Space.World);
            Debug.Log("D");
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            transform.Rotate(Vector3.up * 100f * Time.deltaTime, Space.World);
            Debug.Log("F");
        }
    }
}

