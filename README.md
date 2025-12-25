# A2UI for .NET Blazor

A complete .NET 9 implementation of the A2UI (Agent to UI) protocol for Blazor applications.

## 项目结构

```
blazor-dotnet/
├── src/
│   ├── A2UI.Core/                    # 核心协议处理库
│   ├── A2UI.Blazor.Components/       # Blazor 组件库  
│   ├── A2UI.AgentSDK/                # Agent 端 SDK
│   └── A2UI.Theming/                 # 主题系统
├── samples/
│   ├── A2UI.Sample.BlazorServer/     # Blazor Server 示例
│   ├── A2UI.Sample.BlazorWasm/       # Blazor WASM 示例
│   └── A2UI.Sample.Agent/            # .NET Agent 示例
├── tests/
│   ├── A2UI.Core.Tests/              # 核心库测试
│   └── A2UI.Blazor.Components.Tests/ # 组件测试
└── A2UI.Blazor.sln                   # 解决方案文件
```

## 什么是 A2UI?

A2UI (Agent to UI) 是一个声明式 UI 协议，用于代理驱动的界面。AI 代理生成丰富的交互式 UI，可在各个平台（Web、移动、桌面）上原生渲染，而无需执行任意代码。

### 核心价值

1. **安全性**：声明式数据，而非代码。代理从客户端的可信目录请求组件。无代码执行风险。
2. **原生体验**：无 iframe。客户端使用自己的 UI 框架渲染。继承应用样式、可访问性和性能。
3. **可移植性**：一个代理响应可在任何地方工作。相同的 JSON 可在 Web（Lit/Angular/React/Blazor）、移动（Flutter/SwiftUI）、桌面上渲染。

## 快速开始

### 安装

```bash
# 从解决方案根目录
dotnet restore
dotnet build
```

### 运行示例

```bash
# Blazor Server 示例
cd samples/A2UI.Sample.BlazorServer
dotnet run

# Blazor WASM 示例
cd samples/A2UI.Sample.BlazorWasm
dotnet run
```

## NuGet 包

- `A2UI.Core` - 核心协议库
- `A2UI.Blazor` - Blazor 组件库
- `A2UI.AgentSDK` - Agent 开发 SDK
- `A2UI.Theming` - 主题系统

## 技术栈

- .NET 9.0
- Blazor Server（通过 SignalR）
- Blazor WebAssembly（通过 HttpClient + SSE）
- 符合 A2UI 0.8 协议规范

## 文档

- [A2UI 协议规范](../specification/0.8/docs/a2ui_protocol.md)
- [组件目录](../specification/0.8/json/standard_catalog_definition.json)
- [渲染器开发指南](../docs/guides/renderer-development.md)

## 许可证

Apache 2.0 - 参见 [LICENSE](../LICENSE)

## 贡献

欢迎贡献！请参阅 [CONTRIBUTING.md](../CONTRIBUTING.md) 了解详情。

