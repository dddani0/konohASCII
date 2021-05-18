using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class HyugASCIIBossAnimation : MonoBehaviour
{
    public Animator hAnim;
    public GameObject hitboxTrigger;

    private void Start()
    {
        hAnim = GetComponentInParent<Animator>();
        hitboxTrigger.SetActive(false);
    }

    public void DebugComboTry()
    {
        StartCoroutine(AttackTryIEnum());
    }

    IEnumerator AttackTryIEnum()
    {
        hitboxTrigger.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitboxTrigger.SetActive(false);
    }

    public void ActivateKaitenHitbox()
    {
        GetComponentInParent<HyugASCIIBossScript>().trigramRotation.SetActive(true);
    }
    public void DeActivateKaitenHitbox()
    {
        GetComponentInParent<HyugASCIIBossScript>().trigramRotation.SetActive(false);
        GetComponent<Animator>().ResetTrigger("KaitenDefense");
    }
    public void AttackWithAPunch()
    {
        GetComponentInParent<HyugASCIIBossScript>().AttackWithPunch();
    }

    public void ResetTriggerForCombo()
    {
        hAnim.ResetTrigger("ComboTrigger");
        hAnim.ResetTrigger("Try_Attack");
    }

    public void ActivateSecondPhase()
    {
        GetComponentInParent<HyugASCIIBossScript>().byakugan.GetComponent<Animator>().SetTrigger("Phase_Two");
    }

    public void MinusValue()
    {
        GetComponentInParent<HyugASCIIBossScript>().byakuganCount--;
    }

    public void UnFreezeConst()
    {
        GetComponentInParent<HyugASCIIBossScript>().hBody.constraints = RigidbodyConstraints2D.None;
        GetComponentInParent<HyugASCIIBossScript>().hBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void FreezeConst()
    {
        GetComponentInParent<HyugASCIIBossScript>().hBody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void ResetStagger()
    {
        hAnim.ResetTrigger("Stagger");
    }

    public void LoseToUzumASCII()
    {
        GetComponentInParent<HyugASCIIBossScript>().prefObj.GetComponent<PreferenceStorage>().bossValues = 2;
    }
    public void DeActivateByakugan()
    {
        GetComponentInParent<HyugASCIIBossScript>().byakugan.GetComponent<Animator>().SetTrigger("Deact");
    }
    public void ToSummary()
    {
        StartCoroutine(ToSummaryDelay());
    }
    IEnumerator ToSummaryDelay()
    {
        GetComponentInParent<HyugASCIIBossScript>().gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(6);
    }

    public void FootStepSFX()
    {
        GetComponentInParent<HyugASCIIBossScript>().gameManager.GetComponent<Manager>().sound_Effects[15].Play();
    }

    public void ThrowSFX()
    {
        GetComponentInParent<HyugASCIIBossScript>().gameManager.GetComponent<Manager>().sound_Effects[6].Play();
    }

    public void TrigramSFX()
    {
        GetComponentInParent<HyugASCIIBossScript>().gameManager.GetComponent<Manager>().sound_Effects[26].Play();
    }

    public void GruntSFX()
    {
        GetComponentInParent<HyugASCIIBossScript>().gameManager.GetComponent<Manager>().sound_Effects[29].Play();
    }
}
