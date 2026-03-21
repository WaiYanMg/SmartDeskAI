# 🤖 SmartDesk AI

An intelligent multi-agent AI business portal built with C# .NET 10, 
Microsoft Semantic Kernel, and Groq AI. AI agents automatically review 
and process employee requests with Human-in-the-Loop oversight.

## ✨ Features

- **HR Agent** — Reviews leave requests against company policy
- **Finance Agent** — Reviews expense claims and budget requests
- **IT Agent** — Reviews software/hardware access requests
- **Orchestrator** — Routes requests to the right agent automatically
- **Human-in-the-Loop** — Low confidence decisions sent to human review queue
- **Priority System** — 🔴 Urgent / 🟠 High / 🟡 Medium / 🟢 Low
- **Document Upload** — AI reads PDF and Word forms automatically
- **Audit Trail** — Every decision saved to Supabase PostgreSQL

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | C# .NET 10, ASP.NET Core Web API |
| AI Framework | Microsoft Semantic Kernel |
| AI Model | Groq (Llama 3.3 70B) / OpenAI GPT-4o-mini |
| Database | Supabase PostgreSQL |
| ORM | Entity Framework Core |
| Document Processing | UglyToad.PdfPig, DocumentFormat.OpenXml |
| API Docs | Swagger / OpenAPI |

## 🏗️ Architecture
```
Employee submits request (text/PDF/Word)
        ↓
Orchestrator Agent — routes to specialist
        ↓
HR / Finance / IT Agent — reads policy + decides
        ↓
Confidence Score check
        ↓
High confidence → Auto APPROVED/DENIED
Low confidence  → Human Review Queue 🔔
        ↓
Manager reviews → Final decision
        ↓
Saved to Supabase with full audit trail
```

## 🚀 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/request/submit | Submit a request for AI review |
| GET | /api/request/history | View all past requests |
| GET | /api/request/queue | View pending human review queue |
| POST | /api/request/queue/{id}/decide | Manager makes final decision |
| GET | /api/request/notifications | View manager notifications |
| GET | /api/request/health | Health check |

## 🔧 Setup

1. Clone the repository
```bash
git clone https://github.com/WaiYanMg/SmartDeskAI.git
cd SmartDeskAI
```

2. Set up user secrets
```bash
dotnet user-secrets set "Groq:Key" "your-groq-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-supabase-connection"
```

3. Run the project
```bash
dotnet run
```

4. Open Swagger UI
```
http://localhost:5276/swagger
```

## 📋 Request Types Supported

- `leave` — Annual leave, sick leave requests
- `expense` — Expense claims and reimbursements
- `claim` — Insurance and other claims
- `it` — Software/hardware access requests
- `booking` — Appointment bookings (coming soon)

## 🗄️ Database Schema

15 tables including: businesses, employees, requests, 
review_queue, notifications, bookings, customers, 
staff, services, booking_slots, leave_balances, 
agent_configs, priority_rules, users, email_logs

## 👨‍💻 Author

**Wai Yan Maung** — Full Stack Software Engineer  
📍 Dublin, Ireland | Stamp 1G  
🔗 [Portfolio](https://wym-portfolio.vercel.app)  
🔗 [LinkedIn](https://linkedin.com/in/wai-yan-maung-8ba161263)

## 📄 License

MIT License