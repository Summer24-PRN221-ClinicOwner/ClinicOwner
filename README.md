# Clinic Management System

This is a comprehensive clinic management system that allows you to efficiently manage patient records, doctor information, appointments, medical records, prescriptions, and payments. 

It follows a three-tier architecture consisting of the Data Access Layer, Business Layer, and Presentation Layer for maintainability and scalability.

## Features

### 1. Patients

The system stores detailed information about patients. Each patient is assigned a unique identifier, and their profile includes their name, date of birth, gender, contact information (phone number, email), and address.

### 2. Doctors

The system maintains a comprehensive database of doctors. Each doctor is assigned a unique identifier, and their profile includes their name, specialization, date of birth, gender, contact information (phone number, email), and address.

### 3. Appointments

The system manages appointments effectively. Each appointment is assigned a unique identifier and includes the patient, doctor, appointment date and time, and appointment status. Appointment statuses include:
- Pending: The appointment has been scheduled but has not yet occurred.
- Confirmed: The appointment has been confirmed by both the patient and the healthcare provider.
- Completed: The appointment has taken place as scheduled.
- Canceled: The appointment has been canceled either by the patient or the healthcare provider.
- Rescheduled: The appointment has been rescheduled for a different date or time.
- No Show: The patient did not show up for the appointment without canceling or rescheduling.

### 4. Medical Records

The system maintains comprehensive medical records for patients. Each attended appointment is associated with a medical record. Each medical record is assigned a unique identifier and includes the patient, doctor, description of the visit, diagnosis, prescribed medication, and any additional notes.


## Technology Used

The clinic management system is built using the following technologies:
- Programming Language: C#
- Framework: .NET Framework
- Database: Ms SQL Server
- Data Access: EF Core
- User Interface: Razor Page
- Integrated Development Environment: Visual Studio
