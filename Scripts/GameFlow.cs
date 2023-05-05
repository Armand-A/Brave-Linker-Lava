using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameFlow : MonoBehaviour
{
    public static GameFlow GF;

    public GameObject EnemyGroup, boomerang, InstallButton, Logo, Canvas;

    public TextMeshProUGUI myLevelText;

    public int lv = 6;

    public int state = 0;

    int clickCount = 0;

    public bool CTAactive;

    Animator Hero_ANM;

    BoomerangMotion boomerangController;

    SpriteRenderer boomerangSprRnd;

    TapHandler installTH;

    void Start()
    {
        GF = this; 

        Hero_ANM = GetComponent<Animator>(); 
        
        boomerangController = boomerang.GetComponent<BoomerangMotion>();

        boomerangSprRnd = boomerang.GetComponent<SpriteRenderer>();

        installTH = InstallButton.GetComponent<TapHandler>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Vector2 c = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (CTAactive){
                Luna.Unity.Playable.InstallFullGame();
            } else if (InstallButton.activeSelf && installTH.checkTouch(c)){
                Luna.Unity.Playable.InstallFullGame();
            } else if (state == 1){
                for (int i = 0; i < EnemyGroup.transform.childCount; i++){
                    if (EnemyGroup.transform.GetChild(i).GetComponent<TapHandler>().checkTouch(c) 
                    && EnemyGroup.transform.GetChild(i).GetComponent<EnemyMotion>().alive){
                        clickCount++;
                        StartThrow(EnemyGroup.transform.GetChild(i).gameObject);
                        state = 2;
                        break;
                    }
                }
            }
        }
    }

    void StartThrow(GameObject target){
        Hero_ANM.Play("HeroThrow");
        boomerangController.target = target.transform;
    }

    public void ReleaseBoomerang(){
        boomerangController.thrown = true;
        boomerangSprRnd.enabled = true;
    }

    public void CompleteThrow(bool deflected){
        boomerangSprRnd.enabled = false;
        if (deflected){
            Hero_ANM.Play("HeroHit");
            StartCoroutine(WaitForHurt());
            
        } else {
            Hero_ANM.Play("HeroIdle");
            FinishStep();
        }
    }

    void FinishStep(){
        if (clickCount >= 3){
            state = 3;
            OpenCTA();
        } else {
            if (clickCount == 1){
                InstallButton.SetActive(true);
            }
            state = 1;
        }
    }

    public void UpdateLevel(int gainLv){
        lv += gainLv;
        myLevelText.text = lv.ToString();
    }

    void OpenCTA(){
        CTAactive = true;
        Logo.SetActive(false);
        InstallButton.SetActive(false);
        Canvas.SetActive(false);
        transform.position = new Vector3 (0.5f, 0, 0);
        transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
        GetComponent<SpriteRenderer>().sortingOrder = 25;
        SceneManager.LoadScene("CTA", LoadSceneMode.Additive);
    }

    IEnumerator WaitForHurt(){
        yield return new WaitForSecondsRealtime(0.3f);
        FinishStep();
    }
}
