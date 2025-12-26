# List 组件 Template 模式实现

## 概述

A2UIList 组件现在支持两种渲染模式：

1. **显式列表模式 (Explicit List)**: 手动指定子组件 ID 列表
2. **模板模式 (Template)**: 基于数据绑定动态生成列表项

## 模板模式详解

### 工作原理

模板模式允许 List 组件从数据模型中读取数据集合，并为每个数据项创建一个模板组件的实例。每个实例都有自己的数据上下文路径，使得模板内的相对数据绑定能够正确解析。

### JSON 配置格式

```json
{
  "id": "contact-list",
  "component": {
    "List": {
      "direction": "vertical",
      "children": {
        "template": {
          "componentId": "contact-card-template",
          "dataBinding": "/contacts"
        }
      }
    }
  }
}
```

**参数说明：**

- `template.componentId`: 模板组件的 ID（该组件会被重复渲染）
- `template.dataBinding`: 数据绑定路径，指向数据模型中的集合

### 数据模型结构

```json
{
  "dataModelUpdate": {
    "surfaceId": "demo-surface",
    "path": "/",
    "contents": [
      {
        "key": "contacts",
        "valueMap": [
          {
            "key": "contact1",
            "valueMap": [
              { "key": "name", "valueString": "张三" },
              { "key": "title", "valueString": "工程师" }
            ]
          },
          {
            "key": "contact2",
            "valueMap": [
              { "key": "name", "valueString": "李四" },
              { "key": "title", "valueString": "设计师" }
            ]
          }
        ]
      }
    ]
  }
}
```

### 模板组件定义

模板组件使用相对路径进行数据绑定：

```json
{
  "id": "contact-card-template",
  "component": {
    "Card": {
      "child": "contact-info"
    }
  }
},
{
  "id": "contact-info",
  "component": {
    "Column": {
      "children": {
        "explicitList": ["name-text", "title-text"]
      }
    }
  }
},
{
  "id": "name-text",
  "component": {
    "Text": {
      "text": { "path": "name" },
      "usageHint": "h3"
    }
  }
},
{
  "id": "title-text",
  "component": {
    "Text": {
      "text": { "path": "title" }
    }
  }
}
```

## 实现架构

### 组件层次

```
A2UIList (识别 template 配置)
  └─> A2UIListItem (包装每个列表项)
       └─> A2UIListItemRenderer (克隆模板并设置数据上下文)
            └─> DynamicComponent (渲染实际的模板组件)
```

### 数据上下文传递

1. **A2UIList** 从 `dataBinding` 路径读取数据集合（例如 `/contacts`）
2. 为每个数据项生成一个数据上下文路径（例如 `/contacts/contact1`, `/contacts/contact2`）
3. **A2UIListItemRenderer** 创建模板组件的副本，并设置 `DataContextPath`
4. 模板内的相对路径绑定（如 `{ "path": "name" }`）会相对于该数据上下文解析

### 关键代码片段

#### A2UIList.razor - Template 处理

```csharp
private void ProcessTemplateMode(Dictionary<string, object> templateConfig)
{
    // 获取模板组件ID
    if (templateConfig.TryGetValue("componentId", out var componentIdObj) && 
        componentIdObj is string componentId)
    {
        TemplateComponentId = componentId;
    }

    // 获取数据绑定路径
    if (templateConfig.TryGetValue("dataBinding", out var dataBindingObj) && 
        dataBindingObj is string dataBinding)
    {
        DataBindingPath = dataBinding;
    }

    // 从数据模型获取数据集合
    if (!string.IsNullOrEmpty(DataBindingPath))
    {
        var data = MessageProcessor.GetData(SurfaceId, DataBindingPath, Component.DataContextPath);
        
        if (data is Dictionary<string, object> dataDict)
        {
            TemplateItems = new List<ListItemData>();
            
            // 遍历数据字典的每个键值对
            foreach (var kvp in dataDict)
            {
                var itemPath = $"{DataBindingPath.TrimEnd('/')}/{kvp.Key}";
                TemplateItems.Add(new ListItemData
                {
                    Key = kvp.Key,
                    DataContextPath = itemPath
                });
            }
        }
    }
}
```

#### A2UIListItemRenderer.razor - 数据上下文设置

