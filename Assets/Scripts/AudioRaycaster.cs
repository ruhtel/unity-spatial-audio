using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRaycaster : MonoBehaviour
{

    private AudioRayBasic[] rays;
    private Vector3 lastPos;
    public int resolution = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.hasChanged = true;
        lastPos = transform.position;
        
        // ADD CONSTRUCTOR
        rays = new AudioRayBasic[2];
        rays[0] = new AudioRayBasic(transform.position, transform.up);
        /*rays[1] = new AudioRayBasic(transform.position, -transform.up);
        rays[2] = new AudioRayBasic(transform.position, transform.right);
        rays[3] = new AudioRayBasic(transform.position, -transform.right);
        rays[4] = new AudioRayBasic(transform.position, transform.forward);
        rays[5] = new AudioRayBasic(transform.position, -transform.forward);*/
        rays[1] = new AudioRayBasic(transform.position, Vector3.zero);
        /*rays[7] = new AudioRayBasic(transform.position, Vector3.zero);
        rays[8] = new AudioRayBasic(transform.position, Vector3.zero);
        rays[9] = new AudioRayBasic(transform.position, Vector3.zero);
        rays[10] = new AudioRayBasic(transform.position, Vector3.zero);
        rays[11] = new AudioRayBasic(transform.position, Vector3.zero);*/
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != lastPos) {
            transform.hasChanged = true;
            lastPos = transform.position;

            foreach (AudioRayBasic ray in rays) ray.src = transform.position;
        }
    }

    void FixedUpdate() {
        if(transform.hasChanged) {
            int half = rays.Length / 2;
            for(int i = 0; i < half; i++) {
                rays[i].updateCast(transform);
                if(rays[i].reflectVec != Vector3.zero) {
                    rays[i+half].dir = rays[i].reflectVec;
                    rays[i+half].src = rays[i].hit.point;
                    rays[i+half].updateCast(transform);
                }
            }

            transform.hasChanged = false;
        } 
        
        foreach (AudioRayBasic ray in rays) {
            ray.draw(transform);
        }
    }
}
