using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace PromptTimeline
{

    [CustomEditor(typeof(PromptControl))]
    public class PromptControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var promptControl = target as PromptControl;
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // GUILayout.BeginArea (Rect (0,0,100,150));
            if (GUILayout.Button("    Create prompt by text    "))
            {
                promptControl.Init();
                if (promptControl.playableDirector != null || promptControl.trackName != null)
                {
                    promptControl.InitTimeline();
                }
            }
            // GUILayout.EndArea ();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();   
            base.OnInspectorGUI();

        }
    }

    [Serializable]
    public enum MovePromptVector
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField]
    public enum PromptType
    {
        Lyric,
        Slide
    }
    [Serializable]
    public class PromptValues
    {
        public int start = 0;
        public int end = 0;
        public Vector2 anchoredPosition;
        public string text;
    }

    public class PromptControl : MonoBehaviour
    {
        [SerializeField] private PromptType promptType;
        [SerializeField] private Rect offsetRect;
        [SerializeField] private RectTransform parent;
        [SerializeField] private TMP_FontAsset tmpFontAsset;
        [SerializeField] private Color textColor;
        [SerializeField] private int fontSizeMin = 80;
        [SerializeField] private int fontSizeMax = 120;

        [SerializeField] private FontStyles fontStyles;
        [SerializeField] private Vector2 resolution = new Vector2(1920, 1080);
        [SerializeField] private Vector2 textPadding = new Vector2(30, 30);
        [SerializeField] private Vector2 promptMargin = new Vector2(0, 200);
        [SerializeField] private TextAlignmentOptions textAlignmentOptions;
        [SerializeField] private MovePromptVector movePromptVector;
        [SerializeField][TextArea(10,300)] private string promptText;
        [SerializeField] [TextArea(2,4)]private List<string> textList;
        [SerializeField] private List<PromptValues> promptValues = new List<PromptValues>();
        [SerializeField] private List<TextMeshProUGUI> textMeshProUguis;
        [SerializeField] private int currentPromptIndex = 0;
        private Image _image;
        [SerializeField]private RectTransform textParent;
        public List<PromptValues> PromptValues => promptValues;
        public List<string> TextList => textList;

        [SerializeField] public PlayableDirector playableDirector;
        [SerializeField] public string trackName; 
        // public string text => subtitleTMP.text;
        // Start is called before the first frame update
        void Start()
        {

        }
        [ContextMenu("Init")]
        public void Init()
        {
            var lines = promptText.Split("\n").ToList();
            promptValues.Clear();
            textList.Clear();
            var sb = new StringBuilder();
            var index = 0;

            if (textParent == null)
            {
                textParent = new GameObject("text").AddComponent<RectTransform>();
            } 
            textParent.SetParent(parent);
         
            textParent.anchoredPosition = Vector2.zero;

            textParent.sizeDelta = parent.sizeDelta; 
           
            promptValues.Add(new PromptValues());
            promptValues.Last().start = index;
            foreach (var line in lines)
            {
                
                Debug.Log(line);
                if (string.IsNullOrEmpty(line))
                {
                    promptValues.Last().end = (index - 1);
                    promptValues.Add(new PromptValues());
                    promptValues.Last().start = index;
                }
                else
                {
                    // var currentLine = sb.ToString();
                    textList.Add(line);
                    index++;
                } 
               
            }

            promptValues.Last().end = index - 1;



            foreach (var textMeshProUgui in textMeshProUguis)
            {
                DestroyImmediate(textMeshProUgui.gameObject);
            }

            textMeshProUguis.Clear();

            var pos = offsetRect.position;
            foreach (var promptValue in promptValues)
            {
                sb.Clear();
                var textMeshPro = new GameObject().AddComponent<TextMeshProUGUI>();
                textMeshPro.gameObject.transform.localPosition = Vector3.zero;
                textMeshPro.transform.SetParent(textParent.transform);
                textMeshPro.font = tmpFontAsset;
               
                textMeshPro.fontSizeMin = fontSizeMin;
                textMeshPro.fontSizeMax = fontSizeMax;
                textMeshPro.enableAutoSizing = true;
                textMeshPro.autoSizeTextContainer = false;
                textMeshPro.rectTransform.sizeDelta =resolution-(textPadding*2);
                textMeshPro.fontStyle = fontStyles;
                textMeshPro.enableWordWrapping = true;
                textMeshPro.alignment = textAlignmentOptions;
                textMeshPro.color = textColor;
                
                for (int i = promptValue.start; i < promptValue.end+1; i++)
                {
                    sb.AppendLine(textList[i]);
                }

                textMeshPro.text = sb.ToString();
                textMeshPro.name = $"{promptValue.start} - {promptValue.end}: {textList[promptValue.start]}...";
                textMeshPro.ForceMeshUpdate();

                var vec = new Vector2();
                if (movePromptVector == MovePromptVector.Up) vec = new Vector2(0, 1);
                if (movePromptVector == MovePromptVector.Down) vec = new Vector2(0, -1);
                if (movePromptVector == MovePromptVector.Right) vec = new Vector2(1, 0);
                if (movePromptVector == MovePromptVector.Left) vec = new Vector2(-1, 0);
                if (promptType == PromptType.Lyric)
                {

                    if (textMeshProUguis.Count > 0) pos += new Vector2(textMeshPro.preferredWidth/2f, textMeshPro.preferredHeight/2f)*vec;
               
                    textMeshPro.rectTransform.anchoredPosition3D = new Vector3(pos.x, pos.y, 0);

                    pos += new Vector2(textMeshPro.preferredWidth/2f+promptMargin.x, textMeshPro.preferredHeight / 2f+promptMargin.y)*vec;     
                }
                else
                {
                    textMeshPro.rectTransform.anchoredPosition3D = new Vector3(pos.x, pos.y, 0);
                    promptValue.anchoredPosition = pos;
                    pos += parent.rect.size* vec;
                }
               
                // pos += promptMargin*vec;
                // textMeshPro.rectTransform.position = new Vector3(textMeshPro.rectTransform.position.x, textMeshPro.rectTransform.position.y, 0);
                textMeshPro.rectTransform.ForceUpdateRectTransforms();
                textMeshProUguis.Add(textMeshPro);

            }

        }

        

        [ContextMenu("InitTimeline")]
        public void InitTimeline()
        {
     
            
            IEnumerable<TrackAsset> tracks = (playableDirector.playableAsset as TimelineAsset).GetOutputTracks();
            // // 指定名称のトラックを抜き出す
            // var timelineAsset = playableDirector.playableAsset as TimelineAsset;
            // timelineAsset.get
            
            TrackAsset track = tracks.FirstOrDefault(x => x.name == trackName);
        
            if (track != null)
            {
                // トラック内のクリップ一覧を取得
                var clips = track.GetClips().ToList();
                // 指定名称のクリップを抜き出す
               
                var index = 0;
                TimelineClip prevClip = null;
                foreach (var promptValue in promptValues)
                {
                    var isNewClip = index >= clips.Count;
                    var clip =  index< clips.Count ? clips[index] : track.CreateDefaultClip();

                    if (isNewClip)
                    {
                        clip.duration = 3;
                        clip.start = prevClip != null ? prevClip.end : 0;
                    }
                   
                    var promptClip = clip.asset as PromptTimelineClip;
                    clip.displayName = textList[promptValue.start];
                    promptClip.template.lines = textList;
                    promptClip.template.position = promptValue.anchoredPosition;
                    promptClip.template.startIndex = promptValue.start;
                    promptClip.template.endIndex = promptValue.end;
                    promptClip.template.promptIndex = index;
                    prevClip = clip;
                    index++;
                }
            }
        
            

        }

        public void UpdateText(int index, string text)
        {
            textMeshProUguis[index].text = text;
        }
        public void Move(int index, float process)
        {
            if(index < 0 || promptValues.Count <= index) return;
            
            if (index != currentPromptIndex)
            {
                var next = Math.Clamp(index, 0, promptValues.Count);
                // var prev = Math.Clamp(index-1, 0, promptValues.Count);
                textParent.anchoredPosition = -Vector2.Lerp(promptValues[currentPromptIndex].anchoredPosition, promptValues[next].anchoredPosition, process);
            }
            else
            {
                textParent.anchoredPosition = promptValues[currentPromptIndex].anchoredPosition;
            }
        }
        
        public void SetPosition(Vector2 anchoredPosition)
        {
           
            textParent.anchoredPosition = anchoredPosition;
        }

        public void DisableSubtitle()
        {
            // background.gameObject.SetActive(false);
            // subtitleTMP.gameObject.SetActive(false);
        }

        public void EnableSubtitle()
        {
            // background.gameObject.SetActive(true);
            // subtitleTMP.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
