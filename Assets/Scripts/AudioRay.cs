using UnityEngine;

public class AudioRay : MonoBehaviour {
    [HideInInspector]
    public int reflections = 2;

    public Vector3 dir;
    private Vector3[] dirs;

    private RaycastHit[] rays;

    private Vector3[] srcs;

    private bool[] hits;
    private bool didHit = true;

    // Start is called before the first frame update
    void Start() {
        transform.hasChanged = true;

        rays = new RaycastHit[3];
        dirs = new Vector3[3];
        srcs = new Vector3[3];
        hits = new bool[3];
        
        srcs[0] = transform.position;

        for(int i = 0; i < reflections + 1; i++) {
            hits[i] = false;
        }
    }

    void Update() {
        if(transform.position != srcs[0]) {
            transform.hasChanged = true;
            srcs[0] = transform.position;
        }

        if(dir != dirs[0]) {
            dirs[0] = dir;
        }
    }
    // Update is called once per frame
    void FixedUpdate() {

        if(transform.hasChanged) {
            hits[0] = Physics.Raycast(srcs[0], dirs[0], out rays[0], Mathf.Infinity);

            if(hits[0]) {
                // Is v relative to normal?
                Vector3 v = getReflectionVector(rays[0], srcs[0]);
                dirs[1] = Vector3.Normalize(v);
                srcs[1] = rays[0].point;
                hits[1] = Physics.Raycast(rays[0].point, Vector3.Normalize(v), out rays[1], Mathf.Infinity);

                if(hits[1]) {
                    v = getReflectionVector(rays[1], srcs[1]);
                    dirs[2] = Vector3.Normalize(v);
                    srcs[2] = rays[1].point;
                    hits[2] = Physics.Raycast(rays[1].point, Vector3.Normalize(v), out rays[2], Mathf.Infinity);
                }
            }
        }

        //if(didHit) Debug.DrawRay(transform.position, dir * rays[0].distance, Color.yellow);
        //else Debug.DrawRay(transform.position, dir * 1000, Color.red);

        for(int i = 0; i < 3; i++) {
            if(hits[i]) {
                Debug.DrawRay(srcs[i], dirs[i] * rays[i].distance, Color.yellow);
            } else {
                Debug.DrawRay(srcs[i], dirs[i] * 2000, Color.red);
            }
        }

        transform.hasChanged = false;
    }


    // Given a RaycastHit and Incident vector, determine reflection
    Vector3 getReflectionVector(RaycastHit hit, Vector3 start) {
        Vector3 incident = hit.point - start;
        return incident - 2*(Vector3.Dot(incident, hit.normal)) * hit.normal; 
    }
}
