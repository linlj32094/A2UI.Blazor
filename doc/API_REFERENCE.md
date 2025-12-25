# A2UI Blazor .NET API 参考

## 核心类

### MessageProcessor

消息处理器，负责处理 A2UI 消息并管理 Surface。

#### 方法

```csharp
// 处理单个消息
void ProcessMessage(ServerToClientMessage message)

// 处理多个消息
void ProcessMessages(IEnumerable<ServerToClientMessage> messages)

// 处理 JSONL 流
Task ProcessJsonLinesAsync(Stream stream, CancellationToken cancellationToken = default)

// 获取 Surface
Surface? GetSurface(string surfaceId)

// 清除所有 Surface
void ClearSurfaces()

// 获取数据
object? GetData(string surfaceId, string path, string? dataContextPath = null)

// 设置数据
void SetData(string surfaceId, string path, object value, string? dataContextPath = null)
```

### DataBindingResolver

数据绑定解析器。

#### 方法

```csharp
// 解析绑定值
T? ResolveBoundValue<T>(Dictionary<string, object> boundValue, string surfaceId, string? dataContextPath = null)

// 解析字符串值
string? ResolveString(Dictionary<string, object> boundValue, string surfaceId, string? dataContextPath = null)

// 解析数字值
double? ResolveNumber(Dictionary<string, object> boundValue, string surfaceId, string? dataContextPath = null)

// 解析布尔值
bool? ResolveBool(Dictionary<string, object> boundValue, string surfaceId, string? dataContextPath = null)

// 解析操作上下文
Dictionary<string, object> ResolveActionContext(List<Dictionary<string, object>>? contextEntries, string surfaceId, string? dataContextPath = null)
```

### EventDispatcher

事件分发器。

#### 事件

```csharp
// 用户操作事件
event EventHandler<UserActionEventArgs>? UserActionDispatched

// 错误事件
event EventHandler<ErrorEventArgs>? ErrorDispatched
```

#### 方法

```csharp
// 分发用户操作
void DispatchUserAction(UserActionMessage action)

// 分发错误
void DispatchError(Dictionary<string, object> error)

// 创建用户操作
static UserActionMessage CreateUserAction(string actionName, string surfaceId, string sourceComponentId, Dictionary<string, object> context)
```

### ThemeService

主题管理服务。

#### 属性

```csharp
// 当前主题
IA2UITheme CurrentTheme { get; }
```

#### 事件

```csharp
// 主题变更事件
event EventHandler<ThemeChangedEventArgs>? ThemeChanged
```

#### 方法

```csharp
// 注册主题
void RegisterTheme(IA2UITheme theme)

// 设置主题
bool SetTheme(string themeName)

// 获取主题名称
IEnumerable<string> GetThemeNames()

// 获取主题
IA2UITheme? GetTheme(string themeName)

// 生成主题 CSS
string GenerateThemeCss()
```

## Agent SDK

### SurfaceBuilder

Surface 构建器，使用 Fluent API 创建 UI。

#### 方法

```csharp
// 设置根组件
SurfaceBuilder WithRoot(string rootComponentId)

// 设置 Catalog
SurfaceBuilder WithCatalog(string catalogId)

// 设置样式
SurfaceBuilder WithStyles(Dictionary<string, object> styles)

// 添加组件
SurfaceBuilder AddText(string id, Action<TextComponentBuilder> configure)
SurfaceBuilder AddButton(string id, Action<ButtonComponentBuilder> configure)
SurfaceBuilder AddImage(string id, Action<ImageComponentBuilder> configure)
SurfaceBuilder AddCard(string id, Action<CardComponentBuilder> configure)
SurfaceBuilder AddRow(string id, Action<RowComponentBuilder> configure)
SurfaceBuilder AddColumn(string id, Action<ColumnComponentBuilder> configure)

// 添加数据
SurfaceBuilder AddData(string key, string value)
SurfaceBuilder AddData(string key, double value)
SurfaceBuilder AddData(string key, bool value)

// 构建消息
List<ServerToClientMessage> Build()
```

### 组件构建器

#### TextComponentBuilder

```csharp
TextComponentBuilder WithText(string text)
TextComponentBuilder BindToPath(string path)
TextComponentBuilder WithUsageHint(string usageHint)
TextComponentBuilder WithWeight(double weight)
```

#### ButtonComponentBuilder

```csharp
ButtonComponentBuilder WithChild(string childId)
ButtonComponentBuilder WithAction(string actionName)
ButtonComponentBuilder AddActionContext(string key, string path)
ButtonComponentBuilder AddActionContext(string key, object literal)
ButtonComponentBuilder AsPrimary()
ButtonComponentBuilder WithWeight(double weight)
```

#### ImageComponentBuilder

```csharp
ImageComponentBuilder WithUrl(string url)
ImageComponentBuilder BindToPath(string path)
ImageComponentBuilder WithFit(string fit)
ImageComponentBuilder WithUsageHint(string usageHint)
ImageComponentBuilder WithWeight(double weight)
```

#### CardComponentBuilder

```csharp
CardComponentBuilder WithChild(string childId)
CardComponentBuilder WithWeight(double weight)
```

#### RowComponentBuilder

