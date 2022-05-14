using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class PlayerDrag : MonoBehaviour
{
    public float force = 100;
    public GameObject jugdeRing;
    private Vector3 _ringOffset = new Vector3(0.0f, 0.0f, -5.0f);
    private float jugdeRate = 1.0f;
    private float jugdeTiming = 0.2f;

    public GameObject rotateArrow;

    private Vector3 _clickPosition;
    private Vector3 _spritePosition;
    private Vector3 _deltaPosition;

    private SpriteRenderer _spriteRenderer;
    private PlayerController _rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<PlayerController>();

        jugdeRing.SetActive(false);
        rotateArrow.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        //地面と接しているときだけドラッグアクションを行う
        if(_rigidbody.IsGrounded && _rigidbody.controlEnabled){
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (Input.GetMouseButtonDown(0))
            {
                _clickPosition = mousePosition;
                _spritePosition = _spriteRenderer.transform.localPosition;

                jugdeRing.SetActive(true);
                jugdeRing.transform.localScale = new Vector3(0.5f,0.5f, 0.5f);

            }
            if (Input.GetMouseButton(0))
            {
                _deltaPosition = mousePosition - _clickPosition;

                //判定円に関する処理
                {
                    //アクティブにする
                    if(!jugdeRing.activeInHierarchy){
                        jugdeRing.SetActive(true);
                        jugdeRing.transform.localScale = new Vector3(0.5f,0.5f, 0.5f);
                    }

                    //位置を決める
                    jugdeRing.transform.position = this.transform.position + _ringOffset;

                    //徐々に縮小
                    if(0 < jugdeRing.transform.localScale.x){
                        jugdeRing.transform.localScale -= new Vector3(0.002f,0.002f,0.0f);
                    }
                    //色を変える（現在未反映）
                    if(jugdeRing.transform.localScale.x < jugdeTiming){
                        jugdeRing.GetComponent<Renderer>().material.color = Color.red;
                    }
                }

                //方向矢印に関する処理
                {
                    //アクティブにする
                    if(!rotateArrow.activeInHierarchy){
                        rotateArrow.SetActive(true);
                    }
                    //回転と位置を指定する
                    float radios = 0.25f + _deltaPosition.sqrMagnitude*0.05f;
                    float direction = 180 + Mathf.Atan2(_deltaPosition.y, _deltaPosition.x)* Mathf.Rad2Deg; //操作の方向と飛ぶ方向は180度逆になる
                    rotateArrow.transform.rotation =  Quaternion.Euler(0.0f, 0.0f,direction);
                    rotateArrow.transform.position =  this.transform.position + new Vector3(Mathf.Cos(direction*Mathf.Deg2Rad),Mathf.Sin(direction*Mathf.Deg2Rad),0.0f)*(1+radios);
                    rotateArrow.transform.localScale = new Vector3(radios,0.25f,0.0f);
                }
            }
            else
            {
                _deltaPosition = Vector3.Lerp(_deltaPosition, Vector3.zero, 0.2f);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(jugdeRing.transform.localScale.x < jugdeTiming){
                    jugdeRate =  2.0f;
                }else{
                    jugdeRate =  1.0f;
                }


                Debug.Log("jump: " + _deltaPosition);
                _rigidbody.force = (_deltaPosition * force*jugdeRate);
                _rigidbody.jumpState = PlayerController.JumpState.PrepareToJump;


                jugdeRing.SetActive(false);
                rotateArrow.SetActive(false);
                //_spriteRenderer.transform.localPosition = _spritePosition + _deltaPosition;
            }
        }
        else{
            if(rotateArrow.activeInHierarchy){
                rotateArrow.SetActive(false);
            }
            if(jugdeRing.activeInHierarchy){
                jugdeRing.SetActive(false);
            }
            _deltaPosition = Vector3.zero;
        }



    }
}
