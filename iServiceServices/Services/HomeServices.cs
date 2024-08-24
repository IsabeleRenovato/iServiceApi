using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iServiceServices.Services
{
    public class HomeModel
    {
        public Appointment? NextAppointment { get; set; }
        public UserInfo? Establishment { get; set; }
        public UserInfo? Client { get; set; }
        public List<EstablishmentCategory>? Categories { get; set; }
        public int? TotalAppointments { get; set; }
        public HomeModel()
        {
            NextAppointment = new Appointment();
            Establishment = new UserInfo();
            Client = new UserInfo();
            Categories = new List<EstablishmentCategory>();
        }
    }

    public class HomeServices
    {
        private readonly UserInfoService _userInfoService;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly EstablishmentCategoryRepository _establishmentCategoryRepository;

        public HomeServices(IConfiguration configuration)
        {
            _userInfoService = new UserInfoService(configuration);
            _appointmentRepository = new AppointmentRepository(configuration);
            _establishmentCategoryRepository = new EstablishmentCategoryRepository(configuration);
        }

        public async Task<Result<HomeModel>> GetAsync(int userId)
        {
            try
            {
                var home = new HomeModel();
                var result = await _userInfoService.GetUserInfoByUserId(userId);

                if (result.IsSuccess)
                {
                    var userInfo = result.Value;
                    var role = userInfo.UserRole;
                    var appointment = new Appointment();
                    if (role?.UserRoleId == 2)
                    {
                        home.Establishment = userInfo;
                        home.TotalAppointments = await _appointmentRepository.GetCountByDateAsync(userId, DateTime.Now);
                        home.NextAppointment = await _appointmentRepository.GetNextAppointmentEstablishmentAsync(userId);
                        var client = await _userInfoService.GetUserInfoByUserProfileId(appointment.ClientUserProfileId);
                        home.Client = client.Value;
                    }
                    if (role?.UserRoleId == 3)
                    {
                        home.Client = userInfo;
                        home.NextAppointment = await _appointmentRepository.GetNextAppointmentClientAsync(userId);
                        home.TotalAppointments = home?.NextAppointment?.AppointmentId > 0 ? 1 : 0;
                        var establishment = await _userInfoService.GetUserInfoByUserProfileId(appointment.EstablishmentUserProfileId);
                        home.Establishment = establishment.Value;
                    }
                    home.Categories = await _establishmentCategoryRepository.GetAsync();
                    return Result<HomeModel>.Success(home);
                }
                return Result<HomeModel>.Failure($"Falha ao obter os dados.");
            }
            catch (Exception ex)
            {
                return Result<HomeModel>.Failure($"Falha ao obter os dados: {ex.Message}");
            }
        }

        public async Task<List<Appointment>> GetAllByDateAsync(int userProfileId, DateTime date)
        {
            return await _appointmentRepository.GetAllByDateAsync(userProfileId, date);
        }

        public async Task<int> GetCountByDateAsync(int userProfileId, DateTime date)
        {
            return await _appointmentRepository.GetCountByDateAsync(userProfileId, date);
        }

        public async Task<Appointment> GetNextAppointmentEstablishmentAsync(int userProfileId)
        {
            return await _appointmentRepository.GetNextAppointmentEstablishmentAsync(userProfileId);
        }

        public async Task<Appointment> GetNextAppointmentClientAsync(int userProfileId)
        {
            return await _appointmentRepository.GetNextAppointmentClientAsync(userProfileId);
        }
    }
}
