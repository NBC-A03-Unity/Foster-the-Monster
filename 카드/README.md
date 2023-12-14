# 📌 카드
## ✔️ SelectController 
**🎇 Managed by 김광수**

- SelectController는 가중치 랜덤 방식을 사용해 날짜마다 카드를 가중치 풀에 추가하고 카드를 랜덤으로 선택합니다.

- **``AddCardPoolByRarity(CardRarity rarity, CardType type = CardType.EndPoint, int value = 1)``**
  - 원하는 카드의 등급과 타입을 설정하여 카드의 가중치 풀에 추가하고 가중치를 더합니다.

- **``AddCardWeightByID(int cardid, int value)``**
  - DataManager에서 저장하고 있는 카드의 가중치 풀에 가중치를 추가합니다.
  - 가중치 풀은 Dictionary<int,int>로 만들어져 있으며 key = cardid, value = value 입니다.
 
- **``AddCardPool()``**
   - 현재 날짜에 맞는 레이리트의 카드들의 가중치를 늘립니다.

- **``SelectCardId()``**
  - 가중치 랜덤 방식으로 랜덤한 CardID를 뽑습니다.
 
- **``InitCardSetting()``**
  - 처음 게임을 시작할때의 기본세팅입니다. 기본적인 가중치 풀을 세팅하고 카드들을 덱에 추가합니다

- **``SettingCardObj()``**
  - 선택 카드의 오브젝트들을 생성하고 세팅합니다.
    
- **``InitCardSO()``**
  - 생성된 오브젝트들에 랜덤으로 뽑은 CardSO의 정보를 넣어줍니다.
    
- **``OnCardSelectBtn(SelectCard card)``**
  - 카드를 선택했을때 실행되는 메서드로 선택한 카드를 덱에 넣습니다.

- **``ObjectSetFalse()``**
  - 카드 오브젝트들을 오브젝트 풀로 되돌립니다.
 
- **``ClearAllButton()``**
  - 카드 오브젝트에 버튼들을 Clear 시킵니다.
 
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
