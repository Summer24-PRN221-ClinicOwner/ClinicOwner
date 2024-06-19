using System;
using System.Collections.Generic;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories;

public partial class ClinicContext : DbContext
{
    public ClinicContext()
    {
    }

    public ClinicContext(DbContextOptions<ClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<ClinicOwner> ClinicOwners { get; set; }

    public virtual DbSet<Dentist> Dentists { get; set; }

    public virtual DbSet<DentistAvailability> DentistAvailabilities { get; set; }

    public virtual DbSet<License> Licenses { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomAvailability> RoomAvailabilities { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database=ClinicSchedule; TrustServerCertificate = true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointment");

            entity.HasIndex(e => e.DentistId, "IX_Appointment_DentistID");

            entity.HasIndex(e => e.PatientId, "IX_Appointment_PatientID");

            entity.HasIndex(e => e.RoomId, "IX_Appointment_RoomID");

            entity.HasIndex(e => e.ServiceId, "IX_Appointment_ServiceID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AppointDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DentistId).HasColumnName("DentistID");
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK_Appointment_Dentist");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK_Appointment_Patient");

            entity.HasOne(d => d.Room).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Appointment_Room");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_Appointment_Service");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.ToTable("Clinic");

            entity.HasIndex(e => e.OwnerId, "IX_Clinic_OwnerID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Owner).WithMany(p => p.Clinics)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clinic_ClinicOwner");
        });

        modelBuilder.Entity<ClinicOwner>(entity =>
        {
            entity.ToTable("ClinicOwner");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.ClinicOwner)
                .HasForeignKey<ClinicOwner>(d => d.Id)
                .HasConstraintName("FK_ClinicOwner_User");
        });

        modelBuilder.Entity<Dentist>(entity =>
        {
            entity.ToTable("Dentist");

            entity.HasIndex(e => e.ClinicId, "IX_Dentist_ClinicID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ClinicId).HasColumnName("ClinicID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.Clinic).WithMany(p => p.Dentists)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dentist_Clinic");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Dentist)
                .HasForeignKey<Dentist>(d => d.Id)
                .HasConstraintName("FK_Dentist_User");

            entity.HasMany(d => d.Services).WithMany(p => p.Dentists)
                .UsingEntity<Dictionary<string, object>>(
                    "DentistService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DentistService_Service"),
                    l => l.HasOne<Dentist>().WithMany()
                        .HasForeignKey("DentistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DentistService_Dentist"),
                    j =>
                    {
                        j.HasKey("DentistId", "ServiceId");
                        j.ToTable("DentistService");
                        j.HasIndex(new[] { "ServiceId" }, "IX_DentistService_ServiceId");
                    });
        });

        modelBuilder.Entity<DentistAvailability>(entity =>
        {
            entity.ToTable("DentistAvailability");

            entity.HasIndex(e => e.DentistId, "IX_DentistAvailability_DentistID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AvailableSlots)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Day).HasColumnType("datetime");
            entity.Property(e => e.DentistId).HasColumnName("DentistID");

            entity.HasOne(d => d.Dentist).WithMany(p => p.DentistAvailabilities)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK_DentistAvailability_Dentist");
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.ToTable("License");

            entity.HasIndex(e => e.DentistId, "IX_License_DentistId");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.LicenceType).HasMaxLength(50);
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Dentist).WithMany(p => p.Licenses)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK_License_Dentist");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");

            entity.HasIndex(e => e.DentistId, "IX_Message_DentistID");

            entity.HasIndex(e => e.PatientId, "IX_Message_PatientID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateSeen).HasColumnType("datetime");
            entity.Property(e => e.DateSend).HasColumnType("datetime");
            entity.Property(e => e.DentistId).HasColumnName("DentistID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Sender).HasDefaultValue("");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Messages)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK_Message_Dentist");

            entity.HasOne(d => d.Patient).WithMany(p => p.Messages)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK_Message_Patient");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.HasIndex(e => e.UserId, "IX_Notification_UserID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patient");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Id)
                .HasConstraintName("FK_Patient_User");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Data).HasMaxLength(500);
            entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.HasIndex(e => e.ClinicId, "IX_Room_ClinicID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClinicId).HasColumnName("ClinicID");
            entity.Property(e => e.RoomNumber).HasMaxLength(50);

            entity.HasOne(d => d.Clinic).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK_Room_Clinic");
        });

        modelBuilder.Entity<RoomAvailability>(entity =>
        {
            entity.ToTable("RoomAvailability");

            entity.HasIndex(e => e.RoomId, "IX_RoomAvailability_RoomID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Day).HasColumnType("datetime");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomAvailabilities)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomAvailability_Room");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
