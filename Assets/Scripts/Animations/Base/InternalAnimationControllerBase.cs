using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace ThinIce.Animations.Base
{
    [RequireComponent(typeof(Animator))]
    public class InternalAnimationControllerBase<T> : MonoBehaviour where T : Enum
    {
        private Animator m_Animator;
        private readonly Dictionary<T, int> m_TriggerToAnimationNumber = new Dictionary<T, int>();

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

            foreach (var triggerName in animationTriggerValues)
                m_TriggerToAnimationNumber.Add(triggerName,
                    animatorParameters.IndexOf(animatorParameters.Single(x =>
                        string.Equals(x.name, triggerName.ToString(), StringComparison.InvariantCultureIgnoreCase))));
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