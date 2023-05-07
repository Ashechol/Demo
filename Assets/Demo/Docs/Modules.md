# 模块划分



## Demo. Framework

该模块下是一些核心的框架类代码。

### Demo.Framework.Core



### Demo.Framework.FSM



### Demo.Framework.Camera



### Demo.Framework.Input



## Demo.Base

该模块下主要是一些基类。

#### Class AnimHandler



#### Demo.Base.Character

所有角色都需要继承自 CharacterBase 类。

##### Class CharacterBase

CharacterBase 借由 FSM 实现角色基础状态之间的转换。它接受输入（可以是 PlayerController，也可以是 AIController），并反映出正确的状态。

> 状态机和 AI 的行为树并不互斥，行为树负责更高级的状态，比如面对玩家的一系列行为（状态）。

- `CharacterStateMachine` ：继承自 `StateMachine` 类，控制角色的基础状态转换。
- `CharacterState` ：继承自 `State` 类，实现角色具体状态下的逻辑。
- `CharacterAnimHandler` ：继承自 `AnimHandler` 类，是 `CharacterBase` 与 `Animator` 组件及其中的 `Animator Controller` 之间的桥梁。