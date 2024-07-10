﻿// <auto-generated />
using System;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessObjects.Migrations
{
    [DbContext(typeof(ClinicContext))]
    partial class ClinicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DentistId")
                        .HasColumnType("int")
                        .HasColumnName("DentistID");

                    b.Property<int>("EndSlot")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime");

                    b.Property<int>("PatientId")
                        .HasColumnType("int")
                        .HasColumnName("PatientID");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("RoomID");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int")
                        .HasColumnName("ServiceID");

                    b.Property<int>("StartSlot")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DentistId" }, "IX_Appointment_DentistID");

                    b.HasIndex(new[] { "PatientId" }, "IX_Appointment_PatientID");

                    b.HasIndex(new[] { "RoomId" }, "IX_Appointment_RoomID");

                    b.HasIndex(new[] { "ServiceId" }, "IX_Appointment_ServiceID");

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

                    b.HasIndex(new[] { "OwnerId" }, "IX_Clinic_OwnerID");

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

                    b.HasKey("Id");

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

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ClinicId" }, "IX_Dentist_ClinicID");

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

                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime");

                    b.Property<int?>("DentistId")
                        .HasColumnType("int")
                        .HasColumnName("DentistID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DentistId" }, "IX_DentistAvailability_DentistID");

                    b.ToTable("DentistAvailability", (string)null);
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

                    b.HasIndex(new[] { "DentistId" }, "IX_License_DentistId");

                    b.ToTable("License", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DentistId" }, "IX_Message_DentistID");

                    b.HasIndex(new[] { "PatientId" }, "IX_Message_PatientID");

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
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "IX_Notification_UserID");

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
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Patient", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int")
                        .HasColumnName("AppointmentID");

                    b.Property<string>("Data")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("GeneratedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId")
                        .IsUnique();

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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ClinicId" }, "IX_Room_ClinicID");

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
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("RoomID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RoomId" }, "IX_RoomAvailability_RoomID");

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

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

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

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("DentistService", b =>
                {
                    b.Property<int>("DentistId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("DentistId", "ServiceId");

                    b.HasIndex(new[] { "ServiceId" }, "IX_DentistService_ServiceId");

                    b.ToTable("DentistService", (string)null);
                });

            modelBuilder.Entity("BusinessObjects.Entities.Appointment", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("Appointments")
                        .HasForeignKey("DentistId")
                        .IsRequired()
                        .HasConstraintName("FK_Appointment_Dentist");

                    b.HasOne("BusinessObjects.Entities.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .IsRequired()
                        .HasConstraintName("FK_Appointment_Patient");

                    b.HasOne("BusinessObjects.Entities.Room", "Room")
                        .WithMany("Appointments")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("FK_Appointment_Room");

                    b.HasOne("BusinessObjects.Entities.Service", "Service")
                        .WithMany("Appointments")
                        .HasForeignKey("ServiceId")
                        .IsRequired()
                        .HasConstraintName("FK_Appointment_Service");

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
                    b.HasOne("BusinessObjects.Entities.User", "IdNavigation")
                        .WithOne("ClinicOwner")
                        .HasForeignKey("BusinessObjects.Entities.ClinicOwner", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ClinicOwner_User");

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Dentist", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Clinic", "Clinic")
                        .WithMany("Dentists")
                        .HasForeignKey("ClinicId")
                        .IsRequired()
                        .HasConstraintName("FK_Dentist_Clinic");

                    b.HasOne("BusinessObjects.Entities.User", "IdNavigation")
                        .WithOne("Dentist")
                        .HasForeignKey("BusinessObjects.Entities.Dentist", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Dentist_User");

                    b.Navigation("Clinic");

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("BusinessObjects.Entities.DentistAvailability", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", "Dentist")
                        .WithMany("DentistAvailabilities")
                        .HasForeignKey("DentistId")
                        .HasConstraintName("FK_DentistAvailability_Dentist");

                    b.Navigation("Dentist");
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
                    b.HasOne("BusinessObjects.Entities.User", "IdNavigation")
                        .WithOne("Patient")
                        .HasForeignKey("BusinessObjects.Entities.Patient", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Patient_User");

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Report", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Appointment", "Appointment")
                        .WithOne("Report")
                        .HasForeignKey("BusinessObjects.Entities.Report", "AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Appointment_Report");

                    b.Navigation("Appointment");
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

            modelBuilder.Entity("DentistService", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Dentist", null)
                        .WithMany()
                        .HasForeignKey("DentistId")
                        .IsRequired()
                        .HasConstraintName("FK_DentistService_Dentist");

                    b.HasOne("BusinessObjects.Entities.Service", null)
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .IsRequired()
                        .HasConstraintName("FK_DentistService_Service");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Appointment", b =>
                {
                    b.Navigation("Report");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Clinic", b =>
                {
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
                });

            modelBuilder.Entity("BusinessObjects.Entities.User", b =>
                {
                    b.Navigation("ClinicOwner");

                    b.Navigation("Dentist");

                    b.Navigation("Notifications");

                    b.Navigation("Patient");
                });
#pragma warning restore 612, 618
        }
    }
}