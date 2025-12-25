# 🔍 A2UI Blazor 故障排查指南

## 问题：按钮点击没有效果

如果你点击演示按钮后没有看到任何变化，请按照以下步骤排查：

### 1. 检查调试信息

我已经在演示页面添加了调试信息显示。重新运行应用后，你应该能看到：

```
调试信息：EventDispatcher 已订阅
```

点击加载按钮后，应该看到类似：
```
调试信息：已加载 3 条消息 | Surface 组件数: 6 | 就绪: True
```

### 2. 检查浏览器控制台

打开浏览器开发者工具（F12），查看 Console 标签页是否有错误信息。

**常见错误：**
- 依赖注入失败
- 组件渲染错误
- JavaScript 错误

### 3. 验证服务注册

确保 `Program.cs` 中有以下代码：

```csharp
builder.Services.AddSingleton<MessageProcessor>();
builder.Services.AddSingleton<DataBindingResolver>(sp => 
    new DataBindingResolver(sp.GetRequiredService<MessageProcessor>()));
builder.Services.AddSingleton<EventDispatcher>();
builder.Services.AddSingleton<ThemeService>();
```

### 4. 检查项目引用

运行以下命令确保所有引用正确：

```bash
dotnet build
```

如果有编译错误，请先解决编译错误。

### 5. 清理并重建

```bash
dotnet clean
dotnet build
cd samples\A2UI.Sample.BlazorServer
dotnet run
```

### 6. 检查端口和URL

确保访问的是正确的URL：
- ✅ `https://localhost:5001/a2ui-demo`
- ❌ 不是 `https://localhost:5001` （这是主页）

### 7. 使用改进的调试版本

我已经更新了演示页面，添加了详细的调试信息。重新运行后你应该看到：

**点击"加载简单卡片"后：**
```
调试信息：已加载 3 条消息 | Surface 组件数: X | 就绪: True
```

**点击演示中的按钮后：**
```
最后操作：Action: primary_clicked, Component: primary-btn, Time: 14:30:45
调试信息：事件触发成功！Surface: demo-surface
```

### 8. 验证组件渲染

如果看到空白区域，检查：

1. **CSS 是否加载**：
   - 打开开发者工具 → Elements/元素 标签页
   - 查看 `<head>` 部分是否有 A2UI 的 CSS

2. **组件是否生成**：
   - 在开发者工具中搜索 "a2ui-" 类名
   - 应该能找到 `a2ui-card`, `a2ui-button` 等元素

### 9. 最小化测试

创建一个最简单的测试页面：

```razor
@page "/test"
@rendermode InteractiveServer
@using A2UI.Core.Processing
@inject MessageProcessor MessageProcessor

<h1>简单测试</h1>
<button @onclick="Test">测试</button>

<p>@message</p>

@code {
    string message = "";

    void Test()
    {
        message = "按钮工作！时间: " + DateTime.Now;
    }
}
```

如果这个测试页面的按钮能工作，说明 Blazor 基础功能正常。

### 10. 检查项目文件

确保 `.csproj` 文件包含所有必要的引用：

```xml
<ItemGroup>
  <ProjectReference Include="..\..\src\A2UI.Core\A2UI.Core.csproj" />
  <ProjectReference Include="..\..\src\A2UI.AgentSDK\A2UI.AgentSDK.csproj" />
  <ProjectReference Include="..\..\src\A2UI.Blazor.Components\A2UI.Blazor.Components.csproj" />
  <ProjectReference Include="..\..\src\A2UI.Theming\A2UI.Theming.csproj" />
</ItemGroup>
```

## 常见问题和解决方案

### 问题：点击按钮后页面没有任何反应

**原因**：服务可能没有正确注册或注入

**解决**：
1. 重启应用
2. 检查 Program.cs
3. 查看浏览器控制台错误

### 问题：看到错误 "Cannot resolve service"

**原因**：依赖注入配置不正确

**解决**：
```csharp
// 确保在 Program.cs 中添加了所有服务
builder.Services.AddSingleton<MessageProcessor>();
builder.Services.AddSingleton<DataBindingResolver>(sp => 
    new DataBindingResolver(sp.GetRequiredService<MessageProcessor>()));
builder.Services.AddSingleton<EventDispatcher>();
builder.Services.AddSingleton<ThemeService>();
```

### 问题：组件显示但样式不对

**原因**：主题 CSS 未加载

**解决**：
1. 检查 `A2UIThemeProvider.razor` 是否存在
2. 检查 `App.razor` 是否包含 `<A2UIThemeProvider />`

### 问题：按钮显示但点击没有事件

**原因**：EventDispatcher 未正确订阅

**解决**：
在页面的 `@code` 块中确保：
```csharp
protected override void OnInitialized()
{
    EventDispatcher.UserActionDispatched += OnUserAction;
}

public void Dispose()
{
    EventDispatcher.UserActionDispatched -= OnUserAction;
}
```

## 验证清单

运行应用后，依次验证：

- [ ] 应用启动成功
- [ ] 可以访问 `/a2ui-demo` 页面
- [ ] 看到4个控制按钮（加载简单卡片、加载按钮演示、加载复杂布局、清除）
- [ ] 看到调试信息："EventDispatcher 已订阅"
- [ ] 点击"加载简单卡片"后：
  - [ ] 调试信息更新
  - [ ] surface-container 区域显示内容
  - [ ] 看到卡片样式
- [ ] 点击"加载按钮演示"后：
  - [ ] 看到两个按钮
  - [ ] 按钮有不同颜色（主要按钮是蓝色）
- [ ] 点击演示中的按钮后：
  - [ ] 页面底部显示"最后操作"信息
  - [ ] 调试信息更新

## 获取帮助

如果以上步骤都无法解决问题，请提供以下信息：

1. **浏览器控制台的完整错误信息**
2. **调试信息显示的内容**
3. **dotnet build 的输出**
4. **浏览器和操作系统版本**

## 快速修复脚本

```powershell
# 完全重建项目
cd D:\AI\学习\Blog\A2UI\blazor-dotnet
dotnet clean
Remove-Item -Recurse -Force */bin, */obj -ErrorAction SilentlyContinue
dotnet restore
dotnet build

# 运行示例
cd samples\A2UI.Sample.BlazorServer
dotnet run
```

---

**提示**：我已经在演示页面添加了详细的调试信息，重新运行应用后你应该能看到更多诊断信息！

