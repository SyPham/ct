using System.Threading.Tasks;
using btn_api.DTO;

namespace btn_api._Services.Interface
{
    public interface ILineInfoService: IECService<LineInfoDto>
    {
        Task<LineInfoDto> GetLineInfoByLine(string lineName);
    }
}