<div>
<h1>🎮 Project_C</h1>

</div>

## 목차
- [개요](#개요) 
- [게임 설명](#게임-설명)
- [개발 목표](#개발-목표)
- [시스템 구조](#시스템-구조)
- [사용 기술](#사용-기술)
- [트러블 슈팅](#트러블-슈팅)
- [프로젝트 결과 및 성과](#프로젝트-결과-및-성과)

## 개요
- **개발 기간**: 2025.08.28 ~ 2025.09.30 (약 1개월)
- **개발 인원** : 1인 개발
- **개발 엔진 및 언어**: Unity & C#
- **플랫폼** : Mobile
- **개발자**: 정래규
- **래퍼런스 게임**: 클래시로얄

## 게임 설명
<b>3개의 점령지 중 2곳을 먼저 점령하면 승리하는
빠른 판단과 전략적 선택이 핵심인 멀티플레이 전략 게임</b>
<!-- 메인 플레이 화면 -->
<table>
  <tr>
    <th>로그인</th>
    <th>메인</th>
    <th>랭킹</th>
  </tr>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/0dbb6b90-43d1-4cf2-922b-0fbe86cac3bd" width="240"/>
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/4321474e-03e1-4400-a798-02dbea399f27" width="240"/>
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/dcbfaf94-de95-425b-99aa-4e4a7ee74096" width="240"/>
    </td>
  </tr>
</table>


## ✅개발 목표
1. FireBase를 활용한 로그인과 랭킹만들기
2. Addressable을 이용한 리소스 관리
3. PhotonNetwork를 이용한 네트워크 및 동기화 공부
4. 완성된 루프 만들기

## 시스템 구조
<details>
  <summary>시스템 구조 다이어그램 보기</summary> 

  <img width="3737" height="6335" alt="Frame 1" src="https://github.com/user-attachments/assets/ef75b060-ba4c-4499-81a5-2e4898661b21" />

</details>



## 사용 기술
- 멀티플레이
  - Photon Pun2
  - 매칭, 서버 시간 동기화
  - RPC + CustomProperties조합
- 데이터 관리
  - Firebase Auth(회원가입/로그인)
  - FireStore (랭킹, 정렬)
- 리소스 관리
  - Addresable기반 프리펩/ 이미지 로딩
  - 핸들은 각각의 오브젝트가 가지고 있는 구조
- 유닛로직
  - NavMeshAgent 이동
  - FSM기반 상태 관리
- 기타
  - Grid 기반 유닛 배치
  - CSV->ScriptablObject 변환 에디터

### 멀티플레이 동기화 설계
- CustomProperties 활용
  - Player CustomProperties 
    - 덱 정보(현재 덱)
    - 준비 상태
  - Room CustomProperties
    - 게임 시간
    - 승리 조건
    - 점령 상태
- RPC
  - 유닛 효과 적용
  - 점령 완료 알림
  - 게임종료 트리거

*상태 데이터는 CustomProperties로 유지하고 즉각적인 이벤트성 동작만 RPC로 처리했습니다.*

### 유닛 시스템
- 이동 : NavMeshAgent
- 상태 : FSM
- 효과
  - 소환 시 1회성 버프/디버프
  - 강제이동, 흡수

### 밸런스 관리
- CSV파일로 수치 관리
- CSV -> Scriptableobject 변환 에디터 제작

## 트러블 슈팅
### Firebase 다중 로그인 오류

- 문제: 동일 PC에서 2개 계정 로그인 시 클라이언트 종료

- 원인: Firebase LOCK 파일 충돌

- 해결: Editor.log 분석 후 모바일 실기기 테스트 환경으로 전환

### Addressable 핸들 관리 문제

- 문제: 중앙 집중식 핸들 관리로 해제 타이밍 충돌

- 해결: 생성된 오브젝트가 각자 Handle을 관리하도록 구조 변경

## 프로젝트 결과 및 성과

### 🎊 프로젝트 결과
- 원활한 루프 완성
- 동기화 성공, 멀티플레이 가능

### 🎉 프로젝트 성과
- FireBase의 사용(회원가입, 로그인, 랭킹) 성공
- PhotonNetwork의 사용(멀티플레이, 동기화) 성공
- Addressable을 이용한 이미지 로딩 성공


