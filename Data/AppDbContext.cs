using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Models;

namespace Smart_Desk_AI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<AgentConfig> AgentConfigs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<RequestRecord> Requests { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ReviewQueue> ReviewQueues { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PriorityRule> PriorityRules { get; set; }
        public DbSet<BookingSlot> BookingSlots { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Business
            modelBuilder.Entity<Business>()
                .ToTable("businesses");

            // AgentConfig
            modelBuilder.Entity<AgentConfig>()
                .ToTable("agent_configs");

            // Employee
            modelBuilder.Entity<Employee>()
                .ToTable("employees");

            // LeaveBalance
            modelBuilder.Entity<LeaveBalance>()
                .ToTable("leave_balances");

            // RequestRecord
            modelBuilder.Entity<RequestRecord>()
                .ToTable("requests");
            modelBuilder.Entity<RequestRecord>()
                .Property(r => r.RequestType)
                .HasColumnName("request_type");
            modelBuilder.Entity<RequestRecord>()
                .Property(r => r.BusinessId)
                .HasColumnName("business_id");
            modelBuilder.Entity<RequestRecord>()
                .Property(r => r.EmployeeId)
                .HasColumnName("employee_id");
            modelBuilder.Entity<RequestRecord>()
                .Property(r => r.FinalOutcome)
                .HasColumnName("final_outcome");
            modelBuilder.Entity<RequestRecord>()
                .Property(r => r.AiDecisions)
                .HasColumnName("ai_decisions")
                .HasColumnType("jsonb");
            modelBuilder.Entity<RequestRecord>()
                .Property(r => r.CreatedAt)
                .HasColumnName("created_at");

            // Customer
            modelBuilder.Entity<Customer>()
                .ToTable("customers");

            // Staff
            modelBuilder.Entity<Staff>()
                .ToTable("staff");

            // Service
            modelBuilder.Entity<Service>()
                .ToTable("services");

            // Booking
            modelBuilder.Entity<Booking>()
                .ToTable("bookings");

            // User
            modelBuilder.Entity<User>()
                .ToTable("users");

            // ReviewQueue
            modelBuilder.Entity<ReviewQueue>()
                .ToTable("review_queue");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.RequestId)
                .HasColumnName("request_id");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.AiRecommendation)
                .HasColumnName("ai_recommendation");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.AiConfidence)
                .HasColumnName("ai_confidence");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.AssignedTo)
                .HasColumnName("assigned_to");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.HumanDecision)
                .HasColumnName("human_decision");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.HumanNotes)
                .HasColumnName("human_notes");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.NotifiedAt)
                .HasColumnName("notified_at");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.ReviewedAt)
                .HasColumnName("reviewed_at");
            modelBuilder.Entity<ReviewQueue>()
                .Property(r => r.CreatedAt)
                .HasColumnName("created_at");

            // Notification
            modelBuilder.Entity<Notification>()
                .ToTable("notifications");
            modelBuilder.Entity<Notification>()
                .Property(n => n.UserId)
                .HasColumnName("user_id");
            modelBuilder.Entity<Notification>()
                .Property(n => n.IsRead)
                .HasColumnName("is_read");
            modelBuilder.Entity<Notification>()
                .Property(n => n.RequestId)
                .HasColumnName("request_id");
            modelBuilder.Entity<Notification>()
                .Property(n => n.CreatedAt)
                .HasColumnName("created_at");

            // PriorityRule
            modelBuilder.Entity<PriorityRule>()
                .ToTable("priority_rules");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.BusinessId)
                .HasColumnName("business_id");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.RequestType)
                .HasColumnName("request_type");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.ConditionField)
                .HasColumnName("condition_field");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.ConditionOperator)
                .HasColumnName("condition_operator");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.ConditionValue)
                .HasColumnName("condition_value");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.AlwaysHuman)
                .HasColumnName("always_human");
            modelBuilder.Entity<PriorityRule>()
                .Property(p => p.CreatedAt)
                .HasColumnName("created_at");

            // BookingSlot
            modelBuilder.Entity<BookingSlot>()
                .ToTable("booking_slots");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.BusinessId)
                .HasColumnName("business_id");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.StaffId)
                .HasColumnName("staff_id");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.SlotDate)
                .HasColumnName("slot_date");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.SlotTime)
                .HasColumnName("slot_time");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.IsAvailable)
                .HasColumnName("is_available");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.BookingId)
                .HasColumnName("booking_id");
            modelBuilder.Entity<BookingSlot>()
                .Property(b => b.CreatedAt)
                .HasColumnName("created_at");

            // EmailLog
            modelBuilder.Entity<EmailLog>()
                .ToTable("email_logs");
            modelBuilder.Entity<EmailLog>()
                .Property(e => e.ToEmail)
                .HasColumnName("to_email");
            modelBuilder.Entity<EmailLog>()
                .Property(e => e.SentAt)
                .HasColumnName("sent_at");
            modelBuilder.Entity<EmailLog>()
                .Property(e => e.CreatedAt)
                .HasColumnName("created_at");
        }
    }
}