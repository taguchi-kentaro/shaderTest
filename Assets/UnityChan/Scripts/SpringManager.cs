﻿//
//SpingManager.cs for unity-chan!
//
//Original Script is here:
//ricopin / SpingManager.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//
//Revised by N.Kobayashi 2014/06/24
//           Y.Ebata
//

using System.Reflection;
using UnityEngine;

namespace UnityChan
{
    public class SpringManager : MonoBehaviour
    {
        public AnimationCurve dragCurve;

        public float dragForce;

        //Kobayashi
        // DynamicRatio is paramater for activated level of dynamic animation 
        public float dynamicRatio = 1.0f;
        public SpringBone[] springBones;
        public AnimationCurve stiffnessCurve;

        //Ebata
        public float stiffnessForce;

        private void Start()
        {
            UpdateParameters();
        }

#if UNITY_EDITOR
        private void Update()
        {
            //Kobayashi
            if (dynamicRatio >= 1.0f)
                dynamicRatio = 1.0f;
            else if (dynamicRatio <= 0.0f)
                dynamicRatio = 0.0f;
            //Ebata
            UpdateParameters();
        }
#endif
        private void LateUpdate()
        {
            //Kobayashi
            if (dynamicRatio != 0.0f)
                for (var i = 0; i < springBones.Length; i++)
                    if (dynamicRatio > springBones[i].threshold)
                        springBones[i].UpdateSpring();
        }

        private void UpdateParameters()
        {
            UpdateParameter("stiffnessForce", stiffnessForce, stiffnessCurve);
            UpdateParameter("dragForce", dragForce, dragCurve);
        }

        private void UpdateParameter(string fieldName, float baseValue, AnimationCurve curve)
        {
#if UNITY_EDITOR
            var start = curve.keys[0].time;
            var end = curve.keys[curve.length - 1].time;
            //var step	= (end - start) / (springBones.Length - 1);

            var prop = springBones[0].GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);

            for (var i = 0; i < springBones.Length; i++) //Kobayashi
                if (!springBones[i].isUseEachBoneForceSettings)
                {
                    var scale = curve.Evaluate(start + (end - start) * i / (springBones.Length - 1));
                    prop.SetValue(springBones[i], baseValue * scale);
                }
#endif
        }
    }
}