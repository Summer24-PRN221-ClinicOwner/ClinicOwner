﻿using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task<Appointment> AddAsync(Appointment entity, Payment payment);
        Task UpdateAsync(Appointment entity);
        Task DeleteAsync(int id);
        Task<List<Slot>> GetAvailableSlotAsync(DateTime date, int slotRequired);
        Room GetRoomAvailable(DateTime date, int slotRequired, int startSlot);
        Task<AppointmentDentistSchedule> GetAppoinmentSchedule(int pageWeek, int dentistId);
        Task<List <Appointment>> GetAppoinmentHistoryAsync(int patientId);
        Task<List<Appointment>> GetAppointmentsBeforeDaysAsync(int days);
        Task<bool> UpdateAppointmentStatus(int appointId, int Status, DateTime? newDate);
        public Task<Appointment> GetAppointmentsByIdAsync(int id);
        Task<int> GetAppointmentCountAsync();
        Task<int> GetTodayAppointmentCountAsync();
        Task<decimal> GetTodayTotalEarningsAsync();
    }
}