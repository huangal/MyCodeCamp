using System;
using AutoMapper;
using MyCodeCamp.Models;

namespace MyCodeCamp.Mapper
{
    public class TopicsProfile: Profile
    {
        public TopicsProfile()
        {

            CreateMap<TopicArea, TopicAreaModel>()
             .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Name))
             .ReverseMap();


        }
    }
}
