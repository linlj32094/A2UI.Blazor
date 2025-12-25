# A2UI Blazor .NET 快速入门指南

欢迎使用 A2UI for .NET Blazor！本指南将帮助您快速上手。

## 📦 安装

### 前提条件

- .NET 9.0 SDK
- Visual Studio 2022 或 Visual Studio Code

### 项目引用

在您的 Blazor 项目中添加以下 NuGet 包引用：

```xml
<ItemGroup>
  <ProjectReference Include="path\to\A2UI.Core\A2UI.Core.csproj" />
  <ProjectReference Include="path\to\A2UI.Blazor.Components\A2UI.Blazor.Components.csproj" />
  <ProjectReference Include="path\to\A2UI.AgentSDK\A2UI.AgentSDK.csproj" />
  <ProjectReference Include="path\to\A2UI.Theming\A2UI.Theming.csproj" />
</ItemGroup>
```

## 🚀 快速开始

### 1. 配置服务

在 `Program.cs` 中注册 A2UI 服务：

```csharp
using A2UI.Core.Processing;
using A2UI.Theming;

var builder = WebApplication.CreateBuilder(args);

// 添加 Blazor 服务
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 添加 A2UI 服务
builder.Services.AddSingleton<MessageProcessor>();
builder.Services.AddSingleton<DataBindingResolver>(sp => 
    new DataBindingResolver(sp.GetRequiredService<MessageProcessor>()));
builder.Services.AddSingleton<EventDispatcher>();
builder.Services.AddSingleton<ThemeService>();

var app = builder.Build();

// 配置 HTTP 请求管道
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
```

### 2. 创建您的第一个 A2UI 页面

创建 `Pages/MyFirstA2UI.razor`：

```razor
@page "/my-first-a2ui"
@using A2UI.Core.Processing
@using A2UI.AgentSDK
@rendermode InteractiveServer
@inject MessageProcessor MessageProcessor

<PageTitle>我的第一个 A2UI</PageTitle>

<h1>我的第一个 A2UI 页面</h1>

<button @onclick="LoadUI">加载 UI</button>

@if (showSurface)
{
    <A2UISurface SurfaceId="my-surface" />
}

@code {
    private bool showSurface = false;

    private void LoadUI()
    {
        // 使用 QuickStart 辅助方法
        var messages = A2UIQuickStart.CreateTextCard(
            "my-surface",
            "Hello A2UI!",
            "这是使用 A2UI 创建的第一个卡片。"
        );

        MessageProcessor.ProcessMessages(messages);
        showSurface = true;
    }
}
```

### 3. 运行应用

```bash
dotnet run
```

访问 `https://localhost:5001/my-first-a2ui` 查看结果！

## 💡 核心概念

### Surface（表面）

Surface 是 UI 的容器。一个应用可以有多个 Surface。

```csharp
<A2UISurface SurfaceId="my-surface" />
```

### 使用 Fluent Builder API

```csharp
var messages = new SurfaceBuilder("my-surface")
    .AddText("title", text => text
        .WithText("欢迎")
        .WithUsageHint("h1"))
    .AddButton("btn", btn => btn
        .WithChild("btn-text")
        .WithAction("click_action"))
    .AddText("btn-text", text => text
        .WithText("点击我"))
    .AddColumn("root", col => col
        .AddChild("title")
        .AddChild("btn"))
    .WithRoot("root")
    .Build();

MessageProcessor.ProcessMessages(messages);
```

### 处理用户操作

```csharp
@inject EventDispatcher EventDispatcher

protected override void OnInitialized()
{
    EventDispatcher.UserActionDispatched += OnUserAction;
}

private void OnUserAction(object? sender, UserActionEventArgs e)
{
    Console.WriteLine($"Action: {e.Action.Name}");
    Console.WriteLine($"Component: {e.Action.SourceComponentId}");
    Console.WriteLine($"Context: {JsonSerializer.Serialize(e.Action.Context)}");
}
```

### 数据绑定

```csharp
var messages = new SurfaceBuilder("my-surface")
    .AddText("greeting", text => text
        .BindToPath("/user/name")  // 绑定到数据模型
        .WithUsageHint("h2"))
    .AddData("user", new Dictionary<string, object>
    {
        { "name", "张三" }
    })
    .WithRoot("greeting")
    .Build();
```

## 🎨 主题定制

### 使用内置主题

```csharp
@inject ThemeService ThemeService

// 切换到深色主题
ThemeService.SetTheme("Dark");
```

### 创建自定义主题

```csharp
public class MyCustomTheme : DefaultTheme
{
    public new string Name => "MyTheme";
    public new string PrimaryColor => "#ff6b6b";
    public new string SecondaryColor => "#4ecdc4";
}

// 注册主题
ThemeService.RegisterTheme(new MyCustomTheme());
ThemeService.SetTheme("MyTheme");
```

## 📚 组件库

### 基础组件

- **Text** - 文本显示（支持 h1-h5, body, caption）
- **Button** - 按钮（支持 primary/secondary）
- **Image** - 图片（支持不同的 fit 和 usageHint）
- **Icon** - 图标

### 布局组件

- **Row** - 水平布局
- **Column** - 垂直布局
- **Card** - 卡片容器
- **List** - 列表

### 输入组件

- **TextField** - 文本输入
- **CheckBox** - 复选框
- **DateTimeInput** - 日期时间输入
- **Slider** - 滑块
- **MultipleChoice** - 多选

## 🔥 高级示例

### 创建表单

```csharp
var messages = new SurfaceBuilder("form-surface")
    .AddColumn("root", col => col
        .AddChild("title")
        .AddChild("name-field")
        .AddChild("email-field")
        .AddChild("submit-btn"))
    .AddText("title", text => text
        .WithText("用户注册")
        .WithUsageHint("h2"))
    .AddTextField("name-field", field => field
        .WithLabel("姓名")
        .BindToPath("/form/name"))
    .AddTextField("email-field", field => field
        .WithLabel("邮箱")
        .BindToPath("/form/email"))
    .AddButton("submit-btn", btn => btn
        .WithChild("submit-text")
        .WithAction("submit_form")
        .AsPrimary())
    .AddText("submit-text", text => text
        .WithText("提交"))
    .WithRoot("root")
    .Build();
```

### 动态更新数据

```csharp
// 更新数据模型
var updateMessage = A2UIQuickStart.CreateDataUpdate(
    "my-surface",
    new Dictionary<string, object>
    {
        { "counter", 42 },
        { "message", "已更新" }
    },
    "/state"
);

MessageProcessor.ProcessMessage(updateMessage);
```

### 删除 Surface

```csharp
var deleteMessage = A2UIQuickStart.DeleteSurface("my-surface");
MessageProcessor.ProcessMessage(deleteMessage);
```

## 📖 更多资源

- [A2UI 协议规范](../../specification/0.8/docs/a2ui_protocol.md)
- [组件参考](../../docs/reference/components.md)
- [示例应用](../../samples/A2UI.Sample.BlazorServer/)

## 🤝 贡献

欢迎贡献！请查看 [CONTRIBUTING.md](../../CONTRIBUTING.md)

## 📄 许可证

Apache 2.0

