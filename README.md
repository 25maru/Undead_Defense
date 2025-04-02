# Undead_Defense



<!-- 프로젝트 소개 -->
## 📝 프로젝트 소개


- **에디터 버전** : 2022.3.17f1 LTS
- **렌더 파이프라인** : Built-In


[![unity][unity.com]][unity-url]
[![github][github.com]][github-url]
[![markdown][markdownguide.org]][markdownguide-url]



<!-- 구현 사항 -->
## ⚙️ 구현 사항



<!-- 필수 -->
#### ✅ 필수



<!-- 도전 -->
#### 🔥 도전



<!-- 마크다운 링크 & 이미지 -->
[unity.com]: https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=unity&logoColor=black
[unity-url]: https://unity.com/kr
[github.com]: https://img.shields.io/badge/Github-222222?style=for-the-badge&logo=github&logoColor=white
[github-url]: https://github.com
[markdownguide.org]: https://img.shields.io/badge/Markdown-000000?style=for-the-badge&logo=markdown&logoColor=white
[markdownguide-url]: https://markdownguide.org
## 팀원  
팀장 박지원 (레벨디자인,게임진행)  
팀원 김민성 (플레이어, 아군병력)  
팀원 김현교 (건물)  
팀원 최재혁 (적)  
팀원 신소현 (UI)  

<!-- 구현 사항 -->
## 게임 설명
#### 기본 진행
밤 마다 몰려오는 몬스터들을 잡고 돈을 얻어 건물을 강화, 방어 병력을 소환하여 메인건물을 지키는 게임

<details>
<summary>플레이어 기본 조작</summary>

#### 플레이어 기본 조작 
![Image](https://github.com/user-attachments/assets/23fef22e-8c3a-488a-bf8f-76ecf108420b)  
WASD이동  
![Image](https://github.com/user-attachments/assets/b8948269-3d54-43e9-8e2b-7956d0acd505)  
범위내 적 자동 공격(정지상태에서만 공격)

</details>

<details>
<summary>건물</summary>

#### 건물  
건물 건설방법 : 해당 건물 터 에서 Space키 꾹눌러 돈지불, 건설  

##### 건물 종류
![Image](https://github.com/user-attachments/assets/d7e1ebe8-720a-4088-a9c4-e2a251f6736e)  
자원생산건물 : 낮이 될때마다 돈 획득, 강화로 획득량증가  
  
![Image](https://github.com/user-attachments/assets/8e32f13b-49bd-43f4-ba8e-ac59e8d504ba) ![Image](https://github.com/user-attachments/assets/ae20e778-36ff-4824-b591-b6962d93d265)  
방어타워 : 범위내 적 자동 공격, 3단계 강화가능  
  
![Image](https://github.com/user-attachments/assets/ad66afcc-178d-4320-8c4f-d13d07f56d95)![Image](https://github.com/user-attachments/assets/7f441e2f-401c-4006-9b3d-c799481c4116)  
병력생산 건물 : 일정시간마다 병력소환, 강화상태에 따라 최대 병력수 증가  
  
  </details>

<details>
  <summary>병력(아군AI)</summary>

#### 병력 지휘  

![Image](https://github.com/user-attachments/assets/0ed8de5a-8f7c-47ff-b9c5-6cb38c362a92)  
Shift키를 눌러 범위내 병력을 지휘부대로 지정  
  
![Image](https://github.com/user-attachments/assets/ae9c1101-498f-4546-b3b3-c65fb926a620)  
Shift입력 지속시 플레이어 따라다님(적 무시)  
  
![Image](https://github.com/user-attachments/assets/b5bec939-44b9-4391-a6ec-508b563aa746)  
Shift입력 종료시 종료시점 플레이어 위치로 이동 (이동중 적 감지 시 추적,공격)  
  
![Image](https://github.com/user-attachments/assets/a5d19af5-d016-4f95-ab86-0a4fff8c0835)  
H키 입력시 부대 유닛 위치사수 (이동중지, 적 추적X, 공격범위 적 공격O)  
  
</details>



