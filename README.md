 
# 基于Unity3D的RPG游戏  
  前言部分：  
	本游戏基于Unity3D实现，主要逻辑由Bolt插件编辑。此游戏中，玩家将操控一辆坦克与地图中的怪物作战，要求玩家在保全自己的同时尽可能的杀掉所有怪物。  

## 1. 项目概况  
开发环境：	Windows10  
			Unity 2018.2.10f1  
			Bolt插件  
游戏类型：3D RPG游戏  
游戏控制：  
	使用键盘按键WASD控制玩家移动，鼠标控制视角与坦克朝向。  
	使用空格按键发射炮弹进行攻击，炮弹发射方向为坦克正前方。   
游戏流程：  
	游戏共有两个关卡，每个关卡内有多只怪物，杀死所有怪物后解锁传送门。玩家进入传送门后可进入下一关，通过第二关后返回win场景。玩家死亡后重新进行当前关卡。  
开发中使用的其他插件：Fantasy Monster – Skeleton   作为NPC 的形象  。
Standard asset    标注资源库。  
## 2. 角色控制器  
该模块将完成：  
	角色移动和视角控制  
### 2.1 角色控制器设置  
   将坦克与场景模型放置完成后，调整坦克的大小，将坦克设为玩家。为坦克添加Box Collider与Rigidbody组件，如图2.1.1。调整Box Collider的数值，使绿色胶囊状的外轮廓恰好包裹住Tank本体。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/1.png)

### 2.2 设置玩家视角  
在Hierarchy视图中的 Tank下建立一个名为OrbitPoint的空游戏对象作为摄像机旋转和朝向的参考点，如图2.2.1。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/2.png)
                                     图2.2.1
在Main Camera游戏对象中建立名为Camera View的流机器，流机器的宏储存在Asset/Macros中，并设定对象级别的Target变量，将其指向OrbitPoint。同时设定图级别的浮点型MouseX、MouseY变量。  
当玩家静止时，采用自由视角，玩家视角（主摄像机）可随鼠标移动而改变，人物朝向不变。这一部分主要在CameraFlow中完成。  
首先使用GetAxis方法获得鼠标的移动，并且每帧更新MouseY和MouseX的值，以用来控制相机的旋转角度。（图2.2.1）  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/3.png) 

鼠标的横移动向控制坦克的左右；鼠标纵向移动控制坦克的上下视角，且由Clamp函数将角度限制在0-60°之间。同时将摄像机指向要环绕的对象，从而完成了摄像机对于对象的跟踪和环绕。  
### 2.3 角色移动控制  
在Tank下设置名为PlayerController的流机器，在流图中设置三个变量，如图2.3.1所示：  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/4.png)	                

为响应鼠标水平移动添加单元组，如图2.3.2所示   
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/5.png)

此单元组获取鼠标在水平方向上的移动，并把移动转变为绕Y轴（垂直向上）旋转的角度，使得Ta坦克可以随着鼠标的移动而转向。   
接着获取键盘的输入生成相应的运动矢量，该单元组获取键盘上的横向输入轴（A与D键）以及纵向输入轴（W与S键）从而使得坦克在xz平面上运动，坦克在Y方向没有位置变化，因此保持原Y分量即可。运动速度即为MoveSpeed（默认为5）。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/6.png)

## 3. 角色攻击  
该模块将完成：  
	坦克发射炮弹进行攻击；  
	坦克炮弹的移动与销毁  
### 3.1 为坦克增加发射炮弹功能  
 在Prefabs目录下选择Shell预构件，为Shell建立一个新的流机器并将该流机器组件源设为Embed。之后为Shell设置对象级变量，图3.1.1。DirectionX与DirectionY确定炮弹的飞行方向，而ShellDameage为炮弹伤害，默认为一。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/7.png)

玩家按下空格发射炮弹，发射时，炮弹对象被分配了两个Direction变量表示发射方向，发射速度设为与发射方向匹配的恒定速度，为8（图3.1.2与图3.1.3），为防止它没有击中任何东西而一直飞行下去，要使它在发射两秒内自毁，图3.1.4。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/8.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/9.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/10.png)

最后，炮弹检查击中的对象是否是敌人，如果是，触发敌人的Hurt事件，然后摧毁炮弹对象，如果击中的不是敌人，则只是摧毁了炮弹对象。图3.1.5。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/11.png) 

至此，角色攻击模块构建完成。  
## 4. 敌人控制  
该模块将完成：  
 敌人的状态控制（巡逻与攻击）  
	敌人自动寻路  
