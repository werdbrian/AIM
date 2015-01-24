using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace AIM.Autoplay.Util.Objects
{
    public class Heroes
    {
        public Heroes()
        {
            CreateHeroesList();
        }
        public static Obj_AI_Hero Me = ObjectManager.Player;
        public List<Obj_AI_Hero> AllHeroes;
        public List<Obj_AI_Hero> AllyHeroes;
        public List<Obj_AI_Hero> EnemyHeroes;

        public void CreateHeroesList()
        {

            AllHeroes = ObjectManager.Get<Obj_AI_Hero>().ToList();
            AllyHeroes = AllHeroes.FindAll(hero => hero.IsAlly);
            EnemyHeroes = AllHeroes.FindAll(hero => !hero.IsAlly);
        }

        public void SortHeroesListByDistance()
        {
            AllHeroes = AllHeroes.OrderBy(hero => hero.Distance(Me)).ToList();
            AllyHeroes = AllyHeroes.OrderBy(hero => hero.Distance(Me)).ToList();
            EnemyHeroes = EnemyHeroes.OrderBy(hero => hero.Distance(Me)).ToList();   
        }

        public void RemoveFromHeroList(Obj_AI_Hero hero)
        {
            AllHeroes.Remove(hero);
            AllyHeroes.Remove(hero);
            EnemyHeroes.Remove(hero);
        }
    }
}
