//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DXC.TFSM.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_perfilador_Respuestas
    {
        public int IdRespuesta { get; set; }
        public string Clave { get; set; }
        public int Orden { get; set; }
        public string Respuesta { get; set; }
        public decimal Valor { get; set; }
        public bool Activa { get; set; }
        public int IdPregunta { get; set; }
    }
}
