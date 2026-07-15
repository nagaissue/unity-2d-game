# Crash＆Hop

- ゲームクリエイトサークル所属、Unity 2Dゲーム学習用プロジェクト
- 2026年度 能開大学校祭向け制作

## プロジェクト概要

- プロジェクト名: `unity-2d-game`
- ゲームタイトル: `Crash＆Hop`
- 目的: 2D横スクロール風ジャンプアクションを制作し、UnityとC#スクリプトの基礎を学ぶ
- Unityバージョン: `6000.3.12f1`

## ゲームコンセプト

- 背景と様々なサイズの壁が横から流れてくる
- キャラクターは左右とジャンプで移動し、壁をかわす
- 壁に当たると壁が壊れてキャラクターがダメージを受ける
- HPが0にならないようにゴールまで壁を回避し続ける
- HP0でゲームオーバー

## 現在の実装内容

- `PlayerController` でプレイヤーの左右移動とジャンプを制御
- `BackGroundMover` でUIの背景画像をUVオフセットでスクロール表示
- `SpriteSyncMover` で背景スクロールに合わせてオブジェクトを同期移動
- `Assets/Scenes/SampleScene.unity` にサンプルシーンが存在

## ゲームの基本操作

- 右方向: `→` キー
- 左方向: `←` キー
- ジャンプ: `Space` キー

## プロジェクト構成

- `Assets/` - 画像、シーン、スクリプト、設定ファイル
- `Assets/Scripts/` - C#スクリプト
- `Assets/Scenes/` - Unityシーンファイル（`SampleScene.unity`）
- `Assets/Settings/` - シーンテンプレートやURP設定
- `ProjectSettings/` - Unityプロジェクトの設定
- `Packages/` - Unityパッケージ定義

## 起動方法

1. Unity で `Assets/Scenes/SampleScene.unity` を開く
2. エディタ上部の `Play` ボタンを押す
3. `←` / `→` キーと `Space` キーで操作する

## スクリプトの学習ポイント

- `Assets/Scripts/PlayerController.cs`
- `Assets/Scripts/PlayerController.cs`
  - `Rigidbody2D` を使った物理移動（`FixedUpdate` で速度を反映）
  - 矢印キーで左右移動（`Input.GetKey`）と `Space` キーでジャンプ（`Input.GetKeyDown("space")`）
  - `jumpPower` は `[SerializeField]` で調整可能、`xSpeed` は移動量を保持（`[HideInInspector]`）
  - 接地判定用のセンサー処理はコメント化済み（`isGrounded` フラグを使用）
  - `SpriteRenderer.flipX` で左右向きを反転

- `Assets/Scripts/BackGroundMover.cs`
  - UI `Image` のマテリアルを複製して安全に操作
  - テクスチャオフセットを時間で変化させて背景をスクロール

- `Assets/Scripts/SpriteSyncMover.cs`
  - 背景のスクロール速度と同期してオブジェクトを移動
  - `m_loopWorldDistance` でワールド空間の移動量を調整

## 初心者向けのポイント

- `Assets/Scripts/` を開いて、どのスクリプトがプレイヤー操作に使われているか確認する
- `SampleScene.unity` のオブジェクトを選択し、Inspector でコンポーネントを眺める
- `PlayerController` の `jumpPower` や `xSpeed` を変更して挙動を試す
- `BackGroundMover` の `m_offsetSpeed` を変更してスクロール速度を調整する

## 今後の予定

- 障害物や敵の追加
- スコア表示やHPバーの実装
- ゴール判定やクリア演出の追加
- モバイル操作対応やキー入力の改善