### 4.1 敌人模型设置    
在Unity的资源商店获取Fantasy Monster-Skeleton资源作为NPC的形象。将敌人模型放置到场景中后，对其添加以下几个组件：  
	1）Animator组件，用于控制动画播放  
	2）Rigidbody和Capsule Collider组件，用于敌人的碰撞  
	3）NavMeshAgent组件，用于自动寻路  
	4）状态机组件State Machine用于敌人的控制，4.2节主要介绍其中的实现流程  
### 4.2 敌人巡逻与攻击  
将Skeleton@Attack, Skeleton@Idle, Skeleton@Run放置在Animator视图中，形成NPC动画状态.如图4.2.1  
  ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/12.png)

为NPC添加一个名为EnemyBot的状态机组件，在状态机组件中建立一个Bot AI超级状态，Bot AI中共有四个状态（图4.2.2），用于敌人状态控制。之后为此状态图建立对象级变量与场景级变量（图4.2.3和图4.2.4）。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/13.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/14.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/15.png)

1）**Perception**，在此状态中，NPC会每帧获取玩家与自己的距离，玩家相对自己的角度和方向，图4.2.5和图4.2.6。图4.2.5中NPC获取了自身的位置，同时从场景变量中获取了玩家的位置信息，计算两个位置之间的距离，并把它存储在变量distanceToPlayer中。图4.2.6中NPC先获取自己前进方向的矢量，然后计算自己到玩家的矢量并储存在directionToPlayer变量中  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/16.png)	 

2）**Patrol**，巡逻状态，将视野指示器设为蓝色，将NPC自身的速度设为一个较低的数值（1.5），将NPC自身动画设为Run，同时随机选择一个巡逻点，使NPC沿着导航网格向巡逻点巡逻，当接近巡逻点时，自动切换到随机选择的下一个巡逻点并向该点移动。需注意的是有两个Random Range（int与float），此处所需使用的是int型，否则可能出现Index out of range错误。   
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/17.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/18.png)
                               图4.2.8
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/19.png)


3）**Chase**，追逐状态，将视野指示器设为黄色，将NPC自身的速度设为一个较高的数值（7）（图4.2.10），将NPC自身动画设为Run，设置寻路目标为Player（图4.2.11）。   
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/20.png)
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/21.png)
4）**Attack**，攻击状态，将视野指示器设为红色，将NPC速度设为0（图4.2.12），通过球形插值转动NPC使得NPC朝向玩家并播放Attack动画，如图4.2.13所示。在播放攻击动画时需要在NPC攻击瞄准玩家时快速对准玩家的朝向，而在结束攻击时恢复正常的播放速度，因此需要在Attack动画中添加相应的动画事件。为此需要复制一个Attack动画与Project窗口并将原Animator窗口中相应的动画替换为这个新复制的动画（图4.2.14）。在Attack动画中挥刀与收刀时时添加一个TakeAim与StopAim事件，TakeAim时aniRotation为15，StopAim时aniRotation为2（图4.2.15）。Attack会触发Attacked事件，使玩家收到伤害，伤害值为AttackDamage，默认为1，如图4.2.15。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/22.png)
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/23.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/24.png)
图4.2.14
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/25.png)

 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/26.png)

其中，Patrol与Chase，Chase与Attack状态能互相切换：  
1）**Patrol => Chase**：当玩家与敌人前进方向小于30°，与玩家距离小于10个单位且两者间无遮挡物时，敌人切换到Chase状态追逐敌人。图4.2.17判断玩家是否在附近或前方，图4.2.17实现追逐。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/27.png)
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/28.png)
图4.2.18
2）**Chase => Patrol**：当玩家与敌人前进方向大于等于30°，或与玩家距离大于等于10个单位时，玩家脱离敌人追逐范围，敌人进入巡逻状态。图4.2.19。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/29.png)
3）**Chase => Attack**: 当玩家与敌人前进方向小于60°，与玩家距离小于4个单位时，敌人进入攻击状态。图4.2.20  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/30.png)

4）**Attack => Chase**: 玩家与敌人距离大于5时，玩家脱离敌人攻击范围，敌人进入追逐状态。图4.2.21  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/31.png) 
至此，NPC已经构造完成。  
## 5. 生命值和伤害  
该模块将完成：  
玩家生命值的设置与控制  
敌人生命值的设置与控制  
### 5.1 玩家生命值设置  
为Player添加PlayererHealth流机器，用于管理玩家的生命值。其中，Intrger变量Health为玩家当前生命值。	  
PlayerHeath有一个流状态（图5.1.1）：表示玩家能收到伤害。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/32.png)
	Vulnerable中，设置事件Attacked用于玩家接受攻击并更新生命值，图5.1.2。每次收到攻击，生命值会减少等于NPC伤害值的一个值，并输出收到攻击的消息以及当前生命值。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/33.png)
	Attacked事件在敌人进行攻击时会被触发，具体见图4.2.16.  
	收到伤害时检查玩家当前的生命值，当玩家生命值为0时，触发死亡事件，生命值大于0，触发受伤事件（图5.1.3）。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/34.png)
