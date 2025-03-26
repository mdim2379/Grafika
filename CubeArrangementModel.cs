using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szeminarium
{
    internal class CubeArrangementModel
    {
        public bool AnimationEnabled { get; set; } = false;
        private double Time { get; set; } = 0;

        public double DiamondCubeGlobalYAngle { get; private set; } = 0;
        public double DiamondCubeGlobalZAngle { get; private set; } = 0;

        private bool forogjon;
        private double mennyiszog = 0;

        internal void AdvanceTime(double deltaTime)
        {
            if (AnimationEnabled)
            {
                forogjon = true;
                mennyiszog = 0;
                AnimationEnabled = false;
            }

            if (!forogjon)
                return;
            Time += deltaTime;
            if (mennyiszog > Math.PI / 2)
            {
                forogjon = false;
                return;
            }
            mennyiszog += deltaTime/2;
            DiamondCubeGlobalYAngle += deltaTime/2;
        }

    }
}
