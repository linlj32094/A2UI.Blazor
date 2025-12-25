# A2UI Blazor .NET 实现 - 架构说明

## 架构概览

这个实现遵循 Google A2UI 的标准架构模式：

```
User Input (Text/Action)
    ↓
A2A Client Service
    ↓
Agent (LLM) - 返回 A2UI JSON
    ↓
MessageProcessor - 解析 JSON 并构建组件树
    ↓
A2UISurface - 渲染 UI
    ↓
User Interaction (Button Click, etc.)
    ↓
循环回到 A2A Client Service
```

## 关键组件

### 1. MockA2AAgent (`Services/MockA2AAgent.cs`)

模拟 LLM Agent 的服务。在真实场景中，这应该：
- 调用 Gemini/GPT 等 LLM
- 在 Prompt 中包含 A2UI Schema
- 解析 LLM 返回的 JSON 响应

**当前实现**: 根据关键词返回预定义的 A2UI JSON

**如何集成真实 LLM:**

```csharp
public class RealA2AAgent
{
    private readonly ILlmClient _llm; // 例如 Google.GenerativeAI 或 Azure.AI.OpenAI
    
    public async Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
    {
        // 1. 构建 Prompt (包含 A2UI Schema 和示例)
        var prompt = BuildPromptWithA2UISchema(query);
        
        // 2. 调用 LLM
        var response = await _llm.GenerateContentAsync(prompt);
        
        // 3. 解析响应中的 JSON
        var jsonString = ExtractA2UIJson(response.Text);
        
        // 4. 反序列化为 ServerToClientMessage 列表
        var messages = JsonSerializer.Deserialize<List<ServerToClientMessage>>(jsonString);
        
        return messages;
    }
    
    private string BuildPromptWithA2UISchema(string userQuery)
    {
        return $@"
You are an AI agent that generates user interfaces using A2UI protocol.

## A2UI Protocol
A2UI uses JSON messages to describe UI components. You must return a JSON array with these message types:

1. beginRendering - Signals the start of rendering
2. surfaceUpdate - Defines UI components
3. dataModelUpdate - Provides data for components

## Example Response Format:
---a2ui_JSON---
[
  {{ ""beginRendering"": {{ ""surfaceId"": ""demo"", ""root"": ""root-card"" }} }},
  {{ ""surfaceUpdate"": {{ ""surfaceId"": ""demo"", ""components"": [...] }} }},
  {{ ""dataModelUpdate"": {{ ""surfaceId"": ""demo"", ""path"": ""/"", ""contents"": [...] }} }}
]

## User Query: {userQuery}

Generate appropriate A2UI JSON to respond to the user's query.
";
    }
}
```

### 2. A2AClientService (`Services/A2AClientService.cs`)

客户端服务，负责：
- 将用户输入发送到 Agent
- 接收 A2UI JSON 响应
- 通过 MessageProcessor 处理消息
- 管理 Surface 生命周期

### 3. MessageProcessor (`A2UI.Core.Processing/MessageProcessor.cs`)

核心处理器，负责：
- 解析 A2UI JSON 消息
- 构建组件树
- 管理数据模型
- 触发 UI 更新事件

### 4. A2UISurface (`A2UI.Blazor.Components/A2UISurface.razor`)

Surface 组件，负责：
- 监听 MessageProcessor 的更新事件
- 渲染组件树
- 响应用户交互

## A2UI JSON 消息格式

### 1. BeginRendering Message
```json
{
  "beginRendering": {
    "surfaceId": "demo-surface",
    "root": "root-component-id",
    "catalogId": "org.a2ui.standard@0.8",
    "styles": {
      "primaryColor": "#007BFF"
    }
  }
}
```

### 2. SurfaceUpdate Message
```json
{
  "surfaceUpdate": {
    "surfaceId": "demo-surface",
    "components": [
      {
        "id": "root-card",
        "component": {
          "Card": {
            "child": "content-column"
          }
        }
      },
      {
        "id": "content-column",
        "component": {
          "Column": {
            "children": {
              "explicitList": ["title-text", "body-text", "action-button"]
            }
          }
        }
      },
      {
        "id": "title-text",
        "component": {
          "Text": {
            "text": {
              "literalString": "Welcome"
            },
            "usageHint": "h1"
          }
        }
      }
    ]
  }
}
```

