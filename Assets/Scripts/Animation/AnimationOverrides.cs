using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    [SerializeField] private GameObject character = null;
    [SerializeField] private SO_AnimationType[] soAnimationTypeArray = null;

    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;
    private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey;

    // Start is called before the first frame update
    private void Start()
    {
        animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();

        foreach (SO_AnimationType item in soAnimationTypeArray)
        {
            animationTypeDictionaryByAnimation.Add(item.animationClip, item);
        }

        animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();

        foreach (SO_AnimationType item in soAnimationTypeArray)
        {
            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            animationTypeDictionaryByCompositeAttributeKey.Add(key, item);
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributes)
    {
        foreach (CharacterAttribute characterAttribute in characterAttributes)
        {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            string animatorSOAssetName = characterAttribute.characterPart.ToString();
            animatorSOAssetName = animatorSOAssetName[0].ToString().ToUpper() + animatorSOAssetName.Substring(1);

            Debug.Log(animatorSOAssetName);

            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

            foreach (Animator animator in animatorsArray)
            {

                if (animator.name == animatorSOAssetName)
                {
                    currentAnimator = animator;
                    break;
                }
            }

            Debug.Log(currentAnimator.runtimeAnimatorController.ToString());

            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);


            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);

            foreach (AnimationClip animationClip in animationsList)
            {
                SO_AnimationType sO_AnimationType;
                bool foundAnimation = animationTypeDictionaryByAnimation.TryGetValue(animationClip, out sO_AnimationType);

                if (foundAnimation)
                {
                    string key = characterAttribute.characterPart.ToString()
                        + characterAttribute.partVariantColour.ToString()
                        + characterAttribute.partVariantType.ToString()
                        + sO_AnimationType.animationName.ToString();

                    SO_AnimationType swapSO_AnimationType;
                    bool foundSwapAnimation = animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out swapSO_AnimationType);

                    if (foundSwapAnimation)
                    {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;
                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip));
                    }
                }
            }

            aoc.ApplyOverrides(animsKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
