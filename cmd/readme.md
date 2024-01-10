# DBマイグレーション
## マイグレーション実行前準備
本プロジェクトのマイグレーションコマンドを実行するには[Powershell Core](https://docs.microsoft.com/ja-jp/powershell/scripting/install/installing-powershell-core-on-windows?view=powershell-7.1)のインストールが必要。
下記実行方法を実行前に、インストールを済ませておく。

## マイグレーション実行方法
- Windows
	- `migration.cmd`をダブルクリック
- Mac OS
	- ターミナル上で`cmd`フォルダに移動後、`migration.sh`を実行
	- 実行できなかった場合は、下記ページを参考に実行権限を付与する
		- https://support.apple.com/ja-jp/guide/terminal/apdd100908f-06b3-4e63-8a87-32e71241bab4/mac

## 設定値変更
プロジェクトに応じてマイグレーション対象プロジェクト、DbContextクラス名など、設定値を変える場合は、`ps1/variable.ps1`内の情報を書き換える。