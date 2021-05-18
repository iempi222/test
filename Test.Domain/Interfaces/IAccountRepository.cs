using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Models.Account;
using Test.Domain.Models.ViewModels.Accounts;

namespace Test.Domain.Interfaces
{
    public interface IAccountRepository
    {

        #region Users

        Task<UserViewModels.UserPagingViewModel> GetAllUsers(UserViewModels.UserPagingViewModel userPaging, int take);
        Task AddUser(User user);
        Task<User> GetUserById(Guid userId);
        void EditUser(User user);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        IEnumerable<User> GetAllUsers();
        Task<bool> IsUserPhoneNumberExists(string phoneNumber);
        Task<bool> IsUserPasswordCorrect(Guid userId, string hashedPassword);
        Task DeleteUserByUserId(Guid userId);
        Task UnDeleteUserByUserId(Guid userId);
        Task ActiveUserByUserId(Guid userId);
        Task DeActiveUserByUserId(Guid userId);

        #endregion


        #region Staff

        Task AddStaff(Staff staff);
        Task<Staff> GetStaffById(Guid staffId);
        Task<Staff> GetStaffByPersonnelCode(string personnelCode);
        Task<StaffPagingViewModel> GetAllStaff(StaffPagingViewModel staffPaging, int take);
        Task<bool> IsNationalCodeExists(string nationalCode);
        Task<bool> IsBirthCertificateNumberExists(string birthCertificateNumber);
        Task<bool> IsPersonnelCodeExists(string personnelCode);
        Task<bool> IsStaffPasswordCorrect(Guid staffId, string hashedPassword);
        Task DeleteStaffByStaffId(Guid staffId);
        Task UnDeleteStaffByStaffId(Guid staffId);
        Task ActiveStaffByStaffId(Guid staffId);
        Task DeActiveStaffByStaffId(Guid staffId);

        #endregion






        Task EditUserCity(Guid userId, int locationId);



        Task SaveAsync();

    }
}
