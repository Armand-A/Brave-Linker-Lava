using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyMotion : MonoBehaviour
{
    public GameObject monsterLevelText;
    public Vector3 textOffset = new Vector3(0.5f, 0.95f, 0);

    public bool left;
    public float speedMod = 0.75f;
    public float floatMod = 0.04f;

    bool floatDown; 
    int floatCycle = 0;

    public bool alive = true;
    bool deactivated;

    public int monsterLv;

    SpriteRenderer monsterSprRnd, platformSprRnd;
    Animator monsterANM;

    void Start()
    {
        monsterSprRnd = GetComponent<SpriteRenderer>();
        platformSprRnd = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        monsterANM = GetComponent<Animator>();
        int.TryParse(monsterLevelText.GetComponent<TextMeshProUGUI>().text, out monsterLv);
    }

    void FixedUpdate()
    {
        if (alive){
            monsterLevelText.transform.position = transform.position + textOffset;
        }

        if (!deactivated && !GameFlow.GF.CTAactive){
            if (left){
                transform.Translate(Vector3.left * Time.deltaTime * speedMod);
            } else {
                transform.Translate(Vector3.right * Time.deltaTime * speedMod);
            }

            if (floatDown){
                transform.Translate(Vector3.down * Time.deltaTime * floatMod);
            } else {
                transform.Translate(Vector3.up * Time.deltaTime * floatMod);
            }

            if (floatCycle >= 20){
                floatCycle = 0;
                floatDown = !floatDown;
            } else {
                floatCycle += 1;
            }
        }
    }

    public void Death(){
        alive = false;
        monsterSprRnd.enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        GameFlow.GF.UpdateLevel(monsterLv);
        monsterLevelText.SetActive(false);
    }

    public void Deflect(){
        monsterANM.Play("Deflect");
    }

    void OnTriggerEnter2D(Collider2D col){
        if (alive){
            monsterSprRnd.flipX = !monsterSprRnd.flipX;
            platformSprRnd.flipX = !platformSprRnd.flipX;
            left = !left;
        } else {
            deactivated = true;
        }
    }
}
