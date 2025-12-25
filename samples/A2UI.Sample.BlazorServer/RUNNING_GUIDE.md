# 🚀 A2UI Blazor Server 示例 - 运行指南

## ✅ 快速启动步骤

### 1. 确保所有依赖已安装

```bash
cd D:\AI\学习\Blog\A2UI\blazor-dotnet
dotnet restore
```

### 2. 构建解决方案

```bash
dotnet build
```

### 3. 运行示例应用

```bash
cd samples\A2UI.Sample.BlazorServer
dotnet run
```

### 4. 访问应用

打开浏览器访问：
- **主页**：`https://localhost:5001` 或 `http://localhost:5000`
- **A2UI Demo 页面**：`https://localhost:5001/a2ui-demo`

## 📱 如何使用

1. 启动应用后，在左侧导航栏点击 **"A2UI Demo"**
2. 你会看到三个演示按钮：
   - **加载简单卡片** - 展示基本的文本和卡片组件
   - **加载按钮演示** - 展示主要和次要按钮
   - **加载复杂布局** - 展示行、列和嵌套组件
3. 点击按钮后，A2UI 组件会动态渲染
4. 点击演示中的按钮会触发操作事件，显示在页面下方

## 🎯 演示功能

### 1. 简单卡片演示
展示：
- Card 组件
- Text 组件
- 基本布局

### 2. 按钮演示
展示：
- Primary 和 Secondary 按钮
- 按钮操作处理
- Row 布局
- 事件系统

### 3. 复杂布局演示
展示：
- 嵌套的 Card 组件
- Row 和 Column 布局
- 多层次组件结构
- 完整的事件处理

## 🔧 配置说明

### Program.cs 中的服务配置

```csharp
// A2UI 服务已在 Program.cs 中配置：
builder.Services.AddSingleton<MessageProcessor>();
builder.Services.AddSingleton<DataBindingResolver>();
builder.Services.AddSingleton<EventDispatcher>();
builder.Services.AddSingleton<ThemeService>();
```

### 主题配置

主题 CSS 通过 `A2UIThemeProvider` 组件自动注入到页面中。

## 📁 项目结构

```
A2UI.Sample.BlazorServer/
├── Components/
│   ├── Layout/
│   │   └── NavMenu.razor         # 导航菜单（已添加 A2UI Demo 链接）
│   ├── Pages/
│   │   └── A2UIDemo.razor        # A2UI 演示页面
│   ├── App.razor                 # 应用根组件（已添加主题）
│   ├── A2UIThemeProvider.razor   # 主题提供者组件
│   └── _Imports.razor            # 全局导入
├── Program.cs                    # 应用配置（已添加 A2UI 服务）
└── README.md                     # 本文档
```

## ⚠️ 常见问题

### 问题：页面显示空白或没有内容
**解决方案**：
1. 确保点击了演示按钮加载 UI
2. 检查浏览器控制台是否有错误
3. 确保所有项目引用正确

### 问题：按钮点击没有反应
**解决方案**：
1. 检查 EventDispatcher 是否正确注入
2. 查看页面底部是否显示"最后操作"信息
3. 打开浏览器开发者工具查看控制台

### 问题：样式不正确
**解决方案**：
1. 确保 ThemeService 已注册
2. 确保 A2UIThemeProvider 组件在 App.razor 中
3. 刷新浏览器清除缓存

## 🎨 自定义

### 修改主题

在 `A2UIThemeProvider.razor` 中：

```csharp
// 使用深色主题
ThemeService.SetTheme("Dark");
```

### 添加新演示

在 `A2UIDemo.razor` 中添加新的方法：

```csharp
private void LoadMyCustomDemo()
{
    var messages = new SurfaceBuilder("demo-surface")
        // ... 你的组件定义
        .WithRoot("root")
        .Build();
    
    ProcessMessages(messages);
}
```

## 📚 相关文档

- [快速入门指南](../../QUICKSTART.md)
- [API 参考](../../API_REFERENCE.md)
- [项目总结](../../PROJECT_SUMMARY.md)

## 🐛 调试提示

1. **启用详细日志**：在 `appsettings.Development.json` 中设置日志级别
2. **查看渲染的组件**：使用浏览器开发者工具检查 DOM
3. **检查数据绑定**：在 MessageProcessor 中添加断点

## 🎉 成功运行的标志

当你成功运行时，你应该能够：
1. 看到应用主页
2. 在左侧导航栏看到 "A2UI Demo" 链接
3. 点击链接后看到演示页面
4. 点击按钮后看到动态渲染的 A2UI 组件
5. 点击组件中的按钮后看到操作信息

祝你使用愉快！🚀

