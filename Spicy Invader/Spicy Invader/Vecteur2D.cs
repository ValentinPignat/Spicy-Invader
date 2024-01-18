/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Vecteur2D class allowing to track objects position and movements

using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spicy_Invader
{
    /// <summary>
    /// Vecteur2D class to track objects position and movements
    /// </summary>
    internal class Vecteur2D
    {
        // Initialisations
        private double _x , _y;
        public readonly double norme;

        public Vecteur2D(double x = 0, double y = 0) { 
            _x = x;
            _y = y;
        }
    }
}
