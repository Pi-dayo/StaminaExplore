using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�}�E�X�J�[�\���̑���
public class MouseCursor : MonoBehaviour
{
    [SerializeField] Camera cam;
    void Update()
    {
        transform.position =(Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
