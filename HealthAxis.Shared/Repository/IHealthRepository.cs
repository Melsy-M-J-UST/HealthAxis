using System.Collections.Generic;

namespace HealthAxisWebApp.Repositories.Interfaces
{
    public interface IHealthRecordRepository
    {
        List<HealthRecord> GetAll();

        HealthRecord GetById(int id);

        void Add(HealthRecord record);

        void Update(HealthRecord record);

        void Delete(int id);
    }

}
