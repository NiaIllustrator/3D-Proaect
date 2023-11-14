using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스크립트

/*
앞으로 계속 코드 주석 추가합시당 ^^
 
 Dodge() 부분 에러나는 이유 
        = Item 형식의 변수 item을 선언했으니 변수 item을 사용해야 되는데
          대문자 소문자 실수로 Item을 썼으니 오류가 뜸
            
                Item item = nearObject.GetComponent<Item>(); => Item 형식의 변수 item 선언
                int weaponIndex = Item.value; => Item.value의 값은 없다. 변수 item이 value를 가지고 있음
      
      고친거 =>  int weaponindex = item.value; 
      
      공부해야 할 것 : C# 변수의 타입(형식)과 변수 사용법에 대한 공부


 Interaction() 부분 에러나는 이유
        = SubMachineGun 오브젝트에 인덱스 값 할당 안되있었음
        = Player의 hasWeapon 값 안늘려줌(3개를 가질 수 있다고 해줘야 하는데 0개로 되어있었음)

 4강 9분 부분, 오른쪽 손에 무기 쥐어야 할 공간=Weapon Point(게임오브젝트) 지정 안해줌
 
Weapon Point 지정하고 오브젝트 3개 넣어놓고, Weapon[] 배열에 3개 넣어놨으니 9분부터 다시 볼 것

    그 외 Interation 이 아니라 Interaction 같은 오타 등에 대해 좀 더 신경써야 할 것으로 보입니다

    굿 잘하고있네 
      

*/
public class Player : MonoBehaviour
{
// 앞으로 계속 코드 주석 추가합시당 ^^

    public float speed;

    // 얘네는 배열임. C# Array 관련 공부해볼 것
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public float hAxis;
    public float vAxis;

    public bool wDown;
    public bool jDown;
    public bool iDown;

    public bool isJump;
    public bool isDodge;

    public Vector3 moveVec;
    public Vector3 dodgeVec;

    public Rigidbody rigid;
    public Animator anim;

    public GameObject nearObject;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Interaction();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
    }

    // 캐릭터의 움직임 제어 함수
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // if 문은 이렇게 쓰면 한줄만 적용된다. 가독성을 위해 엔터 쳐놓을 것
        if (isDodge)
            moveVec = dodgeVec;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; 

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    // 캐릭터 바라보는 방향 제어 함수
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }


    // 캐릭터 점프
    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }


    // 캐릭터 구르기 함수
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);   
        }
    }

    // 캐릭터 닷지 false Invoke 함수
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    /* Interation -> Interaction 으로 수정
    
    강의 보니까 Interation 쓰셨던데 앞으로 Interaction으로 통일해보셈

    캐릭터와 아이템 간의 상호작용을 위한 함수

    Submachinegun에 Item Value 설정 안되있었다.
     */
    void Interaction()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name);
    }
}

