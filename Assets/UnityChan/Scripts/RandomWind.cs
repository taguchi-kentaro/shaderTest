//
//RandomWind.cs for unity-chan!
//
//Original Script is here:
//ricopin / RandomWind.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//
//修正2014/12/20
//風の方向変化/重力影響を追加.
//

using System.Collections;
using UnityEngine;

namespace UnityChan
{
    public class RandomWind : MonoBehaviour
    {
        public float gravity = 0.98f; //重力の強さ.
        public float interval = 5.0f; // ランダム判定のインターバル.

        private bool isMinus; //風方向反転用.
        public bool isWindActive;
        private SpringBone[] springBones;
        public float threshold = 0.5f; // ランダム判定の閾値.
        public float windPower = 1.0f; //風の強さ.


        // Use this for initialization
        private void Start()
        {
            springBones = GetComponent<SpringManager>().springBones;
            StartCoroutine("RandomChange");
        }


        // Update is called once per frame
        private void Update()
        {
            var force = Vector3.zero;
            if (isWindActive)
            {
                if (isMinus)
                    force = new Vector3(Mathf.PerlinNoise(Time.time, 0.0f) * windPower * -0.001f, gravity * -0.001f, 0);
                else
                    force = new Vector3(Mathf.PerlinNoise(Time.time, 0.0f) * windPower * 0.001f, gravity * -0.001f, 0);

                for (var i = 0; i < springBones.Length; i++) springBones[i].springForce = force;
            }
        }

        private void OnGUI()
        {
            var rect1 = new Rect(10, Screen.height - 40, 400, 30);
            isWindActive = GUI.Toggle(rect1, isWindActive, "Random Wind");
        }

        // ランダム判定用関数.
        private IEnumerator RandomChange()
        {
            // 無限ループ開始.
            while (true)
            {
                //ランダム判定用シード発生.
                var _seed = Random.Range(0.0f, 1.0f);

                if (_seed > threshold) //_seedがthreshold以上の時、符号を反転する.
                    isMinus = true;
                else
                    isMinus = false;

                // 次の判定までインターバルを置く.
                yield return new WaitForSeconds(interval);
            }
        }
    }
}