# 📌 케이지
## ✔️ CageMainController
**🎇 Managed by 김광수**

- CageMainController는 MVC 패턴으로 만들어진 Cage의 중심에서 기능별로 나누어진 Controller들을 관리합니다.

- **``InitNewCage(RenderTexture renderTexture, CageBtn cageBtn)``**
 - 새로운 케이지 Model을 생성하고 Model과 Controller들을 생성합니다.

- **``InitLoadCage(RenderTexture renderTexture, CageBtn cageBtn, Cage cage)``**
  - 기존의 케이지 Model을 이용하여 Controller들을 생성합니다.

- **``SelectThisCage()``**
  - 케이지 버튼을 클릭했을때 실행되는 메서드로 각 Controller들이 UI 업데이트 메서드를 호출하도록 합니다.

- **``UpdateCage()``**
  - 날짜가 지나갈떄 Cage의 정보를 업데이트하는 메서드로 슬롯에 있는 카드들의 성공 여부를 결정하고
  - Cage의 상태를 결과 창을 띄우도록 변경합니다.
    
- **``UpdateMonster ()``**
  - 결과 창의 확인 버튼을 누르면 실행되는 메서드로 몬스터에게 결과를 반영하고 그에 따른 처리를 합니다.

- **``AddCard(CardSO so)``**
  - 케이지에 카드가 사용되었을때 카드의 종류와 케이지의 상태에 따라서 카드를 케이지에 추가하거나 다시 반환합니다.
 
- **``UseCardAsync()``**
  - 케이지의 Model에 카드의 효과를 반영합니다.
 
- **``AddMonster(MonsterData monsterData)``**
  - 몬스터 데이터를 받아 Cage의 현재 몬스터로 등록합니다.
 
- **``LoadMonster(MonsterData monsterData)``**
  - 몬스터의 데이터로 몬스터를 생성하고 몬스터 사전정보와 연결시킵니다.

- **``TrySave()``**
  - 모든 결과 창이 닫혀있다면 현재 데이터를 세이브 합니다.
 
- **``RemoveMonster()``**
  - 몬스터를 풀로 되돌리고 케이지에서 제거합니다.

- **``AchievementPopUp(Action action)``**
  - 몬스터의 달성도가 100이 됬을때 팝업 창을 띄웁니다.
    
- **``EscapeMonsterPopUp(Action action)``**
 - 몬스터의 스트레스가 100이 됬을때 팝업 창을 띄웁니다.
