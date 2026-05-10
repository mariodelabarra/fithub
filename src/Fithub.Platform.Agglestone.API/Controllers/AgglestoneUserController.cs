using Fithub.Platform.Agglestone.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fithub.Platform.Agglestone.API.Controllers
{
    [Route("api/agglestone")]
    [ApiController]
    [Authorize]
    public class AgglestoneUserController(IAgglestoneUserService _agglestoneUserService) : ControllerBase
    {
        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfo = await _agglestoneUserService.GetUserInfoAsync();
            return Ok(userInfo);
        }
    }
}
