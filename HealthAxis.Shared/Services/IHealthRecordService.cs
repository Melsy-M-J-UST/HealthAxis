using HealthAxis.Shared.Models;
using HealthAxisWebApp;
using System.Collections.Generic;

namespace HealthAxis.Shared.Services.Interfaces
{
    public interface IHealthRecordService
    {
        List<HealthRecord> GetAllRecords();

        HealthRecord GetRecordById(int id);

        void AddRecord(HealthRecord record);

        void UpdateRecord(HealthRecord record);

        void DeleteRecord(int id);
    }

}
