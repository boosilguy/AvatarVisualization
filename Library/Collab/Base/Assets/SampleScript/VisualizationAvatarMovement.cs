using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationAvatarMovement : MonoBehaviour
{
    /********************** Public Variables ************************/
    [Header("Avatar Animator")]
    public Animator avatarAnimator; // Target Avatar Animation.
    [Space(10)]
    [Header("Visualization")]
    [Tooltip("반드시 SkinnedMeshRenderer Component를 포함해야함.")]
    public GameObject realtimeVisualization; // Realtime Visualization Game Object.
    [Tooltip("반드시 SkinnedMeshRenderer Component를 포함해야함.")]
    public GameObject stackedVisualization; // Stacked Visualization Game Object.
    [Space(5)]
    [Tooltip("반드시 SkinnedMeshRenderer Component를 포함해야함.")]
    public GameObject pitchVisualization; // Pitch Visualization Game Object.
    [Tooltip("반드시 SkinnedMeshRenderer Component를 포함해야함.")]
    public GameObject yawVisualization; // Yaw Visualization Game Object.
    [Tooltip("반드시 SkinnedMeshRenderer Component를 포함해야함.")]
    public GameObject rollVisualization; // Roll Visualization Game Object.
    [Space(10)]
    [Header("Highlighting Color")]
    public Color highlightColor; // Realtime Mesh Visual Color.
    public Color stackedHighlightColor; // Stacked Mesh Visual Color.
    public Color pitchHighlightColor; // Pitch Mesh Visual Color.
    public Color yawHighlightColor; // Yaw Mesh Visual Color.
    public Color rollHighlightColor; // Roll Mesh Visual Color.
    [Space(10)]
    [Header("Interpolation Parameters")]
    [Tooltip("이 크기만큼 Bone들의 Euler Rotation 값을 저장할 Queue 크기가 결정됨.")]
    public int interpolationSize = 0; // Mesh의 움직임을 선형 보간할 Frame 길이.
    [Tooltip("모든 Bone들의 보간된 Euler Rotation 값은 해당 값을 넘을 수 없음. Visualization Color를 강하게 표현하고 싶으면, 낮은 값을 사용하면 됨.")]
    [Range(5, 30)]
    public int standardLerpAngle = 20; // Lerp Max Value.
    /****************************************************************/


    /********************** Default Variables ***********************/
    float[] avatarBoneRotation; // 총 Bone 개수만큼 Rotation을 저장할 배열.
    float[] aBRPitch; // avatarBoneRotation Pitch.
    float[] aBRYaw; // avatarBoneRotation Yaw.
    float[] aBRRoll; // avatarBoneRotation Roll.
    float currentAnimLength = 0; // 현재 선택된 Animation 길이.
    float currentAnimTime = 0; // 현재 재생되고 있는 Animation의 Play Time.
    Queue<float>[] interpolation; // 각 Bone의 보간된 Euler Rotation을 담는 저장 공간 (Realtime).
    Queue<float>[] stackedInterpolation; // 각 Bone의 보간된 Euler Rotation을 담는 저장 공간 (Stacked).
    Queue<float>[] pitchInterpolation; // 각 Bone의 보간된 Euler Rotation을 담는 저장 공간 (Pitch).
    Queue<float>[] yawInterpolation; // 각 Bone의 보간된 Euler Rotation을 담는 저장 공간 (Yaw).
    Queue<float>[] rollInterpolation; // 각 Bone의 보간된 Euler Rotation을 담는 저장 공간 (Roll).
    Vector3[] prevBonesVectors; // 직전 프레임의 Bone 방향 벡터들을 저장하는 배열.
    Color32[] color32s; // Realtime Visualization의 Vertex Color를 담는 배열.
    Color32[] stackedColor32s; // Stacked Visualization의 Vertex Color를 담는 배열.
    Color32[] pitchColor32s; // Pitch Visualization의 Vertex Color를 담는 배열.
    Color32[] yawColor32s; // Yaw Visualization의 Vertex Color를 담는 배열.
    Color32[] rollColor32s; // Roll Visualization의 Vertex Color를 담는 배열.
    SkinnedMeshRenderer skinnedMeshRenderer; // Animated Avatar의 SkinnedMeshRenderer Component.
    SkinnedMeshRenderer realtimeSMR; // Realtime Visualization의 SkinnedMeshRenderer Component.
    SkinnedMeshRenderer stackedSMR; // Stacked Visualization의 SkinnedMeshRenderer Component.
    SkinnedMeshRenderer pitchSMR; // Pitch Visualization의 SkinnedMeshRenderer Component.
    SkinnedMeshRenderer yawSMR; // Yaw Visualization의 SkinnedMeshRenderer Component.
    SkinnedMeshRenderer rollSMR; // Roll Visualization의 SkinnedMeshRenderer Component.
    Mesh mesh; // Animated Avatar의 Mesh.
    Mesh realtimeVisMesh; // Realtime Visualization의 Mesh.
    Mesh stackedVisMesh; // Stacked Visualization의 Mesh.
    Mesh pitchVisMesh; // Pitch Visualization의 Mesh.
    Mesh yawVisMesh; // Yaw Visualization의 Mesh.
    Mesh rollVisMesh; // Roll Visualization의 Mesh.
    /****************************************************************/


    /********************** Private Variables ***********************/

    /****************************************************************/
    
    void Start ()
    {
        // Init SkinnedMeshRenderer Component Infos & Other Variables
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        mesh = skinnedMeshRenderer.sharedMesh;

        avatarBoneRotation = new float[skinnedMeshRenderer.bones.Length];
        prevBonesVectors = new Vector3[skinnedMeshRenderer.bones.Length];
        aBRPitch = new float[skinnedMeshRenderer.bones.Length];
        aBRYaw = new float[skinnedMeshRenderer.bones.Length];
        aBRRoll = new float[skinnedMeshRenderer.bones.Length];

        realtimeSMR = realtimeVisualization.GetComponent<SkinnedMeshRenderer>();
        realtimeVisMesh = realtimeSMR.sharedMesh;

        stackedSMR = stackedVisualization.GetComponent<SkinnedMeshRenderer>();
        stackedVisMesh = stackedSMR.sharedMesh;

        pitchSMR = pitchVisualization.GetComponent<SkinnedMeshRenderer>();
        pitchVisMesh = pitchSMR.sharedMesh;
        yawSMR = yawVisualization.GetComponent<SkinnedMeshRenderer>();
        yawVisMesh = yawSMR.sharedMesh;
        rollSMR = rollVisualization.GetComponent<SkinnedMeshRenderer>();
        rollVisMesh = rollSMR.sharedMesh;

        // Init Vertex Color Container
        color32s = new Color32[mesh.boneWeights.Length];
        for(int i=0; i<mesh.boneWeights.Length; i++)
        {
            color32s[i] = new Color32(0, 0, 0, 255);
        }

        stackedColor32s = new Color32[mesh.boneWeights.Length];
        for(int i=0; i<mesh.boneWeights.Length; i++)
        {
            stackedColor32s[i] = new Color32(0, 0, 0, 255);
        }

        pitchColor32s = new Color32[mesh.boneWeights.Length];
        for(int i=0; i<mesh.boneWeights.Length; i++)
        {
            pitchColor32s[i] = new Color32(0, 0, 0, 255);
        }
        yawColor32s = new Color32[mesh.boneWeights.Length];
        for(int i=0; i<mesh.boneWeights.Length; i++)
        {
            yawColor32s[i] = new Color32(0, 0, 0, 255);
        }
        rollColor32s = new Color32[mesh.boneWeights.Length];
        for(int i=0; i<mesh.boneWeights.Length; i++)
        {
            rollColor32s[i] = new Color32(0, 0, 0, 255);
        }

        // Init Interpolation Queue
        interpolation = new Queue<float>[skinnedMeshRenderer.bones.Length];
        for(int i=0; i<skinnedMeshRenderer.bones.Length; i++)
        {
            interpolation[i] = new Queue<float>(interpolationSize);
        }

        // Init Stacked Interpolation Queue
        stackedInterpolation = new Queue<float>[skinnedMeshRenderer.bones.Length];
        for(int i=0; i<skinnedMeshRenderer.bones.Length; i++)
        {
            stackedInterpolation[i] = new Queue<float>();
        }

        pitchInterpolation = new Queue<float>[skinnedMeshRenderer.bones.Length];
        for(int i=0; i<skinnedMeshRenderer.bones.Length; i++)
        {
            pitchInterpolation[i] = new Queue<float>();
        }
        yawInterpolation = new Queue<float>[skinnedMeshRenderer.bones.Length];
        for(int i=0; i<skinnedMeshRenderer.bones.Length; i++)
        {
            yawInterpolation[i] = new Queue<float>();
        }
        rollInterpolation = new Queue<float>[skinnedMeshRenderer.bones.Length];
        for(int i=0; i<skinnedMeshRenderer.bones.Length; i++)
        {
            rollInterpolation[i] = new Queue<float>();
        }

        currentAnimLength = avatarAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Init Viusalization
        InitRealtimeVisualization();
        InitStackedVisualization();

        InitPitchVisualization();
        InitYawVisualization();
        InitRollVisualization();
    }

    void Update()
    {
        // 현재 프레임의 Bone 정보가 담긴 배열 Init.
        Transform[] bones = skinnedMeshRenderer.bones;
        Transform[] stackedBones = stackedSMR.bones;
        
        if (prevBonesVectors[0] != null)
        {
            // i번째 Bone에 대한 Visualization 갱신
            for(int i=0; i<bones.Length; i++)
            {
                Vector3 currentBoneVector = bones[i].parent.InverseTransformDirection(bones[i].forward);

                // 직전 프레임의 Bone 방향 벡터와 현재 프레임의 Bone 방향 벡터 사이의 각도를 저장 (Local Forward Vector).
                avatarBoneRotation[i] = Vector3.Angle(currentBoneVector, prevBonesVectors[i]);
                
                // Detailed (Pitch, Yaw, Roll)
                aBRPitch[i] = Vector3.Angle(new Vector3(0, currentBoneVector.y, currentBoneVector.z), new Vector3(0, prevBonesVectors[i].y, prevBonesVectors[i].z));
                aBRYaw[i] = Vector3.Angle(new Vector3(currentBoneVector.x, 0, currentBoneVector.z), new Vector3(prevBonesVectors[i].x, 0, prevBonesVectors[i].z));
                aBRRoll[i] = Vector3.Angle(new Vector3(currentBoneVector.x, currentBoneVector.y, 0), new Vector3(prevBonesVectors[i].x, prevBonesVectors[i].y, 0));

                Debug.Log(aBRRoll[i]);

                if (interpolation[i].Count == interpolationSize)
                {
                    // Realtime의 Bone Euler Rotation를 저장한 Queue 갱신.
                    interpolation[i].Dequeue();
                    interpolation[i].Enqueue(avatarBoneRotation[i]);

                    // Realtime Visualization 렌더링을 위한 Color 배열 갱신. 
                    Highlight(mesh, i, GetAvgValueInQueue(interpolation[i]), color32s, highlightColor);

                    // Animation이 진행 중이라면, Stacked Visualization에 Euler Rotation 값 추가.
                    if(currentAnimTime < currentAnimLength)
                    {
                        stackedInterpolation[i].Enqueue(avatarBoneRotation[i]);
                        pitchInterpolation[i].Enqueue(aBRPitch[i]);
                        yawInterpolation[i].Enqueue(aBRYaw[i]);
                        rollInterpolation[i].Enqueue(aBRRoll[i]);
                    }
                    else
                    {
                        // Animation이 끝난 직후 프레임이라면, Stacked Visualization 렌더링을 위한 Color 배열 갱신.
                        if(currentAnimTime < currentAnimLength + 0.1f) // 매우 위험한 로직이니, 나중에 수정할 것 (상수 시간 더한 것을 수정할 것).
                        {
                            Highlight(mesh, i, GetAvgValueInQueue(stackedInterpolation[i]), stackedColor32s, 10, stackedHighlightColor);

                            Highlight(mesh, i, GetAvgValueInQueue(pitchInterpolation[i]), pitchColor32s, 10, pitchHighlightColor);
                            Highlight(mesh, i, GetAvgValueInQueue(yawInterpolation[i]), yawColor32s, 10, yawHighlightColor);
                            Highlight(mesh, i, GetAvgValueInQueue(rollInterpolation[i]), rollColor32s, 10, rollHighlightColor);
                        }

                        // i가 마지막 Bone이라면, Stacked Visualization 컬러 렌더링.
                        if (i == bones.Length-1)
                        {
                            stackedVisMesh.colors32 = stackedColor32s;
                            pitchVisMesh.colors32 = pitchColor32s;
                            yawVisMesh.colors32 = yawColor32s;
                            rollVisMesh.colors32 = rollColor32s;
                        }
                    }

                    // i가 마지막 Bone이라면, Realtime Visualization 컬러 렌더링.
                    if (i == bones.Length-1)
                    {
                        realtimeVisMesh.colors32 = color32s;
                    }

                }
                else
                {
                    interpolation[i].Enqueue(avatarBoneRotation[i]);
                }
            }
        }

        // 직전 프레임의 각 Bone 방향 벡터 배열을 갱신.
        for (int i=0; i<bones.Length; i++)
        {
            prevBonesVectors[i] = bones[i].parent.InverseTransformDirection(bones[i].forward);
        }
        
        // 현재 Animation 시간 갱신.
        currentAnimTime += Time.deltaTime;
    }

    /********************** Public Method ************************/

    /// <summary>
    /// Realtime Visualization의 Visualization Init 함수.
    /// </summary>
    public void InitRealtimeVisualization()
    {
        Color32[] cols = new Color32[mesh.colors32.Length];
        for(int i = 0; i<mesh.colors32.Length; i++)
        {
            cols[i] = new Color32(255, 255, 255, 255);
        }
        realtimeVisMesh.colors32 = cols;
    }

    /// <summary>
    /// Stacked Visualization 저장 공간 및 Visualization Init 함수.
    /// </summary>
    public void InitStackedVisualization()
    {
        foreach(Queue<float> q in stackedInterpolation)
        {
            q.Clear();
        }

        Color32[] cols = new Color32[mesh.colors32.Length];
        for(int i = 0; i<mesh.colors32.Length; i++)
        {
            cols[i] = new Color32(255, 255, 255, 255);
        }
        stackedVisMesh.colors32 = cols;
    }

    /// <summary>
    /// Pitch Visualization 저장 공간 및 Visualization Init 함수.
    /// </summary>
    public void InitPitchVisualization()
    {
        foreach(Queue<float> q in pitchInterpolation)
        {
            q.Clear();
        }

        Color32[] cols = new Color32[mesh.colors32.Length];
        for(int i = 0; i<mesh.colors32.Length; i++)
        {
            cols[i] = new Color32(255, 255, 255, 255);
        }
        pitchVisMesh.colors32 = cols;
    }

    /// <summary>
    /// Yaw Visualization 저장 공간 및 Visualization Init 함수.
    /// </summary>
    public void InitYawVisualization()
    {
        foreach(Queue<float> q in yawInterpolation)
        {
            q.Clear();
        }

        Color32[] cols = new Color32[mesh.colors32.Length];
        for(int i = 0; i<mesh.colors32.Length; i++)
        {
            cols[i] = new Color32(255, 255, 255, 255);
        }
        yawVisMesh.colors32 = cols;
    }

    /// <summary>
    /// Roll Visualization 저장 공간 및 Visualization Init 함수.
    /// </summary>
    public void InitRollVisualization()
    {
        foreach(Queue<float> q in rollInterpolation)
        {
            q.Clear();
        }

        Color32[] cols = new Color32[mesh.colors32.Length];
        for(int i = 0; i<mesh.colors32.Length; i++)
        {
            cols[i] = new Color32(255, 255, 255, 255);
        }
        rollVisMesh.colors32 = cols;
    }

    /// <summary>
    /// Animation Init 함수.
    /// </summary>
    public void InitAnimationLength(float length)
    {
        currentAnimLength = length;
        currentAnimTime = 0;
    }

    /*************************************************************/


    /********************** Default Method ***********************/

    /// <summary>
    /// float 제네릭 타입 Queue의 평균 반환 함수.
    /// </summary>
    /// <param name="q">평균을 구할 Queue.</param>
    /// <returns>
    /// 해당 메소드는 float 타입을 반환.
    /// </returns>
    float GetAvgValueInQueue(Queue<float> q)
    {
        float sum = 0;
        foreach(float value in q)
        {
            sum += value;
        }
        return sum/(q.Count);
    }

    /// <summary>
    /// Mesh Highlighting 함수.
    /// </summary>
    /// <param name="mesh">Animated Avatar의 Mesh 정보</param>
    /// <param name="idx">참조할 Bone의 Index (Animated Avatar와 동일한 구조의 Bone 정보여야 함).</param>
    /// <param name="angle">참조된 idx번째 Bone의 Euler Rotation.</param>
    /// <param name="col">Highlight될 색상 정보들을 저장할 Color32 타입 배열.</param>
    /// <returns></returns>
    void Highlight(Mesh mesh, int idx, float angle, Color32[] col, Color highlightColor) {
        BoneWeight[] weights = mesh.boneWeights;
        
        for (int i=0; i<col.Length; ++i) {
            if (weights[i].boneIndex0 == idx && weights[i].weight0 > 0)
            {
                col[i] = Color32.Lerp(Color.white, highlightColor, angle/standardLerpAngle);
            }
        }
    }

    /// <summary>
    /// Mesh Highlighting 함수. Lerp 함수의 t 파라미터를 설정할 수 있도록 설계된 오버로딩 함수.
    /// </summary>
    /// <param name="mesh">Animated Avatar의 Mesh 정보</param>
    /// <param name="idx">참조할 Bone의 Index (Animated Avatar와 동일한 구조의 Bone 정보여야 함).</param>
    /// <param name="angle">참조된 idx번째 Bone의 Euler Rotation.</param>
    /// <param name="col">Highlight될 색상 정보들을 저장할 Color32 타입 배열.</param>
    /// <param name="standard">Lerp 함수의 t 파라미터를 결정하는 float 타입 변수.</param>
    void Highlight(Mesh mesh, int idx, float angle, Color32[] col, float standard, Color highlightColor) {
        BoneWeight[] weights = mesh.boneWeights;
        
        for (int i=0; i<col.Length; ++i) {
            if (weights[i].boneIndex0 == idx && weights[i].weight0 > 0)
            {
                col[i] = Color32.Lerp(Color.white, highlightColor, angle/standard);
            }
        }
    }

    /*************************************************************/
    

    /********************** Private Method ***********************/

    /*************************************************************/
}
