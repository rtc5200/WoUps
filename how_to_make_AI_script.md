- [AIの作り方](#aiの作り方)
    - [C#の基本](#cの基本)
        - [List<T>型について](#listt型について)
        - [Vector3について](#vector3について)
    - [実装するべき関数](#実装するべき関数)
    - [船を移動させる](#船を移動させる)
    - [旋回する](#旋回する)
    - [自動移動,自動旋回](#自動移動自動旋回)
    - [砲弾発射](#砲弾発射)
    - [砲門回転](#砲門回転)
    - [砲門自動回転](#砲門自動回転)
    - [魚雷発射](#魚雷発射)
    - [他の船のデータを取得](#他の船のデータを取得)
    - [飛行中の砲弾のデータを取得](#飛行中の砲弾のデータを取得)
    - [進行中の魚雷のデータを取得](#進行中の魚雷のデータを取得)

# AIの作り方
Unity/C#初見さん向けに書いてますが, 不明な点があれば聞いてください. 自分もUnityを完全に理解しているわけではないのであしからず.

## C#の基本
[Quitaの記事](https://qiita.com/ShirakawaMaru/items/cb24a8b34c9e338d9bba#6%E5%A4%89%E6%95%B0%E3%81%A8%E3%81%AF)の６番目以降を読んでもらえば変数や関数の必要なことはわかるかと思います.
### List<T>型について
他の船や飛んでいる砲弾や魚雷のデータを取得, 処理するときに使うList<T>型については[ここ](https://csharp-ref.com/collection_list.html)で例と解説が出てます.
### Vector3について
座標や方向を表すベクトルを表すVector3については[ここ](https://tech.pjin.jp/blog/2016/02/16/unity_vector3_1/)と[ここ](https://www.sejuku.net/blog/52461)と[ここ](http://developer.wonderpla.net/entry/blog/engineer/Unity_Vector3/)がわかりやすい説明がなされてると思います.

## 実装するべき関数
AIを動作させるのに必要な関数は2つ.onLoadとAIThinkで, onLoadは無くても動きます. 基本形は下の通り.
```cs
public class MyAIShipController : ShipController{
    // ゲーム開始前一度だけ呼び出される
    protected override void onLoad(){

    }
    // 定期的に呼び出される
    protected override void AIThink(){

    }
}
```

## 船を移動させる
船を移動させるには船の航行スピードを変更します. 0でニュートラル, -1<= <0で後退, 0 < <= 1で前進. z,x軸が海の平面を上からみたときの縦横で, y軸は海と垂直な方向です.
```cs
    protected override void AIThink(){
        Speed = 1f;//スピード1で前進
    }
```

上のプログラムはAIThinkが呼ばれるたび座標上で向いている方向に1移動し続けます. 後退したい場合は-1を代入します.
指定秒数, 例えば2秒間移動させたい場合は, 移動開始の時間とタイミングを変数に記録してからSpeedを変更します.
```cs
    float startTime;
    protected override void AIThink(){
        if(AIThinkCount == 1){//AIThink関数が1回目の実行なら
            startTime = getTime();//ゲームの経過時間を代入
        }
        if(getTime() - startTime > 2f){//ゲームの経過時間からstartTimeを引いてstartTimeからの経過時間を求める
            Speed = 0f;//停止
        }else{
            Speed = 1f;//スピード1で前進
        }  
    }
```
2秒おきに前進する場合は下のようにします.
```cs
    float startTime;
    protected override void AIThink(){
        if(AIThinkCount == 1){//AIThink関数が1回目の実行なら
            startTime = getTime();//ゲームの経過時間を代入
        }
        if(getTime() - startTime > 2f){//ゲームの経過時間からstartTimeを引いてstartTimeからの経過時間を求める
            Speed = 0f;//停止
            startTime = getTime();//startTimeをリセット
        }else{
            Speed = 1f;//スピード1で前進
        }  
    }
```
3前進する場合は下のようにします.
```cs
    Vector3 startPos;
    protected override void AIThink(){
        if(AIThinkCount == 1){//AIThink関数が1回目の実行なら
            startPos = transform.position;//始点に現在地点を代入
        }
        if((transform.position - startPos).magnitude > 3f){//始点からの距離を求める
            Speed = 0f;//停止
        }else{
            Speed = 1f;//スピード1で前進
        }  
    }
```

## 旋回する
船を旋回させるには船の旋回スピードを変更します. 0でニュートラル, -1<= <0で左回転, 0 < <= 1 で右回転.
```cs
    protected override void AIThink(){
        RoSpeed = 1f;//スピード1で右回転
    }
```
上のプログラムはAIThinkが呼ばれるたび右回転し続けます. 左回転の場合は-1にします. 
自分が向いている角度をオイラー角で取得するときはtransform.eulerAngle.yでy軸の回転角(0～360度)を取得します.
```cs
    protected override void AIThink(){
        if(transform.eulerAngles.y > 357f && transform.eulerAngles.y<3f){//z軸正方向近くになったら
            RoSpeed = 0f;//回転停止
        }else{
            RoSpeed = 1f;//スピード1で右回転
        }
    }
```
## 自動移動,自動旋回
目的地の座標が与えられれば自動で旋回した後その地点へ移動させられます.
船を自動である地点に移動させる場合は
```cs
    protected override void AIThink(){
        Vector3 target = new Vector3(100,0,100);
        AutoMoveToTarget(target);
    }
```
自動移動は完璧ではないので, おおよそ近くに来たら到着判定がなされます. 到着判定はfinishedAutoMove(bool)に到着済みならtrue, 移動中ならfalseが代入されます. 移動中に目的地の座標を取得したい場合はAutoMoveTargetPosに代入されます.
```cs
    protected override void AIThink(){
        if(finishedAutoMove == true){
            Vector3 target = transform.position;
            target.z += 30;
            AutoMoveToTarget(target);
        }
    }
```
上のプログラムは, z軸正方向に30ずつ自動移動を繰り返します.
移動はなしで自動旋回だけを行いたい場合は
```cs
    protected override void AIThink(){
        Vector3 target = new Vector3(100,0,100);
        AutoRotateToTarget(target);
    }
```
指定角度だけ自動回転させたい場合は
```cs
    protected override void AIThink(){
        if(AIThinkCount == 1){//1回目の呼び出しのみ
            AutoRotateToTarget(180f);//180度右回転
        }
    }
```
自動回転の完了判定はfinishedAutoRotに完了ならtrue,回転中ならfalseが代入されます. finishedAutoMoveまたはfinishedAutoRotにtrueを代入するとその自動移動/旋回は完了とみなされるので,　キャンセルすることができます.

## 砲弾発射
前と後ろに一つずつついている砲門がそれぞれ砲門0と砲門1です. 砲弾を発射したあとクールタイムが3秒発生します.
```cs
    protected override void AIThink(){
       Fire();//両砲門から同時に砲弾発射
    }
```
```cs
    protected override void AIThink(){
       Fire(0);//砲門0から砲弾発射
       Fire(1);//砲門1から砲弾発射
    }
```
砲門の残りクールタイムを取得するにはgetGunCoolTimeLeftを使います.
```cs
    protected override void AIThink(){
       if(getGunCoolTimeLeft(0) == 0f){
           Fire(0);
       }
    }
```
クールタイムが終わっていたら容赦なく撃ちたい場合は残りクールタイムをチェックせずにFireメソッドを実行しても大丈夫です. クールタイム中の場合は射撃せずに終わります.

## 砲門回転
砲門の回転スピードを設定するときは関数SetGunRotationSpeedを使います.
```cs
    protected override void AIThink(){
      SetGunRotationSpeed(0,1f);//砲門0をスピード1で右回転
      SetGunRotationSpeed(1,-1f);//砲門1をスピード1で左回転
    }
```
砲門の回転角はTransform.eulerAnglesのyから取得します.
```cs
    protected override void AIThink(){
      if(getTransform(0).eulerAngles.y < 30f){//z軸正方向からx軸正方向に30度以内なら
          SetGunRotationSpeed(0,1f);
          SetGunRotationSpeed(1,1f);
      }else{
          SetGunRotationSpeed(0,-1f);
          SetGunRotationSpeed(1,-1f);
      }
    }
```
砲門の回転角を, 船首の方向を基準に取得したい場合はTransform.localEulerAnglesのyから取得します.
```cs
    protected override void AIThink(){
      if(getTransform(0).localEulerAngles.y < 30f){//船首から右方向に30度以内なら
          SetGunRotationSpeed(0,1f);
          SetGunRotationSpeed(1,1f);
      }else{
          SetGunRotationSpeed(0,-1f);
          SetGunRotationSpeed(1,-1f);
      }
    }
```

## 砲門自動回転
砲門の自動回転は２つ同時に行われます. 自動移動,旋回とは異なり, 目標地点に向いたら完了とはならず, 常にその方向を向き続けるように自動で修正し続けます.
```cs
    protected override void AIThink(){
        Vector3 target = new Vector3(100,0,100);
       AutoGunRotateToTarget(target);
    }
```
自動回転を止めるにはStopAutoGunRotationを実行します.
```cs
    protected override void AIThink(){
        StopAutoGunRotation();
    }
```
## 魚雷発射
魚雷は3発で１回分の発射になります. 魚雷を１回発射すると10秒クールタイムが発生します.
```cs
    protected override void AIThink(){
       FireTorpedo();//魚雷発射
    }
```
クールタイム中にFireTorpedoが実行された場合は何もせず終了します.
## 他の船のデータを取得
AIスクリプトにRadarクラスのプロパティradarがあるので, これからデータを取得します.
```cs
    protected override void AIThink(){
       ShipData sd = radar.OtherShipsData[0];//他の船のデータ(ShipData)配列の0番目(順番は任意)
       if(sd.hp >= 100){//ある船のHPが100以上

       }
       if(Vector3.Distance(sd.position, transform.position) >= 100f ){//その船との距離が100以上

       }
    }
```
## 飛行中の砲弾のデータを取得
同様にradarから取得します.
```cs
    protected override void AIThink(){
        int num = radar.BulletsData.Count;//飛行中の砲弾の総数
       foreach(BulletData data in radar.BulletsData){//radar.BulletDataを展開ループ
            if(data.shipid != getID()){//発射した船が自分でないなら

            }
       }
    }
```
## 進行中の魚雷のデータを取得
同様に.
```cs
    protected override void AIThink(){
       foreach(TorpedoData data in radar.TorpedosData){//radar.BulletDataを展開ループ
            if(data.shipid != getID()){//発射した船が自分でないなら

            }
       }
    }
```
