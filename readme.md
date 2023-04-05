# Demo 计划

* 游戏类型：ARPG
* 渲染风格：三渲二赛璐璐
* 技术点

  * Gameplay
    * 运动
    * 相机：自己实现 / Cinemachine
    * 战斗
    * 对象池
    * FSM + 动画状态机
    * 行为树
  * 动画
    * BlendTree
    * Animation event
    * IK
    * Retargeting
  * UI
    * UGUI
  * 系统
    * 存档
    * 伤害
    * 寻路：自己实现 / Navigation
      * A*
    * 场景加载
    * 内存管理
      * 资源动态加载卸载：AssetBundle / Addressables
      * GC 优化
    * 事件系统
  * 渲染（人物，场景，特效，后期）
    * shader：ShaderLab（HLSL） + Shader Graph 
  * Editor 工具
  * 热更新
    * huatuo / hybridclr / xlua
  * 后端服务器
    * 帧同步
    * 多人

#### 可能用到的设计模式

* 单例
* 状态机
* 对象池
* 观察者