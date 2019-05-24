//
//IKCtrlRightHand.cs
//
//Sample script for IK Control of Unity-Chan's right hand.
//
//2014/06/20 N.Kobayashi
//

using UnityEngine;

namespace UnityChan
{
    [RequireComponent(typeof(Animator))]
    public class IKCtrlRightHand : MonoBehaviour
    {
        private Animator anim;
        public bool isIkActive;
        public float mixWeight = 1.0f;
        public Transform targetObj;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            //Kobayashi
            if (mixWeight >= 1.0f)
                mixWeight = 1.0f;
            else if (mixWeight <= 0.0f)
                mixWeight = 0.0f;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (isIkActive)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, mixWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, mixWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, targetObj.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, targetObj.rotation);
            }
        }

        private void OnGUI()
        {
            var rect1 = new Rect(10, Screen.height - 20, 400, 30);
            isIkActive = GUI.Toggle(rect1, isIkActive, "IK Active");
        }
    }
}