using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.Services
{
    public interface ILeaveTypesServices
    {
        Task<bool> CheckIfLeaveTypeNameExists(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeeditVM leaveTypeEdit);
        Task Create(LeaveTypeCreateVM model);
        Task Edit(LeaveTypeEditVM model);
        Task<T?> Get<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
        bool LeaveTypeExists(int id);
        Task Remove(int id);
    }
}