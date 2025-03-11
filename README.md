
# Entity Framework Query Tracker Support Site

This application manages incoming user requests (tickets) and streamlines their resolution. It leverages **MS Identity** for secure, role-based access, ensuring different user roles have proper permissions when viewing and handling tickets.

---

## 🚀 Features
- 🔐 **MS Identity Integration**: Secure authentication and authorization, enabling seamless role-based access control (e.g., Admin, Support Agent, User).
- 🏷️ **Entity Framework**: Streamlined data modeling, migrations, and CRUD operations for tickets and user profiles.
- ⏱️ **Real-Time Ticket Management**: Create, assign, and track tickets from open to resolved status.
- ⚙️ **Role-Specific Workflows**: Different roles have access to distinct features (view all tickets, assign to agents, mark as complete, etc.).
- 📈 **Insightful Overview**: Dashboard-style analytics to monitor ticket volume, response times, and completion rates.

---

## 📥 Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/ValsCodes/EF6-QueryTaker_v3
   ```

2. **Restore & Build**:
    ```bash
    dotnet restore
    dotnet build
    ```
