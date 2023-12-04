using NatML.Vision;
using NatML.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace NatML.Visualizers
{
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public class BlazePalmVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Image keypoint;
        [SerializeField]
        private UILineRenderer bones;

        private RawImage rawImage;
        [SerializeField]
        private Image playerImage;
        private AspectRatioFitter aspectFitter;
        [SerializeField]
        private SkillManager _skill = default;
        [SerializeField]
        private List<GameObject> currentHands = new List<GameObject>();
        private int num = 0;
        public List<GameObject> CurrentHands => currentHands;
        public Texture2D image
        {
            get => rawImage.texture as Texture2D;
            set
            {
                rawImage.texture = value;
                aspectFitter.aspectRatio = (float)value.width / (float)value.height;
            }
        }
        // Start is called before the first frame update
        void Awake()
        {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
        }

        private void AddKeyPoint(Vector2 point, int id)
        {
            var prefab = Instantiate(keypoint, transform);
            prefab.gameObject.SetActive(true);

            var prefabTransform = prefab.transform as RectTransform;
            var imageTransform = rawImage.transform as RectTransform;
            if (id == 9 || id == 30)
            {
                var image = prefab.GetComponent<Image>();
                image.sprite = playerImage.sprite;
                image.transform.localScale *= 5;
                image.color = new(255, 255, 255, 1);
            }
            prefabTransform.anchorMin = 0.5f * Vector2.one;
            prefabTransform.anchorMax = 0.5f * Vector2.one;
            prefabTransform.pivot = 0.5f * Vector2.one;
            prefabTransform.anchoredPosition = Rect.NormalizedToPoint(imageTransform.rect, point);

            currentHands.Add(prefab.gameObject);

            //prefab.GetComponent<KeyPoint>().ID = id;
        }
        public void Render(params BlazePalmPredictor.Hand[] hands)
        {
            num = 0;
            foreach (var t in currentHands)
            {
                Destroy(t.gameObject);
            }
            currentHands.Clear();

            var segments = new List<Vector2[]>();
            foreach (var hand in hands)
            {
                foreach (var p in hand.keypoints)
                {
                    AddKeyPoint((Vector2)p, num);
                    num++;
                }
                segments.AddRange(new List<Vector3[]>
            {
                new[]
                {
                    hand.keypoints.wrist,hand.keypoints.thumbCMC,hand.keypoints.thumbMCP,hand.keypoints.thumbIP,
                    hand.keypoints.thumbTip
                },
                new[]
                {
                    hand.keypoints.wrist,hand.keypoints.indexMCP,hand.keypoints.indexPIP,hand.keypoints.indexDIP,
                    hand.keypoints.indexTip
                },
                new[]
                {
                    hand.keypoints.middleMCP,hand.keypoints.middlePIP,hand.keypoints.middleDIP,
                    hand.keypoints.middleTip
                },
                new[]
                {
                    hand.keypoints.ringMCP,hand.keypoints.ringPIP,hand.keypoints.ringDIP, hand.keypoints.ringTip,hand.keypoints.ringTip
                },
                new[]
                {
                    hand.keypoints.wrist,hand.keypoints.pinkyMCP,hand.keypoints.pinkyPIP,hand.keypoints.pinkyDIP,
                    hand.keypoints.pinkyTip
                },
                new[]
                {
                    hand.keypoints.indexMCP,hand.keypoints.middleMCP,hand.keypoints.ringMCP,
                    hand.keypoints.pinkyMCP
                },
            }.Select(points => points.Select(p => GetFixedPoint(p)).ToArray()));

                bones.Points = null;
                bones.Segments = segments;
                //CheckKey();
            }
        }

        private Vector2 GetFixedPoint(Vector2 vec)
        {
            return new Vector2(vec.x * Screen.width - Screen.width / 2, vec.y * Screen.height - Screen.height / 2);
        }
        //public void CheckKey()
        //{
        //    foreach (var key in currentHands)
        //    {
        //        key.gameObject.GetComponent<KeyPoint>().Check();
        //    }
        //    CheckSkill();
        //}
        //public void CheckSkill()
        //{

        //    _skill.CheckActivationSkill();
        //}
    }
}