图5.1.3
5.2 敌人生命值与死亡  
	在敌人的状态机中，增加一个新的状态机Health，由于管理敌人的生命值。此使，敌人的根状态机和Health状态机如下图5.2.1和5.2.2。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/35.png)
Health状体机中的Attacked状态用于设置敌人被攻击时的动画Damage，当敌人生命值大于0时，播放Damage动画，图5.2.3。  
 
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/36.png)
当敌人生命值小于等于0时，切换到Dead状态。图5.2.4。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/37.png)
	Dead状态中，设置敌人的死亡动画，摧毁NPC对象，并且场景中的怪物数量减一（该值后续用于管卡控制，见第六节），图5.2.5。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/38.png)
	敌人由BotAI状态机进入Health状态机的切换由事件Hurt触发，该事件首先减少敌人的生命值。图5.2.6。   
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/39.png)
该事件在坦克炮弹击中敌人时被触发（图5.2.7，位于shell流机器下）  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/40.png)
图5.2.7
	敌人如果生命值大于0，会从受到攻击之后经过一小段延迟再由Health状态切换回BotAI状态，图5.2.8。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/41.png)
图5.2.8
## 6. 游戏控制器   
该模块将完成：  
关卡的控制  
### 6.1关卡控制  
本游戏共有两关，level1与level2，level1中只有一个敌人，生命值为10，level2中有两个敌人，生命值为30，消灭所有敌人后可进入下一关，消灭level2中敌人后进入场景win，此场景中没有敌人。  
	在场景中新建一个空物体GameManager，为其增加流机器GameManager，用于控制游戏流程，为GameManager添加变量SceneName用于控制关卡的变化，图6.1.1。level1的SceneName为level2，level2为win。  
	每一帧，检查当前场景中剩余的敌人数量，当数量为零时，达成过关条件，图6.1.2。解锁并加载下一关卡。  
 ![image](https://github.com/PaulDir/Tanks-war/blob/master/images/42.png)
 
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/43.png)
## 7. 音效  
该模块将完成：  
坦克行动音效设置    
炮弹音效与爆炸效果设置    
本模块内容参考https://github.com/SSGamble/TanksGame，并引用了相关源码。  
音效包位于Assets>AudioClips  
 
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/44.png)

### 7.1 坦克音效   
坦克行进时音效为EngineDriving,闲置是音效为Engineldle。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/45.png)
### 7.2 炮弹音效  
发射炮弹时音效为ShotFiring，图7.1.2,爆炸为ShellExplosion。  
![image](https://github.com/PaulDir/Tanks-war/blob/master/images/46.png)

至此，整个游戏项目搭建完成。  

## 8. 结论    
   通过前面七节，游戏流程大概完成。此游戏要求玩家依靠发射炮弹击倒所有敌人从而获得最终的胜利。   
   经过这一学期的学习，我大体上知道了如何使用unity开发一个简单的小游戏，也意识到了Bolt在unity使用中所能起到的作用。但毕竟一学期的学习时间还是稍显太少，我现在对于unity以及bolt的了解也仅限于入门而已，最终也只是做出了这么个不怎么样的游戏。  
   总而言之，这个游戏算是具有了一个RPG游戏的雏形，能够允许玩家操控角色进行移动、攻击等操作，同时设定了能按一定逻辑行动的NPC，有关卡机制，有音效，还为NPC添加了动画。但游戏仍有很明显的不足：首先是游戏模式太过简单，玩家自始至终所需要对付的都只有一种NPC，同时玩家攻击模式也很单一；此外，由于本人并不会自己编写复杂的动画，我所有动画都是取自现成的材料，因此在一些地方没有能使用的合适动画，因此不得不用一些粗糙的逻辑代替，如因为没有坦克爆炸的动画，所有只能令坦克生命值将为0时直接重新加载场景作为死亡事件；由于不会制作HUD，所以也完全没有添加菜单等视图。  
   知识的缺乏是阻碍游戏质量提升的重要原因，要做出更加优美的游戏，我还需进一步学习Unity和Bolt的相关知识才行。  
