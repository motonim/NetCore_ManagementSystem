using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.Services.LeaveTypes
{
    public interface ILeaveTypesService
    {
        Task<bool> CheckIfLeaveTypeNameExists(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit);
        Task CreateAsync(LeaveTypeCreateVM model);
        Task EditAsync(LeaveTypeEditVM model);
        Task<T?> GetAsync<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
        bool LeaveTypeExists(int id);
        Task RemoveAsync(int id);
    }
}