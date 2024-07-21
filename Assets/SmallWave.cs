using UnityEngine;

public class SmallWave : MonoBehaviour
{
    //private Camera target;
    private GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        //target = Camera.main;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Always face camera
        transform.LookAt(target.transform, Vector3.back);
    }
}
