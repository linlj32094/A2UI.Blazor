# A2UI Blazor Demo - 使用指南

## 🎯 概述

这个 Demo 展示了如何使用 **Google A2UI 协议** 在 Blazor .NET 应用中实现动态 AI 生成的用户界面。

## 🏗️ 架构

```
用户输入 → A2AClientService → Agent (LLM) → A2UI JSON → MessageProcessor → Blazor UI
```

与 Google 官方示例不同，我们使用：
- ❌ **不再使用**: 硬编码的 `SurfaceBuilder`
- ✅ **现在使用**: Agent 返回的 A2UI JSON (符合 Google 标准)

## 🚀 快速开始

### 1. 运行项目

```bash
cd blazor-dotnet/samples/A2UI.Sample.BlazorServer
dotnet run
```

访问: `https://localhost:5001/a2ui-demo`

### 2. 与 Agent 交互

在聊天界面输入以下内容:

- "显示联系人" - 显示联系人列表
- "显示餐厅" - 显示餐厅卡片
- "显示按钮" - 显示交互按钮
- "显示卡片" - 显示简单卡片

或者点击快捷按钮！

### 3. Agent 如何工作

当前实现使用 `MockA2AAgent`，它根据关键词返回预定义的 A2UI JSON:

```csharp
// Services/MockA2AAgent.cs
public Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
{
    return query.ToLower() switch
    {
        var q when q.Contains("联系人") => GetContactListExample(),
        var q when q.Contains("餐厅") => GetRestaurantExample(),
        // ...
    };
}
```

每个示例返回标准的 A2UI JSON 消息数组:

```csharp
return new List<ServerToClientMessage>
{
    new ServerToClientMessage
    {
        BeginRendering = new BeginRenderingMessage
        {
            SurfaceId = "demo-surface",
            Root = "root-card"
        }
    },
    new ServerToClientMessage
    {
        SurfaceUpdate = new SurfaceUpdateMessage
        {
            SurfaceId = "demo-surface",
            Components = [...]
        }
    },
    // 可选: DataModelUpdate 用于数据绑定
};
```

## 🔧 集成真实 LLM

### 方案 1: Google Gemini

```bash
dotnet add package Google.GenerativeAI
```

```csharp
public class GeminiA2AAgent
{
    private readonly GenerativeModel _model;
    
    public GeminiA2AAgent(string apiKey)
    {
        _model = new GenerativeModel(apiKey, "gemini-2.0-flash");
    }
    
    public async Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
    {
        // 1. 构建包含 A2UI Schema 的 Prompt
        var prompt = $@"
You are an AI that generates UI using A2UI JSON protocol.

User Query: {query}

Respond with a JSON array of A2UI messages:
[
  {{ ""beginRendering"": {{ ""surfaceId"": ""demo"", ""root"": ""root-id"" }} }},
  {{ ""surfaceUpdate"": {{ ""surfaceId"": ""demo"", ""components"": [...] }} }}
]

Use these component types: Card, Column, Row, Text, Button, List, Image, Icon
";

        // 2. 调用 Gemini
        var response = await _model.GenerateContentAsync(prompt);
        
        // 3. 解析 JSON
        var jsonMatch = Regex.Match(response.Text, @"\[[\s\S]*\]");
        var json = jsonMatch.Value;
        
        // 4. 反序列化
        return JsonSerializer.Deserialize<List<ServerToClientMessage>>(json);
    }
}
```

### 方案 2: Azure OpenAI

```bash
dotnet add package Azure.AI.OpenAI
```

```csharp
public class OpenAIA2AAgent
{
    private readonly OpenAIClient _client;
    
    public async Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
    {
        var systemPrompt = @"
You are an AI that generates user interfaces using A2UI JSON protocol.
Respond only with valid JSON arrays of A2UI messages.
";

        var response = await _client.GetChatCompletionsAsync(
            "gpt-4",
            new ChatCompletionsOptions
            {
                Messages = {
                    new ChatMessage(ChatRole.System, systemPrompt + A2UI_SCHEMA),
                    new ChatMessage(ChatRole.User, query)
                },
                ResponseFormat = ChatCompletionsResponseFormat.JsonObject
            }
        );
        
        var json = response.Value.Choices[0].Message.Content;
        return JsonSerializer.Deserialize<List<ServerToClientMessage>>(json);
    }
}
```

