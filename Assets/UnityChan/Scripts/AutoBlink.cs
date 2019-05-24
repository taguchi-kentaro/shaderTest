//
//AutoBlink.cs
//オート目パチスクリプト
//2014/06/23 N.Kobayashi
//

using System.Collections;
using UnityEngine;

namespace UnityChan
{
    public class AutoBlink : MonoBehaviour
    {
        private Status eyeStatus; //現在の目パチステータス
        public float interval = 3.0f; // ランダム判定のインターバル

        public bool isActive = true; //オート目パチ有効
        private bool isBlink; //目パチ管理用
        public float ratio_Close = 85.0f; //閉じ目ブレンドシェイプ比率
        public float ratio_HalfClose = 20.0f; //半閉じ目ブレンドシェイプ比率

        [HideInInspector] public float
            ratio_Open;

        public SkinnedMeshRenderer ref_SMR_EL_DEF; //EL_DEFへの参照
        public SkinnedMeshRenderer ref_SMR_EYE_DEF; //EYE_DEFへの参照

        public float threshold = 0.3f; // ランダム判定の閾値

        public float timeBlink = 0.4f; //目パチの時間
        private float timeRemining; //タイマー残り時間
        private bool timerStarted; //タイマースタート管理用

        private void Awake()
        {
            //ref_SMR_EYE_DEF = GameObject.Find("EYE_DEF").GetComponent<SkinnedMeshRenderer>();
            //ref_SMR_EL_DEF = GameObject.Find("EL_DEF").GetComponent<SkinnedMeshRenderer>();
        }


        // Use this for initialization
        private void Start()
        {
            ResetTimer();
            // ランダム判定用関数をスタートする
            StartCoroutine("RandomChange");
        }

        //タイマーリセット
        private void ResetTimer()
        {
            timeRemining = timeBlink;
            timerStarted = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!timerStarted)
            {
                eyeStatus = Status.Close;
                timerStarted = true;
            }

            if (timerStarted)
            {
                timeRemining -= Time.deltaTime;
                if (timeRemining <= 0.0f)
                {
                    eyeStatus = Status.Open;
                    ResetTimer();
                }
                else if (timeRemining <= timeBlink * 0.3f)
                {
                    eyeStatus = Status.HalfClose;
                }
            }
        }

        private void LateUpdate()
        {
            if (isActive)
                if (isBlink)
                    switch (eyeStatus)
                    {
                        case Status.Close:
                            SetCloseEyes();
                            break;
                        case Status.HalfClose:
                            SetHalfCloseEyes();
                            break;
                        case Status.Open:
                            SetOpenEyes();
                            isBlink = false;
                            break;
                    }
                //Debug.Log(eyeStatus);
        }

        private void SetCloseEyes()
        {
            ref_SMR_EYE_DEF.SetBlendShapeWeight(6, ratio_Close);
            ref_SMR_EL_DEF.SetBlendShapeWeight(6, ratio_Close);
        }

        private void SetHalfCloseEyes()
        {
            ref_SMR_EYE_DEF.SetBlendShapeWeight(6, ratio_HalfClose);
            ref_SMR_EL_DEF.SetBlendShapeWeight(6, ratio_HalfClose);
        }

        private void SetOpenEyes()
        {
            ref_SMR_EYE_DEF.SetBlendShapeWeight(6, ratio_Open);
            ref_SMR_EL_DEF.SetBlendShapeWeight(6, ratio_Open);
        }

        // ランダム判定用関数
        private IEnumerator RandomChange()
        {
            // 無限ループ開始
            while (true)
            {
                //ランダム判定用シード発生
                var _seed = Random.Range(0.0f, 1.0f);
                if (!isBlink)
                    if (_seed > threshold)
                        isBlink = true;
                // 次の判定までインターバルを置く
                yield return new WaitForSeconds(interval);
            }
        }


        private enum Status
        {
            Close,
            HalfClose,
            Open //目パチの状態
        }
    }
}