using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class FpsCounter
    {
        private readonly ILogger<FpsCounter> _logger;
        private readonly IConfigurationSection _configuration;
        private readonly int interval;
        private double timer = 0;
        private readonly List<double> times = new List<double>();

        public FpsCounter(IConfiguration configuration, ILogger<FpsCounter> logger)
        {
            _logger = logger;
            _configuration = configuration.GetSection("FpsCounter");
            interval = _configuration.GetValue<int>("Interval");
        }

        public void PushTime(double time)
        {
            timer += time;
            times.Add(time);
            if (timer < interval)
            {
                return;
            }
            Report();
            Reset();
        }

        public void Report()
        {
            var average = Convert.ToInt32(1 / times.Average());
            var min = Convert.ToInt32(1 / times.Max());
            var max = Convert.ToInt32(1 / times.Min());
            var min99 = Convert.ToInt32(1 / times.OrderByDescending(x => x).Take((int)Math.Ceiling(times.Count * 0.01)).Last());
            _logger.LogInformation($"Measure time {timer.ToString("0.00")}s; fps: max:{max} avg:{average} min99:{min99} min:{min}; total: {times.Count}");
        }

        public void Reset()
        {
            timer = 0;
            times.Clear();
        }
    }
}
