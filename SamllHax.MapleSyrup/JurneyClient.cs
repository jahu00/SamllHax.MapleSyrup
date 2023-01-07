using SamllHax.MapleSyrup.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class JurneyClient: IDisposable
    {
        public Stage Stage { get; set; }
        private Window window;
        private Constants constants;

        private bool Running
        {
            get
            {
                return false;// !window.GlfwWindow.IsClosed && !window.GlfwWindow.IsClosing;
            }
        }

        public JurneyClient()
        {
            window = new Window();
            constants = new Constants();
        }


        public void Dispose()
        {
            window?.Dispose();
        }

        public void Loop()
        {
            var timer = new Stopwatch();
            timer.Start();
            long timestep = constants.TimeStep * 1000;
            long accumulator = timestep;

            long period = 0;
            int samples = 0;

            while (Running)
            {
                long elapsed = timer.ElapsedMilliseconds;
                timer.Restart();

                // Update game with constant timestep as many times as possible.
                for (accumulator += elapsed; accumulator >= timestep; accumulator -= timestep)
                {
                    Update();
                }

                // Draw the game. Interpolate to account for remaining time.
                var alpha = (double)accumulator / timestep;
                Draw(alpha);

                if (samples < 100)
                {
                    period += elapsed;
                    samples++;
                    continue;
                }
                //int64_t fps = (samples * 1000000) / period;
                //std::cout << "FPS: " << fps << std::endl;

                period = 0;
                samples = 0;
            }

            //Sound::close();
        }

        private void Draw(double alpha)
        {
            Stage?.draw(alpha);
            //window.End();
        }

        private void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
