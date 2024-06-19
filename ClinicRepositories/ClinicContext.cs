using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClinicRepositories
{
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
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomAvailability> RoomAvailabilities { get; set; }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            return configuration["ConnectionStrings:MyClinicDB"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsRead)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_User");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Appointment");
                entity.ToTable("Appointment");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.DentistId).HasColumnName("DentistID");
                entity.Property(e => e.PatientId).HasColumnName("PatientID");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                entity.Property(e => e.RoomId).HasColumnName("RoomID");


                entity.HasOne(d => d.Dentist).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DentistId)
                    .HasConstraintName("FK_Appointment_Dentist");

                entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Appointment_Patient");

                entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Appointment_Service");

                entity.HasOne(d => d.Room).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_Room");
            });

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.ToTable("Clinic");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Address).HasMaxLength(150);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
                entity.Property(e => e.Phone).HasMaxLength(15).IsUnicode(false);

                entity.HasOne(d => d.Owner).WithMany(p => p.Clinics)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clinic_ClinicOwner");

                entity.HasMany(c => c.Rooms)
                .WithOne(r => r.Clinic)
                .HasForeignKey(r => r.ClinicId)
                .HasConstraintName("FK_Room_Clinic");
            });

            modelBuilder.Entity<DentistAvailability>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_DentistAvailability");
                entity.ToTable("DentistAvailability");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.AvailableSlots).HasMaxLength(10).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.Day).HasColumnType("datetime");
                entity.Property(e => e.DentistId).HasColumnName("DentistID");

                entity.HasOne(d => d.Dentist).WithMany(p => p.DentistAvailabilities)
                    .HasForeignKey(d => d.DentistId)
                    .HasConstraintName("FK_DentistAvailability_Dentist");
            });

            modelBuilder.Entity<License>(entity =>
            {
                entity.ToTable("License");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.ExpireDate).HasColumnType("datetime");
                entity.Property(e => e.IssueDate).HasColumnType("datetime");
                entity.Property(e => e.LicenceType).HasMaxLength(50);
                entity.Property(e => e.LicenseNumber).HasMaxLength(10).IsFixedLength();
            });

            modelBuilder.Entity<Message>(entity =>
            {
            entity.ToTable("Message");
            entity.Property(e => e.MessageId).HasColumnName("ID").ValueGeneratedOnAdd();
            entity.Property(e => e.DateSeen).HasColumnType("datetime");
                entity.Property(e => e.DateSend).HasColumnType("datetime");
                entity.Property(e => e.DentistId).HasColumnName("DentistID");
                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.HasOne(d => d.Patient).WithMany(p => p.Messages)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Message_Patient");

                entity.HasOne(d => d.Dentist).WithMany(p => p.Messages)
                    .HasForeignKey(d => d.DentistId)
                    .HasConstraintName("FK_Message_Dentist");
            });


            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Room");
                entity.ToTable("Room");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.RoomNumber).HasMaxLength(50);
                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.HasOne(d => d.Clinic).WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.ClinicId)
                    .HasConstraintName("FK_Room_Clinic");
            });

            modelBuilder.Entity<RoomAvailability>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_RoomAvailability");
                entity.ToTable("RoomAvailability");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.RoomId).HasColumnName("RoomID");
                entity.Property(e => e.AvailableSlots);
                entity.Property(e => e.Day).HasColumnType("datetime");

                entity.HasOne(d => d.Room).WithMany(p => p.RoomAvailabilities)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoomAvailability_Room");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Report");
                entity.ToTable("Report");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Data).HasMaxLength(500);
                entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Service");
                entity.ToTable("Service");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Patient");
                entity.ToTable("Patient");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(1).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne()
                    .HasForeignKey<Patient>(d => d.Id)
                    .HasConstraintName("FK_Patient_User");

                entity.HasMany(d => d.Appointments).WithOne(p => p.Patient)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Appointment_Patient");

                entity.HasMany(d => d.Messages).WithOne(p => p.Patient)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Message_Patient");
            });

            modelBuilder.Entity<ClinicOwner>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ClinicOwner");
                entity.ToTable("ClinicOwner");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasOne(d => d.User)
                    .WithOne()
                    .HasForeignKey<ClinicOwner>(d => d.Id)
                    .HasConstraintName("FK_ClinicOwner_User");
                entity.HasMany(d => d.Clinics).WithOne(p => p.Owner)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_Clinic_ClinicOwner");
            });

            modelBuilder.Entity<Dentist>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Dentist");
                entity.ToTable("Dentist");
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithOne()
                    .HasForeignKey<Dentist>(d => d.Id)
                    .HasConstraintName("FK_Dentist_User");
                entity.HasOne(d => d.Clinic).WithMany(p => p.Dentists)
                    .HasForeignKey(d => d.ClinicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dentist_Clinic");

                entity.HasMany(d => d.Licenses)
                    .WithOne(l => l.Dentist)
                    .HasForeignKey(l => l.DentistId)
                    .HasConstraintName("FK_License_Dentist");
            });
            modelBuilder.Entity<DentistService>(entity =>
            {
                entity.HasKey(e => new { e.DentistId, e.ServiceId });
                entity.ToTable("DentistService");

                entity.HasOne(ds => ds.Dentist)
                    .WithMany(d => d.DentistServices)
                    .HasForeignKey(ds => ds.DentistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DentistService_Dentist");

                entity.HasOne(ds => ds.Service)
                    .WithMany(s => s.DentistServices)
                    .HasForeignKey(ds => ds.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DentistService_Service");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

