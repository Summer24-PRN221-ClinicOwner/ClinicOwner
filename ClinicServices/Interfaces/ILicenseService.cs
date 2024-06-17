﻿using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface ILicenseService
	{
        Task<IEnumerable<License>> GetAllAsync();
        Task<License> GetByIdAsync(int id);
        Task<License> AddAsync(License entity);
        Task UpdateAsync(License entity);
        Task DeleteAsync(int id);
    }
}