### 在 Program.cs 中替换

```csharp
// 注释掉 Mock Agent
// builder.Services.AddScoped<MockA2AAgent>();

// 添加真实 Agent
builder.Services.AddScoped<GeminiA2AAgent>(sp => 
    new GeminiA2AAgent(builder.Configuration["Gemini:ApiKey"]));

// 修改 A2AClientService 注入
builder.Services.AddScoped<A2AClientService>(sp =>
    new A2AClientService(
        sp.GetRequiredService<GeminiA2AAgent>(), // 使用 Gemini
        sp.GetRequiredService<MessageProcessor>(),
        sp.GetRequiredService<ILogger<A2AClientService>>()
    ));
```

## 📋 A2UI JSON 示例

### 简单卡片

```json
[
  {
    "beginRendering": {
      "surfaceId": "demo",
      "root": "card"
    }
  },
  {
    "surfaceUpdate": {
      "surfaceId": "demo",
      "components": [
        {
          "id": "card",
          "component": {
            "Card": {
              "child": "content"
            }
          }
        },
        {
          "id": "content",
          "component": {
            "Column": {
              "children": {
                "explicitList": ["title", "body"]
              }
            }
          }
        },
        {
          "id": "title",
          "component": {
            "Text": {
              "text": {
                "literalString": "Hello A2UI"
              },
              "usageHint": "h1"
            }
          }
        },
        {
          "id": "body",
          "component": {
            "Text": {
              "text": {
                "literalString": "This UI was generated by an AI agent!"
              }
            }
          }
        }
      ]
    }
  }
]
```

### 带按钮交互

```json
[
  {
    "beginRendering": {
      "surfaceId": "interactive",
      "root": "root"
    }
  },
  {
    "surfaceUpdate": {
      "surfaceId": "interactive",
      "components": [
        {
          "id": "root",
          "component": {
            "Column": {
              "children": {
                "explicitList": ["text", "button"]
              }
            }
          }
        },
        {
          "id": "text",
          "component": {
            "Text": {
              "text": {
                "literalString": "Click the button!"
              }
            }
          }
        },
        {
          "id": "button",
          "component": {
            "Button": {
              "child": "btn-text",
              "primary": true,
              "action": {
                "name": "my_action",
                "context": [
                  {
                    "key": "param1",
                    "value": {
                      "literalString": "value1"
                    }
                  }
                ]
              }
            }
          }
        },
        {
          "id": "btn-text",
          "component": {
            "Text": {
              "text": {
                "literalString": "Click Me"
              }
            }
          }
        }
      ]
    }
  }
]
```

## 🎨 组件库

支持的 A2UI 标准组件:

- ✅ **Card** - 卡片容器
- ✅ **Column** - 垂直布局
- ✅ **Row** - 水平布局
- ✅ **Text** - 文本 (支持 h1-h6, body, caption)
- ✅ **Button** - 按钮 (支持 action)
- ✅ **Image** - 图片
- ✅ **Icon** - 图标
- ✅ **List** - 列表 (支持 template 和数据绑定)
- ✅ **TextField** - 输入框
- ✅ **CheckBox** - 复选框
- ✅ **Divider** - 分隔线
- 🚧 更多组件开发中...

## 📚 深入学习

- 查看 `A2UI_ARCHITECTURE.md` 了解完整架构
- 查看 `Services/MockA2AAgent.cs` 了解 JSON 示例
- 参考 [Google A2UI 官方文档](https://github.com/google/a2ui)

## 🐛 常见问题

### Q: 点击按钮后没有反应？

A: 确保:
1. EventDispatcher 已订阅
2. Button 组件定义了 action
3. OnUserAction 处理器正确实现

### Q: 如何调试 JSON？

A: 在浏览器控制台查看日志:
```
[A2UI] BeginRendering: SurfaceId=demo, Root=root-card
[A2UI] SurfaceUpdate: ComponentCount=5
```

### Q: 如何添加新组件类型？

A: 
1. 在 `A2UI.Core.Types` 中定义组件属性
2. 在 `A2UI.Blazor.Components` 中创建 Razor 组件
3. 在 `A2UIRenderer.razor` 中注册组件映射

## 📞 支持

有问题？查看:
- [A2UI GitHub](https://github.com/google/a2ui)
- [Blazor 文档](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
