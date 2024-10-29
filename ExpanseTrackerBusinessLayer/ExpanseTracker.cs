using ExpanseTrackerDataLayer;
using System.Xml.Linq;

namespace ExpanseTrackerBusinessLayer
{
    public class ExpanseTracker
    {
        public enum EnMode
        {
            AddNew, 
            Update
        }
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public ExpanseTrackerDto Dto { get; set; }
        public EnMode Mode {private set; get;}
        public ExpanseTracker(ExpanseTrackerDto dto, EnMode mode = EnMode.AddNew)
        {
            Id = dto.Id;
            Date = dto.Date;
            Description = dto.Description;
            Amount = dto.Amount;
            Dto = dto;

            Mode = mode;
        }
        public static async Task<List<ExpanseTrackerDto?>> GetExpansesList()
        {
            return await ExpanseTrackerData.GetExpansesList();
        }
        public static async Task<ExpanseTracker?> FindExpanseById(int ExpanseId)
        {
            ExpanseTrackerDto? dto = await ExpanseTrackerData.GetExpanseById(ExpanseId);

            if (dto != null)
            {
                return new ExpanseTracker(dto);
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> AddNewExpanse()
        {
            this.Id = await ExpanseTrackerData.AddNewExpanse(this.Dto);

            return this.Id > 0;
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case EnMode.AddNew:
                    Mode = EnMode.Update;
                    return await AddNewExpanse();
                case EnMode.Update:
                    break;
            }

            return false;
        }
    }
}
