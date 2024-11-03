# 電波祭展示用 風シミュレーション

Webカメラを使用して手を認識し、手の場所から風を出します。

## 環境

- OBS Studio
- Python LTS
- DirectX 12

## 使い方

`Python.zip`をReleaseからダウンロードして、
```bash
pip install -r requirements.txt
```
で必要なパッケージをインストールします。

次に、
```bash
python ./app.py
```
で画像認識サーバーを起動します。

その後、`WindowsRelease.zip`を解凍して`Denpa Boltzmann.exe`を起動すると風のシミュレーションが行えます。
