//using E_CommerceApp_Backend.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace E_CommerceApp_Backend.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthenticateController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> userManager;
//        private readonly RoleManager<IdentityRole> roleManager;
//        private readonly IConfiguration _configuration;

//        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
//        {
//            this.userManager = userManager;
//            this.roleManager = roleManager;
//            _configuration = configuration;
//        }
//        [HttpPost]
//        [Route("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {
//            var user = await userManager.FindByNameAsync(model.Username);
//            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
//            {
//                var userRoles = await userManager.GetRolesAsync(user);

//                var authClaims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, user.UserName),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//                foreach (var userRole in userRoles)
//                {
//                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//                }

//                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

//                var token = new JwtSecurityToken(
//                    issuer: _configuration["JWT:ValidIssuer"],
//                    audience: _configuration["JWT:ValidAudience"],
//                    expires: DateTime.Now.AddHours(3),
//                    claims: authClaims,
//                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
//                    );

//                return Ok(new
//                {
//                    token = new JwtSecurityTokenHandler().WriteToken(token),
//                    expiration = token.ValidTo,
//                    roles = userRoles,
//                });
//            }
//            return Unauthorized();
//        }

//        [HttpPost]
//        [Route("registerUser")]
//        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerModel)
//        {
//            var userExist = await userManager.FindByNameAsync(registerModel.Username);
//            if (userExist != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            
//            var mailExist = await userManager.FindByEmailAsync(registerModel.Email);
//            if (mailExist != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already in use!" });

//            ApplicationUser user = new ApplicationUser()
//            {
//                Email = registerModel.Email,
//                SecurityStamp = Guid.NewGuid().ToString(),
//                UserName = registerModel.Username
//            };

//            var passwordIsValid = await userManager.PasswordValidators.FirstOrDefault().ValidateAsync(userManager,user,registerModel.Password);

//            if (!passwordIsValid.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password must be at least 8 characters long, and contain at least one of each of uppercase, lowercase, numeric and special characters!" });

//            var result = await userManager.CreateAsync(user, registerModel.Password);

//            if (!result.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

//            if (registerModel.UserRole.Equals(UserRoles.Seller))
//            {
//                if (!await roleManager.RoleExistsAsync(UserRoles.Seller))
//                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Seller));

//                if (await roleManager.RoleExistsAsync(UserRoles.Seller))
//                    await userManager.AddToRoleAsync(user, UserRoles.Seller);
//            }
//            else
//            {
//                if (!await roleManager.RoleExistsAsync(UserRoles.Buyer))
//                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Buyer));

//                if (await roleManager.RoleExistsAsync(UserRoles.Buyer))
//                    await userManager.AddToRoleAsync(user, UserRoles.Buyer);
//            }


//            return Ok(new Response { Status = "Success", Message = "User created successfully!" }); 
//        }

//        [HttpPost]
//        [Route("registerBuyer")]
//        public async Task<IActionResult> RegisterBuyer([FromBody] RegisterModel registerModel)
//        {
//            var userExist = await userManager.FindByNameAsync(registerModel.Username);
//            if (userExist != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

//            var mailExist = await userManager.FindByEmailAsync(registerModel.Email);
//            if (mailExist != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already in use!" });

//            ApplicationUser user = new ApplicationUser()
//            {
//                Email = registerModel.Email,
//                SecurityStamp = Guid.NewGuid().ToString(),
//                UserName = registerModel.Username
//            };

//            var passwordIsValid = await userManager.PasswordValidators.FirstOrDefault().ValidateAsync(userManager, user, registerModel.Password);

//            if (!passwordIsValid.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password must be at least 8 characters long, and contain at least one of each of uppercase, lowercase, numeric and special characters!" });

//            var result = await userManager.CreateAsync(user, registerModel.Password);

//            if (!result.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });



//            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
//        }


//        [HttpPost]
//        [Route("registerAdmin")]
//        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel registerModel)
//        {
//            var userExist = await userManager.FindByNameAsync(registerModel.Username);
//            if (userExist != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

//            var mailExist = await userManager.FindByEmailAsync(registerModel.Email);
//            if (mailExist != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already in use!" });

//            ApplicationUser user = new ApplicationUser()
//            {
//                Email = registerModel.Email,
//                SecurityStamp = Guid.NewGuid().ToString(),
//                UserName = registerModel.Username
//            };

//            var passwordIsValid = await userManager.PasswordValidators.FirstOrDefault().ValidateAsync(userManager, user, registerModel.Password);

//            if (!passwordIsValid.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password must be at least 8 characters long, and contain at least one of each of uppercase, lowercase, numeric and special characters!" });

//            var result = await userManager.CreateAsync(user, registerModel.Password);

//            if (!result.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

//            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
//                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

//            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
//                await userManager.AddToRoleAsync(user, UserRoles.Admin);

//            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
//        }
//    }
//}