### 3. DataModelUpdate Message (用于动态数据)
```json
{
  "dataModelUpdate": {
    "surfaceId": "demo-surface",
    "path": "/",
    "contents": [
      {
        "key": "userName",
        "valueString": "John Doe"
      },
      {
        "key": "contacts",
        "valueMap": [
          {
            "key": "contact1",
            "valueMap": [
              { "key": "name", "valueString": "Alice" },
              { "key": "email", "valueString": "alice@example.com" }
            ]
          }
        ]
      }
    ]
  }
}
```

## 数据绑定

A2UI 支持两种文本/数据方式：

### 1. 字面字符串 (literalString)
```json
{
  "Text": {
    "text": {
      "literalString": "Hello World"
    }
  }
}
```

### 2. 数据路径 (path)
```json
{
  "Text": {
    "text": {
      "path": "userName"  // 从 DataModel 中读取 /userName
    }
  }
}
```

## 交互流程

### 用户操作 → Agent 响应

1. **用户点击按钮**:
```json
{
  "Button": {
    "action": {
      "name": "view_contact",
      "context": [
        {
          "key": "contactName",
          "value": { "path": "name" }
        }
      ]
    }
  }
}
```

2. **EventDispatcher 捕获事件**:
```csharp
EventDispatcher.UserActionDispatched += async (sender, e) => {
    var actionName = e.Action.Name; // "view_contact"
    var context = e.Action.Context;  // { "contactName": "Alice" }
    
    // 发送到 Agent
    await A2AClient.SendActionAsync(actionName, context);
};
```

3. **Agent 返回新的 UI**:
```json
[
  { "beginRendering": { "surfaceId": "detail-view", "root": "contact-card" } },
  { "surfaceUpdate": { ... } },
  { "dataModelUpdate": { ... } }
]
```

## 使用示例

### 在 Blazor 页面中使用

```razor
@page "/demo"
@inject A2AClientService A2AClient

<div class="chat-interface">
    <input type="text" @bind="userInput" @onkeyup="HandleEnter" />
    <button @onclick="SendMessage">Send</button>
    
    <A2UISurface SurfaceId="@currentSurfaceId" />
</div>

@code {
    private string userInput = "";
    private string currentSurfaceId = "main-surface";
    
    private async Task SendMessage()
    {
        // 清除旧 Surface
        A2AClient.ClearSurfaces();
        
        // 发送查询到 Agent
        await A2AClient.SendQueryAsync(userInput, currentSurfaceId);
        
        userInput = "";
    }
}
```

## 与 Google 原始实现的对比

| 特性 | Google 实现 (TypeScript/Python) | 本实现 (.NET/Blazor) |
|------|----------------------------------|----------------------|
| Agent 通信 | A2A Protocol (HTTP/SSE) | A2AClientService |
| 消息处理 | MessageProcessor (TypeScript) | MessageProcessor (C#) |
| UI 渲染 | Lit/Angular Components | Blazor Components |
| 数据绑定 | Signals/Observables | Blazor State Management |
| 事件分发 | EventEmitter | EventDispatcher with C# Events |

## 下一步：集成真实 LLM

### 使用 Google Gemini

```csharp
// 安装: dotnet add package Google.GenerativeAI

public class GeminiA2AAgent
{
    private readonly GenerativeModel _model;
    
    public GeminiA2AAgent(string apiKey)
    {
        _model = new GenerativeModel(apiKey, "gemini-2.0-flash");
    }
    
    public async Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
    {
        var prompt = BuildA2UIPrompt(query);
        var response = await _model.GenerateContentAsync(prompt);
        
        // 提取 JSON 并解析
        var json = ExtractJsonFromResponse(response.Text);
        return JsonSerializer.Deserialize<List<ServerToClientMessage>>(json);
    }
}
```

### 使用 Azure OpenAI

```csharp
// 安装: dotnet add package Azure.AI.OpenAI

public class OpenAIA2AAgent
{
    private readonly OpenAIClient _client;
    
    public OpenAIA2AAgent(string endpoint, string apiKey)
    {
        _client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
    }
    
    public async Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
    {
        var prompt = BuildA2UIPrompt(query);
        
        var response = await _client.GetChatCompletionsAsync(
            "gpt-4",
            new ChatCompletionsOptions
            {
                Messages = {
                    new ChatMessage(ChatRole.System, A2UI_SYSTEM_PROMPT),
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

## 参考资源

- [A2UI 官方规范](https://github.com/google/a2ui)
- [Google A2UI Samples](https://github.com/google/a2ui/tree/main/samples)
- [A2A Protocol](https://github.com/google/a2a)

