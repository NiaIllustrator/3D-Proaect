using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� ĳ������ offset�� ���� �����̴� ��ũ��Ʈ
public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        transform.position = target.position + offset;
    }
}
