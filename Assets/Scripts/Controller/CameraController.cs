using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {

        public Transform m_targeTransform;
        private Transform m_selfTransform;
        public float m_offsetZ = 10, m_offsetY = 10;
        // Use this for initialization
        void Start()
        {
            m_selfTransform = this.transform;
            m_selfTransform.LookAt(m_targeTransform);

        }


        void LateUpdate()
        {
            GetCameraPos();
        }

        private void GetCameraPos()
        {
            Vector3 newTagetVector3 = new Vector3(m_targeTransform.position.x, m_targeTransform.position.y + m_offsetY,
                m_targeTransform.position.z + m_offsetZ);
            m_selfTransform.position = Vector3.Lerp(m_selfTransform.position, newTagetVector3, Time.deltaTime * 5);
        }
    }
}

