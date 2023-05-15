using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIBTicketsMVC.Models
{
    public class ListasDesplegables
    {
        public List<Templates> ListTemplates { get; set; } = new List<Templates>();
        public List<StatusDefinition> Estados { get; set; } = new List<StatusDefinition>();
        public List<FieldsUDF> FieldsListDependent { get; set; } = new List<FieldsUDF>();
        public List<TypesRequired> ListTypesRequired { get; set; } = new List<TypesRequired>();
        public List<Status_TypesActions> TypesActions { get; set; } = new List<Status_TypesActions>();
        public List<FieldsTypesUDF> FieldsTypes { get; set; } = new List<FieldsTypesUDF>();
        public List<Sites> Sitios { get; set; } = new List<Sites>();
        public List<Users> Usuarios { get; set; } = new List<Users>();
        public List<Profiles> Perfiles { get; set; } = new List<Profiles>();
        public List<Groups> Grupos { get; set; } = new List<Groups>();
        public List<AlgorithmsAssignment> Algorithms { get; set; } = new List<AlgorithmsAssignment>();
        public List<LineaNegocio> LineasNegocios { get; set; } = new List<LineaNegocio>();
        public List<Categories> Categorias { get; set; } = new List<Categories>();

        public List<Bases> Bases { get; set; } = new List<Bases>();
        public List<SINO> Imagenes { get; set; } = new List<SINO>();
        public List<SINO> Ascard { get; set; } = new List<SINO>();
        public List<SINO> Contrato { get; set; } = new List<SINO>();
        public List<SINO> Grabacion { get; set; } = new List<SINO>();
        public List<SINO> Reasignacion { get; set; } = new List<SINO>();
        public List<Canal> Canal { get; set; } = new List<Canal>();
        public List<SINO> Legalizado { get; set; } = new List<SINO>();
        public List<TipoReclamo> TipoReclamo { get; set; } = new List<TipoReclamo>();
        public List<EstadoBitacora> EstadoBitacora { get; set; } = new List<EstadoBitacora>();
    }
}
