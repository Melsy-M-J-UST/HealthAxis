using System.Collections.Generic;
using HealthAxis_MVC.Models;

namespace HealthAxis_MVC.Services
{
    public interface IHealthRecordService
    {
        void AddRecord(HealthRecord record);

        List<HealthRecord> GetAllRecords();

        HealthRecord GetById(int id);

        void UpdateRecord(int id, HealthRecord record);
    }
}
