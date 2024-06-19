﻿// <auto-generated />
using System;
using ClinicRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClinicRepositories.Migrations
{
    [DbContext(typeof(ClinicContext))]
    [Migration("20240619072306_UpdateAppoint")]
    partial class UpdateAppoint
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusinessObjects.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AppointDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClinicId")
                        .HasColumnType("int")
                        .HasColumnName("ClinicID");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DentistId")
                        .HasColumnType("int")
                        .HasColumnName("DentistID");

                    b.Property<int?>("EndSlot")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int")
                        .HasColumnName("PatientID");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("RoomID");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int")
                        .HasColumnName("ServiceID");

                    b.Property<int?>("StartSlot")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Appointment");

                    b.HasIndex("ClinicId");

                    b.HasIndex("DentistId");

                    b.HasIndex("PatientId");

                    b.HasIndex("RoomId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Appointment", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Clinic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int")
                        .HasColumnName("OwnerID");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Clinic", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.ClinicOwner", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id")
                        .HasName("PK_ClinicOwner");

                    b.ToTable("ClinicOwner", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Dentist", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<int>("ClinicId")
                        .HasColumnType("int")
                        .HasColumnName("ClinicID");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id")
                        .HasName("PK_Dentist");

                    b.HasIndex("ClinicId");

                    b.ToTable("Dentist", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.DentistAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AvailableSlots")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength();

                    b.Property<DateTime?>("Day")
                        .HasColumnType("datetime");

                    b.Property<int?>("DentistId")
                        .HasColumnType("int")
                        .HasColumnName("DentistID");

                    b.HasKey("Id")
                        .HasName("PK_DentistAvailability");

                    b.HasIndex("DentistId");

                    b.ToTable("DentistAvailability", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.DentistService", b =>
                {
                    b.Property<int>("DentistId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("DentistId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("DentistService", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.License", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DentistId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpireDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("IssueDate")
                        .HasColumnType("datetime");

                    b.Property<string>("LicenceType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LicenseNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("DentistId");

                    b.ToTable("License", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateSeen")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DateSend")
                        .HasColumnType("datetime");

                    b.Property<int?>("DentistId")
                        .HasColumnType("int")
                        .HasColumnName("DentistID");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int")
                        .HasColumnName("PatientID");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MessageId");

                    b.HasIndex("DentistId");

                    b.HasIndex("PatientId");

                    b.ToTable("Message", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_Patient");

                    b.ToTable("Patient", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Data")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("GeneratedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_Report");

                    b.ToTable("Report", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClinicId")
                        .HasColumnType("int")
                        .HasColumnName("ClinicID");

                    b.Property<string>("RoomNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_Room");

                    b.HasIndex("ClinicId");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.RoomAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AvailableSlots")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Day")
                        .HasColumnType("datetime");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("RoomID");

                    b.HasKey("Id")
                        .HasName("PK_RoomAvailability");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAvailability", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("Cost")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Service");

                    b.ToTable("Service", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Appointment", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Clinic", "Clinic")
                        .WithMany("Appointments")
                        .HasForeignKey("ClinicId")
                        .IsRequired()
                        .HasConstraintName("FK_Appointment_Clinic");

                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("Appointments")
                        .HasForeignKey("DentistId")
                        .HasConstraintName("FK_Appointment_Dentist");

                    b.HasOne("BusinessObjects.Entities.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_Appointment_Patient");

                    b.HasOne("BusinessObjects.Entities.Room", "Room")
                        .WithMany("Appointments")
                        .HasForeignKey("RoomId")
                        .HasConstraintName("FK_Appointment_Room");

                    b.HasOne("BusinessObjects.Entities.Service", "Service")
                        .WithMany("Appointments")
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("FK_Appointment_Service");

                    b.Navigation("Clinic");

                    b.Navigation("Dentist");

                    b.Navigation("Patient");

                    b.Navigation("Room");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Clinic", b =>
                {
                    b.HasOne("BusinessObjects.Entities.ClinicOwner", "Owner")
                        .WithMany("Clinics")
                        .HasForeignKey("OwnerId")
                        .IsRequired()
                        .HasConstraintName("FK_Clinic_ClinicOwner");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("BusinessObjects.Entities.ClinicOwner", b =>
                {
                    b.HasOne("BusinessObjects.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("BusinessObjects.Entities.ClinicOwner", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ClinicOwner_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Dentist", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Clinic", "Clinic")
                        .WithMany("Dentists")
                        .HasForeignKey("ClinicId")
                        .IsRequired()
                        .HasConstraintName("FK_Dentist_Clinic");

                    b.HasOne("BusinessObjects.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("BusinessObjects.Entities.Dentist", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Dentist_User");

                    b.Navigation("Clinic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObjects.Entities.DentistAvailability", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("DentistAvailabilities")
                        .HasForeignKey("DentistId")
                        .HasConstraintName("FK_DentistAvailability_Dentist");

                    b.Navigation("Dentist");
                });

            modelBuilder.Entity("BusinessObjects.Entities.DentistService", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("DentistServices")
                        .HasForeignKey("DentistId")
                        .IsRequired()
                        .HasConstraintName("FK_DentistService_Dentist");

                    b.HasOne("BusinessObjects.Entities.Service", "Service")
                        .WithMany("DentistServices")
                        .HasForeignKey("ServiceId")
                        .IsRequired()
                        .HasConstraintName("FK_DentistService_Service");

                    b.Navigation("Dentist");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BusinessObjects.Entities.License", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("Licenses")
                        .HasForeignKey("DentistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_License_Dentist");

                    b.Navigation("Dentist");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Message", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("Messages")
                        .HasForeignKey("DentistId")
                        .HasConstraintName("FK_Message_Dentist");

                    b.HasOne("BusinessObjects.Entities.Patient", "Patient")
                        .WithMany("Messages")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_Message_Patient");

                    b.Navigation("Dentist");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Notification", b =>
                {
                    b.HasOne("BusinessObjects.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Notification_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Patient", b =>
                {
                    b.HasOne("BusinessObjects.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("BusinessObjects.Entities.Patient", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Patient_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Room", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Clinic", "Clinic")
                        .WithMany("Rooms")
                        .HasForeignKey("ClinicId")
                        .HasConstraintName("FK_Room_Clinic");

                    b.Navigation("Clinic");
                });

            modelBuilder.Entity("BusinessObjects.Entities.RoomAvailability", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Room", "Room")
                        .WithMany("RoomAvailabilities")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("FK_RoomAvailability_Room");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Clinic", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Dentists");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("BusinessObjects.Entities.ClinicOwner", b =>
                {
                    b.Navigation("Clinics");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Dentist", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("DentistAvailabilities");

                    b.Navigation("DentistServices");

                    b.Navigation("Licenses");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Room", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("RoomAvailabilities");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Service", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("DentistServices");
                });

            modelBuilder.Entity("BusinessObjects.Entities.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
