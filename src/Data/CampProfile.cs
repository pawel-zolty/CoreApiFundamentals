using AutoMapper;
using CoreCodeCamp.Camps;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>();
        }
    }
}
