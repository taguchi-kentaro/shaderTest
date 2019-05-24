using UnityEngine;

namespace UnityChan
{
    public class FaceUpdate : MonoBehaviour
    {
        private Animator anim;
        public AnimationClip[] animations;

        private float current;
        public float delayWeight;
        public bool isKeepFace;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void OnGUI()
        {
            GUILayout.Box("Face Update", GUILayout.Width(170), GUILayout.Height(25 * (animations.Length + 2)));
            var screenRect = new Rect(10, 25, 150, 25 * (animations.Length + 1));
            GUILayout.BeginArea(screenRect);
            foreach (var animation in animations)
                if (GUILayout.RepeatButton(animation.name))
                    anim.CrossFade(animation.name, 0);
            isKeepFace = GUILayout.Toggle(isKeepFace, " Keep Face");
            GUILayout.EndArea();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
                current = 1;
            else if (!isKeepFace) current = Mathf.Lerp(current, 0, delayWeight);
            anim.SetLayerWeight(1, current);
        }


        //アニメーションEvents側につける表情切り替え用イベントコール
        public void OnCallChangeFace(string str)
        {
            var ichecked = 0;
            foreach (var animation in animations)
                if (str == animation.name)
                {
                    ChangeFace(str);
                    break;
                }
                else if (ichecked <= animations.Length)
                {
                    ichecked++;
                }
                else
                {
                    //str指定が間違っている時にはデフォルトで
                    str = "default@unitychan";
                    ChangeFace(str);
                }
        }

        private void ChangeFace(string str)
        {
            isKeepFace = true;
            current = 1;
            anim.CrossFade(str, 0);
        }
    }
}