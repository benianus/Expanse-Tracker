using ExpanseTrackerDataLayer;

namespace ExpanseTrackerBusinessLayer
{
    public class ExpanseTracker
    {
        public enum enMode
        {
            AddNew, 
            Update
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public enMode Mode = enMode.AddNew;
        public ExpanseTracker(ExpanseTrackerDto Dto, enMode Mode = enMode.AddNew)
        {
            Id = Dto.Id;
            Date = Dto.Date;
            Description = Dto.Description;
            Amount = Dto.Amount;
        }
        public static async Task<List<ExpanseTrackerDto>> GetExpansesList()
        {
            return await ExpanseTrackerData.GetExpansesList();
        }
    }
}