```csharp
protected override void OnParametersSet()
{
    var surface = MessageProcessor.GetSurface(SurfaceId);
    if (surface != null && surface.Components.TryGetValue(TemplateId, out var node))
    {
        // 创建一个新的ComponentNode副本，设置DataContextPath
        TemplateComponent = new ComponentNode
        {
            Id = node.Id,
            Type = node.Type,
            Properties = node.Properties,
            Weight = node.Weight,
            DataContextPath = DataContextPath  // 关键：设置数据上下文路径
        };
    }
}
```

## 示例场景

### 联系人列表

完整示例请参考: `samples/A2UI.Sample.BlazorServer/MockData/contacts.json`

运行示例：
1. 启动 A2UI.Sample.BlazorServer
2. 访问 `/a2ui-demo`
3. 点击 "👥 显示联系人" 按钮

### 餐厅列表

完整示例请参考: `samples/A2UI.Sample.BlazorServer/MockData/restaurant.json`

运行示例：
1. 启动 A2UI.Sample.BlazorServer
2. 访问 `/a2ui-demo`
3. 点击 "🍝 显示餐厅" 按钮

## 调试技巧

### Console 日志

List 组件会输出详细的调试日志：

```csharp
Console.WriteLine($"[A2UIList] Template mode: ComponentId={TemplateComponentId}, DataBinding={DataBindingPath}");
Console.WriteLine($"[A2UIList] Template item: Key={kvp.Key}, Path={itemPath}");
Console.WriteLine($"[A2UIList] Created {TemplateItems.Count} template items from data binding");
```

### 浏览器开发者工具

1. 打开浏览器控制台（F12）
2. 查看 Console 选项卡
3. 搜索 `[A2UIList]` 查看列表渲染日志
4. 搜索 `[A2UIListItemRenderer]` 查看模板渲染日志

## 优势

1. **减少组件定义数量**: 不需要为每个列表项单独定义组件
2. **动态数据渲染**: 数据变化时自动更新列表
3. **符合 A2UI 规范**: 完全遵循 A2UI 0.8 协议中的 List 组件定义
4. **易于维护**: 模板和数据分离，便于修改

## 与显式列表模式的对比

| 特性 | 显式列表模式 | 模板模式 |
|------|-------------|----------|
| 适用场景 | 固定数量的子组件 | 动态数量的数据列表 |
| 组件定义 | 每个子组件单独定义 | 只需定义一个模板 |
| 数据绑定 | 每个组件独立绑定 | 自动继承数据上下文 |
| JSON 大小 | 较大 | 较小 |
| 维护性 | 较低 | 较高 |

## 技术要点

### ComponentNode 克隆

为了避免修改原始模板组件，我们创建了一个新的 `ComponentNode` 实例：

```csharp
TemplateComponent = new ComponentNode
{
    Id = node.Id,
    Type = node.Type,
    Properties = node.Properties,  // 共享属性引用（不可变）
    Weight = node.Weight,
    DataContextPath = DataContextPath  // 每个实例独立的数据上下文
};
```

### 数据上下文解析

`DataBindingResolver` 和 `MessageProcessor` 通过 `ResolvePath` 方法处理相对路径：

- 绝对路径（以 `/` 开头）：直接使用
- 相对路径：拼接 `dataContextPath` 和 `path`
- 例如：`dataContextPath="/contacts/contact1"`, `path="name"` → `/contacts/contact1/name`

## 未来改进

1. **支持数组类型数据**: 当前只支持 `valueMap`（字典），未来可以支持 `valueList`（数组）
2. **虚拟滚动**: 对于大量数据，实现虚拟滚动优化性能
3. **排序和过滤**: 在客户端支持基本的排序和过滤功能
4. **动画效果**: 添加列表项的进入/退出动画

## 相关文件

- `src/A2UI.Blazor.Components/Components/A2UIList.razor` - 主列表组件
- `src/A2UI.Blazor.Components/Components/A2UIListItem.razor` - 列表项包装器
- `src/A2UI.Blazor.Components/Components/A2UIListItemRenderer.razor` - 模板渲染器
- `src/A2UI.Core/Processing/DataBindingResolver.cs` - 数据绑定解析器
- `src/A2UI.Core/Processing/MessageProcessor.cs` - 消息处理器
- `samples/A2UI.Sample.BlazorServer/MockData/contacts.json` - 联系人示例
- `samples/A2UI.Sample.BlazorServer/MockData/restaurant.json` - 餐厅示例

## 更新日期

2025-12-26

## 作者

A2UI.Blazor Team

