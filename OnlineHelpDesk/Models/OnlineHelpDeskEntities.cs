using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OnlineHelpDesk.Models
{
    public partial class OnlineHelpDeskEntities : DbContext
    {
        public OnlineHelpDeskEntities()
        {
        }
        public OnlineHelpDeskEntities(DbContextOptions<OnlineHelpDeskEntities> options)
            : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Discussion> Discussions { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.Roleld)
                    .HasConstraintName("FK_Account_Role");
                entity.Property(e => e.Phone)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Discussion>(entity =>
            {
                entity.ToTable("Discussion");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Discussion_Account");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Discussion_Ticket");
            });

            modelBuilder.Entity<Period>(entity =>
            {
                entity.ToTable("Period");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_Photo_Ticket");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Ticket_Category");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TicketEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Ticket_Account");

                entity.HasOne(d => d.Period)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PeriodId)
                    .HasConstraintName("FK_Ticket_Period");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Ticket_Status");

                entity.HasOne(d => d.Supporter)
                    .WithMany(p => p.TicketSupporters)
                    .HasForeignKey(d => d.SupporterId)
                    .HasConstraintName("FK_Ticket_Account1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
