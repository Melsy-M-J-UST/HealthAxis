using HealthAxis_MVC.Models;
using HealthAxis_MVC.Repositories;
using System.Collections.Generic;
using HealthAxis_MVC.Exceptions;
using System;

namespace HealthAxis_MVC.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;

        public HealthRecordService(IHealthRecordRepository repository)
        {
            _repository = repository;
        }

        void IHealthRecordService.AddRecord(HealthRecord record)
        {
            _repository.Add(record);
        }

        List<HealthRecord> IHealthRecordService.GetAllRecords()
        {
            return _repository.GetAll();
        }

        HealthRecord IHealthRecordService.GetById(int id)
        {
            try
            {
                return _repository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Health Record not found: " + ex.Message);
            }
        }

        void IHealthRecordService.UpdateRecord(int id, HealthRecord record)
        {
            try
            {
                _repository.Update(id, record);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Health Record: " + ex.Message);
            }
        }
    }
}