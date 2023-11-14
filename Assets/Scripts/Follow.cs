using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라가 캐릭터의 offset에 맞춰 움직이는 스크립트
public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        transform.position = target.position + offset;
    }
}
