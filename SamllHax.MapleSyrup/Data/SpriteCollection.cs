using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class SpriteCollection
    {
        public int Seed { get; private set; } = 0;
        private List<Sprite> sprites = new List<Sprite>();

        public void Add(Sprite sprite)
        {
            Seed++;
            sprite.Id = Seed;
            sprites.Add(sprite);
        }

        public IEnumerable<Sprite> GetOrderdSprites()
        {
            return sprites.OrderBy(x => x.Z).ThenBy(x => x.Id);
        }
    }
}
