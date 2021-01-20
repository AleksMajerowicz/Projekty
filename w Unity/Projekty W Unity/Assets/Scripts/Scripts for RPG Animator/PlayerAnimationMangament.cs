using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerAnimationMangament : MonoBehaviour
{
    Animator animations;
    float loadingTimeUlt = 0;
    bool ultLoading = false;
    bool loadingFireUlt = false;
    bool loadingAirUlt = false;
    [Header("Skills and EffectsToSkills")]
    [SerializeField]GameObject[] skillsEffect;
    [SerializeField] GameObject[] effectsToSkills;
    [Header("Ults")]
    [SerializeField] GameObject[] effectsToFireUlt;
    [SerializeField] GameObject[] effectsToAirUlt;
    private IEnumerator timeMangamentSkills;
    private IEnumerator timeMangamentEffects;
    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponent<Animator>();

        for (int i = 0; i < skillsEffect.Length; i++)
        {
            skillsEffect[i].SetActive(false);
        }

        for (int i = 0; i < skillsEffect.Length; i++)
        {
            effectsToSkills[i].SetActive(false);
        }

        for (int i = 0; i < effectsToFireUlt.Length; i++)
        {
            effectsToFireUlt[i].SetActive(false);
        }

        for (int i = 0; i < effectsToAirUlt.Length; i++)
        {
            effectsToAirUlt[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ultLoading);
        animations.SetFloat("vertical", Input.GetAxis("Vertical"));
        animations.SetFloat("horizontal",Input.GetAxis("Horizontal"));
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false)
            {
                LaunchSkills(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false)
            {
                LaunchSkills(2); 
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false)
            {
                LaunchSkills(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false)
            {
                LaunchSkills(4);
            }
        }
        CallingUlts();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Sprint();
    }

    public void Jump()
    {
        animations.SetTrigger("jumpig");
    }

    public void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animations.SetBool("sprint", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animations.SetBool("sprint",false);
        }
    }

    void CallingUlts()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            loadingTimeUlt = 0;
            ultLoading = false;
        }
        else if (Input.GetKeyDown(KeyCode.Q) || ultLoading == true)
        {
            loadingTimeUlt += Time.deltaTime;
            ultLoading = true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            loadingFireUlt = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || loadingFireUlt == true)
        {
            loadingFireUlt = true;
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            loadingAirUlt = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || loadingAirUlt == true)
        {
            loadingAirUlt = true;
        }
        loadingTimeUlt =LaunchUlts(loadingTimeUlt, loadingFireUlt,loadingAirUlt);
    }
    void LaunchSkills(int numberSkill)
    {
        if (skillsEffect[numberSkill - 1].activeSelf == false)
        {
            animations.SetTrigger("Skill" + numberSkill);
            if (numberSkill == 1)
            {
                skillsEffect[numberSkill - 1].SetActive(true);

                timeMangamentEffects = ActiwationTimeDust(0.5f, effectsToSkills[numberSkill - 1]);
                StartCoroutine(timeMangamentEffects);

                timeMangamentSkills = DustDuration(1.5f, effectsToSkills[numberSkill - 1]);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = SkillDuration(5, numberSkill, skillsEffect[numberSkill - 1]);
                StartCoroutine(timeMangamentSkills);
            }
            if (numberSkill == 2)
            {
                skillsEffect[numberSkill - 1].SetActive(true);
            }
            if (numberSkill == 3)
            {
                skillsEffect[numberSkill - 1].SetActive(true);

                if (numberSkill == 4)
                {
                    skillsEffect[numberSkill - 1].SetActive(true);
                }
            }
        }
    }
    float LaunchUlts(float ultTime,bool fireUlt,bool airUlt)
    {
        animations.SetFloat("Ult", ultTime);
        animations.SetBool("Fire Ult", fireUlt);
        animations.SetBool("Air Ult", airUlt);
        Debug.Log(animations.GetBool("Air Ult"));
        if (ultTime > 3)
        {
            if(fireUlt == true)
            {
                effectsToFireUlt[0].SetActive(true);
                
                timeMangamentSkills = ActivationPartOneUlt(1.7f,fireUlt);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = DurationPartTwoUlt(2.5f, animations);
                StartCoroutine(timeMangamentSkills);
                
                timeMangamentSkills = ClosingTimeUlt(7);
                StartCoroutine(timeMangamentSkills);
                return 0;
            }
            if(airUlt == true)
            {
                timeMangamentSkills = ActivationPartOneUlt(1.5f,airUltv2:airUlt);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = DurationPartTwoUlt(7.7f,animations);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = ClosingTimeUlt(7);
                StartCoroutine(timeMangamentSkills);
                return 0;
            }
            return 0;
        }
        return ultTime;
    }

    IEnumerator ActivationPartOneUlt(float waitTime,bool fireUltv2 = false,bool airUltv2 = false)
    {
        yield return new WaitForSeconds(waitTime);
        loadingTimeUlt = 0;
        if (fireUltv2 == true && airUltv2 == false)
        {
            effectsToFireUlt[0].SetActive(false);
            effectsToFireUlt[1].SetActive(true);
        }
        if (airUltv2 == true && fireUltv2 == false)
        {
            effectsToAirUlt[0].SetActive(true);
        }
    }

    IEnumerator DurationPartTwoUlt(float waitTime,Animator animationUltName)
    {
        yield return new WaitForSeconds(waitTime);
        loadingTimeUlt = 0;
        if (animationUltName.GetBool("Air Ult") == true && animationUltName.GetBool("Fire Ult") == false)
        {
            effectsToAirUlt[0].SetActive(false);
            effectsToAirUlt[1].SetActive(true);
        }
        if (animationUltName.GetBool("Fire Ult") == true && animationUltName.GetBool("Air Ult") == false)
        {
            effectsToFireUlt[2].SetActive(true);
        }
    }

    IEnumerator ClosingTimeUlt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        effectsToFireUlt[1].SetActive(false);
        effectsToFireUlt[2].SetActive(false);
        effectsToAirUlt[1].SetActive(false);
        loadingTimeUlt = 0;
    }

    IEnumerator SkillDuration(float waitTime,int selectionSkills,GameObject skill)
    {
        yield return new WaitForSeconds(waitTime);
        skill.SetActive(false);
    }

    IEnumerator ActiwationTimeDust(float waitTime,GameObject effects)
    {
        yield return new WaitForSeconds(waitTime);
        effects.SetActive(true);
    }

    IEnumerator DustDuration(float waitTime,GameObject effects)
    {
        yield return new WaitForSeconds(waitTime);
        effects.SetActive(false);
    }
}
