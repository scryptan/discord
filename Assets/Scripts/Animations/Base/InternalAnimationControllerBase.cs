using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ThinIce.Animations.Base
{
    [RequireComponent(typeof(Animator))]
    public class InternalAnimationControllerBase<T> : MonoBehaviour where T : Enum
    {
        private Animator m_Animator;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            var animationTriggerValues = Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToList();

            var animationTriggerNames = animationTriggerValues
                .Select(x => x.ToString().ToLower())
                .ToList();

            var animatorParameters = m_Animator.parameters
                .Where(x => x.type == AnimatorControllerParameterType.Trigger)
                .ToList();

            var animatorParametersNames = animatorParameters.Select(x => x.name.ToLower()).ToList();

            if (!animationTriggerNames.SequenceEqual(animatorParametersNames))
                throw new ArgumentException(
                    $"Animator on object {gameObject.name} has no triggers {JsonConvert.SerializeObject(animationTriggerNames.Except(animatorParametersNames))} and argument haven't  {JsonConvert.SerializeObject(animatorParametersNames.Except(animationTriggerNames))}");
        }

        public float PlayTrigger(T triggerName)
        {
            if (m_Animator == null)
                return 0;

            m_Animator.SetTrigger(triggerName.ToString());
            return m_Animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }
}