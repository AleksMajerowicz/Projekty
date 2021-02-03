using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.WSA;
using UnityEngine.UI;
using TMPro;

public class PlayerFunctionsMangament : MonoBehaviour
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
    private IEnumerator timeMangamentStandStill;
    private IEnumerator timeMangamentActivateSoundsToSkills;

    [Header("Parents")]
    [SerializeField]GameObject[] parentsToSkills;
    [SerializeField]GameObject parentToFireUlt;

    [SerializeField]Vector3 [] skillsPositionInParent;
    Vector3 circleFirePositionInParent;

    [Header("Player Atributes")]
    [SerializeField]float playerSpeed;
    float maxVolumeHp;
    float curretValumeHp;
    float maxVolumeMana;
    float curretValumeMana;

    bool deliveryDamange = false;//Informuje że gracz dostaje obrażenia
    bool wearMana = false;//Informuje czy Mana jest zużywana przez gracza

    bool skillActivation = false;

    [Header("Cancellations")]
    [SerializeField] Slider hpBar;
    [SerializeField] Slider manaBar;
    [SerializeField] TMP_Text hpVolumeIndactior;
    [SerializeField] TMP_Text manaVolumeIndactior;
    
    [Header("ToBars")]
    float curretHpRegenerationTime;
    float curretManaRegenerationTime;
    
    bool isLive = true;

    [Header("Sounds and SoundTracks")]
    [SerializeField] AudioSource audoSorceToSkills;
    [SerializeField] AudioSource audoSorceToSoundTracks;
    [SerializeField] AudioSource audoSorceToUISounds;
    [SerializeField] AudioClip[] soundSkills;
    [SerializeField] AudioClip[] soundToFireUlt;
    [SerializeField] AudioClip soundTracksToUlt;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 1;

        maxVolumeHp = 100;
        maxVolumeMana = 125;
        
        curretValumeHp = maxVolumeHp;
        curretValumeMana = maxVolumeMana;

        hpBar.value = curretValumeHp;
        manaBar.value = curretValumeMana;

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

        skillsPositionInParent[0] = new Vector3(0,0,-0.5f);
        circleFirePositionInParent = new Vector3(0, 0, -23.68473f);
    }

    // Update is called once per frame
    void Update()
    {
        animations.SetFloat("vertical", Input.GetAxis("Vertical"));
        animations.SetFloat("horizontal",Input.GetAxis("Horizontal"));
        if (skillActivation == false && animations.GetCurrentAnimatorStateInfo(0).IsName("Jump") == false)
        {
            transform.position += new Vector3(0, 0, playerSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            transform.position += new Vector3(playerSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false && curretValumeMana >= 25)
            {
                LaunchSkills(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false && curretValumeMana >=25)
            {
                LaunchSkills(2); 
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false && curretValumeMana >= 25)
            {
                LaunchSkills(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (Input.GetKeyDown(KeyCode.Q) == false && ultLoading == false && curretValumeMana >= 25)
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

        SlideHpBar();
        SlideManaBar();
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
            playerSpeed = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animations.SetBool("sprint",false);
            playerSpeed = 1;
        }
    }

    void CallingUlts()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            loadingTimeUlt = 0;
            ultLoading = false;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && curretValumeMana >=50 || ultLoading == true && curretValumeMana >= 50 )
        {
            loadingTimeUlt += Time.deltaTime;
            ultLoading = true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            loadingFireUlt = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && curretValumeMana >= 50 || loadingFireUlt == true && curretValumeMana >= 50)
        {
            loadingFireUlt = true;
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            loadingAirUlt = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && curretValumeMana >= 50 || loadingAirUlt == true && curretValumeMana >= 50)
        {
            loadingAirUlt = true;
        }
        loadingTimeUlt = LaunchUlts(loadingTimeUlt, loadingFireUlt, loadingAirUlt);
    }

    /*<<<Funckja Umożliwiająca Wybór Skilli>>>*/
    public void LaunchSkills(int numberSkill)
    {
        //Insturcja Warunkowa pełni funckje Kuldownu-sprawdza czy Dany efekt jest aktywny,nieżeli nie umożliwa aktywacje wybranego efektu
        if (skillsEffect[numberSkill - 1].activeSelf == false && curretValumeMana >= 25 && loadingTimeUlt < 3)
        {
            skillActivation = true;
            animations.SetTrigger("Skill" + numberSkill);            
            animations.SetFloat("curretMana", curretValumeMana);
            curretValumeMana -= 25;
            //audoSorceToSkills.clip = soundSkills[numberSkill - 1];
            //audoSorceToSkills.Play();
            wearMana = true;

            if (numberSkill == 1)
            {
                skillsEffect[numberSkill - 1].SetActive(true);
                skillActivation = true;

                timeMangamentEffects = ActiwationTimeDust(0.5f, effectsToSkills[numberSkill - 1]);
                StartCoroutine(timeMangamentEffects);

                timeMangamentSkills = DustDuration(1.5f, effectsToSkills[numberSkill - 1]);
                StartCoroutine(timeMangamentSkills);

                timeMangamentActivateSoundsToSkills = ActivateSoundSkill(1);
                StartCoroutine(timeMangamentActivateSoundsToSkills);

                timeMangamentStandStill = StandStillTime(2f, skillsEffect[numberSkill - 1]);
                StartCoroutine(timeMangamentStandStill);

                timeMangamentSkills = SkillDuration(5, numberSkill, skillsEffect[numberSkill - 1],parentsToSkills[numberSkill - 1],skillsPositionInParent[numberSkill - 1]);
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

    /*<<<Funckja Umożliwające Użycie Ultów>>>*/
    float LaunchUlts(float ultTime,bool fireUlt,bool airUlt)
    {
        animations.SetFloat("Ult", ultTime);
        animations.SetBool("Fire Ult", fireUlt);
        animations.SetBool("Air Ult", airUlt);

        if (ultTime > 3 && curretValumeMana >= 50)
        {
            loadingTimeUlt = 0;
            ultLoading = false;
            
            skillActivation = true;
            animations.SetFloat("curretMana", curretValumeMana);
            curretValumeMana -= 50;

            audoSorceToSoundTracks.clip = soundTracksToUlt;
            audoSorceToSoundTracks.Play();
            
            wearMana = true;

            if(fireUlt == true)
            {
                effectsToFireUlt[0].SetActive(true);
                skillActivation = true;

                timeMangamentSkills = ActivationPartOneUlt(1.7f,fireUltv2:fireUlt);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = DurationPartTwoUlt(3,fireUltv3:fireUlt);
                StartCoroutine(timeMangamentSkills);

                timeMangamentStandStill = StandStillTimeUlt(5);
                StartCoroutine(timeMangamentStandStill);

                timeMangamentSkills = ClosingTimeUlt(7,parentToFireUlt);
                StartCoroutine(timeMangamentSkills);
                return 0;
            }
            if(airUlt == true)
            {
                timeMangamentSkills = ActivationPartOneUlt(1.5f,airUltv2:airUlt);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = DurationPartTwoUlt(7.7f,airUltv3:airUlt);
                StartCoroutine(timeMangamentSkills);

                timeMangamentSkills = ClosingTimeUlt(7,parentToFireUlt);
                StartCoroutine(timeMangamentSkills);
                return 0;
            }
            animations.SetFloat("curretMana", curretValumeMana);
            return 0;
        }
        return ultTime;
    }

    IEnumerator StandStillTime(float waitTime,GameObject skill)
    {
        yield return new WaitForSeconds(waitTime);

        skillActivation = false;
        skill.transform.SetParent(null);
        
    }

    IEnumerator StandStillTimeUlt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        skillActivation = false;
    }

    /*<<<IEnumerator pozwala na odpalenie peirwszje częsci Ulta w danym czasie>>>*/
    IEnumerator ActivationPartOneUlt(float waitTime,bool fireUltv2 = false,bool airUltv2 = false)
    {
        yield return new WaitForSeconds(waitTime);
        if (fireUltv2 == true && airUltv2 == false)
        {
            effectsToFireUlt[0].SetActive(false);
            effectsToFireUlt[1].SetActive(true);
            audoSorceToSkills.clip = soundToFireUlt[1];
            audoSorceToSkills.Play();

            animations.SetFloat("curretMana", curretValumeMana);
        }
        if (airUltv2 == true && fireUltv2 == false)
        {
            effectsToAirUlt[0].SetActive(true);

            animations.SetFloat("curretMana", curretValumeMana);
        }

        loadingTimeUlt = 0;
    }

    /*<<<IEnumerator pozwala odpalić drugą część Ulta po danym czasie>>>*/
    IEnumerator DurationPartTwoUlt(float waitTime,bool fireUltv3 = false,bool airUltv3 = false)
    {
        yield return new WaitForSeconds(waitTime);
        animations.SetFloat("curretMana", curretValumeMana);
        if (airUltv3 == true && fireUltv3 == false && curretValumeMana >= 75)
        {
            curretValumeMana -= 75;
            animations.SetFloat("curretMana", curretValumeMana);
            wearMana = true;
            effectsToAirUlt[0].SetActive(false);
            effectsToAirUlt[1].SetActive(true);
        }
        if (fireUltv3 == true && airUltv3 == false && curretValumeMana >= 75)
        {
            curretValumeMana -= 75;
            animations.SetFloat("curretMana", curretValumeMana);
            wearMana = true;
            effectsToFireUlt[2].SetActive(true);
            effectsToFireUlt[2].transform.SetParent(null);
            effectsToFireUlt[2].transform.localScale = Vector3.one;

            audoSorceToSkills.Stop();
            audoSorceToSkills.clip = soundToFireUlt[2];
            audoSorceToSkills.Play();
        }
        else
        {
            skillActivation = false;
        }

        loadingTimeUlt = 0;
    }

    /*<<<wyłącza Wsyzstkie Ulty po danym Czasie>>>*/
    IEnumerator ClosingTimeUlt(float waitTime,GameObject parent)
    {
        yield return new WaitForSeconds(waitTime);

        effectsToFireUlt[1].SetActive(false);
        effectsToFireUlt[2].SetActive(false);
        effectsToFireUlt[2].transform.SetParent(parent.transform);
        effectsToFireUlt[2].transform.localPosition = circleFirePositionInParent;
        effectsToAirUlt[1].SetActive(false);

        loadingTimeUlt = 0;
    }

    /*<<<Funckja wyboru Skilla jak i aktywacji Ulta przez Przycisk>>>*/
    public void SkillsButton(int skillBooton)
    {
        if (skillBooton == 1)
        {
            if (ultLoading == true)
            {
                //Tutaj będzie bool aktywujący ult Ziemi
            }
            else
            {
                LaunchSkills(skillBooton);
            }
        }
        else if (skillBooton == 2)
        {
            if (ultLoading == true)
            {
                loadingAirUlt = true;
            }
            else 
            {
                Debug.Log("Aktualnie nie mam skilla wiatu");
                //LaunchSkills(skillBooton);
            }
        }
        else if (skillBooton == 3)
        {
            if (ultLoading == true)
            {
                loadingFireUlt = true;
            }
            else
            {
                Debug.Log("Aktualnie nie mam skilla ognia");
                //LaunchSkills(skillBooton);
            }
        }
        else if (ultLoading == true)
        {
            if (loadingTimeUlt > 3)
            {
                //Tutaj będzie bool aktywujący ult Wody
            }
            else
            {
                Debug.Log("Aktualnie nie mam skilla wody");
                LaunchSkills(skillBooton);
            }
        }
    }

    public void UltButton()
    {
        ultLoading = !ultLoading;
    }

    /*<<<Uaktywnai danego skilla pobierając jako orguyment GameObject>>>*/
    IEnumerator SkillDuration(float waitTime,int selectionSkills,GameObject skill,GameObject parent,Vector3 positionInParent)
    {
        yield return new WaitForSeconds(waitTime);
        skill.SetActive(false);
        skill.transform.SetParent(parent.transform);
        skill.transform.localPosition = positionInParent;
    }

    /*Jest to IENumerator specialnie pod Skilla Ziemi-aktywuje jego Pył*/
    IEnumerator ActiwationTimeDust(float waitTime,GameObject effects)
    {
        yield return new WaitForSeconds(waitTime);
        effects.SetActive(true);
    }

    /*JEst to IEnumeraotr dezaktywujący pył z skilla ziemi*/
    IEnumerator DustDuration(float waitTime,GameObject effects)
    {
        yield return new WaitForSeconds(waitTime);
        effects.SetActive(false);
    }

    IEnumerator ActivateSoundSkill(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        audoSorceToSkills.Stop();
        audoSorceToSkills.clip = soundSkills[0];
        audoSorceToSkills.Play();
    }

    void SlideHpBar()
    {
        hpVolumeIndactior.text = "" + curretValumeHp + "/" + maxVolumeHp;

        if (curretValumeHp <= maxVolumeHp * 0.75f)
        {
            if (audoSorceToUISounds.isPlaying == false)
            {
                audoSorceToUISounds.Play();
            }
            float heartBeatVolume = 1 - hpBar.value;
            audoSorceToUISounds.volume = heartBeatVolume;
            audoSorceToSoundTracks.volume = Mathf.Clamp(1 - heartBeatVolume,0,0.30f);
        }
        else if (curretValumeHp >= maxVolumeHp * 0.75f)
        {
            if (audoSorceToUISounds.isPlaying == true)
            {
                audoSorceToUISounds.Stop();
            }
        }

        /*Przesuwanie Paska Zdrowia zgodnie z Aktualną Ilością Życia*/
        if (maxVolumeHp > curretValumeHp && deliveryDamange == true && isLive == true)
        {
            hpBar.value = curretValumeHp / maxVolumeHp;
            deliveryDamange = false;
            if (curretValumeHp <= 0)
            {
                curretValumeHp = 0;
                isLive = false;
            }
        }

        /*<<<Regeneracja Życia Gracza>>>*/
        if (maxVolumeHp > curretValumeHp && deliveryDamange == false && isLive == true)
        {
            if (curretHpRegenerationTime > 0.5f)
            {
                curretHpRegenerationTime = 0;
                curretValumeHp += 1;
                hpBar.value = curretValumeHp / maxVolumeHp;

                if (curretValumeHp >= maxVolumeHp)
                {
                    curretValumeHp = maxVolumeHp;
                }
            }
            else
            {
                curretHpRegenerationTime += Time.deltaTime;
            }
        }
    }

    void SlideManaBar()
    {
        manaVolumeIndactior.text = "" + curretValumeMana + "/" + maxVolumeMana;
        /*Przesuwanie paska Many zgodnie z Aktualną Ilością Many*/
        if (maxVolumeMana > curretValumeMana && wearMana == true)
        {
            manaBar.value = curretValumeMana / maxVolumeMana;
            wearMana = false;
        }

        /*<<<Regeneracja Many Gracza>>>*/
        if (maxVolumeMana > curretValumeMana && wearMana == false)
        {
            if (curretManaRegenerationTime > 0.5f)
            {
                curretManaRegenerationTime = 0;

                curretValumeMana += 1;
                manaBar.value = curretValumeMana / maxVolumeMana;

                if (curretValumeMana >= maxVolumeMana)
                {
                    curretValumeMana = maxVolumeMana;
                }
            }
            else
            {
                curretManaRegenerationTime += Time.deltaTime;
            }
        }
    }

    public void Damage(int damage)
    {
        curretValumeHp -= damage;
        deliveryDamange = true;
    }
}
