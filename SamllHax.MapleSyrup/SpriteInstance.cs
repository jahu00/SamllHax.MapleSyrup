using SamllHax.MapleSyrup.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class ObjectSpriteInstance
    {
        private readonly ResourceManager _resourceManager;
        private readonly WzMapObject _mapObject;
        private WzObjectPart _sprite;
        private Dictionary<string, SKBitmap> _bitmaps;
        private List<string> _frameIds;
        private int frameNo = 0;
        private int timer = 0;

        public ObjectSpriteInstance(ResourceManager resourceManager, WzMapObject mapObject, WzObjectPart objectPart)
        {
            _resourceManager = resourceManager;
            _mapObject = mapObject;
            _sprite = objectPart;
            _bitmaps = new Dictionary<string, SKBitmap>();
            _frameIds = _sprite.Frames.Keys.OrderBy(x => Convert.ToInt32(x)).ToList();
            foreach (var frameId in _frameIds)
            {
                var bitmap = _resourceManager.GetObjectImage(mapObject.GroupName, mapObject.Name, mapObject.SubsetName, mapObject.PartId, frameId);
                _bitmaps.Add(frameId, bitmap);
            }
        }

        public void Update(int delta)
        {
            WzFrame frame;
            do
            {
                var frameId = _frameIds[frameNo];
                frame = _sprite.Frames[frameId];
                if (!frame.Delay.HasValue)
                {
                    return;
                }
                timer += delta;
                if (frame.Delay.Value > timer)
                {
                    return;
                }
                timer -= frame.Delay.Value;
                frameNo++;
            }while(timer >= frame.Delay.Value);
        }


    }
}
