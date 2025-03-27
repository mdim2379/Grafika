using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szeminarium
{
    internal class CubeArrangementModel
    {
        public bool[] Forgatasok;

        public int index {  get; set; }
        private double Time { get; set; } = 0;

        public double DiamondCubeGlobalYAngle { get; private set; } = 0;
        public double DiamondCubeGlobalZAngle { get; private set; } = 0;
        public double DiamondCubeGlobalXAngle { get; private set; } = 0;

        private bool[] forogjon;
        private double mennyiszog = 0;
        
        public CubeArrangementModel()
        {
            Forgatasok = new bool[6];
            for(int i = 0; i < Forgatasok.Length; i++)
                Forgatasok[i] = false;
            forogjon = new bool[6];
            for(int i = 0; i < forogjon.Length; i++)
                forogjon[i] = false;
        }

        internal void AdvanceTime(double deltaTime)
        {
            for (int i = 0; i < Forgatasok.Length; i++)
            {
                if (Forgatasok[i])
                {
                    forogjon[i] = true;
                    mennyiszog = 0;
                    Forgatasok[i] = false;
                }
            }

            for (int i = 0; i < forogjon.Length; i++) 
                if (forogjon[i]) 
                { 
                    Time += deltaTime;
                if (mennyiszog > Math.PI / 2)
                {
                    forogjon[i] = false;
                    return;
                }

                mennyiszog += deltaTime / 2;
                switch (i)
                {
                    case 0:
                        DiamondCubeGlobalXAngle += deltaTime / 2;
                        break;
                    case 1:
                        DiamondCubeGlobalXAngle -= deltaTime / 2;
                        break;
                    case 2:
                        DiamondCubeGlobalYAngle += deltaTime / 2;
                        break;
                    case 3:
                        DiamondCubeGlobalYAngle -= deltaTime / 2;
                        break;
                    case 4:
                        DiamondCubeGlobalZAngle += deltaTime / 2;
                        break;
                    case 5:
                        DiamondCubeGlobalZAngle -= deltaTime / 2;
                        break;
                }
            }
            

        }

    }
}
