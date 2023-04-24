using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Contracts.Managers;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Dto.Requests;
using Sat.Recruitment.Dto.Response;
using Sat.Recruitment.Shared;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public UsersController(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/list-users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserResponse>))]
        public ActionResult<List<UserResponse>> GetAll()
        {
            var users = _userManager.GetAll();
            var response = _mapper.Map<List<UserResponse>>(users);
            return Ok(response);
        }

        [HttpGet]
        [Route("/user/{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public ActionResult<UserResponse> GetById([FromRoute] int id)
        {
            var user = _userManager.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserResponse>(user));
        }

        [HttpPost]
        [Route("/create-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        public ActionResult<Result> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var response = _userManager.AddUser(user);
            return Ok(response);
        }

        [HttpDelete]
        [Route("/delete-user/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        public ActionResult<Result> DeleteById([FromRoute] int id)
        {
            var response = _userManager.DeleteById(id);
            return Ok(response);
        }
    }
}
