
# 🛠️ Build Instructions: MVPChatbot (Blazor + OpenAI API)

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

```csharp
// Services/OpenAIChatService.cs
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
        // Use apiKey and send request to OpenAI API...
    }
}
```

## ✅ 3. Add to `Program.cs`:

```csharp
builder.Services.AddHttpClient<OpenAIChatService>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Configuration.AddUserSecrets<Program>(); // <-- Add this for user-secrets
```

## ✅ 4. Use `dotnet user-secrets` for API Key

```bash
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "sk-..."
```

## ✅ 5. Update UI

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

### B. Update `NavMenu.razor`

```razor
<NavLink class="nav-link" href="chat">
    <span class="oi oi-comment-square" aria-hidden="true"></span> Chat
</NavLink>
```

### C. Update `Index.razor` (Optional)

Add a button linking to `/chat` or a short welcome message.

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
