using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialSys.Samples.InputOverride
{
    /// <summary>
    /// Simple script representing the "capsule monsters" that spawn on the DREADMILL.
    /// They don't do anything other than move along the negative Z axis and then destroy themselves
    /// at the end of their lifespan. Not much of a thrilling existence.
    /// </summary>
    public class CapsuleMonster : MonoBehaviour
    {
        /// <summary>
        /// Speed modifier for when the object is spawned
        /// </summary>
        public float speed = 5f;

        /// <summary>
        /// How long the object lives after being spawned
        /// </summary>
        public float lifespan = 15f;

        /// <summary>
        /// Counter variable used for determining when the object should be destroyed
        /// </summary>
        float lifespanCounter = 0f;

        void Update()
        {
            // when spawned the object moves along the negative Z axis until its time is up
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            lifespanCounter += Time.deltaTime;
            if(lifespanCounter > lifespan)
                Destroy(gameObject);
        }
    }
}
