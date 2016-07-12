using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace CEPsBrasil.Model
{
    public enum Zona
    {
        [Display(Name = "Centro")]
        Centro = 1,
        [Display(Name = "Norte")]
        Norte = 2,
        [Display(Name = "Sul")]
        Sul = 3,
        [Display(Name = "Leste")]
        Leste = 5,
        [Display(Name = "Oeste")]
        Oeste = 6
    }
}
