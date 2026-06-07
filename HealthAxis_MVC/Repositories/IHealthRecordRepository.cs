using HealthAxis_MVC.Models;
using System.Collections.Generic;

namespace HealthAxis_MVC.Repositories
{
    public interface IHealthRecordRepository
    {
        List<HealthRecord> GetAll();
        HealthRecord GetById(int id);
        void Add(HealthRecord record);
        void Update(int id, HealthRecord record);
    }
}
