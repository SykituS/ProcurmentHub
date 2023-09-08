using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using ProcurementService.DbModels;
using ProcurementService.Functions;

namespace ProcurementService.Services
{
    public partial class ProcurementService
    {
        public override Task<GRPCValidationResponse> RegisterUser(GRPCRegisterNewUser request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call Register User: " + DateTime.Now);
            if (request.Password != request.ConfirmPassword)
            {
                _logger.LogInformation("End of call Register User: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "Password and confirm password must be this same!"
                });
            }

            var passwordStrenghtScore = PasswordValidator.CheckStrength(request.Password);

            switch (passwordStrenghtScore)
            {
                case PasswordScore.Blank:
                case PasswordScore.VeryWeak:
                case PasswordScore.Weak:
                    _logger.LogInformation("End of call Register User: " + DateTime.Now);
                    return Task.FromResult(new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "Given password is too week!"
                    });
            }

            var isEmailInUse = _context.Users.Any(e => e.UserName == request.Person.Email);
            if (isEmailInUse)
            {
                _logger.LogInformation("End of call Register User: " + DateTime.Now);
                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "Given email is already in use!"
                });
            }

            try
            {
                var person = new Persons
                {
                    FirstName = request.Person.FirstName,
                    LastName = request.Person.LastName,
                    Email = request.Person.Email,
                };

                _context.Add(person);
                _context.SaveChanges();

                var user = new Users
                {
                    Id = default,
                    UserName = request.Person.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    SecurityStamp = new Guid().ToString(),
                    PersonID = person.Id,
                    Disabled = false,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                };

                _context.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in function RegisterUser: " + ex);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was a problem with connection! Please try again later."
                });
            }

            _logger.LogInformation("End of call Register User: " + DateTime.Now);
            return Task.FromResult(new GRPCValidationResponse()
            {
                Successful = true,
                Information = "Account was created successfully"
            });
        }

        public override Task<GRPCLoggedUser> LoginUserToApplication(GRPCLoginUser request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call Login User to application: " + DateTime.Now);

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogInformation("End of call Login User to application: " + DateTime.Now);
                return Task.FromResult(new GRPCLoggedUser()
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "Entered values are empty!"
                    }
                });
            }

            var userVerify = _verifyLogin.CheckIfUsersExists(request.Email, request.Password);

            if (!userVerify.Successful)
            {
                _logger.LogInformation("End of call Login User to application: " + DateTime.Now);

                return Task.FromResult(new GRPCLoggedUser()
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            _logger.LogInformation("End of call Login User to application: " + DateTime.Now);

            return _usersQuery.GetUserDataFromDb(new GRPCLoginInformationForUser()
            { Id = userVerify.Information, Password = request.Password, Username = request.Email });
        }

        public override Task<GRPCLoggedUser> GetUserData(GRPCLoginInformationForUser request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call get user data: " + DateTime.Now);

            var isUsersExists =
                _verifyLogin.CheckIfUsersExists(request.Username, request.Password, Guid.Parse(request.Id));

            if (!isUsersExists.Successful)
            {
                _logger.LogInformation("End of call get user data: " + DateTime.Now);

                return Task.FromResult(new GRPCLoggedUser()
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            _logger.LogInformation("End of call get user data: " + DateTime.Now);
            return _usersQuery.GetUserDataFromDb(request);
        }
    }
}
