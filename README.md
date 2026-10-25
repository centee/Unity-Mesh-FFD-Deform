# Unity-Mesh-FFD-Deform
基于Unity C#实现三维网格变形实验

## 功能
- Taper：锥化变形
- Twist：扭曲变形
- FFD：自由变形（中间鼓包）
- Reset：恢复原始圆柱模型

## 环境
Unity 2022
C# 脚本

## 使用说明
1. 场景中创建圆柱，调高Height Segments、Radial Segments
2. 给圆柱挂载MeshDeformer脚本，赋值MeshFilter
3. 创建UI按钮，分别绑定四个变形函数
4. 点击按钮切换变形效果
