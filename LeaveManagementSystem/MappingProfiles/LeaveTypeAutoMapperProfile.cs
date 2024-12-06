using AutoMapper;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile : Profile
    {
        public LeaveTypeAutoMapperProfile() {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
                //.ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays));
            // you are automatically matching the names of fields in LeaveType and IndexVM
            // but for this particular member or property in the destination (dest => dest.Days)
            // I would like you to take this value (src => src.NumberOfDays) and place it in dest.Days

            CreateMap<LeaveTypeCreateVM, LeaveType>();
            
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap(); // load data and update data so both direction
        }
    }
}
