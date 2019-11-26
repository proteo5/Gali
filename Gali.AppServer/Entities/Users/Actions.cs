using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gali.AppServer.Entities;
using Gali.AppServer.Resources;

namespace Gali.AppServer.Entities.Users
{
    public class Actions
    {
        private readonly Steward<Users.Model> userColl = new Steward<Users.Model>("Users");
        private MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GetByUser_input, Users.Model>();
        });

        #region Register
        public class Register_input
        {
            public string User;
            public string Names;
            public string LastNames;
            public string Password;
            public string ConfirmPassword;
        }
        public Result Register(Register_input input)
        {
            try
            {
                //Clean Data
                input.User = input.User.Clean().ToLower();
                input.Names = input.Names.Clean();
                input.LastNames = input.LastNames.Clean();

                //Validate
                Validator validator = new Validator();
                validator.Add(input.User.Required("User", "El Usuario es Requerido"));
                validator.Add(input.User.IsEmail("User", "El Usuario debe de ser un correo electronico valido"));
                validator.Add(input.Names.Required("Names", "Los Nombres son Requeridos"));
                validator.Add(input.LastNames.Required("LastNames", "Los Apellidos son Requeridos"));
                validator.Add(input.Password.Required("Password", "El Password es Requerido"));
                validator.Add(input.ConfirmPassword.Required("PasswordConfirm", "El Password es Requerido"));
                validator.Add(input.ConfirmPassword.IsEquals("PasswordConfirm", "El Password de Confirmacion no es igual al Password", input.Password));

                if (validator.ValidateItem("User"))
                {
                    //Check if user exist on database
                    var response = this.GetByUser(new GetByUser_input() { User = input.User });
                    if (response.State == ResultsStates.success)
                    {
                        validator.Add(new ValidatorResult() { IsValid = false, ItemName = "User", Message = "El correo electrónico ya esta en uso. Haga Login o recupere su contraseña." });
                    }
                }

                var validation = validator.Validate();
                if (!validation.IsValid)
                {
                    return new Result() { State = ResultsStates.invalid, Message = "Datos invalidos", ValidationResults = validation };
                }

                //Process
                IMapper Mapper = config.CreateMapper();
                var data = Mapper.Map<Register_input, Users.Model>(input);

                data.PasswordSalt = Guid.NewGuid().ToString();
                data.Password = HashRes.SHA256Of($"{input.User}{input.Password}{data.PasswordSalt}");

                var result2 = userColl.Insert(data);

                return result2;
            }
            catch (Exception ex)
            {
                return new Result() { State = ResultsStates.error, Exception = ex };
            }
        }
        #endregion //Create

        #region GetByUser
        public class GetByUser_input
        {
            public string User;
        }

        public Result<List<Users.Model>> GetByUser(GetByUser_input input)
        {
            try
            {
                //Clean data
                input.User = input.User.Clean().ToLower();

                //Validate
                Validator validator = new Validator();
                validator.Add(input.User.Required("User", "El Usuario es Requerido"));
                var validation = validator.Validate();
                if (!validation.IsValid)
                {
                    return new Result<List<Users.Model>>() { State = ResultsStates.invalid, Message = "Datos invalidos", ValidationResults = validation };
                }

                //Process
                var result = userColl.GetBy(x => x.User == input.User);

                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<Users.Model>>() { State = ResultsStates.error, Exception = ex };
            }
        }
        #endregion //GetByUser
    }
}
