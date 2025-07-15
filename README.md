# MF2024_API

## 概要
MF2024_APIは、会議室・事業所・部署・課（セクション）・予約・入退室管理など、オフィス運営に必要な各種情報を管理・操作するためのRESTful APIです。C# 12.0 / .NET 8で実装されています。

## 主な機能
- **会議室管理**  
  部屋情報の検索・登録・更新・削除、設備・画像管理、入退室履歴管理
- **予約管理**  
  予約情報の検索・登録・更新・削除、QRコード生成、トークン発行
- **部署・課（セクション）管理**  
  部署・課情報の検索・登録・更新・削除
- **事業所管理**  
  事業所情報の検索・登録・更新・削除
- **NFC割当管理**  
  NFCデバイスの割当情報の管理
- **入退室管理**  
  NFCとデバイスを用いた入退室記録
- **オプトアウト管理**  
  デバイス・NFC単位でのオプトアウト情報管理
- **Discord通知連携**  
  予約やメッセージをDiscordへ通知

## APIエンドポイント例
- `/api/ReceptionDevice/GetReservation`  
  予約情報の取得
- `/api/RoomDevice/EnterRoom`  
  入室処理
- `/api/RoomDevice/ExitRoom`  
  退室処理
- `/api/ReceptionDevice/DiscordSend`  
  Discordへのメッセージ送信

## データモデル
- **Department**: 部署情報
- **Section**: 課（セクション）情報
- **Room**: 会議室情報
- **Reservation**: 予約情報
- **Device**: デバイス情報
- **Nfc/Nfcallotment**: NFCデバイス・割当情報
- **OptOut**: オプトアウト情報
- **Entrants**: 入退室情報

## セットアップ
1. .NET 8 SDKをインストール
2. 必要なNuGetパッケージを復元
3. `appsettings.json`でDB接続等の設定
4. マイグレーション・DB初期化
5. APIプロジェクトを起動

## 使用方法
APIは認証（JWT/ASP.NET Identity）とロール管理（例: `RoomDevice`, `Reception`）に対応しています。  
各エンドポイントはSwaggerやPostman等でテスト可能です。

## 注意事項
- 各APIは例外処理・バリデーションが実装されています。
- 画像処理にはImageSharp、QRコード生成にはQRCoderを利用しています。
- Discord連携にはWebhook/APIを利用します。

---

本READMEは主要な機能・構成・利用方法を簡潔にまとめています。詳細なAPI仕様やモデル定義は各コントローラー・モデルクラスをご参照ください。
