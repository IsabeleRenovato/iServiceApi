using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class UserInfoService
    {
        private readonly UserRepository _userRepository;
        private readonly UserRoleRepository _userRoleRepository;
        private readonly UserProfileRepository _userProfileRepository;
        private readonly AddressRepository _addressRepository;
        private readonly EstablishmentCategoryRepository _establishmentCategoryRepository;
        private readonly FeedbackRepository _feedbackRepository;

        public UserInfoService(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
            _userRoleRepository = new UserRoleRepository(configuration);
            _userProfileRepository = new UserProfileRepository(configuration);
            _addressRepository = new AddressRepository(configuration);
            _establishmentCategoryRepository = new EstablishmentCategoryRepository(configuration);
            _feedbackRepository = new FeedbackRepository(configuration);
        }

        public Result<UserInfo> GetUserInfoByUserId(int userId)
        {
            try
            {
                var user = _userRepository.GetById(userId);

                if (user?.UserId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (UserRole)");
                }

                return GetUserInfo(user);
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure($"Falha ao obter os usuários: {ex.Message}");
            }
        }

        public Result<List<UserInfo>> GetUserInfoByUserRoleId(int userRoleId)
        {
            try
            {
                var userRole = _userRoleRepository.GetById(userRoleId);

                if (userRole?.UserRoleId > 0 == false)
                {
                    return Result<List<UserInfo>>.Failure("Falha ao recuperar os dados do usuário. (UserRole)");
                }

                var users = _userRepository.GetUserByUserRoleId(userRoleId);

                if (users?.Count > 0)
                {
                    return GetUserInfo(users);
                }

                return Result<List<UserInfo>>.Success([]);
            }
            catch (Exception ex)
            {
                return Result<List<UserInfo>>.Failure($"Falha ao obter os usuários: {ex.Message}");
            }
        }

        public Result<List<UserInfo>> GetUserInfoByEstablishmentCategoryId(int establishmentCategoryId)
        {
            try
            {
                var establishmentCategory = _establishmentCategoryRepository.GetById(establishmentCategoryId);

                if (establishmentCategory?.EstablishmentCategoryId > 0 == false)
                {
                    return Result<List<UserInfo>>.Failure("Falha ao recuperar a categoria.");
                }

                var users = _userRepository.GetUserByEstablishmentCategoryId(establishmentCategoryId);

                if (users?.Count > 0)
                {
                    return GetUserInfo(users);
                }

                return Result<List<UserInfo>>.Success([]);
            }
            catch (Exception ex)
            {
                return Result<List<UserInfo>>.Failure($"Falha ao obter os usuários: {ex.Message}");
            }
        }

        private Result<UserInfo> GetUserInfo(User user)
        {
            var userRole = _userRoleRepository.GetById(user.UserRoleId);

            if (userRole?.UserRoleId > 0 == false)
            {
                return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (UserRole)");
            }

            var userProfile = _userProfileRepository.GetByUserId(user.UserId);

            if (userProfile?.UserProfileId > 0 == false)
            {
                return Result<UserInfo>.Failure("Falha ao recuperar os dados do perfil do usuário. (UserProfile)");
            }

            var address = _addressRepository.GetById(userProfile.AddressId.GetValueOrDefault());

            return Result<UserInfo>.Success(new UserInfo
            {
                User = user,
                UserRole = userRole,
                UserProfile = userProfile,
                Address = address
            });
        }
        private Result<List<UserInfo>> GetUserInfo(List<User> users)
        {
            var result = new List<UserInfo>();

            foreach (var user in users)
            {
                if (user?.UserId > 0 == false) continue;

                var userRole = _userRoleRepository.GetById(user.UserRoleId);

                if (userRole?.UserRoleId > 0 == false)
                {
                    return Result<List<UserInfo>>.Failure("Falha ao recuperar os dados do usuário. (UserRole)");
                }

                var userProfile = _userProfileRepository.GetByUserId(user.UserId);

                if (userProfile?.UserProfileId > 0 == false)
                {
                    return Result<List<UserInfo>>.Failure("Falha ao recuperar os dados do perfil do usuário. (UserProfile)");
                }

                if (userRole.UserRoleId == 1)
                {
                    var feedbacks = _feedbackRepository.GetFeedbackByUserProfileId(userProfile.UserProfileId);

                    if (feedbacks?.Count > 0)
                    {
                        userProfile.Rating = new Rating
                        {
                            Value = feedbacks.Sum(f => f.Rating) / feedbacks.Count,
                            Total = feedbacks.Count
                        };
                    }
                }

                var address = _addressRepository.GetById(userProfile.AddressId.GetValueOrDefault());

                result.Add(new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                    UserProfile = userProfile,
                    Address = address
                });
            }

            return Result<List<UserInfo>>.Success(result);
        }
    }
}
