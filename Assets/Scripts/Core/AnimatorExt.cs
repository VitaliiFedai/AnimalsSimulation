using UnityEngine;

namespace AnimalSimulation.Tools
{
    public static class AnimatorExt
    {
        public static bool HasParameter(this Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasParameter(this Animator animator, int paramHash)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.nameHash == paramHash)
                {
                    return true;
                }
            }
            return false;
        }
    }
}