using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    /********************** Public Variables ************************/
    [Header("Log Text")]
    public Text infoRenderTxt;
    [Space(10)]
    [Header("Avatar GameObject")]
    public GameObject avatarObject; // Avatar GameObject.
    public GameObject animAvatarObject; // Animating Avatar GameObject.
    public Vector3 initPos = new Vector3(0, 0, -6.1325f);
    public Vector3 initRot = new Vector3(0, 180, 0);
    [Space(10)]
    [Header("Avatar Animator")]
    public Animator avatarAnimator; // Target Avatar Animatior.
    public Animator animAnimator; // Animating Avatar Animatior.
    [Space(10)]
    [Header("Visualization Avatar Movement")]
    public VisualizationAvatarMovement vam;
    [Space(10)]
    [Header("Empty Objs for Mod")]
    public GameObject realAndStackedMod;
    public GameObject detailMod;
    [Space(10)]
    [Header("Mod Info Txt")]
    public GameObject realStackedModTxt;
    public GameObject detailedModTxt;
    /****************************************************************/


    /********************** Default Variables ***********************/
    bool isItRealStackedMod = false;
    /****************************************************************/


    /********************** Private Variables ***********************/

    /****************************************************************/

    void Start() {
        SetVisualizationMod();
    }

    /********************** Public Method ************************/

    /// <summary>
    /// HappyDance Animation Play Button 함수.
    /// </summary>
    public void GetDownHappyDance ()
    {
        InitAvatar ();
        avatarAnimator.SetBool("@HappyDance", true);
        avatarAnimator.SetBool("@RusianRoulette", false);
        avatarAnimator.SetBool("@SadDance", false);
        avatarAnimator.SetBool("@Breakdance", false);
        avatarAnimator.SetBool("@Idle", false);

        animAnimator.SetBool("@HappyDance", true);
        animAnimator.SetBool("@RusianRoulette", false);
        animAnimator.SetBool("@SadDance", false);
        animAnimator.SetBool("@Breakdance", false);
        animAnimator.SetBool("@Idle", false);
        
        vam.InitAnimationLength(avatarAnimator.runtimeAnimatorController.animationClips[0].length);
        vam.InitStackedVisualization();
        vam.InitPitchVisualization();
        vam.InitYawVisualization();
        vam.InitRollVisualization();
        
        infoRenderTxt.text = "HappyMood";
    }

    /// <summary>
    /// RusianRoulette Animation Play Button 함수.
    /// </summary>
    public void GetDownRusianRoulette ()
    {
        InitAvatar ();
        avatarAnimator.SetBool("@HappyDance", false);
        avatarAnimator.SetBool("@RusianRoulette", true);
        avatarAnimator.SetBool("@SadDance", false);
        avatarAnimator.SetBool("@Breakdance", false);
        avatarAnimator.SetBool("@Idle", false);

        animAnimator.SetBool("@HappyDance", false);
        animAnimator.SetBool("@RusianRoulette", true);
        animAnimator.SetBool("@SadDance", false);
        animAnimator.SetBool("@Breakdance", false);
        animAnimator.SetBool("@Idle", false);
        
        vam.InitAnimationLength(avatarAnimator.runtimeAnimatorController.animationClips[1].length);
        vam.InitStackedVisualization();
        vam.InitPitchVisualization();
        vam.InitYawVisualization();
        vam.InitRollVisualization();
        
        infoRenderTxt.text = "RusianRoulette";
    }

    /// <summary>
    /// SadDance Animation Play Button 함수.
    /// </summary>
    public void GetDownSadDance ()
    {
        InitAvatar ();
        avatarAnimator.SetBool("@HappyDance", false);
        avatarAnimator.SetBool("@RusianRoulette", false);
        avatarAnimator.SetBool("@SadDance", true);
        avatarAnimator.SetBool("@Breakdance", false);
        avatarAnimator.SetBool("@Idle", false);

        animAnimator.SetBool("@HappyDance", false);
        animAnimator.SetBool("@RusianRoulette", false);
        animAnimator.SetBool("@SadDance", true);
        animAnimator.SetBool("@Breakdance", false);
        animAnimator.SetBool("@Idle", false);
        
        vam.InitAnimationLength(avatarAnimator.runtimeAnimatorController.animationClips[2].length);
        vam.InitStackedVisualization();
        vam.InitPitchVisualization();
        vam.InitYawVisualization();
        vam.InitRollVisualization();
        
        infoRenderTxt.text = "SadMood";
    }

    /// <summary>
    /// Breakdance Animation Play Button 함수.
    /// </summary>
    public void GetDownBreakdance ()
    {
        InitAvatar ();
        avatarAnimator.SetBool("@HappyDance", false);
        avatarAnimator.SetBool("@RusianRoulette", false);
        avatarAnimator.SetBool("@SadDance", false);
        avatarAnimator.SetBool("@Breakdance", true);
        avatarAnimator.SetBool("@Idle", false);

        animAnimator.SetBool("@HappyDance", false);
        animAnimator.SetBool("@RusianRoulette", false);
        animAnimator.SetBool("@SadDance", false);
        animAnimator.SetBool("@Breakdance", true);
        animAnimator.SetBool("@Idle", false);
        
        vam.InitAnimationLength(avatarAnimator.runtimeAnimatorController.animationClips[4].length);
        vam.InitStackedVisualization();
        vam.InitPitchVisualization();
        vam.InitYawVisualization();
        vam.InitRollVisualization();
        
        infoRenderTxt.text = "Breakdance";
    }

    /// <summary>
    /// Animation Reset (Idle Animation Play) Button 함수.
    /// </summary>
    public void GetDownResetAnim ()
    {
        avatarAnimator.SetBool("@HappyDance", false);
        avatarAnimator.SetBool("@RusianRoulette", false);
        avatarAnimator.SetBool("@SadDance", false);
        avatarAnimator.SetBool("@Breakdance", false);
        avatarAnimator.SetBool("@Idle", true);
        
        animAnimator.SetBool("@HappyDance", false);
        animAnimator.SetBool("@RusianRoulette", false);
        animAnimator.SetBool("@SadDance", false);
        animAnimator.SetBool("@Breakdance", false);
        animAnimator.SetBool("@Idle", true);

        vam.InitAnimationLength(avatarAnimator.runtimeAnimatorController.animationClips[3].length);
        vam.InitStackedVisualization();
        vam.InitPitchVisualization();
        vam.InitYawVisualization();
        vam.InitRollVisualization();

        InitAvatar ();

        infoRenderTxt.text = "Idle";
    }

    /// <summary>
    /// Mod 전환 메소드.
    /// </summary>
    public void SetVisualizationMod ()
    {
        if (isItRealStackedMod == true)
        {
            infoRenderTxt.text = "Detailed Mod";
            detailedModTxt.SetActive(true);
            realStackedModTxt.SetActive(false);
            foreach (SkinnedMeshRenderer smr in realAndStackedMod.GetComponentsInChildren<SkinnedMeshRenderer>())
                smr.enabled = false;
            foreach (SkinnedMeshRenderer smr in detailMod.GetComponentsInChildren<SkinnedMeshRenderer>())
                smr.enabled = true;

            isItRealStackedMod = false;
        }
        else
        {
            infoRenderTxt.text = "Realtime/Stacked Mod";
            detailedModTxt.SetActive(false);
            realStackedModTxt.SetActive(true);
            foreach (SkinnedMeshRenderer smr in realAndStackedMod.GetComponentsInChildren<SkinnedMeshRenderer>())
                smr.enabled = true;
            foreach (SkinnedMeshRenderer smr in detailMod.GetComponentsInChildren<SkinnedMeshRenderer>())
                smr.enabled = false;

            isItRealStackedMod = true;
        }
        
    }

    /*************************************************************/


    /********************** Default Method ***********************/

    /// <summary>
    /// Avatar Position 및 Euler Rotation Init 함수.
    /// </summary>
    void InitAvatar ()
    {
        avatarObject.transform.position = initPos;
        avatarObject.transform.eulerAngles = initRot;

        animAvatarObject.transform.position = initPos;
        animAvatarObject.transform.eulerAngles = initRot;
    }

    /*************************************************************/
}
