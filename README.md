# 🤖 MVPChatbot

A minimal Blazor Server chatbot that integrates with the OpenAI API — simple, extensible, and beginner-friendly (but not just for beginners).

## 🧠 What It Is

**MVPChatbot** is a Minimum Viable Product chatbot built in **Blazor Server** using C#. It demonstrates how to send messages to a large language model (LLM) like OpenAI’s ChatGPT and display the response — all in a real-time .NET web app.

Whether you're new to Blazor, exploring AI APIs, or just want to create a chatbot that speaks like a pirate, philosopher, or Homer Simpson, this project is for you.

---

## 🚀 Features

- ✅ Blazor Server real-time UI
- ✅ Configurable system prompts for persona modeling
- ✅ Secure API key storage using `dotnet user-secrets`
- ✅ Clean HttpClient-based service layer
- ✅ Fully working local chatbot in under 10 minutes

# 🛠️ Build Instructions: MVPChatbot (Blazor + OpenAI API)

- Estimated Time: ~20–30 min | Skill Level: Beginner-Friendly (some C# experience helpful)

## 🔧 Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)  
  *(Yes, it's EOL, but this project uses it)*
- Visual Studio 2022 or later (with Blazor support)
- OpenAI API key
- Git

## ✅ 1. Create the Project

```bash
dotnet new blazorserver -n MVPChatbot
cd MVPChatbot
dotnet new sln -n MVPChatbot
dotnet sln add MVPChatbot.csproj
```

## ✅ 2. Set Up OpenAI Integration

### A. Add the `OpenAIChatService.cs` to `Services/`:

- Create a new folder named Services
```bash
mkdir Services
```
- Add this class to Services:

```csharp
// Services/OpenAIChatService.cs
using System.Net.Http;
using System.Text;
using System.Text.Json;

public class OpenAIChatService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public OpenAIChatService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> GetChatCompletionAsync(string userInput)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = userInput }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "[No response]";
    }
}
```

## ✅ 3. Add to `Program.cs`:

```csharp
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<OpenAIChatService>();
builder.Configuration.AddUserSecrets<Program>(); // for secure API keys
```

## ✅ 4. Use `dotnet user-secrets` for API Key

```bash
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "sk-..."
```

## ✅ 5. Create the Chat UI

### A. Create `Chat.razor` under `Pages/`

```razor
@page "/chat"
@inject OpenAIChatService ChatService

<h3>Simple Chatbot</h3>

<input @bind="userInput" placeholder="Ask something..." />
<button @onclick="SendMessage">Send</button>

<p>@response</p>

@code {
    private string userInput;
    private string response;

    private async Task SendMessage()
    {
        response = await ChatService.GetChatCompletionAsync(userInput);
    }
}

```

### B. Update `NavMenu.razor` in Shared/

```razor
<NavLink class="nav-link" href="chat">
    <span class="oi oi-comment-square" aria-hidden="true"></span> Chat
</NavLink>
```

### C. Update `Index.razor` (Optional)

Add a button linking to `/chat` or a short welcome message.
```razor
@page "/"

<h1>Welcome to MVPChatbot</h1>
<p>This is a simple Blazor chatbot powered by the OpenAI API.</p>

<a href="chat" class="btn btn-primary">Try the Chatbot</a>
```
## ✅ 6. Build and Run

```bash
dotnet run
```

Navigate to `https://localhost:5001/chat`

## ✅ 7. Git Setup

```bash
git init
echo ".vs/
bin/
obj/
*.user
*.suo
*.userosscache
*.sln.docstates" > .gitignore
git add .
git commit -m "Initial commit of MVPChatbot"
```
( --- )

 **You now have a working chatbot powered by OpenAI and Blazor!**

Feel free to enhance it by:

- Changing the system prompt to act like a pirate, philosopher, etc.
- Adding message history
- Letting users choose a character/persona

Happy hacking!
