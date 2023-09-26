using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CustomizeStatusCodePage.Roles.Dto;
using CustomizeStatusCodePage.Users.Dto;

namespace CustomizeStatusCodePage.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();
    }
}