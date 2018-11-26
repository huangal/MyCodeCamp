using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MyCodeCamp.Models
{
    public class TopicAreaService: ITopicAreaService
    {
        private IMapper _mapper;

        public TopicAreaService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<string> GetAllInstances()
        {
            return Tools.GetAllEntities<IMock>();
        }



        public IEnumerable<TopicAreaModel> GetAllTopics()
        {
            var topics = GetTopics();

            return _mapper.Map<IEnumerable<TopicAreaModel>>(topics);
        }


        public IEnumerable<Hero> GetAllHeroes()
        {
            return GetHeroes();

        }

        public async Task<IEnumerable<Hero>> GetAllHeroesAsync()
        {
            return await Task.Run(() => GetHeroes());
        }


        public async Task<Hero> GetHeroAsync(int id)
        {
            return await Task.Run(() => GetHero(id));
        }




        private IEnumerable<Hero> GetHeroes()
        {
            return new List<Hero>
            {
                new Hero{Id=1, Name="Mr. Nice", IsSecret = true},
                new Hero{Id=2, Name="Narco", IsSecret = true},
                new Hero{Id=3, Name="Bombasto", IsSecret = true},
                new Hero{Id=4, Name="Celeritas", IsSecret = true},
                new Hero{Id=5, Name="Magneta", IsSecret = true},
                new Hero{Id=6, Name="RubberMan", IsSecret = true},
                new Hero{Id=7, Name="Dynama", IsSecret = true},
                new Hero{Id=8, Name="Dr IQ", IsSecret = false},
                new Hero{Id=9, Name="Magma", IsSecret = false},
                new Hero{Id=10, Name="Tornado", IsSecret = false}
            };
        }


        private Hero GetHero(int id)
        {
            return GetHeroes().Where(x => x.Id == id).FirstOrDefault();
        }



        private IEnumerable<TopicArea> GetTopics()
        {
            return new List<TopicArea>
            {
                new TopicArea {Name =".NET Core" },
                new TopicArea {Name ="Docker" },
                new TopicArea { Name ="C#" }
            };
        }








    }
}
