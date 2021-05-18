using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Interfaces;
using Test.Domain.Models.Account;
using Test.Domain.Models.ViewModels.Accounts;
using Test.Infra.Data.Context;


namespace Test.Infra.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        #region Dependancy Injection

        private readonly TestDbContext _context;

        public AccountRepository(TestDbContext context)
        {
            _context = context;
        }

        #endregion


        #region Users

        //User Paging
        public async Task<UserViewModels.UserPagingViewModel> GetAllUsers(UserViewModels.UserPagingViewModel userPaging, int take)
        {

            IQueryable<User> usersQuery;


            if (userPaging.IsDeletedUsersShow)
            {
                usersQuery = _context.Users.IgnoreQueryFilters().AsQueryable();
            }
            else
            {
                usersQuery = _context.Users.Where(u => !u.IsDelete).AsQueryable();
            }



            #region Filter


            switch (userPaging.UserGenderEnum)
            {

                case UserViewModels.UserGenderFilterEnum.All:
                    break;

                case UserViewModels.UserGenderFilterEnum.Male:
                    usersQuery = usersQuery.Where(u => u.Gender == GenderEnum.Male);
                    break;


                case UserViewModels.UserGenderFilterEnum.Female:
                    usersQuery = usersQuery.Where(u => u.Gender == GenderEnum.Female);
                    break;
            }

            switch (userPaging.UserFilterEnum)
            {

                case UserViewModels.UserFilterEnum.All:
                    break;

                case UserViewModels.UserFilterEnum.IsAccountActive:
                    usersQuery = usersQuery.Where(u => u.IsAccountActive);
                    break;

                case UserViewModels.UserFilterEnum.IsDelete:
                    usersQuery = usersQuery.Where(u => u.IsDelete);
                    break;

                case UserViewModels.UserFilterEnum.IsPhoneNumberActive:
                    usersQuery = usersQuery.Where(u => u.IsPhoneNumberActive);
                    break;
            }


            if (!string.IsNullOrEmpty(userPaging.UserName))
            {
                usersQuery = usersQuery.Where(u => EF.Functions.Like(u.UserName, $"%{userPaging.UserName}%"));
            }

            if (!string.IsNullOrEmpty(userPaging.MobileNumber))
            {
                usersQuery = usersQuery.Where(u => u.PhoneNumber == userPaging.MobileNumber);
            }

            //if (argFilter.StartDate.HasValue)
            //{
            //    productQuery = productQuery.Where(p => p.CreatedDate >= argFilter.StartDate);
            //}

            //if (argFilter.EndDate.HasValue)
            //{
            //    productQuery = productQuery.Where(p => p.CreatedDate <= argFilter.StartDate);
            //}


            #endregion

            var resultQuery = usersQuery.Select(u => new UserViewModels.MainUserViewModel()
            {
                UserId = u.UserId,
                Password = u.Password,
                PhoneNumber = u.PhoneNumber,
                IpAddress = u.IpAddress,
                IsDelete = u.IsDelete,
                Avatar = u.Avatar,
                IsAccountActive = u.IsAccountActive,
                IsPhoneNumberActive = u.IsPhoneNumberActive,
                LocationId = u.LocationId,
                RegistrationDate = u.RegistrationDate,
                UserName = u.UserName
            }).OrderByDescending(s => s.RegistrationDate);


            #region Paging

            var usersCount = resultQuery.Count();

            var pages = usersCount / take;

            if (usersCount % take != 0)
                pages += 1;


            var finalUsers = await resultQuery
                .Skip((userPaging.CurrentPage - 1) * take).Take(take).ToListAsync();

            #endregion

            return new UserViewModels.UserPagingViewModel()
            {
                CurrentPage = userPaging.CurrentPage,
                PageCount = pages,
                Users = finalUsers
            };
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void EditUser(User user)
        {
            _context.Users.Update(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _context.Users.IgnoreQueryFilters().SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> IsUserPhoneNumberExists(string phoneNumber)
        {
            return await _context.Users.IgnoreQueryFilters().AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> IsUserPasswordCorrect(Guid userId, string hashedPassword)
        {
            var personnel = await _context.Users.SingleOrDefaultAsync(p => p.UserId == userId && p.Password == hashedPassword);

            if (personnel != null)
                return true;

            return false;
        }

        public async Task EditUserCity(Guid userId, int locationId)
        {
            var user = await GetUserById(userId);
            user.LocationId = locationId;
            _context.Users.Update(user);
        }

        public async Task DeleteUserByUserId(Guid userId)
        {
            var user = await GetUserById(userId);
            user.IsDelete = true;
            _context.Users.Update(user);
        }

        public async Task UnDeleteUserByUserId(Guid userId)
        {
            var user = await GetUserById(userId);
            user.IsDelete = false;
            _context.Users.Update(user);
        }

        public async Task ActiveUserByUserId(Guid userId)
        {
            var user = await GetUserById(userId);
            user.IsAccountActive = true;
            _context.Users.Update(user);
        }

        public async Task DeActiveUserByUserId(Guid userId)
        {
            var user = await GetUserById(userId);
            user.IsAccountActive = false;
            _context.Users.Update(user);
        }

        #endregion


        #region Staff

        public async Task AddStaff(Staff staff)
        {
            await _context.Staff.AddAsync(staff);
        }

        //Staff Paging
        public async Task<StaffPagingViewModel> GetAllStaff(StaffPagingViewModel staffPaging, int take)
        {

            IQueryable<Staff> staffQuery;


            if (staffPaging.IsDeactiveStaffShow)
            {
                staffQuery = _context.Staff.IgnoreQueryFilters().AsQueryable();
            }
            else
            {
                staffQuery = _context.Staff.Where(s => s.IsWorking).AsQueryable();
            }



            #region Filter


            switch (staffPaging.StaffFilter)
            {

                case StaffFilterEnum.AllGenders:
                    break;

                case StaffFilterEnum.Male:
                    staffQuery = staffQuery.Where(s => s.Gender == GenderEnum.Male);
                    break;


                case StaffFilterEnum.Female:
                    staffQuery = staffQuery.Where(s => s.Gender == GenderEnum.Female);
                    break;

            }


            if (!string.IsNullOrEmpty(staffPaging.Name))
            {
                staffQuery = staffQuery.Where(s => EF.Functions.Like(s.Name, $"%{staffPaging.Name}%") ||
                                                   EF.Functions.Like(s.Family, $"%{staffPaging.Name}%"));
            }

            if (!string.IsNullOrEmpty(staffPaging.PersonnelCode))
            {
                staffQuery = staffQuery.Where(s => s.PersonnelCode == staffPaging.PersonnelCode);
            }

            if (!string.IsNullOrEmpty(staffPaging.EmailAddress))
            {
                staffQuery = staffQuery.Where(s => s.EmailAddress == staffPaging.EmailAddress);
            }

            if (!string.IsNullOrEmpty(staffPaging.BirthCertificateNumber))
            {
                staffQuery = staffQuery.Where(s => s.BirthCertificateNumber == staffPaging.BirthCertificateNumber);
            }

            if (!string.IsNullOrEmpty(staffPaging.NationalCode))
            {
                staffQuery = _context.Staff.Where(s => s.NationalCode == staffPaging.NationalCode);
            }

            if (!string.IsNullOrEmpty(staffPaging.Address))
            {
                staffQuery = staffQuery.Where(s => EF.Functions.Like(s.Address, $"%{staffPaging.Address}%"));
            }

            if (!string.IsNullOrEmpty(staffPaging.MobileNumber))
            {
                staffQuery = staffQuery.Where(s => s.MobileNumber == staffPaging.MobileNumber);
            }

            if (!string.IsNullOrEmpty(staffPaging.PhoneNumber))
            {
                staffQuery = staffQuery.Where(s => s.PhoneNumber == staffPaging.PhoneNumber);
            }

            //if (argFilter.StartDate.HasValue)
            //{
            //    productQuery = productQuery.Where(p => p.CreatedDate >= argFilter.StartDate);
            //}

            //if (argFilter.EndDate.HasValue)
            //{
            //    productQuery = productQuery.Where(p => p.CreatedDate <= argFilter.StartDate);
            //}


            #endregion

            var resultQuery = staffQuery.Select(s => new MainStaffViewModel()
            {
                Address = s.Address,
                BirthCertificateNumber = s.BirthCertificateNumber,
                EmailAddress = s.EmailAddress,
                Family = s.Family,
                Gender = s.Gender,
                ImageName = s.ImageName,
                MobileNumber = s.MobileNumber,
                Name = s.Name,
                NationalCardImageName = s.NationalCardImageName,
                NationalCode = s.NationalCode,
                Password = s.Password,
                PersonnelCode = s.PersonnelCode,
                PhoneNumber = s.PhoneNumber,
                Registrant = s.RegistrantId,
                RegistrantIpAddress = s.RegistrantIpAddress,
                RegistrationDate = s.RegistrationDate,
                StaffId = s.StaffId,
                IsWorking = s.IsWorking,
                IsActive = s.IsActive
                
            }).OrderByDescending(s => s.RegistrationDate);


            #region Paging

            var staffCount = resultQuery.Count();

            var pages = staffCount / take;

            if (staffCount % take != 0)
                pages += 1;


            var finalStaff = await resultQuery
                .Skip((staffPaging.CurrentPage - 1) * take).Take(take).ToListAsync();

            #endregion

            return new StaffPagingViewModel()
            {
                CurrentPage = staffPaging.CurrentPage,
                PageCount = pages,
                Staff = finalStaff
            };
        }

        public async Task<Staff> GetStaffById(Guid staffId)
        {
            return await _context.Staff.IgnoreQueryFilters().SingleOrDefaultAsync(s => s.StaffId == staffId);
        }

        public async Task<Staff> GetStaffByPersonnelCode(string personnelCode)
        {
            return await _context.Staff.SingleOrDefaultAsync(s => s.PersonnelCode == personnelCode);
        }

        public async Task<bool> IsNationalCodeExists(string nationalCode)
        {
            return await _context.Staff.AnyAsync(s => s.NationalCode == nationalCode);
        }

        public async Task<bool> IsBirthCertificateNumberExists(string birthCertificateNumber)
        {
            return await _context.Staff.AnyAsync(s => s.BirthCertificateNumber == birthCertificateNumber);
        }

        public async Task<bool> IsPersonnelCodeExists(string personnelCode)
        {
            return await _context.Staff.AnyAsync(p => p.PersonnelCode == personnelCode);
        }

        public async Task<bool> IsStaffPasswordCorrect(Guid staffId, string hashedPassword)
        {
            var personnel = await _context.Staff.SingleOrDefaultAsync(p => p.StaffId == staffId && p.Password == hashedPassword);

            if (personnel != null)
                return true;

            return false;
        }

        public async Task DeleteStaffByStaffId(Guid staffId)
        {
            var personnel = await GetStaffById(staffId);
            personnel.IsWorking = false;
            _context.Staff.Update(personnel);
        }

        public async Task UnDeleteStaffByStaffId(Guid staffId)
        {
            var personnel = await GetStaffById(staffId);
            personnel.IsWorking = true;
            _context.Staff.Update(personnel);
        }

        public async Task ActiveStaffByStaffId(Guid staffId)
        {
            var personnel = await GetStaffById(staffId);
            personnel.IsActive = true;
            _context.Staff.Update(personnel);
        }

        public async Task DeActiveStaffByStaffId(Guid staffId)
        {
            var personnel = await GetStaffById(staffId);
            personnel.IsActive = false;
            _context.Staff.Update(personnel);
        }

        #endregion




        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
