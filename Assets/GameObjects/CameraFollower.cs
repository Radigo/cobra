using UnityEngine;

public class CameraFollower : MonoBehaviour {
    private GameObject target;
    private Vector3 targetFeet;
    public float viewDistance;// Setting a good view distance ensures no clipping
    public float viewAheadDistanceMax;
    public float viewFriction = 0.5f;
    private float viewAheadDistance;
    private Quaternion viewAngle;
    private Vector3 positionOffset;


    private PlayerControls boat;

    // Start is called before the first frame update
    void Start() {
        target = GameObject.FindWithTag("Player");
        targetFeet = new Vector3();
        viewAngle = Quaternion.Euler(30.0f, 45.0f, 0.0f);
        positionOffset = getOffsetFromAngle(viewAngle);

        boat = target.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update() {
        // Points towards ahead of target's feet
        targetFeet.x = target.transform.position.x;
        targetFeet.z = target.transform.position.z;

        // Get boat speed
        viewAheadDistance = boat.getThrustRatio() * viewAheadDistanceMax;
        // Align to current direction
        Vector3 aheadOffset = boat.getVelocityNormalized() * viewAheadDistance;
        // Jitter when boat accelerate from zero speed
        //Debug.Log("aheadOffset: " + aheadOffset + ", viewAheadDistance: " + viewAheadDistance);

        transform.SetPositionAndRotation(targetFeet + positionOffset + aheadOffset, viewAngle);
    }

    Vector3 getOffsetFromAngle(Quaternion p_angle) {
        // Dynamically gets camera position if we need to rotate the camera
        Vector3 offset = p_angle * Vector3.back * viewDistance;
        return offset;
    }
}