```csharp
RowComponentBuilder AddChild(string childId)
RowComponentBuilder WithDistribution(string distribution)
RowComponentBuilder WithAlignment(string alignment)
RowComponentBuilder WithWeight(double weight)
```

#### ColumnComponentBuilder

```csharp
ColumnComponentBuilder AddChild(string childId)
ColumnComponentBuilder WithDistribution(string distribution)
ColumnComponentBuilder WithAlignment(string alignment)
ColumnComponentBuilder WithWeight(double weight)
```

### A2UIQuickStart

快速辅助方法。

```csharp
// 创建文本卡片
static List<ServerToClientMessage> CreateTextCard(string surfaceId, string title, string body)

// 创建按钮
static List<ServerToClientMessage> CreateButton(string surfaceId, string buttonId, string label, string actionName, bool isPrimary = false)

// 创建简单表单
static List<ServerToClientMessage> CreateSimpleForm(string surfaceId, string title, string submitLabel, string submitActionName)

// 创建数据更新
static ServerToClientMessage CreateDataUpdate(string surfaceId, Dictionary<string, object> data, string? path = null)

// 删除 Surface
static ServerToClientMessage DeleteSurface(string surfaceId)
```

### A2UIMessageSerializer

消息序列化器。

```csharp
// 序列化单个消息
string SerializeMessage(ServerToClientMessage message)

// 序列化多个消息为 JSONL
string SerializeMessages(IEnumerable<ServerToClientMessage> messages)

// 写入消息到流
Task WriteMessagesAsync(Stream stream, IEnumerable<ServerToClientMessage> messages, CancellationToken cancellationToken = default)

// 序列化客户端消息
string SerializeClientMessage(ClientToServerMessage message)
```

## Blazor 组件

### A2UISurface

Surface 容器组件。

#### 参数

```csharp
[Parameter] required string SurfaceId { get; set; }
```

### A2UIRenderer

动态组件渲染器。

#### 参数

```csharp
[Parameter] required string SurfaceId { get; set; }
[Parameter] required string ComponentId { get; set; }
```

### 所有 A2UI 组件

所有组件继承自 `A2UIComponentBase`，具有以下参数：

```csharp
[Parameter] required string SurfaceId { get; set; }
[Parameter] required ComponentNode Component { get; set; }
```

## 常量

### A2UIConstants

协议常量。

```csharp
// 标准 Catalog ID
const string StandardCatalogId

// 默认 Surface ID
const string DefaultSurfaceId

// 组件类型
class ComponentTypes
{
    const string Text, Image, Icon, Button, Card, Row, Column, List,
                 TextField, CheckBox, DateTimeInput, Slider, MultipleChoice,
                 Divider, Tabs, Modal, Video, AudioPlayer
}

// 文本使用提示
class TextUsageHints { const string H1, H2, H3, H4, H5, Caption, Body }

// 图片适配
class ImageFit { const string Contain, Cover, Fill, None, ScaleDown }

// 图片使用提示
class ImageUsageHints { const string Icon, Avatar, SmallFeature, MediumFeature, LargeFeature, Header }

// 布局分布
class Distribution { const string Start, End, Center, SpaceBetween, SpaceAround, SpaceEvenly }

// 布局对齐
class Alignment { const string Start, End, Center, Stretch, Baseline }

// 文本字段类型
class TextFieldTypes { const string ShortText, LongText, Number, Obscured, Date }

// 方向
class Direction { const string Horizontal, Vertical }
```

## 类型

### 消息类型

- `ServerToClientMessage` - 服务器到客户端消息
- `BeginRenderingMessage` - 开始渲染
- `SurfaceUpdateMessage` - Surface 更新
- `DataModelUpdateMessage` - 数据模型更新
- `DeleteSurfaceMessage` - 删除 Surface
- `ClientToServerMessage` - 客户端到服务器消息
- `UserActionMessage` - 用户操作
- `ComponentDefinition` - 组件定义
- `DataEntry` - 数据条目

### 核心类型

- `Surface` - Surface
- `ComponentNode` - 组件节点
- `DataModel` - 数据模型
- `BoundValue<T>` - 绑定值

## 示例

### 完整示例：创建用户资料卡片

```csharp
using A2UI.AgentSDK.Builders;
using A2UI.Core;

var messages = new SurfaceBuilder("profile")
    .AddCard("root", card => card.WithChild("content"))
    .AddColumn("content", col => col
        .AddChild("avatar")
        .AddChild("name")
        .AddChild("bio")
        .AddChild("button"))
    .AddImage("avatar", img => img
        .WithUrl("https://example.com/avatar.jpg")
        .WithUsageHint(A2UIConstants.ImageUsageHints.Avatar))
    .AddText("name", text => text
        .BindToPath("/user/name")
        .WithUsageHint(A2UIConstants.TextUsageHints.H2))
    .AddText("bio", text => text
        .BindToPath("/user/bio"))
    .AddButton("button", btn => btn
        .WithChild("button-text")
        .WithAction("view_profile")
        .AddActionContext("userId", "/user/id")
        .AsPrimary())
    .AddText("button-text", text => text
        .WithText("查看资料"))
    .AddData("user", new Dictionary<string, object>
    {
        { "id", 123 },
        { "name", "张三" },
        { "bio", "软件工程师" }
    })
    .WithRoot("root")
    .Build();

messageProcessor.ProcessMessages(messages);
```

