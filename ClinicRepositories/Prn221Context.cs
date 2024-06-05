using System;
using System.Collections.Generic;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClinicRepositories;

public partial class Prn221Context : DbContext
{
    public Prn221Context()
    {
    }

    public Prn221Context(DbContextOptions<Prn221Context> options)
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

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }
    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:MyClinicDB"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA240D53A9D");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.ClinicId).HasColumnName("ClinicID");
            entity.Property(e => e.DentistId).HasColumnName("DentistID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Clinic");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__Appointme__Denti__4316F928");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__4222D4EF");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Appointme__Servi__440B1D61");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.ToTable("Clinic");

            entity.Property(e => e.ClinicId)
                .ValueGeneratedNever()
                .HasColumnName("ClinicID");
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
            entity.HasKey(e => e.OwnerId).HasName("PK__ClinicOw__81938598F0068CE8");

            entity.ToTable("ClinicOwner");

            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Dentist>(entity =>
        {
            entity.HasKey(e => e.DentistId).HasName("PK__Dentist__9157336F13F532B2");

            entity.ToTable("Dentist");

            entity.Property(e => e.DentistId).HasColumnName("DentistID");
            entity.Property(e => e.ClinicId).HasColumnName("ClinicID");
            entity.Property(e => e.ContactNumber).HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Specialization).HasMaxLength(100);

            entity.HasOne(d => d.Clinic).WithMany(p => p.Dentists)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dentist_Clinic");

            entity.HasOne(d => d.License).WithMany(p => p.Dentists)
                .HasForeignKey(d => d.LicenseId)
                .HasConstraintName("FK_Dentist_License");
        });

        modelBuilder.Entity<DentistAvailability>(entity =>
        {
            entity.HasKey(e => e.DentistAvailabilityId).HasName("PK__DentistA__9FD910EDC7F15BFB");

            entity.ToTable("DentistAvailability");

            entity.Property(e => e.DentistAvailabilityId).HasColumnName("DentistAvailabilityID");
            entity.Property(e => e.AvailableSlots)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Day).HasColumnType("datetime");
            entity.Property(e => e.DentistId).HasColumnName("DentistID");

            entity.HasOne(d => d.Dentist).WithMany(p => p.DentistAvailabilities)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__DentistAv__Denti__3D5E1FD2");
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.ToTable("License");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.LicenceType).HasMaxLength(50);
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");

            entity.Property(e => e.MessageId)
                .ValueGeneratedNever()
                .HasColumnName("MessageID");
            entity.Property(e => e.DateSeen).HasColumnType("datetime");
            entity.Property(e => e.DateSend).HasColumnType("datetime");
            entity.Property(e => e.DentistId).HasColumnName("DentistID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");

            entity.HasOne(d => d.Patient).WithMany(p => p.Messages)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK_Message_Dentist");

            entity.HasOne(d => d.PatientNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK_Message_Patient");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__970EC3462924A5B5");

            entity.ToTable("Patient");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E5661F397D");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.ReportName).HasMaxLength(100);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB0EAB303B4CC");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Dentist");

            entity.HasOne(d => d.Id1).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_ClinicOwner");

            entity.HasOne(d => d.Id2).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Patient");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
