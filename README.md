<h1>Avatar Visualization (2021)</h1>
<p>해당 페이지에서 Avatar Visualization 프로젝트의 분석 프로그램을 소개합니다.</p>

<h2>Tech Stack</h2>
<ul>
  <li>Programming Language</li>
  <ul>
    <li><img src="https://img.shields.io/badge/C Sharp-239120?style=flat-square&logo=c-sharp&logoColor=white"/></li>
  </ul>
  <li>Toolkit</li>
  <ul>
    <li><img src="https://img.shields.io/badge/Unity-000000?style=flat-square&logo=Unity&logoColor=white"/></li>
    <li><img src="https://img.shields.io/badge/OptiTrack-000000?style=flat-square&logo=Unity&logoColor=white"/></li>
    <li><img src="https://img.shields.io/badge/Mixamo-FF0000?style=flat-square&logo=Adobe&logoColor=white"/></li>
  </ul>
</ul>

<h2>Summary</h2>
<p><b>사람의 감정에 따라, 몸 움직임이 다를까?</b> 졸업 논문을 마무리하고, 여유가 되어 진행한 프로젝트입니다. 교내 다른 연구자분과 대학 병원측 교수와 함께 진행했던 과제로 기억합니다. 정확히 어떠한 레퍼런스인지 기억하지 못하지만, 정서 불안이 있는 환자에게 다양한 감정의 분위기를 유도하고 '가장 많이 사용할 신체 부위를 알려주세요'라는 응답을 받았던 것으로 기억합니다. 이를 기반으로, 모션 캡처를 통해서 사람의 동작을 레코딩한 스켈레톤의 부위별 움직임 빈도를 Visualization하는 프로그램을 구현하였습니다. </p>

<h2>Detail</h2>
<p>OptiTrack의 Motive의 Avatar 데이터 Format은 Unity의 Humanoid Avatar Format에 적합합니다. 특히, Avatar 움직임을 부위별로 하이라이팅하기 위해서 스켈레톤의 부위별 Rigging Weight에 따라, Mesh의 rgb 값을 조절하였습니다. 이를테면, 어깨를 돌리는 동작이라면, 어깨 스켈레톤과 관련된 (Weight가 높은) Mesh (e.g. 팔의 상, 하박, 어깨)는 짙은 색상으로 하이라이트됩니다.</p>
<p>Humanoid Avatar의 Mesh에 액세스하기 위해서, SkinnedMeshRenderer Component를 사용하였습니다. 해당 Component의 sharedMesh 프로퍼티를 이용해, Mesh들의 Vertex에 접근할 수 있었습니다. 앞서 언급한 것과 같이, Mesh의 Rigging Weight를 이용하여 해당 Mesh가 현재 움직임에 어느 정도 관여되었는지 확인합니다. 만약 해당 Mesh가 Avatar의 현 움직임에 충분히 관여되는 Mesh라면, 이 Mesh에 접근한 Vertex의 색상을 조건에 맞게 채색합니다. </p>
<p>위와 같은 원리를 이용해, 메인 화면에서 두 가지 방식으로 Visualization 됩니다. 직전 10 프레임 동안의 평균 움직임을 렌더링하는 Realtime Visualization과 애니메이션이 끝난 직후의 총 움직임을 렌더링하는 Stacked Visualization으로 나뉩니다.</p>
<p>또한, 좀 더 자세한 분석을 위해, 스켈레톤 움직임을 Yaw, Pitch, Roll로 구분하여 하이라이트하는 옵션을 추가하였습니다.</p>

<h2>Behind Story</h2>
<p>초기, 긍정적인 정서의 춤과 부정적인 정서의 춤을 찾다가 도저히 원하던 자료를 찾을 수 없었습니다. 해서, 연구실에 취미로 춤 좀 춘다는 후배 석사생을 붙잡아서 도움을 받아냈습니다. 연구실 나오기 직전에, 해당 프로그램은 과제 1년차의 프로토타입이 된다고 들었습니다. 부디 좋은 성과가 있었으면 합니다.</p>

<h2>프로젝트 영상</h2>

[![Unity-MotionCapture](http://img.youtube.com/vi/YxoRnT_WZvE/0.jpg)](http://www.youtube.com/watch?v=YxoRnT_WZvE "AvatarVisualization")

