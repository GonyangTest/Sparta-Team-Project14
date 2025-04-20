# 스파르타 던전 (Sparta Dungeon)

## 프로젝트 개요
스파르타 던전은 콘솔 기반의 텍스트 RPG 게임입니다. 플레이어는 던전에 입장하여 몬스터와 전투하고, 골드을 획득하여 아이템 구매를 통해 캐릭터를 성장시키는 방식으로 진행됩니다.

## 주요 기능
- **캐릭터 관리**: 플레이어 상태 확인
- **인벤토리 시스템**: 아이템 관리, 장비 장착/해제
- **상점 시스템**: 아이템 구매 및 판매
- **던전 시스템**: 난이도별 던전 입장 및 전투
- **휴식 시스템**: 골드를 소모하여 캐릭터 회복
- **저장 기능**: 게임 진행 상황 저장 및 불러오기

## 프로젝트 구조
```
SpartaDungeon/
│
├── Core/
│   ├── Constants/      # 상수 정의
│   ├── Data/           # 데이터 관리
│   ├── Dungeon/        # 던전 관련 로직
│   │   └── Interface/  
│   ├── Equipment/      # 장비 관련 로직
│   │   └── Interface/  
│   ├── Inventory/      # 인벤토리 관련 로직
│   │   └── Interface/  
│   ├── Item/           # 아이템 관련 로직
│   ├── Shop/           # 상점 관련 로직
│   │   └── Interface/  
│   ├── UI/             # 게임 화면 관리
│   │   └── Scene/      
│   └── Utils/          # 유틸리티
│
├── Managers/
│   ├── GameManager.cs  # 게임 전반 관리
│   ├── SceneManager.cs # 화면 생성 및 전환 관리
│   └── SaveManager.cs  # 저장 기능 관리
│
├── User/
│   ├── Player.cs       # 플레이어 관련 로직
│   └── Interface/      
│
├── Resource/           # 게임 리소스 (아이템 정의 데이터)
│
├── Program.cs          # 프로그램 진입점
├── SpartaDungeon.csproj # 프로젝트 파일
└── SpartaDungeon.sln   # 솔루션 파일
```

## 주요 클래스
- **GameManager**: 게임의 핵심 관리자 클래스로 플레이어, 상점, 던전 등 모든 기능 연결
- **Player**: 플레이어 캐릭터 관리
- **BaseScene**: 모든 게임 화면의 기본 클래스
- **ItemShop**: 상점 관리
- **DungeonManager**: 던전 입장 및 결과 처리

## 개발 환경
- **언어**: C#
- **플랫폼**: Console Application (.NET Core)

## 실행 방법
1. 프로젝트를 클론 또는 다운로드
2. Visual Studio에서 솔루션 파일(SpartaDungeon.sln) 열기
3. 빌드 후 실행

## 개선 필요 사항

### 아키텍처 개선
1. **GameManager의 책임 분산**
   - 현재 GameManager가 너무 많은 역할을 담당하고 있어 인벤토리, 상점, 던전 등의 기능을 별도 매니저로 분리 필요

2. **Scene 전환 방식 개선**
   - 현재 씬 전환 시  `RegisterAction()`에 매번 `Action` 델리게이트가 덮어씌어지는 부분이 있어 성능 부하 문제 발생 예상하여 개선 방안 검토 필요

### 기능 개선
1. **로깅 시스템 도입**
   - 디버깅 및 게임 활동 추적을 위한 로깅 시스템 필요

2. **데이터 저장 보안 강화**
   - 간단한 암호화 알고리즘 적용 필요

3. **예외 처리 강화**
   - 부적절한 입력이나 예상치 못한 상황에 대한 예외 처리 미흡 