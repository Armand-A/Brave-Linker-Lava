using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangMotion : MonoBehaviour
{
    public float speed = 1.0f; 
    public float rotation = 10.0f;
    public bool thrown;
    public bool flyback;
    public Transform target;

    public AudioSource boomerangSfx, deflectSfx, hitSfx, explosionSfx;

    bool deflectFlag;
    int actionVal;
    EnemyMotion targetController;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thrown){
            if (actionVal == 0){
                targetController = target.gameObject.GetComponent<EnemyMotion>();
                if (targetController.monsterLv <= GameFlow.GF.lv){
                    actionVal = 1; //success
                } else {
                    actionVal = 2; //deflect 
                }
                boomerangSfx.Play();
            }

            transform.Rotate(0, 0, rotation, Space.Self);

            var step =  speed * Time.deltaTime; // calculate distance to move
            if (flyback){
                transform.position = Vector3.MoveTowards(transform.position, GameFlow.GF.gameObject.transform.position, step);
                if (Vector3.Distance(transform.position, GameFlow.GF.gameObject.transform.position) < 0.001f){
                    flyback = false;
                    thrown = false;
                    GameFlow.GF.CompleteThrow(deflectFlag);
                    if (deflectFlag){
                        hitSfx.Play();
                    }
                    deflectFlag = false;
                    actionVal = 0;
                    boomerangSfx.Stop();
                }
            } else {
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);

                if (actionVal == 2 && deflectFlag == false){
                    if ((targetController.monsterLv != 999 && Vector3.Distance(transform.position, target.position) < 3.35f)
                    || (Vector3.Distance(transform.position, target.position) < 2f)){
                        targetController.Deflect();
                        StartCoroutine(DeflectSound());
                        deflectFlag = true;
                    }
                }

                if (Vector3.Distance(transform.position, target.position) < 0.001f){
                    flyback = true;
                    if (actionVal == 1){
                        explosionSfx.Play();
                        targetController.Death();
                    }
                }
            }
        }
    }

    IEnumerator DeflectSound(){
        float time = 0.5f;
        if (targetController.monsterLv == 999) time = 0.1f;
        yield return new WaitForSecondsRealtime(time);
        deflectSfx.Play();
    }
}
