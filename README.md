# MR Music Laser
### 概要
部屋の天井から、音に反応して光るレーザーが照射される。

### 動作確認済み環境
Unity version: 6000.0.70f1<br>
MR device: MetaQuest3<br>
PC: Windows11

### 前提条件
必要機材: MetaQuestとPC(操作用)が必要<br>
MetaQuest3の本体設定より、「スペースの設定」を実行済み

### 実行方法
Photonを使用してネットワークマルチプレイングを可能にする<br>
参考:https://developers.meta.com/horizon/documentation/unity/unity-use-case-networking/ <br>
PCでの操作は、Windows用にビルドするのではなくUnityEditorでプレイして使用することを前提としている

### ビルド方法
レーザー演出に noriben様の[レーザーライトシェーダー](https://booth.pm/ja/items/2141514)を使用している<br>

MetaHorizon開発者ダッシュボードでアプリを登録する<br>
参考: https://developers.meta.com/horizon/documentation/unity/unity-ssa-sf<br>
[MetaQuestDeveloperHub](https://developers.meta.com/horizon/documentation/unity/ts-mqdh/?locale=ja_JP)でapkファイルをMetaQuestにアップロード<br>









