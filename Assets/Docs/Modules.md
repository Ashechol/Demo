# 模块划分



## Demo. Framework

该模块下是一些核心的框架类代码。

### Demo.Framework.Core

#### Singleton\<T>



#### SingletonMono\<T>



#### StateMachine



#### IState



### Demo.Framework.Gameplay

Gameplay 相关类

#### Character



#### Controller



#### PlayerController



#### AIController



#### InputHandler

直接与 InputSystem 交互，获取输入值（raw input value）然后处理为更方便使用的值，绑定玩家输入相关事件。

