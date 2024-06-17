using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices
{
	public class AppointmentService: IAppointmentService
	{
		private readonly IAppointmentRepository _appointmentRepository;
		public AppointmentService(IAppointmentRepository iAppointmentRepository)
		{
			_appointmentRepository =  iAppointmentRepository;
		}

		public Task<Appointment> AddAsync(Appointment entity)
		{
			return _appointmentRepository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _appointmentRepository.DeleteAsync(id);
		}

		public Task<IEnumerable<Appointment>> GetAllAsync()
		{
			return _appointmentRepository.GetAllAsync();
		}

		public Task<Appointment> GetByIdAsync(int id)
		{
			return _appointmentRepository.GetByIdAsync(id);
		}

		public Task UpdateAsync(Appointment entity)
		{
			return _appointmentRepository.UpdateAsync(entity);
		}
	}
}
