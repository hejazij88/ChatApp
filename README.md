# 📬 Blazor Chat App with SignalR and EF Core

A simple real-time chat application built with **Blazor Server**, **SignalR**, and **Entity Framework Core**.  
Supports group messaging, private messaging, group creation, and message persistence in a database.

---

## ✨ Features

- ✅ Group chat support
- ✅ Private messaging between users
- ✅ Create and join groups
- ✅ Persist messages using EF Core and SQLite
- ✅ Load previous messages when entering a group

---

## 🖼️ Overview

This is a basic chat app to demonstrate real-time communication using SignalR and Blazor.  
All messages are saved in a local SQLite database and grouped by room or recipient.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- EF Core CLI tools:

```bash
dotnet tool install --global dotnet-ef
Setup
bash
Copy code
git clone https://github.com/your-username/blazor-chat-app.git
cd blazor-chat-app
dotnet restore
Create the Database
bash
Copy code
dotnet ef migrations add InitialCreate
dotnet ef database update
This creates a chat.db SQLite file in your project root.

Run the App
bash
Copy code
dotnet run
Then open your browser at:

arduino
Copy code
http://localhost:5000
📁 Project Structure
arduino
Copy code
📁 Models
    └── ChatMessage.cs     → Message model
📁 Data
    └── ChatDbContext.cs   → EF Core DbContext
📁 Hubs
    └── ChatHub.cs         → SignalR Hub logic
📄 Pages/Chat.razor        → Main Blazor chat page
📄 Program.cs              → Dependency injection and setup
🔧 Future Enhancements (TODO)
 Support for image/file sharing

 Typing indicators

 Blazor WASM version with separate Web API

👨‍💻 Author
This project was created for educational purposes to learn Blazor + SignalR + EF Core.

Made with ❤️ by [Mohammad Javad]

