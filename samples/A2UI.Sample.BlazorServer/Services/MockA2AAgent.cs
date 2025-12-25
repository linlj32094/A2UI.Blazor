using System.Text.Json;
using A2UI.Core.Messages;

namespace A2UI.Sample.BlazorServer.Services;

/// <summary>
/// Mock A2A Agent that simulates an LLM returning A2UI JSON responses
/// In a real application, this would call an actual LLM/Agent service
/// </summary>
public class MockA2AAgent
{
    private readonly ILogger<MockA2AAgent> _logger;

    public MockA2AAgent(ILogger<MockA2AAgent> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Simulates an Agent processing a user query and returning A2UI JSON messages
    /// </summary>
    public Task<List<ServerToClientMessage>> ProcessQueryAsync(string query)
    {
        _logger.LogInformation($"[MockA2AAgent] Processing query: {query}");

        // In a real implementation, this would:
        // 1. Send the query to an LLM with A2UI schema in the prompt
        // 2. Parse the LLM's JSON response
        // 3. Return the A2UI messages

        // For now, we'll match specific keywords and return appropriate UIs
        var messages = query.ToLower() switch
        {
            var q when q.Contains("联系人") || q.Contains("contact") => GetContactListExample(),
            var q when q.Contains("餐厅") || q.Contains("restaurant") => GetRestaurantExample(),
            var q when q.Contains("按钮") || q.Contains("button") => GetButtonExample(),
            var q when q.Contains("卡片") || q.Contains("card") => GetSimpleCardExample(),
            _ => GetWelcomeExample()
        };

        _logger.LogInformation($"[MockA2AAgent] Returning {messages.Count} messages");
        return Task.FromResult(messages);
    }

    private List<ServerToClientMessage> GetWelcomeExample()
    {
        return new List<ServerToClientMessage>
        {
            new ServerToClientMessage
            {
                BeginRendering = new BeginRenderingMessage
                {
                    SurfaceId = "demo-surface",
                    Root = "root-card",
                    CatalogId = "org.a2ui.standard@0.8"
                }
            },
            new ServerToClientMessage
            {
                SurfaceUpdate = new SurfaceUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Components = new List<ComponentDefinition>
                    {
                        new ComponentDefinition
                        {
                            Id = "root-card",
                            Component = new Dictionary<string, object>
                            {
                                ["Card"] = new Dictionary<string, object>
                                {
                                    ["child"] = "content-column"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "content-column",
                            Component = new Dictionary<string, object>
                            {
                                ["Column"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "title", "description", "hint" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "title",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "欢迎使用 A2UI！"
                                    },
                                    ["usageHint"] = "h1"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "description",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "这是一个由 AI Agent 生成的动态界面。"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "hint",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "💡 试试输入: 显示联系人、显示餐厅、显示按钮"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private List<ServerToClientMessage> GetSimpleCardExample()
    {
        return new List<ServerToClientMessage>
        {
            new ServerToClientMessage
            {
                BeginRendering = new BeginRenderingMessage
                {
                    SurfaceId = "demo-surface",
                    Root = "root",
                    CatalogId = "org.a2ui.standard@0.8"
                }
            },
            new ServerToClientMessage
            {
                SurfaceUpdate = new SurfaceUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Components = new List<ComponentDefinition>
                    {
                        new ComponentDefinition
                        {
                            Id = "root",
                            Component = new Dictionary<string, object>
                            {
                                ["Card"] = new Dictionary<string, object>
                                {
                                    ["child"] = "content"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "content",
                            Component = new Dictionary<string, object>
                            {
                                ["Column"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "title", "body" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "title",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "简单卡片"
                                    },
                                    ["usageHint"] = "h2"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "body",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "这是一个由 Agent 返回的简单卡片。"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private List<ServerToClientMessage> GetButtonExample()
    {
        return new List<ServerToClientMessage>
        {
            new ServerToClientMessage
            {
                BeginRendering = new BeginRenderingMessage
                {
                    SurfaceId = "demo-surface",
                    Root = "root",
                    CatalogId = "org.a2ui.standard@0.8"
                }
            },
            new ServerToClientMessage
            {
                SurfaceUpdate = new SurfaceUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Components = new List<ComponentDefinition>
                    {
                        new ComponentDefinition
                        {
                            Id = "root",
                            Component = new Dictionary<string, object>
                            {
                                ["Card"] = new Dictionary<string, object>
                                {
                                    ["child"] = "content"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "content",
                            Component = new Dictionary<string, object>
                            {
                                ["Column"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "title", "desc", "button-row" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "title",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "交互按钮演示"
                                    },
                                    ["usageHint"] = "h2"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "desc",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "点击按钮与 Agent 交互："
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "button-row",
                            Component = new Dictionary<string, object>
                            {
                                ["Row"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "btn1", "btn2" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "btn1",
                            Component = new Dictionary<string, object>
                            {
                                ["Button"] = new Dictionary<string, object>
                                {
                                    ["child"] = "btn1-text",
                                    ["primary"] = true,
                                    ["action"] = new Dictionary<string, object>
                                    {
                                        ["name"] = "like_action"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "btn1-text",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "👍 喜欢"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "btn2",
                            Component = new Dictionary<string, object>
                            {
                                ["Button"] = new Dictionary<string, object>
                                {
                                    ["child"] = "btn2-text",
                                    ["action"] = new Dictionary<string, object>
                                    {
                                        ["name"] = "share_action"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "btn2-text",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "🔗 分享"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private List<ServerToClientMessage> GetContactListExample()
    {
        return new List<ServerToClientMessage>
        {
            new ServerToClientMessage
            {
                BeginRendering = new BeginRenderingMessage
                {
                    SurfaceId = "demo-surface",
                    Root = "root-column",
                    CatalogId = "org.a2ui.standard@0.8"
                }
            },
            new ServerToClientMessage
            {
                SurfaceUpdate = new SurfaceUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Components = new List<ComponentDefinition>
                    {
                        new ComponentDefinition
                        {
                            Id = "root-column",
                            Component = new Dictionary<string, object>
                            {
                                ["Column"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "title", "list" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "title",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "团队联系人"
                                    },
                                    ["usageHint"] = "h1"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "list",
                            Component = new Dictionary<string, object>
                            {
                                ["List"] = new Dictionary<string, object>
                                {
                                    ["direction"] = "vertical",
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["template"] = new Dictionary<string, object>
                                        {
                                            ["componentId"] = "card-template",
                                            ["dataBinding"] = "/contacts"
                                        }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "card-template",
                            Component = new Dictionary<string, object>
                            {
                                ["Card"] = new Dictionary<string, object>
                                {
                                    ["child"] = "card-row"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "card-row",
                            Component = new Dictionary<string, object>
                            {
                                ["Row"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "card-content", "view-btn" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "card-content",
                            Component = new Dictionary<string, object>
                            {
                                ["Column"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "name-text", "title-text" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "name-text",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["path"] = "name"
                                    },
                                    ["usageHint"] = "h3"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "title-text",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["path"] = "title"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "view-btn",
                            Component = new Dictionary<string, object>
                            {
                                ["Button"] = new Dictionary<string, object>
                                {
                                    ["child"] = "view-btn-text",
                                    ["primary"] = true,
                                    ["action"] = new Dictionary<string, object>
                                    {
                                        ["name"] = "view_contact",
                                        ["context"] = new[]
                                        {
                                            new Dictionary<string, object>
                                            {
                                                ["key"] = "contactName",
                                                ["value"] = new Dictionary<string, object>
                                                {
                                                    ["path"] = "name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "view-btn-text",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "查看"
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new ServerToClientMessage
            {
                DataModelUpdate = new DataModelUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Path = "/",
                    Contents = new List<DataEntry>
                    {
                        new DataEntry
                        {
                            Key = "contacts",
                            ValueMap = new List<DataEntry>
                            {
                                new DataEntry
                                {
                                    Key = "contact1",
                                    ValueMap = new List<DataEntry>
                                    {
                                        new DataEntry { Key = "name", ValueString = "张三" },
                                        new DataEntry { Key = "title", ValueString = "高级工程师" },
                                        new DataEntry { Key = "email", ValueString = "zhangsan@example.com" }
                                    }
                                },
                                new DataEntry
                                {
                                    Key = "contact2",
                                    ValueMap = new List<DataEntry>
                                    {
                                        new DataEntry { Key = "name", ValueString = "李四" },
                                        new DataEntry { Key = "title", ValueString = "产品经理" },
                                        new DataEntry { Key = "email", ValueString = "lisi@example.com" }
                                    }
                                },
                                new DataEntry
                                {
                                    Key = "contact3",
                                    ValueMap = new List<DataEntry>
                                    {
                                        new DataEntry { Key = "name", ValueString = "王五" },
                                        new DataEntry { Key = "title", ValueString = "UI设计师" },
                                        new DataEntry { Key = "email", ValueString = "wangwu@example.com" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private List<ServerToClientMessage> GetRestaurantExample()
    {
        return new List<ServerToClientMessage>
        {
            new ServerToClientMessage
            {
                BeginRendering = new BeginRenderingMessage
                {
                    SurfaceId = "demo-surface",
                    Root = "root",
                    CatalogId = "org.a2ui.standard@0.8"
                }
            },
            new ServerToClientMessage
            {
                SurfaceUpdate = new SurfaceUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Components = new List<ComponentDefinition>
                    {
                        new ComponentDefinition
                        {
                            Id = "root",
                            Component = new Dictionary<string, object>
                            {
                                ["Card"] = new Dictionary<string, object>
                                {
                                    ["child"] = "content"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "content",
                            Component = new Dictionary<string, object>
                            {
                                ["Column"] = new Dictionary<string, object>
                                {
                                    ["children"] = new Dictionary<string, object>
                                    {
                                        ["explicitList"] = new[] { "title", "address", "button" }
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "title",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["path"] = "name"
                                    },
                                    ["usageHint"] = "h2"
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "address",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["path"] = "address"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "button",
                            Component = new Dictionary<string, object>
                            {
                                ["Button"] = new Dictionary<string, object>
                                {
                                    ["child"] = "button-text",
                                    ["primary"] = true,
                                    ["action"] = new Dictionary<string, object>
                                    {
                                        ["name"] = "book_restaurant"
                                    }
                                }
                            }
                        },
                        new ComponentDefinition
                        {
                            Id = "button-text",
                            Component = new Dictionary<string, object>
                            {
                                ["Text"] = new Dictionary<string, object>
                                {
                                    ["text"] = new Dictionary<string, object>
                                    {
                                        ["literalString"] = "立即预订"
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new ServerToClientMessage
            {
                DataModelUpdate = new DataModelUpdateMessage
                {
                    SurfaceId = "demo-surface",
                    Path = "/",
                    Contents = new List<DataEntry>
                    {
                        new DataEntry { Key = "name", ValueString = "意大利餐厅" },
                        new DataEntry { Key = "address", ValueString = "北京市朝阳区建国路88号" },
                        new DataEntry { Key = "rating", ValueNumber = 4.5 }
                    }
                }
            }
        };
    }
}

