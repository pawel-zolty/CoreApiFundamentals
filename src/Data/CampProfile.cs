﻿using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>()
                .ForMember(campModel => campModel.Venue, o => o.MapFrom(camp => camp.Location.VenueName))
                .ForMember(campModel => campModel.Address1, o => o.MapFrom(camp => camp.Location.Address1))
                .ForMember(campModel => campModel.Address2, o => o.MapFrom(camp => camp.Location.Address2))
                .ForMember(campModel => campModel.Address3, o => o.MapFrom(camp => camp.Location.Address3))
                .ForMember(campModel => campModel.CityTown, o => o.MapFrom(camp => camp.Location.CityTown))
                .ForMember(campModel => campModel.StateProvince, o => o.MapFrom(camp => camp.Location.StateProvince))
                .ForMember(campModel => campModel.PostalCode, o => o.MapFrom(camp => camp.Location.PostalCode))
                .ForMember(campModel => campModel.Country, o => o.MapFrom(camp => camp.Location.Country))
                .ReverseMap();

            this.CreateMap<Talk, TalkModel>()
                .ReverseMap()
                .ForMember(t => t.Camp, o => o.Ignore())
                .ForMember(t => t.Speaker, o => o.Ignore())
                .ForMember(t => t.TalkId, o => o.Ignore());

            this.CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}
