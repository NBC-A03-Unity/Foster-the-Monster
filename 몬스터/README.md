# 📌 몬스터
## ✔️ Monster
**🎇 Managed by 최수용**

- Monster 메서드들은 몬스터의 생명주기 관리, 상태 변화, 이벤트 통지, 데미지 및 포획 처리 등 몬스터의 동작에 필수적인 부분들을 담당합니다.

- **``Awake()``**
  - 객체가 초기화될 때 호출되며, 몬스터 관련 컴포넌트를 가져오고, 센서를 설정합니다.

- **``OnEnable()``**
  - 객체가 활성화될 때 호출되며, 몬스터 데이터를 초기화하고 Init 메서드를 호출합니다.

- **``Init()``**
  - 몬스터의 초기화 로직을 처리합니다. 센서를 초기화하고, 이벤트 리스너를 설정하며, 몬스터의 기본 모드와 상태를 설정합니다.

- **``AddListenerToSensor()``**
  - 센서에 자극을 감지하거나 감지하지 못할 때 호출될 이벤트 리스너를 추가합니다.

- **``RemoveListenerToSensor()``**
  - 센서에서 이벤트 리스너를 제거합니다.

- **``AddStimulusList()``**
  - 자극 리스트에 새로운 자극을 추가하고, 이 리스트가 변경되었음을 알리는 이벤트를 발생시킵니다.

- **``RemoveStimulusList()``**
  - 자극 리스트에서 특정 자극을 제거하고, 현재 자극이 해당 자극과 같다면 null로 설정합니다. 또한, 이 리스트가 변경되었음을 알리는 이벤트를 발생시킵니다.

- **``BeAttacked()``**
  - 몬스터가 공격을 받았을 때 호출되어 몬스터의 모드를 변경하고, 애니메이션과 사운드 효과를 재생합니다. 또한 포획될 확률을 계산하여 포획 여부를 결정합니다.

- **``FallInHappy()``**
  - 몬스터가 기쁨의 상태로 전환될 때 호출되어 모드를 업데이트하고, 대상을 새로운 목표로 설정합니다.

- **``BeDamaged()``**
  - 몬스터가 데미지를 입었을 때 호출되어 HP를 감소시킵니다.

- **``BeCaught()``**
  - 몬스터가 포획되었을 때 호출되어, 몬스터를 데이터 매니저에 추가하고, 오브젝트를 오브젝트 풀로 반환합니다.