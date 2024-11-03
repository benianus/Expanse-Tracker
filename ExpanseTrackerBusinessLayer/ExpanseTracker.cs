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
        public enum EnCategories
        {
            Groceries,
            Leisure,
            Electronics,
            Utilities,
            Clothing,
            Health,
            Others
        }
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public int? CategoryId { get; set; }
        public ExpanseTrackerDto Dto { get; set; }
        public EnMode Mode { private set; get; }
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
        public static async Task<ExpanseTracker?> FindExpanseById(int expanseId)
        {
            ExpanseTrackerDto? dto = await ExpanseTrackerData.GetExpanseById(expanseId);

            if (dto != null)
            {
                return new ExpanseTracker(dto, EnMode.Update);
            }
            else
            {
                return null;
            }
        }
        public static async Task<int> GetAllExpansesSummary()
        {
            return await ExpanseTrackerData.GetAllExpansesSummary();
        }
        public static async Task<int> GetExpansesSummaryByMonth(int month)
        {
            return await ExpanseTrackerData.GetExpansesSummaryByMonth(month);
        }
        public static async Task<List<ExpanseTrackerDto>?> GetExpansesByCategory(int category)
        {
            return await ExpanseTrackerData.GetExpansesByCategory(category);
        }
        private async Task<bool> AddNewExpanse()
        {
            this.Id = await ExpanseTrackerData.AddNewExpanse(this.Dto);

            return this.Id > 0;
        }
        private async Task<bool> UpdateExpanse()
        {
            return await ExpanseTrackerData.UpdateExpanse(this.Id, this.Dto);
        }
        public async Task<bool> DeleteExpanse(int Id)
        {
            return await ExpanseTrackerData.DeleteExpanse(Id);
        }
        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case EnMode.AddNew:
                    Mode = EnMode.Update;
                    return await AddNewExpanse();
                case EnMode.Update:
                    return await UpdateExpanse();
            }

            return false;
        }
    }
}
