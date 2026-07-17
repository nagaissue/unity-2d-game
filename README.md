# Crash＆Hop

- ゲームクリエイトサークル所属、Unity 2Dゲーム学習用プロジェクト
- 2026年度 能開大学校祭向け制作

## プロジェクト概要

- プロジェクト名: `unity-2d-game`
- ゲームタイトル: `Crash＆Hop`
- 目的: 2D横スクロール風ジャンプアクションを制作し、UnityとC#スクリプトの基礎を学ぶ
- Unityバージョン: `6000.3.12f1`
- 本日の学校祭までに完成は間に合いませんでしたが、今後もゲームクリエイトサークルの活動で完成を目指します。

## ゲームコンセプト

- 背景と様々なサイズの壁が横から流れてくる
- キャラクターは左右とジャンプで移動し、壁をかわす
- 壁に当たると壁が壊れてキャラクターがダメージを受ける
- HPが0にならないようにゴールまで壁を回避し続ける
- HP0でゲームオーバー

## 現在の実装内容

- `PlayerController` でプレイヤーの左右移動とジャンプを制御
- `BackGroundMover` で `SpriteRenderer` ベースのワールド空間背景をUVオフセットでスクロール表示
- `SpriteSyncMover` で `BackGroundMover` の速度と同期してオブジェクトを移動
- `WallMover` で `GameSpeedManager` に基づく一貫した壁スクロールを実装
- `WallSpawner` でランダム間隔・ランダム高さで壁をスポーン
- `Assets/Scenes/SampleScene.unity` にサンプルシーンが存在

## ゲームの基本操作

- 右方向: `→` キー / `D` キー
- 左方向: `←` キー / `A` キー
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

## スクリプトと学習ポイント

- `Assets/Scripts/PlayerController.cs`
  - `Rigidbody2D` を使った物理移動（`FixedUpdate` で速度を反映）
  - 矢印キー・`A/D` キーで左右移動（`Input.GetKey`）と `Space` キーでジャンプ（`Input.GetKeyDown(KeyCode.Space)`）
  - `jumpPower` は `[SerializeField]` で調整可能
  - 接地判定を `OnCollisionEnter2D` / `OnCollisionStay2D` / `OnCollisionExit2D` で扱い、`isGrounded` を更新
  - `SpriteRenderer.flipX` で左右向きを反転

- `Assets/Scripts/BackGroundMover.cs`
  - `SpriteRenderer` ベースのワールド空間背景に変更
  - `SpriteRenderer` のマテリアルを複製して共有マテリアルを汚さずにUVオフセットを適用
  - `sortingLayerName` と `sortingOrder` を設定可能にしてUIとの混在を回避

- `Assets/Scripts/SpriteSyncMover.cs`
  - `BackGroundMover.OffsetSpeed` を直接参照して同期移動
  - リフレクションを廃止し、速度取得を安定化

### 初心者向けの学習ポイント

- `Assets/Scripts/` を開いて、どのスクリプトがプレイヤー操作に使われているか確認する
- `SampleScene.unity` のオブジェクトを選択し、Inspector でコンポーネントを眺める
- `PlayerController` の `jumpPower` や `xSpeed` を変更して挙動を試す
- `BackGroundMover` の `m_offsetSpeed` を変更してスクロール速度を調整する
- Unity の基本用語（`GameObject`、`Component`、`Prefab`、`Scene`、`Inspector` など）も合わせて学ぶと理解が深まる

## 今後の予定

- 障害物や敵の追加
- スコア表示やHPバーの実装
- ゴール判定やクリア演出の追加
- モバイル操作対応やキー入力の改善
