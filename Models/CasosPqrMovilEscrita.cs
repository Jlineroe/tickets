using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class CasosPqrMovilEscrita
    {

        public string WorKOrder { get; set; }
        public string idWorKOrderSolutions { get; set; }
        //prepago
        public string PRE_Radicacion { get; set; }
        public string PRE_Radicado { get; set; }
        public string PRE_Tipo_Reclamo { get; set; }
        public string PRE_Nombre_Titular { get; set; }
        public string PRE_Min { get; set; }
        public string PRE_CUSTCODE { get; set; }
        public string PRE_Valor { get; set; }
        public string PRE_Concepto { get; set; }
        public string PRE_Analista { get; set; }
        public string PRE_Periodo_Ajustado_Desde { get; set; }
        public string PRE_Periodo_Ajustado_Hasta { get; set; }
        public string PRE_Aliado { get; set; }
        public string PRE_Estado { get; set; }
        public string PRE_Actualizacion_Anterior { get; set; }
        public string PRE_Ultima_Actualizacion { get; set; }

        //Pospago

        public string BPB_MIN { get; set; }
        public string BPB_CUN { get; set; }
        public string BPB_CUSTCODE { get; set; }
        public string BPB_CUSTOMER_ID { get; set; }
        public string BPB_AREA_QUE_GENERO_INCONSISTENCIA { get; set; }
        public string BPB_USUARIO_RESPONSABLE { get; set; }
        public string BPB_ANALISTA { get; set; }
        public string BPB_USER_RED { get; set; }
        public string BPB_TIPO_RECLAMO { get; set; }
        public string BPB_Valor { get; set; }
        public string BPB_Justificación { get; set; }
        public string BPB_Periodo_ajustar_desde { get; set; }
        public string BPB_Periodo_ajustar_Hasta { get; set; }
        public string BPB_SERVICIO { get; set; }
        public string BPB_CTA_CONTABLE { get; set; }
        public string BPB_IVA { get; set; }
        public string BPB_GERENCIA { get; set; }
        public string BPB_CENTRALES { get; set; }
        public string BPB_ALIADO { get; set; }
        public string BPB_CAUSAL { get; set; }
        public string BPB_ESTADO { get; set; }
        public string BPB_ACTUALIZACION_ANTERIOR { get; set; }
        public string BPB_ULTIMA_ACTUALIZACION { get; set; }

        //AScard
        public string Ascard1_Numero_Credito { get; set; }
        public string Ascard1_Valor_Ajuste { get; set; }
        public string Ascard1_Descripcion_Motivo_Ajuste { get; set; }
        public string Ascard1_Area_Solicita_Ajuste { get; set; }
        public string Ascard1_Comentario { get; set; }
        public string Ascard1_Documento_Usuario { get; set; }
        public string Ascard1_Reclamo_del_Usuario { get; set; }
        public string Ascard1_Custcode_Asociado_al_Crédito { get; set; }
        public string Ascard1_Motivo_Ajuste { get; set; }
        public string Ascard1_Nombre_de_Quien_Solicita { get; set; }
        public string Ascard1_CUN_NR { get; set; }
        public string Ascard1_Tipo_Reclamo { get; set; }
        public string Ascard1_Aliado { get; set; }
        public string Ascard1_Estado { get; set; }
        public string Ascard1_Actualizacion_Anterior { get; set; }
        public string Ascard1_Ultima_Actualizacion { get; set; }

        //cuotas ascard

        public string Ascard2_Numero_de_credito { get; set; }
        public string Ascard2_Valor_Nueva_Cuota { get; set; }
        public string Ascard2_Concepto { get; set; }
        public string Ascard2_Área_soliciita_ajuste { get; set; }
        public string Ascard2_Reclamo_Usuario { get; set; }
        public string Ascard2_Cantidad_cuotas { get; set; }
        public string Ascard2_Aliado { get; set; }
        public string Ascard2_Estado { get; set; }
        public string Ascard2_Actualizacion_Anterior { get; set; }
        public string Ascard2_Ultima_Actualizacion { get; set; }

        //ELIMINACION DE CENTRALES

        public string CEN_CUN { get; set; }
        public string CEN_Tipo_Documento { get; set; }
        public string CEN_Nombre { get; set; }
        public string CEN_Cedula { get; set; }
        public string CEN_CUSTCODE_O_No_CREDITO_A_ELIMINAR { get; set; }
        public string CEN_Estado { get; set; }
        public string CEN_Motivo_Eliminacion { get; set; }
        public string CEN_Analista { get; set; }
        public string CEN_Estado_Solicitud { get; set; }
        public string CEN_Actualizacion_Anterior { get; set; }
        public string CEN_Ultima_Actualizacion { get; set; }


        /// <summary>
        /// //movil verbales
        /// </summary>

        //prepago
        public string PRE_Radicacion_V { get; set; }
        public string PRE_Radicado_V { get; set; }
        public string PRE_Tipo_Reclamo_V { get; set; }
        public string PRE_Nombre_Titular_V { get; set; }
        public string PRE_Min_V { get; set; }
        public string PRE_CUSTCODE_V { get; set; }
        public string PRE_Valor_V { get; set; }
        public string PRE_Concepto_V { get; set; }
        public string PRE_Analista_V { get; set; }
        public string PRE_Periodo_Ajustado_Desde_V { get; set; }
        public string PRE_Periodo_Ajustado_Hasta_V { get; set; }
        public string PRE_Aliado_V { get; set; }
        public string PRE_Estado_V { get; set; }
        public string PRE_Actualizacion_Anterior_V { get; set; }
        public string PRE_Ultima_Actualizacion_V { get; set; }

        //Pospago

        public string BPB_MIN_V { get; set; }
        public string BPB_CUN_V { get; set; }
        public string BPB_CUSTCODE_V { get; set; }
        public string BPB_CUSTOMER_ID_V { get; set; }
        public string BPB_AREA_QUE_GENERO_INCONSISTENCIA_V { get; set; }
        public string BPB_USUARIO_RESPONSABLE_V { get; set; }
        public string BPB_ANALISTA_V { get; set; }
        public string BPB_USER_RED_V { get; set; }
        public string BPB_TIPO_RECLAMO_V { get; set; }
        public string BPB_Valor_V { get; set; }
        public string BPB_Justificación_V { get; set; }
        public string BPB_Periodo_ajustar_desde_V { get; set; }
        public string BPB_AREA_SOLICITA_AJUSTE_V { get; set; }
        public string BPB_USUARIO_AJUSTE_V { get; set; }
        public string BPB_Periodo_a_ajustar_desde_V { get; set; }
        public string BPB_Hasta_V { get; set; }
        
            
            
        public string BPB_Periodo_ajustar_Hasta_V { get; set; }
        public string BPB_SERVICIO_V { get; set; }
        public string BPB_CTA_CONTABLE_V { get; set; }
        public string BPB_IVA_V { get; set; }
        public string BPB_GERENCIA_V { get; set; }
        public string BPB_CENTRALES_V { get; set; }
        public string BPB_ALIADO_V { get; set; }
        public string BPB_CAUSAL_V { get; set; }
        public string BPB_ESTADO_V { get; set; }
        public string BPB_ACTUALIZACION_ANTERIOR_V { get; set; }
        public string BPB_ULTIMA_ACTUALIZACION_V { get; set; }

        //AScard
        public string Ascard1_Numero_Credito_V { get; set; }
        public string Ascard1_Valor_Ajuste_V { get; set; }
        public string Ascard1_Descripcion_Motivo_Ajuste_V { get; set; }
        public string Ascard1_Descripcion_Motivo_ajuste_V { get; set; }
        public string Ascard1_Area_Solicita_Ajuste_V { get; set; }
        public string Ascard1_Area_Solicitante_V { get; set; }
        public string Ascard1_Comentario_V { get; set; }
        public string Ascard1_Documento_Usuario_V { get; set; }
        public string Ascard1_Reclamo_del_Usuario_V { get; set; }
        public string Ascard1_Custcode_Asociado_al_Crédito_V { get; set; }
        public string Ascard1_Motivo_Ajuste_V { get; set; }
        public string Ascard1_Nombre_de_Quien_Solicita_V { get; set; }
        public string Ascard1_CUN_NR_V { get; set; }
        public string Ascard1_Tipo_Reclamo_V { get; set; }
        public string Ascard1_Aliado_V { get; set; }
        public string Ascard1_Estado_V { get; set; }
        public string Ascard1_Actualizacion_Anterior_V { get; set; }
        public string Ascard1_Ultima_Actualizacion_V { get; set; }

        //cuotas ascard

        public string Ascard2_Numero_de_credito_V { get; set; }
        public string Ascard2_Valor_Nueva_Cuota_V { get; set; }
        public string Ascard2_Concepto_V { get; set; }
        public string Ascard2_Área_soliciita_ajuste_V { get; set; }
        public string Ascard2_Reclamo_Usuario_V { get; set; }
        public string Ascard2_Cantidad_cuotas_V { get; set; }
        public string Ascard2_Aliado_V { get; set; }
        public string Ascard2_Estado_V { get; set; }
        public string Ascard2_Actualizacion_Anterior_V { get; set; }
        public string Ascard2_Ultima_Actualizacion_V { get; set; }

        //ELIMINACION DE CENTRALES

        public string CEN_CUN_V { get; set; }
        public string CEN_Tipo_Documento_V { get; set; }
        public string CEN_Nombre_V { get; set; }
        public string CEN_Cedula_V { get; set; }
        public string CEN_CUSTCODE_O_No_CREDITO_A_ELIMINAR_V { get; set; }
        public string CEN_Estado_V { get; set; }
        public string CEN_Motivo_Eliminacion_V { get; set; }
        public string CEN_Analista_V { get; set; }
        public string CEN_Estado_Solicitud_V { get; set; }
        public string CEN_Actualizacion_Anterior_V { get; set; }
        public string CEN_Ultima_Actualizacion_V { get; set; }




        // cargadata
        public int IdDataImported { get; set; }
        public Users UserImport { get; set; }
        public Users UserDesactivated { get; set; }
        public int NumRecords { get; set; }
        public string NameData { get; set; }
        public long NameDataEncrypted { get; set; }
        public string Extension { get; set; }
        public string DesactivationReason { get; set; }
        public string DateDesactivation { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
        public string ConexString { get; set; }
        public string returnError { get; set; }
        public string HojaSelected { get; set; }
        public string SQLTableTemp { get; set; }
        public List<string> NameHojas { get; set; }
        public List<string> Columnas { get; set; }

        public List<Users> ListUsers { get; set; } = new List<Users>();
        public Groups Grupo { get; set; } = new Groups();
        public AlgorithmsAssignment Algorithms { get; set; } = new AlgorithmsAssignment();
        public List<string> ColumnsSQL { get; set; }
        public List<string> ColumnsExcel { get; set; }
        public FieldsUDF FieldUDF { get; set; } = new FieldsUDF();
        public Templates Plantilla { get; set; } = new Templates();
        public List<long> ListIdWorkOrder { get; set; } = new List<long>();
    }
}