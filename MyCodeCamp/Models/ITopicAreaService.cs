using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCodeCamp.Models
{
    public interface ITopicAreaService
    {
        IEnumerable<TopicAreaModel> GetAllTopics();

        IEnumerable<string> GetAllInstances();
        IEnumerable<Hero> GetAllHeroes();
        Task<IEnumerable<Hero>> GetAllHeroesAsync();
        Task<Hero> GetHeroAsync(int id);
    }
}
