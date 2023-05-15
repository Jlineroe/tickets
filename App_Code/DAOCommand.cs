using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using AIBTicketsMVC.ViewModels;
using Newtonsoft.Json;
using ClosedXML.Excel;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Configuration;

namespace AIBTicketsMVC.App_Code
{
    public class DAOCommand
    {
        public async static Task<DataTable> EmailUsersException()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_UsersMailException";
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DAOCommand.EmailUsersException({Tools.GetLineErr(ex)}): {ex.Message}";

                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task<bool> VerifyAccessForm(List<Profiles> PerfilesUserActual, string Controller)
        {
            bool Swi = false;
            try
            {
                //Se envian los perfiles al sp
                DataTable dtPerfiles = new DataTable();
                dtPerfiles.Columns.Add("Id");
                if (PerfilesUserActual != null)
                {
                    foreach (var Item in PerfilesUserActual)
                    {
                        var RowA = dtPerfiles.NewRow();
                        RowA["Id"] = Item.IdMasterProfiles;
                        dtPerfiles.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_VerificarPermisosForm";
                cmd.Parameters.AddWithValue("dtPerfiles", dtPerfiles);
                cmd.Parameters.AddWithValue("Controller", Controller);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt.Rows.Count > 0)
                {
                    Swi = true;
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DAOCommand.VerifyAccessForm({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Swi;
        }
        public async static Task<Users> InforUserActual(bool? Profiles = null, bool? Sites = null, bool? Groups = null)
        {
            ConstProject P = new ConstProject();
            List<Users> InforUser = await ListUsersBasic(null, P.WinuserActual);
            if (InforUser.Count > 0)
            {
                if (Profiles == true)
                {
                    InforUser[0].Perfiles = await ListProfilesXUser(InforUser[0].IdMasterUsers);
                }
                if (Sites == true)
                {
                    InforUser[0].Sitios = await ListSitiosXProfiles(InforUser[0].Perfiles);
                }
                if (Groups == true)
                {
                    List<Sites> Sitios = await ListSitiosConPermisos(InforUser[0].Perfiles, 8, true); //Grupos
                    InforUser[0].Grupos = await ListGroups(Sitios, null, true);
                }
            }
            else
            {
                return null;
            }
            return InforUser[0];
        }
        public async static Task<DataTable> DataTableExcel(string ConexString, string HojaSelected)
        {
            DataTable dtExcel = new DataTable();
            try
            {
                OleDbCommand cmdExcel = await DAOConfig.OleCommandExcel(ConexString);
                cmdExcel.CommandText = $"SELECT * From [{HojaSelected}]";
                cmdExcel.CommandTimeout = 500000;
                dtExcel = await DAOConfig.DataTableOleCommand(cmdExcel);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DAOCommand.DataTableExcel({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dtExcel;
        }
        public async static Task<List<Location>> ListLocation()
        {
            List<Location> ListData = new List<Location>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandELC();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_ListLocation";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.AsEnumerable()
                                select new Location()
                                {
                                    IdLocation = int.Parse(Linq["IdLocation"].ToString()),
                                    Description = Linq["Description"].ToString(),
                                    IdCity = int.Parse(Linq["IdCity"].ToString())
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DAOCommand.ListLocation({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }
        public async static Task<List<WH_Platform>> ListPlatform(int? IdPlatform = null, int? IdLocation = null, string NamePlatform = "")
        {
            List<WH_Platform> ListData = new List<WH_Platform>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandELC();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WH_Platform_Sel_ListPlatform";
                if (IdPlatform != null) cmd.Parameters.AddWithValue("IdPlatform", IdPlatform);
                if (IdLocation != null) cmd.Parameters.AddWithValue("IdLocation", IdLocation);
                if (NamePlatform != "") cmd.Parameters.AddWithValue("NamePlatform", NamePlatform);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.Rows.Cast<DataRow>()
                                select new WH_Platform()
                                {
                                    IdPlatform = int.Parse(Linq["IdPlatform"].ToString()),
                                    Description = Linq["Platform"].ToString(),
                                    Location = new Location
                                    {
                                        IdLocation = int.Parse(Linq["IdLocation"].ToString()),
                                        Description = Linq["Location"].ToString()
                                    },
                                    Record_Creation_Date = Linq["Record_Creation_Date"].ToString(),
                                    Record_Update_Date = Linq["Record_Update_Date"].ToString()
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DAOCommand.ListPlatform({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }
        public async static Task<List<WH_Booth>> ListBooth(int? IdPlatform = null, int? IdBooth = null)
        {
            List<WH_Booth> ListData = new List<WH_Booth>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandELC();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WH_Booth_Sel_ListBooth";
                if (IdPlatform != null) cmd.Parameters.AddWithValue("IdPlatform", IdPlatform);
                if (IdBooth != null) cmd.Parameters.AddWithValue("IdBooth", IdBooth);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.Rows.Cast<DataRow>()
                                select new WH_Booth()
                                {
                                    IdBooth = int.Parse(Linq["IdBooth"].ToString()),
                                    Location = Linq["Location"].ToString(),
                                    Platform = Linq["Platform"].ToString(),
                                    Island = Linq["Island"].ToString(),
                                    BoothName = Linq["BoothName"].ToString(),
                                    BoothNumber = int.Parse(Linq["BoothNumber"].ToString()),
                                    ServiceLine = Linq["ServiceLine"].ToString(),
                                    WAH = bool.Parse(Linq["WAH"].ToString()),
                                    SwSite = bool.Parse(Linq["SwSite"].ToString()),
                                    IP = Linq["IP"].ToString(),
                                    Extension = Linq["Extension"].ToString()
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DAOCommand.ListBooth({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }

        #region Algorithms
        public async static Task<List<AlgorithmsAssignment>> ListAlgorithms(bool? ImportData = null, int? IdAlgorithms = null, bool? Full = null)
        {
            List<AlgorithmsAssignment> Algorithms = new List<AlgorithmsAssignment>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Algorithms_Sel_ListAlgorithms";
                if (IdAlgorithms != null) cmd.Parameters.AddWithValue("IdAlgorithms", IdAlgorithms);
                if (ImportData != null) cmd.Parameters.AddWithValue("ImportData", ImportData);
                if (Full != null) cmd.Parameters.AddWithValue("Full", Full);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Algorithms = (from Linq in dt.Rows.Cast<DataRow>()
                                  select new AlgorithmsAssignment()
                                  {
                                      IdAlgorithmsAssignment = int.Parse(Linq["IdAlgorithmsAssignment"].ToString()),
                                      NameAlgorithm = Linq["NameAlgorithm"].ToString(),
                                      DescriptionAlgorithm = Linq["DescriptionAlgorithm"].ToString(),
                                      SQLStringAlgorithm = Linq["SQLStringAlgorithm"].ToString(),
                                      ImportData = bool.Parse(Linq["ImportData"].ToString())
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>Algorithms.DAOCommand.ListAlgorithms({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Algorithms;
        }
        #endregion
        #region Navigation
        public async static Task<NivelesMenu> MainMenu(List<Profiles> Perfiles = null, int? Permisos = null)
        {
            NivelesMenu LvlMenu = new NivelesMenu();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtPerfiles = new DataTable();
                dtPerfiles.Columns.Add("Id");
                if (Perfiles != null)
                {
                    foreach (var Item in Perfiles)
                    {
                        var RowA = dtPerfiles.NewRow();
                        RowA["Id"] = Item.IdMasterProfiles;
                        dtPerfiles.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_ListMenusActions";
                if (Perfiles != null) cmd.Parameters.AddWithValue("dtPerfiles", dtPerfiles);
                if (Permisos != null) cmd.Parameters.AddWithValue("Permisos", Permisos);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    LvlMenu.MenuLvl1 = (from Linq in dt.Rows.Cast<DataRow>()
                                        where Linq["Level"].ToString() == "1"
                                        select new MenuAndActions()
                                        {
                                            IdMasterMenu = int.Parse(Linq["IdMasterMenu"].ToString()),
                                            Level = int.Parse(Linq["Level"].ToString()),
                                            Name = Linq["Name"].ToString(),
                                            Icono = Linq["Icono"].ToString(),
                                            Parent_IdMenu = int.Parse((Linq["Parent_IdMenu"].ToString() == "" ? "0" : Linq["Parent_IdMenu"].ToString())),
                                            Controller = Linq["Controller"].ToString(),
                                            Permiso = int.Parse(Linq["Permiso"].ToString())
                                        }).ToList();

                    LvlMenu.MenuLvl2 = (from Linq in dt.Rows.Cast<DataRow>()
                                        where Linq["Level"].ToString() == "2"
                                        select new MenuAndActions()
                                        {
                                            IdMasterMenu = int.Parse(Linq["IdMasterMenu"].ToString()),
                                            Level = int.Parse(Linq["Level"].ToString()),
                                            Name = Linq["Name"].ToString(),
                                            Icono = Linq["Icono"].ToString(),
                                            Parent_IdMenu = int.Parse((Linq["Parent_IdMenu"].ToString() == "" ? "0" : Linq["Parent_IdMenu"].ToString())),
                                            Controller = Linq["Controller"].ToString(),
                                            Permiso = int.Parse(Linq["Permiso"].ToString())
                                        }).ToList();

                    LvlMenu.MenuLvl3 = (from Linq in dt.Rows.Cast<DataRow>()
                                        where Linq["Level"].ToString() == "3"
                                        select new MenuAndActions()
                                        {
                                            IdMasterMenu = int.Parse(Linq["IdMasterMenu"].ToString()),
                                            Level = int.Parse(Linq["Level"].ToString()),
                                            Name = Linq["Name"].ToString(),
                                            Icono = Linq["Icono"].ToString(),
                                            Parent_IdMenu = int.Parse((Linq["Parent_IdMenu"].ToString() == "" ? "0" : Linq["Parent_IdMenu"].ToString())),
                                            Controller = Linq["Controller"].ToString(),
                                            Permiso = int.Parse(Linq["Permiso"].ToString())
                                        }).ToList();

                    LvlMenu.Actions = (from Linq in dt.Rows.Cast<DataRow>()
                                       where Linq["Level"].ToString() == "0"
                                       select new MenuAndActions()
                                       {
                                           IdMasterMenu = int.Parse(Linq["IdMasterMenu"].ToString()),
                                           Level = int.Parse(Linq["Level"].ToString()),
                                           Name = Linq["Name"].ToString(),
                                           Icono = Linq["Icono"].ToString(),
                                           Parent_IdMenu = int.Parse((Linq["Parent_IdMenu"].ToString() == "" ? "0" : Linq["Parent_IdMenu"].ToString())),
                                           Controller = Linq["Controller"].ToString(),
                                           Permiso = int.Parse(Linq["Permiso"].ToString())
                                       }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>Navigation.DAOCommand.MainMenu({Tools.GetLineErr(ex)}): {ex.Message}";
                Tools.LogAplications("ERROR", Error);
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return LvlMenu;
        }
        #endregion
        #region SitesControllers
        public async static Task<List<Sites>> ListSitios(int? IdMasterSites = null)
        {
            List<Sites> ListSitios = new List<Sites>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sites_Sel_ListSites";
                if (IdMasterSites != null) cmd.Parameters.AddWithValue("IdMasterSites", IdMasterSites);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListSitios = (from Linq in dt.Rows.Cast<DataRow>()
                                  select new Sites()
                                  {
                                      IdMasterSites = int.Parse(Linq["IdMasterSites"].ToString()),
                                      NameSite = Linq["NameSite"].ToString(),
                                      DescriptionSite = Linq["DescriptionSite"].ToString(),
                                      RequiredSubStatus = bool.Parse(Linq["RequiredSubStatus"].ToString()),
                                      DateLog = Linq["DateLog"].ToString(),
                                      State = bool.Parse(Linq["State"].ToString())
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListSitios;
        }
        public async static void SaveSitio(long IdMasterUsersGestiona, Sites Sitios)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sites_Ins_SaveSite";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdMasterUsersGestiona);
                cmd.Parameters.AddWithValue("NameSite", Sitios.NameSite);
                cmd.Parameters.AddWithValue("RequiredSubStatus", Sitios.RequiredSubStatus);
                cmd.Parameters.AddWithValue("DescriptionSite", Sitios.DescriptionSite);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Sitios");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.GuardarSitio({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static void UpdateSitio(long IdMasterUsersGestiona, Sites Sitios)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sites_Upd_UpdateSite";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdMasterUsersGestiona);
                cmd.Parameters.AddWithValue("IdMasterSites", Sitios.IdMasterSites);
                cmd.Parameters.AddWithValue("NameSite", Sitios.NameSite);
                cmd.Parameters.AddWithValue("DescriptionSite", Sitios.DescriptionSite);
                cmd.Parameters.AddWithValue("RequiredSubStatus", Sitios.RequiredSubStatus);
                cmd.Parameters.AddWithValue("State", Sitios.State);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Actualizar", "Sitios");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.UpdateSitio({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static void DeleteSitio(long IdMasterUsersGestiona, int IdMasterSites)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sites_Upd_DeleteSite";
                cmd.Parameters.AddWithValue("IdMasterSites", IdMasterSites);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdMasterUsersGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "Sitios");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.DeleteSitio({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<DataTable> VerifyNameSitio(string NameSitio)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdMasterSites FROM MasterSites WITH (NOLOCK) WHERE NameSite=@NameSitio;";
                cmd.Parameters.AddWithValue("NameSitio", NameSitio);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.VerifyNameSitio({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task<List<Sites>> ListSitiosXProfiles(List<Profiles> Perfiles)
        {
            List<Sites> ListSitios = new List<Sites>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtPerfiles = new DataTable();
                dtPerfiles.Columns.Add("Id");
                if (Perfiles != null)
                {
                    foreach (var Item in Perfiles)
                    {
                        var RowA = dtPerfiles.NewRow();
                        RowA["Id"] = Item.IdMasterProfiles;
                        dtPerfiles.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sites_Sel_ListSitesXProfiles";
                cmd.Parameters.AddWithValue("dtPerfiles", dtPerfiles);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListSitios = (from Linq in dt.Rows.Cast<DataRow>()
                                  select new Sites()
                                  {
                                      IdMasterSites = int.Parse(Linq["IdMasterSites"].ToString()),
                                      NameSite = Linq["NameSite"].ToString()
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListSitiosXProfiles({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListSitios;
        }
        public async static Task<List<Sites>> ListSitiosConPermisos(List<Profiles> Perfiles, int IdMenuAction, bool IsMenu)
        {
            List<Sites> ListSites = new List<Sites>();
            foreach (var item in Perfiles)
            {
                List<Profiles> Profiles = new List<Profiles> { item };
                var PermisoPerfiles = await ListMenusActions(Profiles, IdMenuAction, IsMenu); //Menu de perfiles
                if (PermisoPerfiles.Count > 0)
                {
                    ListSites.AddRange(await ListSitiosXProfiles(Profiles));
                }
            }
            List<Sites> GroupsListSites = ListSites
                .GroupBy(x => new { x.IdMasterSites, x.NameSite })
                .Select(y => new Sites()
                {
                    IdMasterSites = y.Key.IdMasterSites,
                    NameSite = y.Key.NameSite
                }
            ).ToList();
            return GroupsListSites;
        }
        #endregion
        #region ProfilesControllers
        public async static Task<List<Profiles>> ListProfilesXUser(long IdMasterUsers)
        {
            List<Profiles> InforProfile = new List<Profiles>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Sel_ListProfilesXUser";
                cmd.Parameters.AddWithValue("IdMasterUsers", IdMasterUsers);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int IdMasterProfile = int.Parse(dr["IdMasterProfiles"].ToString());
                        InforProfile.Add(new Profiles
                        {
                            IdMasterProfiles = IdMasterProfile,
                            NameProfile = dr["NameProfile"].ToString(),
                            Sitios = await ListSitios(IdMasterProfile)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListProfilesXUser({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return InforProfile;
        }
        public async static Task<List<MenuAndActions>> ListPermisos(List<Profiles> Perfiles, int? IdAction = null, bool? IdIsMenu = null)
        {
            List<MenuAndActions> Menu = new List<MenuAndActions>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtPerfiles = new DataTable();
                dtPerfiles.Columns.Add("Id");
                foreach (var Item in Perfiles)
                {
                    var RowA = dtPerfiles.NewRow();
                    RowA["Id"] = Item.IdMasterProfiles;
                    dtPerfiles.Rows.Add(RowA);
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_ListMenusActions";
                cmd.Parameters.AddWithValue("dtPerfiles", dtPerfiles);
                if (IdAction != null) cmd.Parameters.AddWithValue("IdAction", IdAction);
                if (IdIsMenu != null) cmd.Parameters.AddWithValue("IdIsMenu", IdIsMenu);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Menu = (from Linq in dt.Rows.Cast<DataRow>()
                            select new MenuAndActions()
                            {
                                IdMasterMenu = int.Parse(Linq["IdMasterMenu"].ToString()),
                                Level = int.Parse(Linq["Level"].ToString()),
                                Name = Linq["Name"].ToString(),
                                Icono = Linq["Icono"].ToString(),
                                Parent_IdMenu = int.Parse(Linq["Parent_IdMenu"].ToString() == "" ? "0" : Linq["Parent_IdMenu"].ToString()),
                                Controller = Linq["Controller"].ToString(),
                                Permiso = int.Parse(Linq["Permiso"].ToString()),
                                IdIsMenu = bool.Parse(Linq["IdIsMenu"].ToString())
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListPermisos({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Menu;
        }
        public async static Task<List<MenuAndActions>> ListMenusActions(List<Profiles> Perfiles, int? IdAction = null, bool? IdIsMenu = null)
        {
            List<MenuAndActions> Menu = new List<MenuAndActions>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtPerfiles = new DataTable();
                dtPerfiles.Columns.Add("Id");
                if (Perfiles != null)
                {
                    foreach (var Item in Perfiles)
                    {
                        var RowA = dtPerfiles.NewRow();
                        RowA["Id"] = Item.IdMasterProfiles;
                        dtPerfiles.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Sel_ListMenusActionsPrecise";
                cmd.Parameters.AddWithValue("dtPerfiles", dtPerfiles);
                if (IdAction != null) cmd.Parameters.AddWithValue("IdAction", IdAction);
                if (IdIsMenu != null) cmd.Parameters.AddWithValue("IdIsMenu", IdIsMenu);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Menu = (from Linq in dt.Rows.Cast<DataRow>()
                            select new MenuAndActions()
                            {
                                IdMasterMenu = int.Parse(Linq["IdMasterMenu"].ToString()),
                                Level = int.Parse(Linq["Level"].ToString()),
                                Name = Linq["Name"].ToString(),
                                Icono = Linq["Icono"].ToString(),
                                Parent_IdMenu = int.Parse(Linq["Parent_IdMenu"].ToString()),
                                Controller = Linq["Controller"].ToString(),
                                Permiso = int.Parse(Linq["Permiso"].ToString()),
                                IdIsMenu = bool.Parse(Linq["IdIsMenu"].ToString())
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListMenusActionsPrecise({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Menu;
        }
        public async static Task<List<Profiles>> ListProfile(List<Sites> SitiosUserActual, int? IdProfile = null, bool? State = null)
        {
            List<Profiles> InforProfile = new List<Profiles>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (SitiosUserActual != null)
                {
                    foreach (var Item in SitiosUserActual)
                    {
                        var RowA = dtSitios.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Sel_ListProfiles";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                if (IdProfile != null) cmd.Parameters.AddWithValue("IdProfile", IdProfile);
                if (State != null) cmd.Parameters.AddWithValue("State", State);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int IdMasterProfile = int.Parse(dr["IdMasterProfiles"].ToString());
                        List<Profiles> IdPerfil = new List<Profiles>();
                        IdPerfil.Add(new Profiles { IdMasterProfiles = IdMasterProfile });
                        InforProfile.Add(new Profiles
                        {
                            IdMasterProfiles = IdMasterProfile,
                            NameProfile = dr["NameProfile"].ToString(),
                            DescriptionProfile = dr["DescriptionProfile"].ToString(),
                            DateLog = dr["DateLog"].ToString(),
                            State = bool.Parse(dr["State"].ToString()),
                            Sitios = await ListSitiosXProfiles(IdPerfil)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return InforProfile;
        }
        public async static Task SaveProfile(long IdUserGestiona, Profiles Perfil)
        {
            try
            {
                //Agregar sitios a datatable
                DataTable dtSites = new DataTable();
                dtSites.Columns.Add("Id");
                if (Perfil.Sitios != null)
                {
                    foreach (var Item in Perfil.Sitios)
                    {
                        var RowA = dtSites.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSites.Rows.Add(RowA);
                    }
                }
                //Agregar menus y acciones a datatable
                DataTable dtMenu = new DataTable();
                dtMenu.Columns.Add("Id");
                DataTable dtActions = new DataTable();
                dtActions.Columns.Add("Id");
                if (Perfil.Menu != null)
                {
                    foreach (var Item in Perfil.Menu)
                    {
                        if (Item.Level == 1)
                        {
                            var RowM = dtMenu.NewRow();
                            RowM["Id"] = Item.IdMasterMenu;
                            dtMenu.Rows.Add(RowM);
                        }
                        else
                        {
                            var RowA = dtActions.NewRow();
                            RowA["Id"] = Item.IdMasterMenu;
                            dtActions.Rows.Add(RowA);
                        }
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Upd_SaveProfile";
                cmd.Parameters.AddWithValue("IdMasterUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("NameProfile", Perfil.NameProfile);
                cmd.Parameters.AddWithValue("DescriptionProfile", Perfil.DescriptionProfile);
                cmd.Parameters.AddWithValue("dtMenu", dtMenu);
                cmd.Parameters.AddWithValue("dtActions", dtActions);
                cmd.Parameters.AddWithValue("dtSites", dtSites);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Perfiles");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.SaveProfile({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task UpdateProfile(long IdUserGestiona, Profiles Perfil)
        {
            try
            {
                //Agregar sitios a datatable
                DataTable dtSites = new DataTable();
                dtSites.Columns.Add("Id");
                foreach (var Item in Perfil.Sitios)
                {
                    var RowA = dtSites.NewRow();
                    RowA["Id"] = Item.IdMasterSites;
                    dtSites.Rows.Add(RowA);
                }
                //Agregar menus y acciones a datatable
                DataTable dtMenu = new DataTable();
                dtMenu.Columns.Add("Id");
                DataTable dtActions = new DataTable();
                dtActions.Columns.Add("Id");
                if (Perfil.Menu != null)
                {
                    foreach (var Item in Perfil.Menu)
                    {
                        if (Item.Level == 1)
                        {
                            var RowM = dtMenu.NewRow();
                            RowM["Id"] = Item.IdMasterMenu;
                            dtMenu.Rows.Add(RowM);
                        }
                        else
                        {
                            var RowA = dtActions.NewRow();
                            RowA["Id"] = Item.IdMasterMenu;
                            dtActions.Rows.Add(RowA);
                        }
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Upd_SaveProfile";
                cmd.Parameters.AddWithValue("IdProfile", Perfil.IdMasterProfiles);
                cmd.Parameters.AddWithValue("IdMasterUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("NameProfile", Perfil.NameProfile);
                cmd.Parameters.AddWithValue("State", Perfil.State);
                cmd.Parameters.AddWithValue("dtMenu", dtMenu);
                cmd.Parameters.AddWithValue("dtActions", dtActions);
                cmd.Parameters.AddWithValue("dtSites", dtSites);
                if (Perfil.DescriptionProfile != null)
                    cmd.Parameters.AddWithValue("DescriptionProfile", Perfil.DescriptionProfile);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Actualizar", "Perfiles");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.UpdateProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task DeleteProfile(long IdUserGestiona, int IdMasterProfiles)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Upd_DisabledProfile";
                cmd.Parameters.AddWithValue("IdProfile", IdMasterProfiles);
                cmd.Parameters.AddWithValue("IdMasterUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "Perfiles");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.DeleteProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<DataTable> VerifyNameProfile(string NameProfile)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Profiles_Sel_VerifyNameProfile";
                cmd.Parameters.AddWithValue("NameProfile", NameProfile);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.VerifyNameProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        #endregion
        #region GroupsControllers
        public async static Task<List<Groups>> ListGroups(List<Sites> SitiosUserActual, int? IdGroup = null, bool? State = null)
        {
            List<Groups> ListGroups = new List<Groups>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (SitiosUserActual != null)
                {
                    foreach (var Item in SitiosUserActual)
                    {
                        var RowA = dtSitios.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListGroups";
                if (IdGroup != null) cmd.Parameters.AddWithValue("IdGroup", IdGroup);
                if (State != null) cmd.Parameters.AddWithValue("State", State);
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        LineaNegocio Linea = new LineaNegocio();
                        Linea.Id = int.Parse(dr["IdLOB"].ToString());
                        Linea.Nombre = dr["NombreLOB"].ToString();
                        Sites Site = new Sites();
                        Site.IdMasterSites = int.Parse(dr["IdMasterSites"].ToString());
                        Site.NameSite = dr["NameSite"].ToString();
                        ListGroups.Add(new Groups
                        {
                            LOB = Linea,
                            Sitio = Site,
                            IdMasterGroups = int.Parse(dr["IdMasterGroups"].ToString()),
                            NameGroup = dr["NameGroup"].ToString(),
                            DescriptionGroup = dr["DescriptionGroup"].ToString(),
                            ReturnUser = bool.Parse(dr["ReturnUser"].ToString()),
                            DateLog = dr["DateLog"].ToString(),
                            State = bool.Parse(dr["State"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.ListGroups({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListGroups;
        }
        public async static Task<List<Users>> ListUsersXGroup(int IdGroups, bool? PermisoAsignar = null)
        {
            List<Users> Usuarios = new List<Users>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListUsersXGroups";
                cmd.Parameters.AddWithValue("IdGroups", IdGroups);
                if (PermisoAsignar != null) cmd.Parameters.AddWithValue("PermisoAsignar", PermisoAsignar);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Usuarios = (from dr in dt.Rows.Cast<DataRow>()
                                select new Users()
                                {
                                    IdMasterUsers = long.Parse(dr["IdMasterUsers"].ToString()),
                                    Identificacion = dr["Identificacion"].ToString(),
                                    Nombres = dr["Nombres"].ToString(),
                                    PrimerApellido = dr["PrimerApellido"].ToString(),
                                    SegundoApellido = dr["SegundoApellido"].ToString()
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.ListUsersXGroup({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Usuarios;
        }
        public async static Task<List<LineaNegocio>> ListLOB(int? IdLOB = null)
        {
            List<LineaNegocio> ListadoLOB = new List<LineaNegocio>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListLOB";
                if (IdLOB != 0) cmd.Parameters.AddWithValue("IdLOB", IdLOB);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListadoLOB = (from Linq in dt.Rows.Cast<DataRow>()
                                  select new LineaNegocio()
                                  {
                                      Id = int.Parse(Linq["Id"].ToString()),
                                      Nombre = Linq["Nombre"].ToString()
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.ListLOB({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListadoLOB;
        }
        /*TIPOS DE ESCALAMIENTOS*/
        public async static Task<DataTable> VerifyNameTipoEscalamiento(int IdGroups, string TipoEscalamiento)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_TypesScaled";
                cmd.Parameters.AddWithValue("IdGroups", IdGroups);
                cmd.Parameters.AddWithValue("NameTypeScaled", TipoEscalamiento);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.VerifyNameTipoEscalamiento({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task<List<string>> TypesScaledXGroups(int IdGroup)
        {
            List<string> TiposEscalamientos = new List<string>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_TypesScaled";
                cmd.Parameters.AddWithValue("IdGroups", IdGroup);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    TiposEscalamientos = dt.AsEnumerable().Select(LQ => LQ["NameTypeScaled"].ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.TiposEscalamientosXGrupo({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return TiposEscalamientos;
        }
        public async static Task SaveTipoEscalaXGrupo(int IdGroups, string TipoEscalamiento)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO FK_Groups_TypesScaled(IdMasterGroups,NameTypeScaled)
                VALUES(@IdGroups,@TipoEscalamiento);";
                cmd.Parameters.AddWithValue("IdGroups", IdGroups);
                cmd.Parameters.AddWithValue("TipoEscalamiento", TipoEscalamiento);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Grupos - Tipo Escalamiento");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.SaveTipoEscalaXGrupo({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task DeleteTipoEscala(int IdGroups, string TipoEscalamiento)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"DELETE FK_Groups_TypesScaled WHERE IdMasterGroups=@IdGroups AND NameTypeScaled=@TipoEscalamiento;";
                cmd.Parameters.AddWithValue("IdGroups", IdGroups);
                cmd.Parameters.AddWithValue("TipoEscalamiento", TipoEscalamiento);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Eliminar", "Grupos - Tipo Escalamiento");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.DeleteTipoEscala({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        /*END TIPOS DE ESCALAMIENTOS*/
        public async static Task<List<Groups>> GruposAEscalar(int IdGroup)
        {
            List<Groups> ListGrupos = new List<Groups>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListGroups_ScaledGroups";
                cmd.Parameters.AddWithValue("IdGroups", IdGroup);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListGrupos = (from Linq in dt.Rows.Cast<DataRow>()
                                  select new Groups()
                                  {
                                      IdMasterGroups = int.Parse(Linq["IdMasterScaledGroups"].ToString()),
                                      NameGroup = Linq["NameGroup"].ToString()
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.GruposAEscalar({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListGrupos;
        }
        public async static Task SaveGroup(long IdUserGestiona, Groups NewGroup)
        {
            try
            {
                //GRUPOS A ESCALAR
                DataTable dtGroupsEscalar = new DataTable();
                dtGroupsEscalar.Columns.Add("Id");
                if (NewGroup.GruposAEscalar != null)
                {
                    foreach (var Item in NewGroup.GruposAEscalar)
                    {
                        var RowM = dtGroupsEscalar.NewRow();
                        RowM["Id"] = Item.IdMasterGroups;
                        dtGroupsEscalar.Rows.Add(RowM);
                    }
                }
                //Tipos escalamientos
                DataTable dtTiposEscalamientos = new DataTable();
                dtTiposEscalamientos.Columns.Add("Id");
                dtTiposEscalamientos.Columns.Add("Name");
                if (NewGroup.TypesScaled != null)
                {
                    foreach (var Item in NewGroup.TypesScaled)
                    {
                        var RowM = dtTiposEscalamientos.NewRow();
                        RowM["Name"] = Item;
                        dtTiposEscalamientos.Rows.Add(RowM);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Ins_SaveGroups";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (NewGroup.IdMasterGroups != 0)
                    cmd.Parameters.AddWithValue("IdGroups", NewGroup.IdMasterGroups);
                cmd.Parameters.AddWithValue("NameGroup", NewGroup.NameGroup);
                cmd.Parameters.AddWithValue("IdLOB", NewGroup.LOB.Id);
                cmd.Parameters.AddWithValue("IdSite", NewGroup.Sitio.IdMasterSites);
                cmd.Parameters.AddWithValue("DevolverAUsuario", NewGroup.ReturnUser);
                cmd.Parameters.AddWithValue("State", NewGroup.State);
                if (NewGroup.DescriptionGroup != null)
                    cmd.Parameters.AddWithValue("DescriptionGroup", NewGroup.DescriptionGroup);
                if (NewGroup.GruposAEscalar != null)
                    cmd.Parameters.AddWithValue("dtGroupsEscalar", dtGroupsEscalar);
                if (NewGroup.TypesScaled != null)
                    cmd.Parameters.AddWithValue("dtTiposEscalamientos", dtTiposEscalamientos);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Grupos");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.SaveGroup({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<DataTable> VerifyNameGroup(string NameGroup)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdMasterGroups FROM MasterGroups WITH (NOLOCK) WHERE NameGroup=@NameGroup;";
                cmd.Parameters.AddWithValue("NameGroup", NameGroup);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.VerifyNameGroup({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task DeleteGroups(long IdUserGestiona, int IdGroup)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Upd_DeleteGroups";
                cmd.Parameters.AddWithValue("IdGroup", IdGroup);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "Grupos");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.DeleteGroup({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }

        #endregion
        #region UsersControllers
        public async static Task<List<Users>> ListUsersBasic(long? IdMasterUser = null, string Winuser = null)
        {
            List<Users> InforUser = new List<Users>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Users_Sel_ListUsersBasic";
                if (IdMasterUser != null) cmd.Parameters.AddWithValue("IdMasterUser", IdMasterUser);
                if (Winuser != null) cmd.Parameters.AddWithValue("Winuser", Winuser);

                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        InforUser.Add(new Users
                        {
                            IdMasterUsers = long.Parse(dr["IdMasterUsers"].ToString()),
                            PkEmpleado = long.Parse(dr["PkEmpleado"].ToString()),
                            Identificacion = dr["Identificacion"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"].ToString(),
                            Winuser = dr["Winuser"].ToString(),
                            CentroCosto = dr["CentroCosto"].ToString(),
                            EmailCorporativo = dr["EmailCorporativo"].ToString(),
                            State = bool.Parse(dr["State"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>UsersControllers.DAOCommand.ListUsersBasic({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return InforUser;
        }
        public async static Task<List<Groups>> ListGroupsXUsers(long IdMasterUsers)
        {
            List<Groups> GroupsXUsers = new List<Groups>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Users_Sel_ListGroupsXUsers";
                cmd.Parameters.AddWithValue("IdMasterUsers", IdMasterUsers);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GroupsXUsers = (from Linq in dt.Rows.Cast<DataRow>()
                                    select new Groups()
                                    {
                                        IdMasterGroups = int.Parse(Linq["IdMasterGroups"].ToString()),
                                        NameGroup = Linq["NameGroup"].ToString()
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>UsersControllers.DAOCommand.ListGroupsXUsers({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return GroupsXUsers;
        }
        public async static Task<List<Users>> ListUsers(List<Sites> SitiosUserActual, long? IdMasterUser = null, string Winuser = null)
        {
            List<Users> InforUser = new List<Users>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (SitiosUserActual != null)
                {
                    foreach (var Item in SitiosUserActual)
                    {
                        var RowA = dtSitios.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Users_Sel_ListUsers";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                if (IdMasterUser != null) cmd.Parameters.AddWithValue("IdMasterUser", IdMasterUser);
                if (Winuser != null) cmd.Parameters.AddWithValue("Winuser", Winuser);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        long IdUser = long.Parse(dr["IdMasterUsers"].ToString());
                        InforUser.Add(new Users
                        {
                            IdMasterUsers = IdUser,
                            Grupos = await ListGroupsXUsers(IdUser),
                            Perfiles = await ListProfilesXUser(IdUser),
                            PkEmpleado = long.Parse(dr["PkEmpleado"].ToString()),
                            Identificacion = dr["Identificacion"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"].ToString(),
                            Winuser = dr["Winuser"].ToString(),
                            CentroCosto = dr["CentroCosto"].ToString(),
                            EmailCorporativo = dr["EmailCorporativo"].ToString(),
                            State = bool.Parse(dr["State"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>UsersControllers.DAOCommand.ListUsers({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return InforUser;
        }
        public async static Task<List<Users>> ListUsersBaseUnificada()
        {
            List<Users> InforUser = new List<Users>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Users_Sel_ListUsersBaseUnificada";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    InforUser = (from Linq in dt.Rows.Cast<DataRow>()
                                 select new Users()
                                 {
                                     PkEmpleado = long.Parse(Linq["PkEmpleado"].ToString()),
                                     Identificacion = Linq["Identificacion"].ToString(),
                                     Nombres = Linq["Nombres"].ToString(),
                                     PrimerApellido = Linq["PrimerApellido"].ToString(),
                                     SegundoApellido = Linq["SegundoApellido"].ToString(),
                                     Winuser = Linq["Winuser"].ToString(),
                                     CentroCosto = Linq["CentroCosto"].ToString(),
                                     EmailCorporativo = Linq["EmailCorporativo"].ToString()
                                 }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>UsersControllers.DAOCommand.ListUsersBaseUnificada({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return InforUser;
        }
        public async static Task SaveUsers(long IdUserGestiona, List<Users> Usuarios, Users UpdUsers)
        {
            try
            {
                //GRUPOS
                DataTable dtGroups = new DataTable();
                dtGroups.Columns.Add("Id");
                if (UpdUsers.Grupos != null)
                {
                    foreach (var Item in UpdUsers.Grupos)
                    {
                        var RowM = dtGroups.NewRow();
                        RowM["Id"] = Item.IdMasterGroups;
                        dtGroups.Rows.Add(RowM);
                    }
                }
                //Perfiles
                DataTable dtPerfiles = new DataTable();
                dtPerfiles.Columns.Add("Id");
                if (UpdUsers.Perfiles != null)
                {
                    foreach (var Item in UpdUsers.Perfiles)
                    {
                        var RowM = dtPerfiles.NewRow();
                        RowM["Id"] = Item.IdMasterProfiles;
                        dtPerfiles.Rows.Add(RowM);
                    }
                }
                //Usuarios
                DataTable dtUsuarios = new DataTable();
                dtUsuarios.Columns.Add("Id");
                if (Usuarios != null)
                {
                    foreach (var Item in Usuarios)
                    {
                        var RowM = dtUsuarios.NewRow();
                        RowM["Id"] = Item.IdMasterUsers;
                        dtUsuarios.Rows.Add(RowM);
                    }
                }

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Users_Ins_SaveUsers";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (UpdUsers.IdMasterUsers != 0)
                    cmd.Parameters.AddWithValue("IdMasterUsers", UpdUsers.IdMasterUsers);
                cmd.Parameters.AddWithValue("State", UpdUsers.State);
                cmd.Parameters.AddWithValue("dtUsuarios", dtUsuarios);
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                cmd.Parameters.AddWithValue("dtPerfiles", dtPerfiles);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Usuarios");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>UsersControllers.DAOCommand.SaveUsers({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task DeleteUsers(long IdUserGestiona, long IdMasterUser)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Users_Upd_DisabledUser";
                cmd.Parameters.AddWithValue("IdMasterUser", IdMasterUser);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "Usuarios");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>UsersControllers.DAOCommand.DeleteUsers({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region CategoriesControllers
        public async static Task<List<Categories>> ListCategories(List<Sites> SitiosUserActual, int? IdCategoria = null, int? Parent_IdCategory = null, int? Level = null)
        {
            List<Categories> ListCategorias = new List<Categories>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (SitiosUserActual != null)
                {
                    foreach (var Item in SitiosUserActual)
                    {
                        var RowA = dtSitios.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Categories_Sel_ListCategories";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                if (Level != null) cmd.Parameters.AddWithValue("Level", Level);
                if (IdCategoria != null) cmd.Parameters.AddWithValue("IdCategory", IdCategoria);
                if (Parent_IdCategory != null) cmd.Parameters.AddWithValue("Parent_IdCategory", Parent_IdCategory);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListCategorias.Add(new Categories
                        {
                            Template = new Templates
                            {
                                IdTemplates = int.Parse(dr["IdTemplates"].ToString()),
                                NameTemplate = dr["NameTemplate"].ToString()
                            },
                            Sitio = new Sites
                            {
                                IdMasterSites = int.Parse(dr["IdMasterSites"].ToString()),
                                NameSite = dr["NameSite"].ToString()
                            },
                            Grupo = new Groups()
                            {
                                IdMasterGroups = int.Parse(dr["IdMasterGroupsAssigned"].ToString()),
                                NameGroup = dr["NameGroup"].ToString()
                            },
                            IdCategory = int.Parse(dr["IdCategory"].ToString()),
                            Level = int.Parse(dr["Level"].ToString()),
                            Parent_IdCategory = int.Parse(dr["Parent_IdCategory"].ToString()),
                            NameCategory = dr["NameCategory"].ToString(),
                            SLA_HOUR = int.Parse(dr["SLA_HOUR"].ToString()),
                            DescriptionCategory = dr["DescriptionCategory"].ToString(),
                            DateLog = dr["DateLog"].ToString(),
                            State = bool.Parse(dr["State"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>CategoriesControllers.DAOCommand.ListCategories({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListCategorias;
        }
        public async static Task SaveCategoria(long IdUserGestiona, Categories Categoria)
        {
            try
            {
                //Sub categorias
                DataTable dtSubCategory = new DataTable();
                dtSubCategory.Columns.Add("Id");
                dtSubCategory.Columns.Add("Name");
                if (Categoria.SubCategory != null)
                {
                    for (int i = 0; i < Categoria.SubCategory.Count; i++)
                    {
                        var Row = dtSubCategory.NewRow();
                        Row["Id"] = Categoria.SubCategory[i].SLA_HOUR;
                        Row["Name"] = Categoria.SubCategory[i].NameCategory;
                        dtSubCategory.Rows.Add(Row);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Categories_Ins_SaveCategories";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (Categoria.IdCategory != 0)
                    cmd.Parameters.AddWithValue("IdCategory", Categoria.IdCategory);
                cmd.Parameters.AddWithValue("NameCategory", Categoria.NameCategory);
                if (Categoria.DescriptionCategory != null)
                    cmd.Parameters.AddWithValue("DescriptionCategory", Categoria.DescriptionCategory);
                cmd.Parameters.AddWithValue("IdGroups", Categoria.Grupo.IdMasterGroups);
                cmd.Parameters.AddWithValue("IdMasterSites", Categoria.Sitio.IdMasterSites);
                if (Categoria.Template.IdTemplates != 0)
                    cmd.Parameters.AddWithValue("IdTemplates", Categoria.Template.IdTemplates);
                cmd.Parameters.AddWithValue("State", Categoria.State);
                if (dtSubCategory != null)
                    cmd.Parameters.AddWithValue("dtSubCategory", dtSubCategory);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Categoria");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>CategoriesControllers.DAOCommand.SaveCategoria({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<DataTable> VerifyNameCategory(string NameCategory)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdCategory FROM CategoryDefinitions WITH(NOLOCK) WHERE NameCategory=@NameCategory;";
                cmd.Parameters.AddWithValue("NameCategory", NameCategory);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>CategoriesControllers.DAOCommand.VerifyNameCategory({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task<Categories> DeleteCategory(long IdUserGestiona, int IdCategory)
        {
            Categories ReturnCategory = new Categories();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Categories_Upd_DeleteCategory";
                cmd.Parameters.AddWithValue("IdCategory", IdCategory);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "Categoria");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>CategoriesControllers.DAOCommand.DeleteCategory({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ReturnCategory;
        }
        public async static Task UpdateSLASubCategory(long IdUserGestiona, Categories InfoCategory)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Categories_Upd_UpdateSLASubCategory";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdCategory", InfoCategory.IdCategory);
                cmd.Parameters.AddWithValue("SLA_HOUR", InfoCategory.SLA_HOUR);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Update SLA", "Categoria");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>CategoriesControllers.DAOCommand.UpdateSLASubCategory({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region StatusControllers
        public async static Task<List<StatusDefinition>> ListStatusDefinition(List<Sites> SitiosUserActual, int? IdStatus = null, int? Parent_IdStatus = null, int? Level = null, bool? Status = null)
        {
            List<StatusDefinition> ListStatus = new List<StatusDefinition>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (SitiosUserActual != null)
                {
                    foreach (var Item in SitiosUserActual)
                    {
                        var RowA = dtSitios.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Status_Sel_ListStatusDefinitions";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                if (Level != null) cmd.Parameters.AddWithValue("Level", Level);
                if (IdStatus != null) cmd.Parameters.AddWithValue("IdStatus", IdStatus);
                if (Parent_IdStatus != null) cmd.Parameters.AddWithValue("Parent_IdStatus", Parent_IdStatus);
                if (Status != null) cmd.Parameters.AddWithValue("Status", Status);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListStatus = (from dr in dt.Rows.Cast<DataRow>()
                                  select new StatusDefinition()
                                  {
                                      TypeAction = {
                                          IdTypeActions= int.Parse(dr["IdTypeActions"].ToString()),
                                          NameTypeAction= dr["NameTypeAction"].ToString(),
                                          IdTypeRequired=int.Parse(dr["IdTypeRequired"].ToString())
                                      },
                                      Sitio = {
                                            IdMasterSites = int.Parse(dr["IdMasterSites"].ToString()),
                                            NameSite = dr["NameSite"].ToString()
                                      },
                                      IdStatusDefinition = int.Parse(dr["IdStatusDefinition"].ToString()),
                                      Level = int.Parse(dr["Level"].ToString()),
                                      NameStatus = dr["NameStatusDefinition"].ToString(),
                                      Parent_IdStatus = int.Parse(dr["Parent_IdStatus"].ToString()),
                                      DescriptionStatus = dr["DescriptionStatusDefinition"].ToString(),
                                      Universal = bool.Parse(dr["Universal"].ToString()),
                                      DateLog = dr["DateLog"].ToString(),
                                      State = bool.Parse(dr["State"].ToString())
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"StatusControllers.DAOCommand.ListStatusDefinition({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListStatus;
        }
        public async static Task<List<Status_TypesActions>> ListStatus_TypeActions()
        {
            List<Status_TypesActions> ListTypesActions = new List<Status_TypesActions>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdTypeActions,NameTypeAction FROM TypeStatusActions WITH(NOLOCK) where State=1 AND Visible=1;";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListTypesActions = (from Linq in dt.Rows.Cast<DataRow>()
                                        select new Status_TypesActions()
                                        {
                                            IdTypeActions = int.Parse(Linq["IdTypeActions"].ToString()),
                                            NameTypeAction = Linq["NameTypeAction"].ToString()
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"StatusControllers.DAOCommand.ListStatus_TypeActions({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListTypesActions;
        }
        public async static Task<DataTable> VerifyNameStatus(string NameStatus, int? pIdSitio = null)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select IdStatusDefinition from StatusDefinition WITH(NOLOCK) 
                where NameStatusDefinition=@NameStatus 
                and (IdMasterSites = @IdMasterSite or @IdMasterSite is Null);";
                cmd.Parameters.AddWithValue("NameStatus", NameStatus);
                cmd.Parameters.AddWithValue("IdMasterSite", pIdSitio == null ? (object)DBNull.Value : pIdSitio);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"StatusControllers.DAOCommand.VerifyNameStatus({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task DeleteStatus(long IdUserGestiona, int IdStatus)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Status_Upd_DeleteStatus";
                cmd.Parameters.AddWithValue("IdStatus", IdStatus);
                cmd.Parameters.AddWithValue("IdMasterUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "StatusDefinition");
            }
            catch (Exception ex)
            {
                string Error = $"StatusControllers.DAOCommand.DeleteStatusDefinition({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<string> SaveStatusDefinition(long IdUserGestiona, StatusDefinition InfoStatus)
        {
            try
            {
                //Sub Status
                DataTable dtSubStatus = new DataTable();
                dtSubStatus.Columns.Add("Id");
                dtSubStatus.Columns.Add("Name");
                if (InfoStatus.SubStatus != null)
                {
                    for (int i = 0; i < InfoStatus.SubStatus.Count; i++)
                    {
                        var Row = dtSubStatus.NewRow();
                        Row["Name"] = InfoStatus.SubStatus[i].NameStatus;
                        dtSubStatus.Rows.Add(Row);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Status_Ins_SaveStatusDefinitions";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (InfoStatus.IdStatusDefinition != 0)
                    cmd.Parameters.AddWithValue("IdStatus", InfoStatus.IdStatusDefinition);
                if (InfoStatus.DescriptionStatus != null)
                    cmd.Parameters.AddWithValue("DescriptionStatus", InfoStatus.DescriptionStatus);
                cmd.Parameters.AddWithValue("NameStatus", InfoStatus.NameStatus);
                cmd.Parameters.AddWithValue("IdMasterSites", InfoStatus.Sitio.IdMasterSites);
                cmd.Parameters.AddWithValue("IdTypeActions", InfoStatus.TypeAction.IdTypeActions);
                cmd.Parameters.AddWithValue("State", InfoStatus.State);
                if (dtSubStatus != null)
                    cmd.Parameters.AddWithValue("dtSubStatus", dtSubStatus);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt.Rows.Count > 0)
                {
                    return "ERROR: Ya existe un estado con este nombre y este sitio, verifique";
                }
                else
                {
                    Tools.LogAplications("Guardar", "Status");
                }

            }
            catch (Exception ex)
            {
                string Error = $"StatusControllers.DAOCommand.SaveStatusDefinition({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return "";
        }
        #endregion
        #region TemplatesControllers
        public async static Task<FieldsUDF> SaveNewFieldsUDF(long IdUserGestiona, FieldsUDF Fields)
        {
            FieldsUDF Field = new FieldsUDF();
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            try
            {
                //string NameTransaction = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //cmd.Connection.Open();
                //cmd.Transaction = cmd.Connection.BeginTransaction("NewFields" + NameTransaction);
                Field = await SaveTransFieldsUDF(cmd, Fields, IdUserGestiona);
                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.SaveNewFieldsUDF({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
            return Field;
        }
        public async static Task UpdateTemplates(long IdUserGestiona, Templates Template)
        {
            try
            {
                //Se envian los perfiles al sp
                DataTable dtFieldsUDF = new DataTable();
                dtFieldsUDF.Columns.Add("Id");
                dtFieldsUDF.Columns.Add("Id2");
                if (Template.ListFieldsUDF != null)
                {
                    foreach (var Item in Template.ListFieldsUDF)
                    {
                        var RowA = dtFieldsUDF.NewRow();
                        RowA["Id"] = Item.IdFieldsUDF;
                        RowA["Id2"] = Item.Position;
                        dtFieldsUDF.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Upd_UpdateTemplates";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdTemplates", Template.IdTemplates);
                cmd.Parameters.AddWithValue("NameTemplate", Template.NameTemplate);
                if (Template.DescriptionTemplate != null)
                    cmd.Parameters.AddWithValue("DescriptionTemplate", Template.DescriptionTemplate);
                cmd.Parameters.AddWithValue("IdMasterSites", Template.Sitio.IdMasterSites);
                cmd.Parameters.AddWithValue("dtFieldsUDF", dtFieldsUDF);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.UpdateTemplates({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task UpdateFieldsUDF(long IdUserGestiona, FieldsUDF Fields)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Upd_UpdateFieldsUDF";
                cmd.Parameters.AddWithValue("IdFieldsUDF", Fields.IdFieldsUDF);
                if (Fields.ParentDispositions.Parent_IdDispositions != 0)
                    cmd.Parameters.AddWithValue("Parent_IdDispositions", Fields.ParentDispositions.Parent_IdDispositions);
                cmd.Parameters.AddWithValue("NameField", Fields.NameField);
                cmd.Parameters.AddWithValue("IdTypeRequired", Fields.TypeRequired.IdTypeRequired);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                /*----------GUARDAR DISPOSITIONS----------*/
                if (Fields.FieldType.IdFieldsTypesUDF == 6)
                {
                    foreach (var Disp in Fields.Dispositions)
                    {
                        Disp.IdFieldsUDF = Fields.IdFieldsUDF;
                        await SaveDispositions(IdUserGestiona, Disp);
                    }
                }
                /*----------END GUARDAR DISPOSITIONS----------*/
                Tools.LogAplications("Update", "FieldsUDF");
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.UpdateFieldsUDF({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task SaveDispositions(long IdUserGestiona, FieldsDispositions Disposition)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Ins_SaveFieldsDispositions";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (Disposition.IdFieldsUDF != 0)
                    cmd.Parameters.AddWithValue("IdFieldsUDF", Disposition.IdFieldsUDF);
                cmd.Parameters.AddWithValue("Dispositions", Disposition.Dispositions);
                if (Disposition.Parent_IdDispositions != 0)
                    cmd.Parameters.AddWithValue("Parent_IdDispositions", Disposition.Parent_IdDispositions);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.SaveDispositions({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task SaveTransDispositions(SqlCommand cmd, FieldsDispositions Disposition, long IdUserGestiona)
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Ins_SaveFieldsDispositions";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (Disposition.IdFieldsUDF != 0)
                    cmd.Parameters.AddWithValue("IdFieldsUDF", Disposition.IdFieldsUDF);
                cmd.Parameters.AddWithValue("Dispositions", Disposition.Dispositions);
                if (Disposition.Parent_IdDispositions != 0)
                    cmd.Parameters.AddWithValue("Parent_IdDispositions", Disposition.Parent_IdDispositions);
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.SaveTransDispositions({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<FieldsUDF> SaveTransFieldsUDF(SqlCommand cmd, FieldsUDF Fields, long IdUserGestiona)
        {
            FieldsUDF ObjFields = new FieldsUDF();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Ins_SaveFieldsUDF";
                cmd.Parameters.Clear();
                if (Fields.Template.IdTemplates != 0) cmd.Parameters.AddWithValue("IdTemplates", Fields.Template.IdTemplates);
                cmd.Parameters.AddWithValue("IdFieldsTypesUDF", Fields.FieldType.IdFieldsTypesUDF);
                cmd.Parameters.AddWithValue("IdTypeRequired", Fields.TypeRequired.IdTypeRequired);
                if (Fields.Longitud != 0) cmd.Parameters.AddWithValue("Longitud", Fields.Longitud);
                if (Fields.ParentDispositions.Parent_IdDispositions != 0)
                    cmd.Parameters.AddWithValue("Parent_IdDispositions", Fields.ParentDispositions.Parent_IdDispositions);
                cmd.Parameters.AddWithValue("NameField", Fields.NameField);
                cmd.Parameters.AddWithValue("SolutionField", Fields.SolutionField);
                cmd.Parameters.AddWithValue("Position", Fields.Position);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                DataTable dtFields = await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                try
                {
                    if (dtFields != null && dtFields.Rows.Count > 0)
                    {
                        ObjFields = (from dr in dtFields.Rows.Cast<DataRow>()
                                     select new FieldsUDF()
                                     {
                                         Dispositions = Fields.Dispositions,
                                         IdFieldsUDF = int.Parse(dr["IdFieldsUDF"].ToString()),
                                         ParentDispositions = {
                                        Parent_IdDispositions=int.Parse(dr["Parent_IdDispositions"].ToString())
                                    },
                                         Template = {
                                        IdTemplates=int.Parse(dr["IdTemplates"].ToString())
                                    },
                                         FieldType = {
                                        IdFieldsTypesUDF = int.Parse(dr["IdFieldsTypesUDF"].ToString()),
                                        TypeDataSQL = dr["TypeDataSQL"].ToString()
                                    },
                                         Longitud = int.Parse(dr["Longitud"].ToString()),
                                         NameFieldsUDF = dr["NameFieldsUDF"].ToString(),
                                         NameField = dr["NameField"].ToString(),
                                         SolutionField = bool.Parse(dr["SolutionField"].ToString()),
                                         Position = int.Parse(dr["Position"].ToString()),
                                         State = bool.Parse(dr["State"].ToString())
                                     }).Single();
                    }
                }
                catch (Exception)
                {
                    cmd.Transaction.Rollback();
                    throw;
                }

                /*----------GUARDAR DISPOSITIONS----------*/
                if (ObjFields.FieldType.IdFieldsTypesUDF == 6)
                {
                    foreach (var Disp in ObjFields.Dispositions)
                    {
                        Disp.IdFieldsUDF = ObjFields.IdFieldsUDF;
                        await SaveTransDispositions(cmd, Disp, IdUserGestiona);
                    }
                }
                /*----------END GUARDAR DISPOSITIONS----------*/

                /*----------SCRIPT ALTER TABLE----------*/
                string NameTableSQL = "WorkOrder_Fields";
                if (ObjFields.SolutionField == true) NameTableSQL = "WorkOrder_Solutions_Fields";
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 600000;
                cmd.CommandText = $@"ALTER TABLE {NameTableSQL} ADD {ObjFields.NameFieldsUDF} {ObjFields.FieldType.TypeDataSQL.Replace("#", ObjFields.Longitud.ToString())} sparse;";
                cmd.Parameters.Clear();
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                /*----------END ALTER TABLES----------*/
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.SaveTransFieldsUDF({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ObjFields;
        }
        public async static Task SaveTemplateFull(long IdUserGestiona, Templates Template)
        {
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            //string NameTransaction = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
            //cmd.Connection.Open();
            //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
            bool Finaliza = false;
            try
            {
                if (Template.ListFieldsUDF.Count == 0) Finaliza = true;
                //cmd.Transaction = cmd.Connection.BeginTransaction("SaveTemplate" + NameTransaction);
                /*----------GUARDAR INFORMACION DE PLANTILLA----------*/
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Ins_SaveTemplate";
                cmd.Parameters.AddWithValue("NameTemplate", Template.NameTemplate);
                if (Template.DescriptionTemplate != null)
                    cmd.Parameters.AddWithValue("DescriptionTemplate", Template.DescriptionTemplate);
                cmd.Parameters.AddWithValue("IdMasterSites", Template.Sitio.IdMasterSites);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                DataTable dtTemplate = await DAOConfig.MultiSetDataTableExecuteCommand(cmd, Finaliza);
                /*----------END GUARDAR INFORMACION DE PLANTILLA----------*/
                if (Template.ListFieldsUDF.Count > 0)
                {
                    int IdTemplate = 0;
                    try
                    {
                        if (dtTemplate != null && dtTemplate.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtTemplate.Rows) IdTemplate = int.Parse(dr["IdTemplates"].ToString());
                        }
                    }
                    catch (Exception)
                    {
                        cmd.Transaction.Rollback();
                        throw;
                    }
                    /*----------GUARDAR LOS CAMPOS----------*/
                    foreach (var Fields in Template.ListFieldsUDF)
                    {
                        Fields.Template.IdTemplates = IdTemplate;
                        FieldsUDF ObjFields = await SaveTransFieldsUDF(cmd, Fields, IdUserGestiona);
                    }
                    /*----------END GUARDAR LOS CAMPOS----------*/
                    cmd.Transaction.Commit();
                }
                //Tools.LogAplications("Guardar", "Status");
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.SaveTemplateFull({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
        }
        public async static Task EnabledDisabledTemplate(long IdUserGestiona, int IdTemplate, bool State)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Upd_EnabledDisabledTemplate";
                cmd.Parameters.AddWithValue("IdTemplate", IdTemplate);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("State", State);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications((State == true ? "Activar" : "Desactivar"), "Template");
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.EnabledDisabledTemplate({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task EnabledDisabledFieldsUDF(long IdUserGestiona, int IdFieldsUDF, bool State)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Upd_EnabledDisabledFieldsUDF";
                cmd.Parameters.AddWithValue("IdFieldsUDF", IdFieldsUDF);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("State", State);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications((State == true ? "Activar" : "Desactivar"), "FieldsUDF");
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.EnabledDisabledFieldsUDF({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task<List<Templates>> ListTemplates(int? IdTemplates = null, bool? State = null, List<Sites> ListSites = null)
        {
            List<Templates> ListTemplates = new List<Templates>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (ListSites != null)
                {
                    foreach (var Item in ListSites)
                    {
                        var RowA = dtSitios.NewRow();
                        RowA["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(RowA);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Sel_ListTemplates";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                if (ListSites != null) cmd.Parameters.AddWithValue("ListIdSites", string.Join(",", ListSites.Select(lq => lq.IdMasterSites).ToList()));
                if (IdTemplates != null) cmd.Parameters.AddWithValue("IdTemplates", IdTemplates);
                if (State != null) cmd.Parameters.AddWithValue("State", State);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Sites Site = new Sites();
                        Site.IdMasterSites = int.Parse(dr["IdMasterSites"].ToString());
                        Site.NameSite = dr["NameSite"].ToString();
                        ListTemplates.Add(new Templates
                        {
                            Sitio = Site,
                            IdTemplates = int.Parse(dr["IdTemplates"].ToString()),
                            NameTemplate = dr["NameTemplate"].ToString(),
                            DescriptionTemplate = dr["DescriptionTemplate"].ToString(),
                            DateLog = dr["DateLog"].ToString(),
                            State = bool.Parse(dr["State"].ToString())
                        }); ;
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.ListTemplates({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListTemplates;
        }
        public async static Task<List<FieldsUDF>> ListFieldsUDF(FieldsUDF objFieldsUDF, bool? SolutionField = null, bool? State = null, bool? FieldTypeFull = null, bool? Universal = null)
        {
            List<FieldsUDF> ListFieldsUDF = new List<FieldsUDF>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Sel_ListFieldsUDF";
                if (objFieldsUDF.IdFieldsUDF != 0)
                    cmd.Parameters.AddWithValue("IdFieldsUDF", objFieldsUDF.IdFieldsUDF);
                if (objFieldsUDF.Template.IdTemplates != 0)
                    cmd.Parameters.AddWithValue("IdTemplates", objFieldsUDF.Template.IdTemplates);
                if (objFieldsUDF.FieldType.IdFieldsTypesUDF != 0)
                    cmd.Parameters.AddWithValue("IdFieldsTypesUDF", objFieldsUDF.FieldType.IdFieldsTypesUDF);
                if (SolutionField != null)
                    cmd.Parameters.AddWithValue("SolutionField", SolutionField);
                if (State != null)
                    cmd.Parameters.AddWithValue("State", State);
                if (Universal != null)
                    cmd.Parameters.AddWithValue("Universal", Universal);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int IdFieldUDF = int.Parse(dr["IdFieldsUDF"].ToString());
                        int IdFieldTypeUDF = int.Parse(dr["IdFieldsTypesUDF"].ToString());
                        int IdParentFieldsUDF = int.Parse(dr["Parent_IdFieldsUDF"].ToString());
                        List<FieldsDispositions> ListDisposition = new List<FieldsDispositions>();
                        if (IdFieldTypeUDF == 6 & IdParentFieldsUDF == 0) //Lista deplegable
                        {
                            ListDisposition = await ListDispositions(IdFieldUDF);
                        }
                        FieldsTypesUDF SingleFieldsTypes = new FieldsTypesUDF();
                        if (FieldTypeFull == true)
                        {
                            List<FieldsTypesUDF> ObjFieldsTypes = await ListFieldsTypesUDF(IdFieldTypeUDF, true);
                            SingleFieldsTypes = ObjFieldsTypes[0];
                        }
                        else
                        {
                            SingleFieldsTypes.IdFieldsTypesUDF = IdFieldTypeUDF;
                        }
                        ListFieldsUDF.Add(new FieldsUDF
                        {
                            Dispositions = ListDisposition,
                            IdFieldsUDF = IdFieldUDF,
                            NameFieldsUDF = dr["NameFieldsUDF"].ToString(),
                            NameField = dr["NameField"].ToString(),
                            Longitud = int.Parse(dr["Longitud"].ToString()),
                            SolutionField = bool.Parse(dr["SolutionField"].ToString()),
                            Position = int.Parse(dr["Position"].ToString()),
                            Template = {
                                IdTemplates = int.Parse(dr["IdTemplates"].ToString())
                            },
                            FieldType = SingleFieldsTypes,
                            TypeRequired = {
                                IdTypeRequired = int.Parse(dr["IdTypeRequired"].ToString())
                            },
                            ParentDispositions = {
                                IdFieldsUDF = int.Parse(dr["Parent_IdFieldsUDF"].ToString()),
                                Parent_IdDispositions=long.Parse(dr["Parent_IdDispositions"].ToString())
                            },
                            DateLog = dr["DateLog"].ToString(),
                            State = bool.Parse(dr["State"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.ListFieldsUDF({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListFieldsUDF;
        }
        public async static Task<List<FieldsDispositions>> ListDispositions(int? IdFieldsUDF = null, long? ParentIdDisposi = null, long? IdFieldsDispositions = null)
        {
            List<FieldsDispositions> ListDisp = new List<FieldsDispositions>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Sel_ListFieldsDispositions";
                if (IdFieldsUDF != null)
                    cmd.Parameters.AddWithValue("IdFieldsUDF", IdFieldsUDF);
                if (ParentIdDisposi != null)
                    cmd.Parameters.AddWithValue("ParentIdDisposi", ParentIdDisposi);
                if (IdFieldsDispositions != null)
                    cmd.Parameters.AddWithValue("IdFieldsDispositions", IdFieldsDispositions);

                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListDisp = (from Linq in dt.Rows.Cast<DataRow>()
                                select new FieldsDispositions()
                                {
                                    IdFieldsDispositions = long.Parse(Linq["IdFieldsDispositions"].ToString()),
                                    IdFieldsUDF = int.Parse(Linq["IdFieldsUDF"].ToString()),
                                    Dispositions = Linq["Dispositions"].ToString(),
                                    Parent_IdDispositions = long.Parse(Linq["Parent_IdDispositions"].ToString())
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.ListDispositions({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListDisp;
        }
        public async static Task<DataTable> VerifyNameTemplate(string NameTemplate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdTemplates from Templates WITH (NOLOCK) where NameTemplate=@NameTemplate;";
                cmd.Parameters.AddWithValue("NameTemplate", NameTemplate);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.VerifyNameTemplate({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task<List<FieldsTypesUDF>> ListFieldsTypesUDF(int? IdFieldsTypes = null, bool StateTrue = false)
        {
            List<FieldsTypesUDF> ListFieldsTypesUDF = new List<FieldsTypesUDF>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Sel_ListFieldsTypesUDF";
                if (IdFieldsTypes != null) cmd.Parameters.AddWithValue("IdFieldsTypes", IdFieldsTypes);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListFieldsTypesUDF = (from Linq in dt.Rows.Cast<DataRow>()
                                          select new FieldsTypesUDF()
                                          {
                                              IdFieldsTypesUDF = int.Parse(Linq["IdFieldsTypesUDF"].ToString()),
                                              TypeDataSQL = Linq["TypeDataSQL"].ToString(),
                                              NameUDF = Linq["NameUDF"].ToString(),
                                              NameTypeFieldsShort = Linq["NameTypeFieldsShort"].ToString(),
                                              NameTypeFields = Linq["NameTypeFields"].ToString(),
                                              IconFields = Linq["IconFields"].ToString(),
                                              State = bool.Parse(Linq["State"].ToString())
                                          }).ToList();

                    if (StateTrue != false)
                        ListFieldsTypesUDF = ListFieldsTypesUDF.Where(lq => lq.State == StateTrue).ToList();

                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>TemplatesControllers.DAOCommand.ListFieldsTypesUDF({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(() => Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListFieldsTypesUDF;
        }
        public async static Task<List<TypesRequired>> ListTypesRequired(int? IdTypeRequired = null, bool? TemplateSolutions = null)
        {
            List<TypesRequired> ListTypeRequired = new List<TypesRequired>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Sel_ListTypesRequired";
                if (IdTypeRequired != null) cmd.Parameters.AddWithValue("IdTypeRequired", IdTypeRequired);
                if (TemplateSolutions != null) cmd.Parameters.AddWithValue("TemplateSolutions", TemplateSolutions);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListTypeRequired = (from Linq in dt.Rows.Cast<DataRow>()
                                        select new TypesRequired()
                                        {
                                            IdTypeRequired = int.Parse(Linq["IdTypeRequired"].ToString()),
                                            NameTypeRequired = Linq["NameTypeRequired"].ToString(),
                                            TemplateSolutions = bool.Parse(Linq["TemplateSolutions"].ToString())
                                        }).ToList();

                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>TemplatesControllers.DAOCommand.ListTypesRequired({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(() => Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListTypeRequired;
        }
        public async static Task DisabledDispositions(long IdUserGestiona, int IdDispositions)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Templates_Upd_DisabledDispositions";
                cmd.Parameters.AddWithValue("IdDispositions", IdDispositions);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Desactivar", "StatusDefinition");
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.DisabledDispositions({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region ImportWorkOrderControllers
        public async static Task<DataTable> VerifyNameDataExcel(string NameDataExcel)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdDataImported FROM WorkOrder_DataImported WITH(NOLOCK) Where NameData=@NameData;";
                cmd.Parameters.AddWithValue("NameData", NameDataExcel);
                dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"ImportWorkOrder.DAOCommand.VerifyNameDataExcel({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }
        public async static Task<WorkOrder_DataImported> UploadFileServer(HttpPostedFileBase fileID, string path)
        {
            WorkOrder_DataImported Excel = new WorkOrder_DataImported();
            try
            {
                Excel.NameData = fileID.FileName;
                Excel.NameDataEncrypted = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                Excel.Extension = Path.GetExtension(fileID.FileName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string filePath = path + Path.GetFileName(Excel.NameDataEncrypted + Excel.Extension);
                fileID.SaveAs(filePath);
                //Crear conexion Excel
                string conStringExcel = (Excel.Extension == ".xlsx" ? FormatsExcel.XLSX : FormatsExcel.XLS);
                conStringExcel = string.Format(conStringExcel, filePath);
                Excel.ConexString = conStringExcel;
                OleDbCommand cmdExcel = await DAOConfig.OleCommandExcel(conStringExcel);
                DataTable dt = cmdExcel.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Excel.NameHojas = dt.AsEnumerable().Select(Linq => Linq.Field<string>("TABLE_NAME")).ToList();
                }
                else
                {
                    Excel.returnError = "El archivo no cuenta con hojas, favor verificar.";
                }
                Tools.SessionSetObject("InforExcel", Excel);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not registered on the local machine"))
                {
                    Excel.returnError = "Utilize una version mas reciente de excel, para cargar esta data.";
                }
                else
                {
                    Excel.returnError = ex.Message;
                    string Error = $"ImportWorkOrder.DAOCommand.UploadFileServer({Tools.GetLineErr(ex)}): {ex.Message}";
                    Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                    { IsBackground = true };
                    hilo.Start();
                }
            }
            return Excel;
        }
        public async static Task<CasosPqrMovilEscrita> UploadFileServerPQR(HttpPostedFileBase fileID, string path)
        {
            CasosPqrMovilEscrita Excel = new CasosPqrMovilEscrita();
            try
            {
                Excel.NameData = fileID.FileName;
                Excel.NameDataEncrypted = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                Excel.Extension = Path.GetExtension(fileID.FileName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string filePath = path + Path.GetFileName(Excel.NameDataEncrypted + Excel.Extension);
                fileID.SaveAs(filePath);
                //Crear conexion Excel
                string conStringExcel = (Excel.Extension == ".xlsx" ? FormatsExcel.XLSX : FormatsExcel.XLS);
                conStringExcel = string.Format(conStringExcel, filePath);
                Excel.ConexString = conStringExcel;
                OleDbCommand cmdExcel = await DAOConfig.OleCommandExcel(conStringExcel);
                DataTable dt = cmdExcel.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Excel.NameHojas = dt.AsEnumerable().Select(Linq => Linq.Field<string>("TABLE_NAME")).ToList();
                }
                else
                {
                    Excel.returnError = "El archivo no cuenta con hojas, favor verificar.";
                }
                Tools.SessionSetObject("InforExcel", Excel);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not registered on the local machine"))
                {
                    Excel.returnError = "Utilize una version mas reciente de excel, para cargar esta data.";
                }
                else
                {
                    Excel.returnError = ex.Message;
                    string Error = $"ImportWorkOrder.DAOCommand.UploadFileServer({Tools.GetLineErr(ex)}): {ex.Message}";
                    Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                    { IsBackground = true };
                    hilo.Start();
                }
            }
            return Excel;
        }
        public async static Task<string> ArmarColumnSQL(FieldsUDF Field)
        {
            string Columna = $"{Field.NameFieldsUDF} {Field.FieldType.TypeDataSQL.Replace("#", Field.Longitud.ToString())}";
            if (Field.TypeRequired.IdTypeRequired == 2) Columna += " NOT NULL";
            return Columna;
        }
        public async static Task<bool> SqlBulkXColumn(ImportWorkOrder Import, WorkOrder_DataImported InforExcel)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                List<FieldsUDF> Field = await ListFieldsUDF(Import.FieldUDF, false, true, true);
                string Columna = await ArmarColumnSQL(Field[0]);
                /*----------CREA TABLA TEMPORAL-----------*/
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"CREATE TABLE #WorkOrder_Fields_Temp({Columna});";
                await DAOConfig.MultiGetDataTableExecuteCommand(cmd);
                /*----------END CREA TABLA TEMPORAL-----------*/
                /*----------SE INSERTAN DATOS TEMPORALES----------*/
                SqlBulkCopy SqlBulk = new SqlBulkCopy(cmd.Connection);
                SqlBulk.DestinationTableName = "#WorkOrder_Fields_Temp";
                cmd.CommandTimeout = 500000;
                for (int i = 0; i < Import.ColumnsSQL.Count; i++)
                {
                    if (Import.ColumnsExcel[i] != "")
                    {
                        SqlBulk.ColumnMappings.Add(Import.ColumnsExcel[i].Trim(), Import.ColumnsSQL[i]);
                    }
                }
                //EXCEL
                DataTable dtExcel = await DataTableExcel(InforExcel.ConexString, InforExcel.HojaSelected);
                SqlBulk.WriteToServer(dtExcel);
                /*----------END SE INSERTAN DATOS TEMPORALES----------*/
            }
            catch (Exception ex)
            {
                string MsjErrorCliente = "";
                if (ex.Message.Contains("bigint"))
                {
                    MsjErrorCliente = "Esta columna solo admite valores numericos enteros, favor verifique.";
                }
                else if (ex.Message.Contains("datetime"))
                {
                    MsjErrorCliente = "Esta columna solo admite valores tipo fecha, favor verifique.";
                }
                else if (ex.Message.Contains("DBNull.Value"))
                {
                    MsjErrorCliente = "Esta columna no admite valores vacios, favor verifique.";
                }
                else
                {
                    MsjErrorCliente = ex.Message;
                }
                throw new Exception(MsjErrorCliente);
            }
            return true;
        }
        public async static Task SqlBulkFullTable(SqlCommand cmd, ImportWorkOrder Import)
        {
            try
            {
                //VARIABLE SESSION INFOR EXCEL
                WorkOrder_DataImported InforExcel = await Tools.SessionGetObject<WorkOrder_DataImported>("InforExcel");
                /*----------CREA TABLA TEMPORAL-----------*/
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = InforExcel.SQLTableTemp;
                cmd.Parameters.Clear();
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                /*----------END CREA TABLA TEMPORAL-----------*/
                /*----------SE INSERTAN DATOS TEMPORALES----------*/
                SqlBulkCopy SqlBulk = new SqlBulkCopy(cmd.Connection, SqlBulkCopyOptions.Default, cmd.Transaction);
                SqlBulk.DestinationTableName = "#WorkOrder_Fields_Temp";
                cmd.Parameters.Clear();
                cmd.CommandTimeout = 500000;
                for (int i = 0; i < Import.ColumnsSQL.Count; i++)
                {
                    if (Import.ColumnsExcel[i].Trim() != "")
                    {
                        SqlBulk.ColumnMappings.Add(Import.ColumnsExcel[i].Trim(), Import.ColumnsSQL[i]);
                    }
                }
                //EXCEL
                DataTable dtExcel = await DataTableExcel(InforExcel.ConexString, InforExcel.HojaSelected);
                SqlBulk.WriteToServer(dtExcel);
                /*----------END SE INSERTAN DATOS TEMPORALES----------*/
            }
            catch (Exception ex)
            {
                string Error = $"ImportWorkOrderControllers.DAOCommand.SqlBulkFullTable({Tools.GetLineErr(ex)}): {ex.Message}";
                throw new Exception(Error);
            }
        }
        public async static Task SqlBulkFullTablePQR(SqlCommand cmd, CasosPqrMovilEscrita Import)
        {
            try
            {
                //VARIABLE SESSION INFOR EXCEL
                CasosPqrMovilEscrita InforExcel = await Tools.SessionGetObjectPQR<CasosPqrMovilEscrita>("InforExcel");
                /*----------CREA TABLA TEMPORAL-----------*/
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = InforExcel.SQLTableTemp;
                cmd.Parameters.Clear();
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                /*----------END CREA TABLA TEMPORAL-----------*/
                /*----------SE INSERTAN DATOS TEMPORALES----------*/
                SqlBulkCopy SqlBulk = new SqlBulkCopy(cmd.Connection, SqlBulkCopyOptions.Default, cmd.Transaction);
                SqlBulk.DestinationTableName = "#WorkOrder_Fields_Temp";
                cmd.Parameters.Clear();
                cmd.CommandTimeout = 500000;
                for (int i = 0; i < Import.ColumnsSQL.Count; i++)
                {
                    if (Import.ColumnsExcel[i].Trim() != "")
                    {
                        SqlBulk.ColumnMappings.Add(Import.ColumnsExcel[i].Trim(), Import.ColumnsSQL[i]);
                    }
                }
                //EXCEL
                DataTable dtExcel = await DataTableExcel(InforExcel.ConexString, InforExcel.HojaSelected);
                SqlBulk.WriteToServer(dtExcel);
                /*----------END SE INSERTAN DATOS TEMPORALES----------*/
            }
            catch (Exception ex)
            {
                string Error = $"ImportWorkOrderControllers.DAOCommand.SqlBulkFullTable({Tools.GetLineErr(ex)}): {ex.Message}";
                throw new Exception(Error);
            }
        }
        public async static Task FinalizeImport(long IdUserGestiona, ImportWorkOrder Import)
        {
            //string NameTransaction = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
            //cmd.Connection.Open();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                //cmd.Transaction = cmd.Connection.BeginTransaction("FinalizeImport" + NameTransaction);
                //VARIABLE SESSION INFOR EXCEL
                WorkOrder_DataImported InforExcel = await Tools.SessionGetObject<WorkOrder_DataImported>("InforExcel");
                await SqlBulkFullTable(cmd, Import);

                List<AlgorithmsAssignment> Algorithm = new List<AlgorithmsAssignment>();

                if (Import.Plantilla.IdTemplates == 23)
                {
                    int idAlgoritmo = 0;
                    idAlgoritmo = (Import.Algorithms.IdAlgorithmsAssignment == 6 ? 10 : Import.Algorithms.IdAlgorithmsAssignment);
                    if (idAlgoritmo == 0)
                    {
                        idAlgoritmo = (Import.Algorithms.IdAlgorithmsAssignment == 4 ? 8 : Import.Algorithms.IdAlgorithmsAssignment);
                    }
                    if (idAlgoritmo == 0)
                    {
                        idAlgoritmo = (Import.Algorithms.IdAlgorithmsAssignment == 5 ? 9 : Import.Algorithms.IdAlgorithmsAssignment);
                    }

                    //Se consulta el algoritmo seleccionado
                    Algorithm = await ListAlgorithms(null, idAlgoritmo, true);
                }
                else
                {
                    //Se consulta el algoritmo seleccionado
                    Algorithm = await ListAlgorithms(null, Import.Algorithms.IdAlgorithmsAssignment, true);
                }
                string ColumnSQL = string.Join(",", Import.ColumnsSQL.ToArray());
                string SQLAlgoritmo = Algorithm[0].SQLStringAlgorithm;
                string ListUsers = string.Join(",", Import.ListUsers.Select(lq => lq.IdMasterUsers).ToArray());
                SQLAlgoritmo = SQLAlgoritmo.Replace("{ListIdUsers}", Import.ListUsers.Count > 0 ? $"AND U.IdMasterUsers IN ({ListUsers})" : "");
                SQLAlgoritmo = SQLAlgoritmo.Replace("{ColumnSQL}", ColumnSQL);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SQLAlgoritmo;
                cmd.CommandTimeout = 500000;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdTemplates", Import.Plantilla.IdTemplates);
                cmd.Parameters.AddWithValue("IdUsersGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdMasterGroups", Import.Grupo.IdMasterGroups);
                cmd.Parameters.AddWithValue("IdAlgorithm", Import.Algorithms.IdAlgorithmsAssignment);
                cmd.Parameters.AddWithValue("NameData", InforExcel.NameData);
                cmd.Parameters.AddWithValue("NameDataEncrypted", InforExcel.NameDataEncrypted);
                cmd.Parameters.AddWithValue("Extension", InforExcel.Extension);
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, true);
            }
            catch (Exception ex)
            {
                string Error = $"ImportWorkOrderControllers.DAOCommand.FinalizeImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
        }
        #endregion
        #region CreateWorkOrderControllers
        public async static Task<Users> UserMenosTicketsAsignados(int IdGroups)
        {
            Users User = new Users();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_NumTicketsXUsers";
                cmd.Parameters.AddWithValue("IdGroups", IdGroups);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    User = (from Linq in dt.Rows.Cast<DataRow>()
                            select new Users()
                            {
                                IdMasterUsers = int.Parse(Linq["IdMasterUsers"].ToString())
                            }).Single();
                }
            }
            catch (Exception ex)
            {
                string Error = $"CreateWorkOrderControllers.DAOCommand.UserMenosTicketsAsignados({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return User;
        }
        public async static Task<WorkOrder> SaveCrearTicket(long IdUserGestiona, WorkOrder Ticket)
        {
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            try
            {
                //string NameTransaction = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //cmd.Connection.Open();
                //cmd.Transaction = cmd.Connection.BeginTransaction("FinalizeImport" + NameTransaction);

                Users UserActual = await InforUserActual(true, true);
                var Categorias = await ListCategories(UserActual.Sitios, Ticket.SubCategory.IdCategory);
                int IdGroupsAsign = Categorias[0].Grupo.IdMasterGroups;
                int SLA_HOUR = Categorias[0].SLA_HOUR;
                Users UsersAssigned = await UserMenosTicketsAsignados(IdGroupsAsign);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_SaveCrearTicket";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdCategory", Ticket.Category.IdCategory);
                cmd.Parameters.AddWithValue("IdSubCategory", Ticket.SubCategory.IdCategory);
                cmd.Parameters.AddWithValue("IdTemplates", Ticket.Template.IdTemplates);
                cmd.Parameters.AddWithValue("IdUsersAssigned", UsersAssigned.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdGroupsAssigned", IdGroupsAsign);
                cmd.Parameters.AddWithValue("SLA_HOUR", SLA_HOUR);
                DataTable dtWorkOrder = await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                if (dtWorkOrder != null && dtWorkOrder.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtWorkOrder.Rows)
                    {
                        Ticket.IdWorkOrder = long.Parse(dr["IdWorkOrder"].ToString());
                        Ticket.DateSAP = dr["DateSAP"].ToString();
                    }
                }
                Ticket.IdWorkOrder = Ticket.IdWorkOrder;
                /*AGREGAR LOS ADJUNTOS*/
                List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
                if (ListAdjuntos != null)
                {
                    foreach (WorkOrder_Attachments item in ListAdjuntos)
                    {
                        item.IdWorkOrder = Ticket.IdWorkOrder;
                        item.UsersGestiona.IdMasterUsers = IdUserGestiona;
                        await SaveAttachment(cmd, item);
                    }
                }
                /*END AGREGAR LOS ADJUNTOS*/
                string ColumnSQL = "," + string.Join(",", Ticket.ListFieldsUDF.Select(lq => lq.NameFieldsUDF).ToArray());
                string QuerySQLFields = $@"INSERT INTO WorkOrder_Fields(IdWorkOrder,Title,Description,DateSAP,Category,SubCategory{ColumnSQL})
                VALUES(@IdWorkOrder,@Title,@Description,@DateSAP,@Category,@SubCategory{ColumnSQL.Replace(",", ",@")});";
                QuerySQLFields = QuerySQLFields.Replace(",)", ")").Replace(",@)", ")");
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = QuerySQLFields;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdWorkOrder", Ticket.IdWorkOrder);
                cmd.Parameters.AddWithValue("Title", Ticket.Title);
                cmd.Parameters.AddWithValue("Description", Ticket.Description);
                cmd.Parameters.AddWithValue("Category", Ticket.Category.NameCategory);
                cmd.Parameters.AddWithValue("SubCategory", Ticket.SubCategory.NameCategory);
                cmd.Parameters.AddWithValue("DateSAP", Ticket.DateSAP);
                int IdBooth = 0;
                foreach (FieldsUDF Field in Ticket.ListFieldsUDF)
                {
                    if (Field.Value != null)
                    {
                        if (Field.NameFieldsUDF.Contains("UDF_INTBOOTH")) IdBooth = int.Parse(Field.Value);
                        cmd.Parameters.AddWithValue(Field.NameFieldsUDF, Field.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(Field.NameFieldsUDF, DBNull.Value);
                    }
                }
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, true);
                if (IdBooth != 0)
                {
                    cmd = await DAOConfig.SqlCommandELC();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO WH_Booth_WorkOrder(IdWorkOrder,IdBooth)VALUES(@IdWorkOrder,@IdBooth);";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("IdWorkOrder", Ticket.IdWorkOrder);
                    cmd.Parameters.AddWithValue("IdBooth", IdBooth);
                    await DAOConfig.SetDataTableExecuteCommand(cmd);
                }
                Tools.LogAplications("Guardar", "WorkOrder");
            }
            catch (Exception ex)
            {
                string Error = $"CreateWorkOrderControllers.DAOCommand.SaveCrearTicket({Tools.GetLineErr(ex)}): {ex.Message}</br>" +
                    JsonConvert.SerializeObject(Ticket);
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
            return Ticket;
        }

        #endregion
        #region ListWorkOrdersControllers
        public async static Task UpdateReassignWorkOrder(long IdUserGestiona, ImportWorkOrder ReasigWorkOrder)
        {
            try
            {
                List<AlgorithmsAssignment> Algorithm = await ListAlgorithms(null, ReasigWorkOrder.Algorithms.IdAlgorithmsAssignment, true);
                string SQLAlgoritmo = Algorithm[0].SQLStringAlgorithm;
                string ListUsers = string.Join(",", ReasigWorkOrder.ListUsers.Select(lq => lq.IdMasterUsers).ToArray());
                SQLAlgoritmo = SQLAlgoritmo.Replace("{ListIdUsers}", ReasigWorkOrder.ListUsers.Count > 0 ? $"AND U.IdMasterUsers IN ({ListUsers})" : "");
                string ListIdWorkOrder = string.Join(",", ReasigWorkOrder.ListIdWorkOrder);
                SQLAlgoritmo = SQLAlgoritmo.Replace("{ListIdWorkOrder}", ListIdWorkOrder);
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SQLAlgoritmo;
                cmd.CommandTimeout = 500000;
                cmd.Parameters.AddWithValue("IdUsersGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdMasterGroups", ReasigWorkOrder.Grupo.IdMasterGroups);
                cmd.Parameters.AddWithValue("IdAlgorithm", ReasigWorkOrder.Algorithms.IdAlgorithmsAssignment);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Update", "Reasignacion Solicitudes");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ListWorkOrdersControllers.DAOCommand.UpdateReassignWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public static string QuerySQLWorkOrder(bool ListData, string OtherConditionals, string OrderBy = "")
        {
            string Columnas = $@"SELECT * FROM (
		            SELECT ROW_NUMBER() OVER ({OrderBy}) Position,W.IdWorkOrder, isnull(w.IdWorkOrderReference,0) IdWorkOrderReference,
                    TicketsUnificado.dbo.Fn_SemaphoreGeneral(W.IdWorkOrder) Semaphore,
                    WF.Title,
		            /*ESTADOS*/
		            S.IdStatusDefinition,S.NameStatusDefinition, 
		            ISNULL(SubS.IdStatusDefinition,0) IdSubStatusDefinition, 
		            SubS.NameStatusDefinition NameSubStatusDefinition, 
		            /*END ESTADOS*/
		            W.IdTemplates,T.NameTemplate,
		            /*USER CREA*/
		            W.IdMasterUsersCreate,
		            EC.FirstName+' '+ISNULL(EC.SecondName,'')+' '+EC.LastName+' '+ISNULL(EC.SecondLastName,'') FullNameCreate,
		            /*USER ASIGNADO*/
		            W.IdMasterUsersAssigned,
		            EA.FirstName+' '+ISNULL(EA.SecondName,'')+' '+EA.LastName+' '+ISNULL(EA.SecondLastName,'') FullNameAssigned,
		            /*USER ESCALADO*/
		            ISNULL(W.IdMasterUsersScaled,0) IdMasterUsersScaled,
		            ES.FirstName+' '+ISNULL(ES.SecondName,'')+' '+ES.LastName+' '+ISNULL(ES.SecondLastName,'') FullNameScaled,
		            ISNULL(W.IdWorkOrderSolutions,0) IdWorkOrderSolutions,
		            ISNULL(W.IdDataImported,0) IdDataImported,
		            W.IdAlgorithmAssigned,
		            G.IdMasterGroups, G.NameGroup, 
		            /*ESCALAMIENTOS*/
		            ISNULL(WE.IdWorkOrderEscalations,0) IdWorkOrderEscalations,
		            WE.TypeScaling,
		            FORMAT(WE.DateLog,'yyyy-MM-dd HH:mm:ss') DateEscalations,
		            /*ESCALAMIENTOS*/
		            FORMAT(W.DateModification,'yyyy-MM-dd HH:mm:ss') DateModification, 
		            FORMAT(W.DateSAP,'yyyy-MM-dd HH:mm:ss') DateSAP, 
		            FORMAT(W.DateLog,'yyyy-MM-dd HH:mm:ss') DateLog,
                    DATEDIFF(day,w.DateLog,GETDATE()) DiasPQR,
                    /*CAMPOS DE PQR BOG*/
                    ISNULL(WF.UDF_BIGINT18, 0) PQR, ISNULL(WF.UDF_BIGINT19, 0) CUENTA,
                    ISNULL(WF.UDF_VARCHAR60,ISNULL(CONVERT(VARCHAR(30),WF.UDF_BIGINT29),CONVERT(VARCHAR(30),WF.UDF_BIGINT34))) X_COORDINATE,
                    ISNULL(ISNULL(WF.UDF_BIGINT28,WF.UDF_BIGINT32),WF.UDF_VARCHAR88) NUMERO, 
                    /*CAMPOS PQR AIR-E*/
                    WF.UDF_VARCHAR369 FechaCorreo,
                    WF.UDF_VARCHAR370 FechaCorreoVIP
                    ";
            string Paginacion = @" ) W
	            WHERE W.Position >=(@position-1)*@top AND W.Position<(@position*@top)+1;";
            string Query = $@" FROM WorkOrder W with (nolock) 
		            LEFT JOIN WorkOrder_Solutions WS WITH(NOLOCK) ON WS.IdWorkOrderSolutions=W.IdWorkOrderSolutions
		            LEFT JOIN WorkOrder_Escalations WE WITH(NOLOCK) ON WE.IdWorkOrderEscalations=WS.IdWorkOrderEscalations
		            LEFT JOIN MasterUsers UA WITH(NOLOCK) ON UA.IdMasterUsers=W.IdMasterUsersAssigned				--ASIGNADO
		            LEFT JOIN ATLANTIC..Employees EA WITH(NOLOCK) ON EA.IdOldMis=UA.PkEmpleado						--ASIGNADO
		            LEFT JOIN MasterUsers UC WITH(NOLOCK) ON UC.IdMasterUsers=W.IdMasterUsersCreate					--CREADO
		            LEFT JOIN ATLANTIC..Employees EC WITH(NOLOCK) ON EC.IdOldMis=UC.PkEmpleado						--CREADO
		            LEFT JOIN MasterUsers US WITH(NOLOCK) ON US.IdMasterUsers=W.IdMasterUsersScaled					--ESCALADO
		            LEFT JOIN ATLANTIC..Employees ES WITH(NOLOCK) ON ES.IdOldMis=US.PkEmpleado						--ESCALADO
		            LEFT JOIN Templates T WITH(NOLOCK) ON T.IdTemplates=W.IdTemplates
		            INNER JOIN MasterGroups G WITH(NOLOCK) ON G.IdMasterGroups=W.IdMasterGroupsAssigned
		            LEFT JOIN WorkOrder_Fields WF WITH(NOLOCK) ON WF.IdWorkOrder=W.IdWorkOrder
		            LEFT JOIN StatusDefinition S WITH(NOLOCK) ON S.IdStatusDefinition=W.IdStatusDefinition			--ESTADOS
		            LEFT JOIN StatusDefinition SubS WITH(NOLOCK) ON SubS.IdStatusDefinition=W.IdSubStatusDefinition	--SUB ESTADOS
		            WHERE (@PQR IS NULL OR WF.UDF_BIGINT18=@PQR) 
                    AND (@Title IS NULL OR IIF(@Title IS NULL,NULL,WF.Title) LIKE '%'+@Title+'%')
                    AND (@Description IS NULL OR IIF(@Description IS NULL,NULL,WF.Description) LIKE '%'+@Description+'%')
                    AND (@Cuenta IS NULL OR WF.UDF_BIGINT19=@Cuenta)
                    AND (@X_COORDINATE IS NULL OR ISNULL(WF.UDF_VARCHAR60,ISNULL(CONVERT(VARCHAR(30),WF.UDF_BIGINT29),CONVERT(VARCHAR(30),WF.UDF_BIGINT34)))=@X_COORDINATE)
                    AND (@Numero IS NULL OR ISNULL(WF.UDF_VARCHAR88,ISNULL(WF.UDF_BIGINT28,WF.UDF_BIGINT32))=@Numero)
		            AND S.IdTypeActions NOT IN (1) /*Desactivados*/
		            AND (
			            ISNULL(W.IdMasterUsersCreate,0)=@IdUsersCreate OR 
			            ISNULL(W.IdMasterUsersScaled,0)=@IdUsersScaled OR
			            (@IdUserAssigned IS NULL OR ISNULL(W.IdMasterUsersAssigned,0)=@IdUserAssigned)
		            ){OtherConditionals} 
                ";

            if (ListData == true)
            {
                Query = Columnas + Query + Paginacion;
            }
            else
            {
                Query = "SELECT COUNT(1) NumResult " + Query;
            }
            return Query;
        }
        private static string StringOrderBy(string OrderBy, string OrderType)
        {
            string Result = "";
            if (OrderType == "desc")
            {
                if (OrderBy == "DateLog")
                    Result = $" ORDER BY W.DateLog {OrderType} ";
                else if (OrderBy == "DateSAP")
                    Result = $" ORDER BY W.DateSAP {OrderType} ";
                else if (OrderBy == "DateUltModi")
                    Result = $" ORDER BY W.DateModification {OrderType} ";
            }
            else if (OrderType == "asc")
            {
                if (OrderBy == "DateLog")
                    Result = $" ORDER BY W.DateLog {OrderType} ";
                else if (OrderBy == "DateSAP")
                    Result = $" ORDER BY W.DateSAP {OrderType} ";
                else if (OrderBy == "DateUltModi")
                    Result = $" ORDER BY W.DateModification {OrderType} ";
            }
            return Result;
        }
        public async static Task<List<WorkOrder>> ListWorkOrderNew(Users UserActual, FiltersWorkOrder Filters)
        {
            List<WorkOrder> ListData = new List<WorkOrder>();
            try
            {
                #region Filtros
                List<string> ListConditionalsSQL = new List<string>();
                if (Filters.ListIdWorkOrder.Count > 0)
                {
                    ListConditionalsSQL.Add($" AND W.IdWorkOrder IN ({string.Join(",", Filters.ListIdWorkOrder)}) ");
                }
                if (Filters.IdTemplate != 0) ListConditionalsSQL.Add($" AND W.IdTemplates IN ({Filters.IdTemplate}) ");
                string WhereGroupsAssign = "";
                if (Filters.IdGroupsAssign.Count > 0)
                {
                    ListConditionalsSQL.Add($" AND W.IdMasterGroupsAssigned IN ({string.Join(",", Filters.IdGroupsAssign)}) ");
                    WhereGroupsAssign = $" AND W.IdMasterGroupsAssigned IN ({string.Join(",", Filters.IdGroupsAssign)}) ";
                }
                if (Filters.IdUsersCrea.Count > 0) ListConditionalsSQL.Add($" AND W.IdMasterUsersCreate IN ({string.Join(",", Filters.IdUsersCrea)}) ");
                if (Filters.IdUsersAssign.Count > 0) ListConditionalsSQL.Add($" AND W.IdMasterUsersAssigned IN ({string.Join(",", Filters.IdUsersAssign)}) ");
                if (Filters.IdUsersScaled.Count > 0) ListConditionalsSQL.Add($" AND W.IdMasterUsersScaled IN ({string.Join(",", Filters.IdUsersScaled)}) ");
                if (Filters.IdStatus.Count > 0) ListConditionalsSQL.Add($" AND W.IdStatusDefinition IN ({string.Join(",", Filters.IdStatus)}) ");
                else if (ListConditionalsSQL.Count == 0) ListConditionalsSQL.Add(" AND S.IdTypeActions NOT IN (2) ");
                #endregion Filtros

                string OrderBySQL = StringOrderBy(Filters.OrderBy, Filters.OrderType);
                if (OrderBySQL == "") OrderBySQL = "ORDER BY W.IdWorkOrder";

                var AllOrders = await ListPermisos(UserActual.Perfiles, 5); //Mostrar todas las solicitudes
                List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                if (ListGrupos.Count > 0 & AllOrders.Count > 0 & WhereGroupsAssign == "")
                {
                    WhereGroupsAssign = string.Join(",", ListGrupos.Select(x => x.IdMasterGroups).ToList());
                    ListConditionalsSQL.Add($" AND W.IdMasterGroupsAssigned IN ({WhereGroupsAssign}) ");
                }
                string OtherConditionals = string.Join(" ", ListConditionalsSQL);
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = QuerySQLWorkOrder(true, OtherConditionals, OrderBySQL);
                cmd.Parameters.AddWithValue("position", Filters.pag);
                cmd.Parameters.AddWithValue("top", Filters.top);
                if (AllOrders.Count == 0) cmd.Parameters.AddWithValue("IdUserAssigned", UserActual.IdMasterUsers);
                else cmd.Parameters.AddWithValue("IdUserAssigned", DBNull.Value);
                cmd.Parameters.AddWithValue("IdUsersCreate", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdUsersScaled", UserActual.IdMasterUsers);
                if (Filters.PQR == 0) cmd.Parameters.AddWithValue("PQR", DBNull.Value);
                else cmd.Parameters.AddWithValue("PQR", Filters.PQR);
                if (Filters.Cuenta == 0) cmd.Parameters.AddWithValue("Cuenta", DBNull.Value);
                else cmd.Parameters.AddWithValue("Cuenta", Filters.Cuenta);
                if (Filters.X_COORDINATE == null) cmd.Parameters.AddWithValue("X_COORDINATE", DBNull.Value);
                else cmd.Parameters.AddWithValue("X_COORDINATE", Filters.X_COORDINATE);
                if (Filters.Numero == 0) cmd.Parameters.AddWithValue("Numero", DBNull.Value);
                else cmd.Parameters.AddWithValue("Numero", Filters.Numero);

                if (Filters.Title == null) cmd.Parameters.AddWithValue("Title", DBNull.Value);
                else cmd.Parameters.AddWithValue("Title", Filters.Title);
                if (Filters.Description == null) cmd.Parameters.AddWithValue("Description", DBNull.Value);
                else cmd.Parameters.AddWithValue("Description", Filters.Description);
                cmd.CommandTimeout = 500000;
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListData.Add(new WorkOrder
                        {
                            IdWorkOrder = long.Parse(dr["IdWorkOrder"].ToString()),
                            IdWorkOrderReference = long.Parse(dr["IdWorkOrderReference"].ToString()),
                            Semaphore = dr["Semaphore"].ToString(),
                            WorkOrderSolution = {
                                IdWorkOrder=long.Parse(dr["IdWorkOrder"].ToString()),
                                IdWorkOrderSolutions=long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                                WorkOrderEscalations = {
                                    IdWorkOrderEscalations=long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                                    TypeScaling=dr["TypeScaling"].ToString(),
                                    DateLog=dr["DateEscalations"].ToString()
                                }
                            },
                            Title = dr["Title"].ToString(),
                            Status = {
                                IdStatusDefinition= int.Parse(dr["IdStatusDefinition"].ToString()),
                                NameStatus=dr["NameStatusDefinition"].ToString()
                            },
                            SubStatus = {
                                IdStatusDefinition= int.Parse(dr["IdSubStatusDefinition"].ToString()),
                                NameStatus=dr["NameSubStatusDefinition"].ToString()
                            },
                            Template = {
                                IdTemplates = int.Parse(dr["IdTemplates"].ToString()),
                                NameTemplate=dr["NameTemplate"].ToString()
                            },
                            GrupoAsignado = {
                                IdMasterGroups=int.Parse(dr["IdMasterGroups"].ToString()),
                                NameGroup=dr["NameGroup"].ToString()
                            },
                            UsersCreate = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersCreate"].ToString()),
                                FullName =dr["FullNameCreate"].ToString()
                            },
                            UsersAssigned = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersAssigned"].ToString()),
                                FullName =dr["FullNameAssigned"].ToString()
                            },
                            UsersScaled = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersScaled"].ToString()),
                                FullName =dr["FullNameScaled"].ToString()
                            },
                            WorkOrderDataImported = {
                                IdDataImported=int.Parse(dr["IdDataImported"].ToString())
                            },
                            Algorithm = {
                                IdAlgorithmsAssignment=int.Parse(dr["IdAlgorithmAssigned"].ToString())
                            },
                            DateLog = dr["DateLog"].ToString(),
                            DateModification = dr["DateModification"].ToString(),
                            DateSAP = dr["DateSAP"].ToString(),
                            PQR = long.Parse(dr["PQR"].ToString()),
                            Cuenta = long.Parse(dr["Cuenta"].ToString()),
                            X_COORDINATE = dr["X_COORDINATE"].ToString(),
                            NUMERO = dr["NUMERO"].ToString(),
                            DiasPQR = int.Parse(dr["DiasPQR"].ToString()),
                            FechaCorreo = dr["FechaCorreo"].ToString(),
                            FechaCorreoVIP = dr["FechaCorreoVIP"].ToString()

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrderNew({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }
        public async static Task<int> CountWorkOrder(Users UserActual, FiltersWorkOrder Filters)
        {
            int TotalRegis = 0;
            try
            {
                #region Filtros
                List<string> ListConditionalsSQL = new List<string>();
                if (Filters.ListIdWorkOrder.Count > 0)
                {
                    ListConditionalsSQL.Add($" AND W.IdWorkOrder IN ({string.Join(",", Filters.ListIdWorkOrder)}) ");
                }
                if (Filters.IdTemplate != 0) ListConditionalsSQL.Add($" AND W.IdTemplates IN ({Filters.IdTemplate}) ");
                string WhereGroupsAssign = "";
                if (Filters.IdGroupsAssign.Count > 0)
                {
                    ListConditionalsSQL.Add($" AND W.IdMasterGroupsAssigned IN ({string.Join(",", Filters.IdGroupsAssign)}) ");
                    WhereGroupsAssign = $" AND W.IdMasterGroupsAssigned IN ({string.Join(",", Filters.IdGroupsAssign)}) ";
                }
                if (Filters.IdUsersCrea.Count > 0) ListConditionalsSQL.Add($" AND W.IdMasterUsersCreate IN ({string.Join(",", Filters.IdUsersCrea)}) ");
                if (Filters.IdUsersAssign.Count > 0) ListConditionalsSQL.Add($" AND W.IdMasterUsersAssigned IN ({string.Join(",", Filters.IdUsersAssign)}) ");
                if (Filters.IdUsersScaled.Count > 0) ListConditionalsSQL.Add($" AND W.IdMasterUsersScaled IN ({string.Join(",", Filters.IdUsersScaled)}) ");
                if (Filters.IdStatus.Count > 0) ListConditionalsSQL.Add($" AND W.IdStatusDefinition IN ({string.Join(",", Filters.IdStatus)}) ");
                else if (ListConditionalsSQL.Count == 0) ListConditionalsSQL.Add(" AND S.IdTypeActions NOT IN (2) ");
                if (Filters.OrderBy == null) Filters.OrderBy = "ORDER BY W.IdWorkOrder";
                #endregion Filtros
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5); //Mostrar todas las solicitudes
                List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                if (ListGrupos.Count > 0 & AllOrders.Count > 0 & WhereGroupsAssign == "")
                {
                    WhereGroupsAssign = string.Join(",", ListGrupos.Select(x => x.IdMasterGroups).ToList());
                    ListConditionalsSQL.Add($" AND W.IdMasterGroupsAssigned IN ({WhereGroupsAssign}) ");
                }
                string OtherConditionals = string.Join(" ", ListConditionalsSQL);
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                //NUM TOTAL REGISTROS
                cmd.CommandText = QuerySQLWorkOrder(false, OtherConditionals);
                if (AllOrders.Count == 0) cmd.Parameters.AddWithValue("IdUserAssigned", UserActual.IdMasterUsers);
                else cmd.Parameters.AddWithValue("IdUserAssigned", DBNull.Value);
                cmd.Parameters.AddWithValue("IdUsersCreate", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdUsersScaled", UserActual.IdMasterUsers);

                if (Filters.PQR == 0) cmd.Parameters.AddWithValue("PQR", DBNull.Value);
                else cmd.Parameters.AddWithValue("PQR", Filters.PQR);
                if (Filters.Cuenta == 0) cmd.Parameters.AddWithValue("Cuenta", DBNull.Value);
                else cmd.Parameters.AddWithValue("Cuenta", Filters.Cuenta);
                if (Filters.X_COORDINATE == null) cmd.Parameters.AddWithValue("X_COORDINATE", DBNull.Value);
                else cmd.Parameters.AddWithValue("X_COORDINATE", DBNull.Value);
                if (Filters.Numero == 0) cmd.Parameters.AddWithValue("Numero", DBNull.Value);
                else cmd.Parameters.AddWithValue("Numero", Filters.Numero);

                if (Filters.Title == null) cmd.Parameters.AddWithValue("Title", DBNull.Value);
                else cmd.Parameters.AddWithValue("Title", Filters.Title);
                if (Filters.Description == null) cmd.Parameters.AddWithValue("Description", DBNull.Value);
                else cmd.Parameters.AddWithValue("Description", Filters.Description);
                cmd.CommandTimeout = 500000;
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                TotalRegis = int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.CountWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return TotalRegis;
        }
        public async static Task<List<WorkOrder>> ListWorkOrder(Users UserActual, long IdWorkOrder)
        {
            List<WorkOrder> ListWorkOrders = new List<WorkOrder>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);
                List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                DataTable dtGroups = new DataTable();
                dtGroups.Columns.Add("Id");
                if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                {
                    foreach (Groups Item in ListGrupos)
                    {
                        var row = dtGroups.NewRow();
                        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListWorkOrder";
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                if (AllOrders.Count == 0)
                    cmd.Parameters.AddWithValue("IdUserAssigned", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdUsersCreate", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdUsersScaled", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListWorkOrders.Add(new WorkOrder
                        {
                            IdWorkOrder = long.Parse(dr["IdWorkOrder"].ToString()),
                            IdWorkOrderReference = long.Parse(dr["IdWorkOrderReference"].ToString()),
                            WorkOrderSolution = {
                                IdWorkOrder=long.Parse(dr["IdWorkOrder"].ToString()),
                                IdWorkOrderSolutions=long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                                WorkOrderEscalations = {
                                    IdWorkOrderEscalations=long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                                    TypeScaling=dr["TypeScaling"].ToString(),
                                    DateLog=dr["DateEscalations"].ToString()
                                },
                                BPBServicio = {
                                    ID =dr["Id_BPB_Servicio"].ToString(),
                                    Nombre =dr["BPB_Servicio"].ToString()
                                },
                                CtaContable = {
                                    ID =dr["BPB_Cta_Contable"].ToString(),
                                    Nombre =dr["TypeScaling"].ToString()
                                }
                            },
                            Title = dr["Title"].ToString(),
                            Status = {
                                IdStatusDefinition= int.Parse(dr["IdStatusDefinition"].ToString()),
                                NameStatus=dr["NameStatusDefinition"].ToString()
                            },
                            SubStatus = {
                                IdStatusDefinition= int.Parse(dr["IdSubStatusDefinition"].ToString()),
                                NameStatus=dr["NameSubStatusDefinition"].ToString()
                            },
                            Template = {
                                IdTemplates = int.Parse(dr["IdTemplates"].ToString()),
                                NameTemplate=dr["NameTemplate"].ToString(),
                                Sitio= new Sites
                                {
                                    IdMasterSites=int.Parse(dr["IdMasterSites"].ToString()),
                                    RequiredSubStatus=bool.Parse(dr["RequiredSubStatus"].ToString())
                                }
                            },
                            GrupoAsignado = {
                                IdMasterGroups=int.Parse(dr["IdMasterGroups"].ToString()),
                                NameGroup=dr["NameGroup"].ToString()
                            },
                            UsersCreate = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersCreate"].ToString()),
                                FullName =dr["FullNameCreate"].ToString()
                            },
                            UsersAssigned = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersAssigned"].ToString()),
                                FullName =dr["FullNameAssigned"].ToString()
                            },
                            UsersScaled = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersScaled"].ToString()),
                                FullName =dr["FullNameScaled"].ToString()
                            },
                            WorkOrderDataImported = {
                                IdDataImported=int.Parse(dr["IdDataImported"].ToString())
                            },
                            Algorithm = {
                                IdAlgorithmsAssignment=int.Parse(dr["IdAlgorithmAssigned"].ToString())
                            },
                            DateLog = dr["DateLog"].ToString(),
                            DateModification = dr["DateModification"].ToString(),
                            DateSAP = dr["DateSAP"].ToString(),
                            FechaCorreo = dr["FechaCorreo"].ToString(),
                            FechaCorreoVIP = dr["FechaCorreoVIP"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListWorkOrders;
        }
        public async static Task<List<WorkOrder>> ListRelacionados(Users UserActual, long IdWorkOrder)
        {
            List<WorkOrder> ListWorkOrders = new List<WorkOrder>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListWorkOrder_Relations";
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                if (AllOrders.Count == 0)
                    cmd.Parameters.AddWithValue("IdUserAssigned", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdUsersCreate", UserActual.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdUsersScaled", UserActual.IdMasterUsers);
                //cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListWorkOrders.Add(new WorkOrder
                        {
                            IdWorkOrder = long.Parse(dr["IdWorkOrder"].ToString()),
                            IdWorkOrderReference = long.Parse(dr["IdWorkOrderReference"].ToString()),
                            WorkOrderSolution = {
                                IdWorkOrder=long.Parse(dr["IdWorkOrder"].ToString()),
                                IdWorkOrderSolutions=long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                                WorkOrderEscalations = {
                                    IdWorkOrderEscalations=long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                                    TypeScaling=dr["TypeScaling"].ToString(),
                                    DateLog=dr["DateEscalations"].ToString()
                                }
                            },
                            Title = dr["Title"].ToString(),
                            Status = {
                                IdStatusDefinition= int.Parse(dr["IdStatusDefinition"].ToString()),
                                NameStatus=dr["NameStatusDefinition"].ToString()
                            },
                            SubStatus = {
                                IdStatusDefinition= int.Parse(dr["IdSubStatusDefinition"].ToString()),
                                NameStatus=dr["NameSubStatusDefinition"].ToString()
                            },
                            Template = {
                                IdTemplates = int.Parse(dr["IdTemplates"].ToString()),
                                NameTemplate=dr["NameTemplate"].ToString(),
                                Sitio= new Sites
                                {
                                    IdMasterSites=int.Parse(dr["IdMasterSites"].ToString()),
                                    RequiredSubStatus=bool.Parse(dr["RequiredSubStatus"].ToString())
                                }
                            },
                            GrupoAsignado = {
                                IdMasterGroups=int.Parse(dr["IdMasterGroups"].ToString()),
                                NameGroup=dr["NameGroup"].ToString()
                            },
                            UsersCreate = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersCreate"].ToString()),
                                FullName =dr["FullNameCreate"].ToString()
                            },
                            UsersAssigned = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersAssigned"].ToString()),
                                FullName =dr["FullNameAssigned"].ToString()
                            },
                            UsersScaled = {
                                IdMasterUsers=int.Parse(dr["IdMasterUsersScaled"].ToString()),
                                FullName =dr["FullNameScaled"].ToString()
                            },
                            WorkOrderDataImported = {
                                IdDataImported=int.Parse(dr["IdDataImported"].ToString())
                            },
                            Algorithm = {
                                IdAlgorithmsAssignment=int.Parse(dr["IdAlgorithmAssigned"].ToString())
                            },
                            DateLog = dr["DateLog"].ToString(),
                            DateModification = dr["DateModification"].ToString(),
                            DateSAP = dr["DateSAP"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListWorkOrders;
        }

        #endregion
        #region WorkOrderSolutionsControllers
        public async static Task<List<WorkOrder_Escalations>> ListWorkOrder_Escalations(long? IdWorkOrder = null, long? IdWorkOrderEscalations = null)
        {
            List<WorkOrder_Escalations> Escalations = new List<WorkOrder_Escalations>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListWorkOrderEscalations";
                if (IdWorkOrder != null)
                    cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                if (IdWorkOrderEscalations != null)
                    cmd.Parameters.AddWithValue("IdWorkOrderEscalations", IdWorkOrderEscalations);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Escalations = (from Linq in dt.Rows.Cast<DataRow>()
                                   select new WorkOrder_Escalations()
                                   {
                                       IdWorkOrder = long.Parse(Linq["IdWorkOrder"].ToString()),
                                       IdWorkOrderEscalations = long.Parse(Linq["IdWorkOrderEscalations"].ToString()),
                                       UserGestiona = {
                                        IdMasterUsers=int.Parse(Linq["IdMasterUserGestiona"].ToString())
                                    },
                                       UserScaled ={
                                        IdMasterUsers=int.Parse(Linq["IdMasterUserScaled"].ToString())
                                    },
                                       GroupsScaled = {
                                        IdMasterGroups=int.Parse(Linq["IdMasterGroupsScaled"].ToString())
                                    },
                                       TypeScaling = Linq["TypeScaling"].ToString(),
                                       Comments = Linq["Comments"].ToString(),
                                       DateLog = Linq["DateLog"].ToString()
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.WorkOrder_Escalations({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Escalations;
        }
        public async static Task<List<WorkOrder_Solution>> ListWorkOrder_Solution(long? IdWorkOrder = null, long? IdWorkOrderSolutions = null)
        {
            List<WorkOrder_Solution> Solution = new List<WorkOrder_Solution>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListWorkOrderSolutions";
                if (IdWorkOrder != null)
                    cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                if (IdWorkOrderSolutions != null)
                    cmd.Parameters.AddWithValue("IdWorkOrderSolutions", IdWorkOrderSolutions);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        WorkOrder_Escalations WorkOrderEscal = new WorkOrder_Escalations();
                        long IdWorkOrderEscalations = long.Parse(dr["IdWorkOrderEscalations"].ToString());
                        if (IdWorkOrderEscalations != 0)
                        {
                            List<WorkOrder_Escalations> ListWorkOrderEscal = await ListWorkOrder_Escalations(null, IdWorkOrderEscalations);
                            WorkOrderEscal = ListWorkOrderEscal[0];
                        }
                        Solution.Add(new WorkOrder_Solution
                        {
                            WorkOrderEscalations = WorkOrderEscal,
                            IdWorkOrderSolutions = long.Parse(dr["IdWorkOrderSolutions"].ToString()),
                            IdWorkOrder = long.Parse(dr["IdWorkOrder"].ToString()),
                            Status = {
                                IdStatusDefinition=int.Parse(dr["IdStatusDefinition"].ToString())
                            },
                            SubStatus = {
                                IdStatusDefinition=int.Parse(dr["IdSubStatusDefinition"].ToString())
                            },
                            Resolutions = dr["Resolutions"].ToString(),
                            DateLog = dr["DateLog"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.WorkOrder_Solution({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Solution;
        }
        public async static Task<List<FieldsUDF>> WorkOrder_FieldsGeneral(bool SolutionField, long IdWorkOrder, FieldsUDF ObjField)
        {
            List<FieldsUDF> ListFields = new List<FieldsUDF>();
            try
            {
                ListFields = await ListFieldsUDF(ObjField, SolutionField, true, true);
                if (SolutionField == false)
                {
                    ObjField = new FieldsUDF();
                    ListFields.AddRange(await ListFieldsUDF(ObjField, SolutionField, true, null, true));
                }
                if (ListFields.Count > 0)
                {
                    string TableSQL = "WorkOrder_Fields", ColumnId = "IdWorkOrder";
                    if (SolutionField)
                    {
                        TableSQL = "WorkOrder_Solutions_Fields";
                        ColumnId = "IdWorkOrderSolutions";
                    }
                    string ColumnSQL = string.Join(",", ListFields.Select(lq => lq.NameFieldsUDF).ToArray());
                    ColumnSQL = ColumnSQL.Replace("Title,", "").Replace("DateSAP,", "");
                    SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                    cmd.CommandType = CommandType.Text;
                    if (ObjField.Template.IdTemplates == 52)
                    {
                        string cadena = HttpContext.Current.Request.Url.AbsoluteUri;
                        string[] separar = cadena.Split('=');
                        string idwork = separar[separar.Length - 1];
                        cmd.CommandText = $"SELECT wsf.UDF_VARCHAR488,wsf.UDF_VARCHAR509,wsf.UDF_VARCHAR489,wsf.UDF_VARCHAR490,wsf.UDF_VARCHAR491,wsf.UDF_VARCHAR492,wsf.UDF_VARCHAR493,wsf.UDF_VARCHAR494,wsf.UDF_VARCHAR495,wsf.UDF_VARCHAR496,wsf.UDF_VARCHAR497,wsf.UDF_VARCHAR499,wsf.UDF_VARCHAR500,wsf.UDF_VARCHAR501,wsf.UDF_VARCHAR502,wsf.UDF_VARCHAR503,wsf.UDF_VARCHAR504,wsf.UDF_VARCHAR505,wsf.UDF_VARCHAR506,wsf.UDF_VARCHAR507,wsf.UDF_VARCHAR508 FROM WorkOrder_Solutions_Fields wsf,WorkOrder_Solutions ws WITH(NOLOCK) WHERE ws.IdWorkOrder=" + idwork + " and wsf.IdWorkOrderSolutions=ws.IdWorkOrderSolutions;";
                    }
                    else
                    {
                        cmd.CommandText = $"SELECT {ColumnSQL} FROM {TableSQL} WITH(NOLOCK) WHERE {ColumnId}=@IdWorkOrder;";
                    }
                    cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                    DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string[] CamposNA = { "Title", "DateSAP" };
                        DataRow row = dt.Rows[0];
                        for (int i = 0; i < ListFields.Count; i++)
                        {
                            if (CamposNA.Contains(ListFields[i].NameFieldsUDF))
                            {
                                ListFields.RemoveAt(i);
                                i--;
                            }
                            else
                            {
                                ListFields[i].Value = row[ListFields[i].NameFieldsUDF].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.WorkOrder_FieldsGeneral({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListFields;
        }
        public async static Task<List<Disposition>> WorkOrder_GetDispositions(string servicio)
        {
            List<Disposition> ListDispositions = new List<Disposition>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrderSolutions_Disposition_Service";
                cmd.Parameters.AddWithValue("@Servicio", servicio);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListDispositions.Add(
                           new Disposition
                           {
                               ID = Convert.ToString(dt.Rows[i]["ID"]),
                               Nombre = Convert.ToString(dt.Rows[i]["Disposition"])
                           }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.WorkOrder_GetDispositions({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListDispositions;
        }
        public async static Task<List<Disposition>> WorkOrder_GetCtaContable(string ID)
        {
            List<Disposition> ListDispositions = new List<Disposition>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrderSolutions_Cta_Contable_Service";
                cmd.Parameters.AddWithValue("@ID", ID);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListDispositions.Add(
                           new Disposition
                           {
                               ID = Convert.ToString(dt.Rows[i]["ID"]),
                               Nombre = Convert.ToString(dt.Rows[i]["Disposition"])
                           }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.WorkOrder_GetCtaContable({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListDispositions;
        }
        public async static Task SaveWorkOrderSolutions(long IdUserGestiona, WorkOrder_Solution Solution)
        {
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            try
            {
                //string NameTransaction = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //cmd.Connection.Open();
                //cmd.Transaction = cmd.Connection.BeginTransaction("Solutions" + NameTransaction);
                Users UsersAssigned = await UserMenosTicketsAsignados(Solution.WorkOrderEscalations.GroupsScaled.IdMasterGroups);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_SaveSolution";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdWorkOrder", Solution.IdWorkOrder);
                cmd.Parameters.AddWithValue("IdStatus", Solution.Status.IdStatusDefinition);
                if (Solution.SubStatus.IdStatusDefinition != 0)
                    cmd.Parameters.AddWithValue("IdSubStatus", Solution.SubStatus.IdStatusDefinition);
                cmd.Parameters.AddWithValue("Resolutions", Solution.Resolutions);
                if (UsersAssigned.IdMasterUsers != 0)
                {
                    cmd.Parameters.AddWithValue("IdUsersScaled", UsersAssigned.IdMasterUsers);
                    cmd.Parameters.AddWithValue("IdGroupScaled", Solution.WorkOrderEscalations.GroupsScaled.IdMasterGroups);
                    cmd.Parameters.AddWithValue("TypeScaling", Solution.WorkOrderEscalations.TypeScaling);
                    cmd.Parameters.AddWithValue("Comments", Solution.WorkOrderEscalations.Comments);
                }
                cmd.Parameters.AddWithValue("BPBServicio", Solution.BPBServicio.Nombre);
                cmd.Parameters.AddWithValue("CtaContable", Solution.CtaContable.Nombre);
                DataTable dt = await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Solution.IdWorkOrderSolutions = long.Parse(row["IdWorkOrderSolutions"].ToString());
                }
                /*AGREGAR LOS ADJUNTOS*/
                List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
                if (ListAdjuntos != null)
                {
                    foreach (WorkOrder_Attachments item in ListAdjuntos)
                    {
                        item.IdWorkOrder = Solution.IdWorkOrder;
                        item.IdWorkOrderSolutions = Solution.IdWorkOrderSolutions;
                        item.UsersGestiona.IdMasterUsers = IdUserGestiona;
                        await SaveAttachment(cmd, item);
                    }
                }
                /*END AGREGAR LOS ADJUNTOS*/
                string ColumnSQL = "," + string.Join(",", Solution.ListFielsUDFSolution.Select(lq => lq.NameFieldsUDF).ToArray());
                string QuerySQLFields = $@"INSERT INTO WorkOrder_Solutions_Fields(IdWorkOrderSolutions{ColumnSQL})
                VALUES(@IdWorkOrderSolutions{ColumnSQL.Replace(",", ",@")});";
                QuerySQLFields = QuerySQLFields.Replace(",)", ")").Replace(",@)", ")");
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = QuerySQLFields;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdWorkOrderSolutions", Solution.IdWorkOrderSolutions);
                foreach (FieldsUDF Field in Solution.ListFielsUDFSolution)
                {
                    if (Field.Value != null)
                    {
                        cmd.Parameters.AddWithValue(Field.NameFieldsUDF, Field.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(Field.NameFieldsUDF, DBNull.Value);
                    }
                }
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, true);
                Tools.LogAplications("Guardar", "WorkOrderSolutions");
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.SaveWorkOrderSolutions({Tools.GetLineErr(ex)}): {ex.Message}</br>";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
        }

        public async static Task SaveWorkOrderSolutions_2(long IdUserGestiona, WorkOrder_Solution Solution)
        {
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            try
            {

                Users UsersAssigned = await UserMenosTicketsAsignados(Solution.WorkOrderEscalations.GroupsScaled.IdMasterGroups);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_SaveSolution_2";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdWorkOrder", Solution.IdWorkOrder);
                cmd.Parameters.AddWithValue("IdStatus", Solution.Status.IdStatusDefinition);
                if (Solution.SubStatus.IdStatusDefinition != 0)
                    cmd.Parameters.AddWithValue("IdSubStatus", Solution.SubStatus.IdStatusDefinition);
                cmd.Parameters.AddWithValue("Resolutions", Solution.Resolutions);
                if (UsersAssigned.IdMasterUsers != 0)
                {
                    cmd.Parameters.AddWithValue("IdUsersScaled", UsersAssigned.IdMasterUsers);
                    cmd.Parameters.AddWithValue("IdGroupScaled", Solution.WorkOrderEscalations.GroupsScaled.IdMasterGroups);
                    cmd.Parameters.AddWithValue("TypeScaling", Solution.WorkOrderEscalations.TypeScaling);
                    cmd.Parameters.AddWithValue("Comments", Solution.WorkOrderEscalations.Comments);
                }
                DataTable dt = await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Solution.IdWorkOrderSolutions = long.Parse(row["IdWorkOrderSolutions"].ToString());
                }
                /*AGREGAR LOS ADJUNTOS*/
                List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
                if (ListAdjuntos != null)
                {
                    foreach (WorkOrder_Attachments item in ListAdjuntos)
                    {
                        item.IdWorkOrder = Solution.IdWorkOrder;
                        item.IdWorkOrderSolutions = Solution.IdWorkOrderSolutions;
                        item.UsersGestiona.IdMasterUsers = IdUserGestiona;
                        await SaveAttachment(cmd, item);
                    }
                }
                /*END AGREGAR LOS ADJUNTOS*/
                //string ColumnSQL = "," + string.Join(",", Solution.ListFielsUDFSolution.Select(lq => lq.NameFieldsUDF).ToArray());
                //string QuerySQLFields = $@"INSERT INTO WorkOrder_Solutions_Fields(IdWorkOrderSolutions{ColumnSQL})
                //VALUES(@IdWorkOrderSolutions{ColumnSQL.Replace(",", ",@")});";
                //QuerySQLFields = QuerySQLFields.Replace(",)", ")").Replace(",@)", ")");
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = QuerySQLFields;
                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("IdWorkOrderSolutions", Solution.IdWorkOrderSolutions);
                //foreach (FieldsUDF Field in Solution.ListFielsUDFSolution)
                //{
                //    if (Field.Value != null)
                //    {
                //        cmd.Parameters.AddWithValue(Field.NameFieldsUDF, Field.Value);
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue(Field.NameFieldsUDF, DBNull.Value);
                //    }
                //}
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, true);
                Tools.LogAplications("Guardar", "WorkOrderSolutions");
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.SaveWorkOrderSolutions({Tools.GetLineErr(ex)}): {ex.Message}</br>";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
        }
        public async static Task<List<WorkOrder_Attachments>> ListWorkOrder_Attachments(long? IdWorkOrder = null, long? IdAttachment = null)
        {
            List<WorkOrder_Attachments> ListAttachments = new List<WorkOrder_Attachments>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListWorkOrderAttachments";
                if (IdWorkOrder != null)
                    cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                if (IdAttachment != null)
                    cmd.Parameters.AddWithValue("IdAttachment", IdAttachment);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListAttachments = (from Linq in dt.Rows.Cast<DataRow>()
                                       select new WorkOrder_Attachments()
                                       {
                                           IdWorkOrder = long.Parse(Linq["IdWorkOrder"].ToString()),
                                           IdAttachment = long.Parse(Linq["IdAttachment"].ToString()),
                                           IdWorkOrderSolutions = long.Parse(Linq["IdWorkOrderSolutions"].ToString()),
                                           NameAttachment = Linq["NameAttachment"].ToString(),
                                           NameEncryptedAttachment = long.Parse(Linq["NameEncryptedAttachment"].ToString()),
                                           Extension = Linq["Extension"].ToString(),
                                           DateLog = Linq["DateLog"].ToString(),
                                           FileSizeKB = Math.Round(decimal.Parse(Linq["FileSizeKB"].ToString()), 2),
                                           FilePathDownload = $@"/Uploads/WorkOrder/{long.Parse(Linq["IdWorkOrder"].ToString())}/{long.Parse(Linq["NameEncryptedAttachment"].ToString()) + Linq["Extension"].ToString()}"
                                       }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.ListWorkOrder_Attachments({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListAttachments;
        }
        public async static Task SaveAttachmentSQL(CasosPqrMovilEscrita Attachments)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            try
            {
                //SqlCommand cmd = new SqlCommand();
                SqlCommand cmd = new SqlCommand("TicketsUnificado..sp_WorkOrder_Ins_PQR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "sp_WorkOrder_Ins_PQR";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("idWorKOrderSolutions", Attachments.idWorKOrderSolutions);
                cmd.Parameters.AddWithValue("BPB_Estado", Attachments.BPB_ESTADO);
                cmd.Parameters.AddWithValue("BPB_ULTIMA_ACTUALIZACION", DateTime.Now);
                cmd.Parameters.AddWithValue("PRE_Estado", Attachments.PRE_Estado);
                cmd.Parameters.AddWithValue("PRE_Ultima_Actualizacion", DateTime.Now);
                cmd.Parameters.AddWithValue("Ascard1_Estado", Attachments.Ascard1_Estado);
                cmd.Parameters.AddWithValue("Ascard1_Ultima_Actualizacion", DateTime.Now);
                cmd.Parameters.AddWithValue("Ascard2_Estado", Attachments.Ascard2_Estado);
                cmd.Parameters.AddWithValue("Ascard2_Ultima_Actualizacion", DateTime.Now);
                cmd.Parameters.AddWithValue("CEN_Estado_Solicitud", Attachments.CEN_Estado_Solicitud);
                cmd.Parameters.AddWithValue("CEN_Ultima_Actualizacion", DateTime.Now);
                cmd.Parameters.AddWithValue("BPB_Estado_V", Attachments.BPB_ESTADO_V);
                cmd.Parameters.AddWithValue("BPB_ULTIMA_ACTUALIZACION_V", DateTime.Now);
                cmd.Parameters.AddWithValue("PRE_Estado_V", Attachments.PRE_Estado_V);
                cmd.Parameters.AddWithValue("PRE_Ultima_Actualizacion_V", DateTime.Now);
                cmd.Parameters.AddWithValue("Ascard1_Estado_V", Attachments.Ascard1_Estado_V);
                cmd.Parameters.AddWithValue("Ascard1_Ultima_Actualizacion_V", DateTime.Now);
                cmd.Parameters.AddWithValue("Ascard2_Estado_V", Attachments.Ascard2_Estado_V);
                cmd.Parameters.AddWithValue("Ascard2_Ultima_Actualizacion_V", DateTime.Now);
                cmd.Parameters.AddWithValue("CEN_Estado_Solicitud_V", Attachments.CEN_Estado_Solicitud_V);
                cmd.Parameters.AddWithValue("CEN_Ultima_Actualizacion_V", DateTime.Now);
                //await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string Error = $"ImportarPQRController.DAOCommand.SaveAttachmentSQL({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task SaveAttachment(SqlCommand cmd, WorkOrder_Attachments Attachments)
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_SaveWorkOrderAttachments";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdUserGestiona", Attachments.UsersGestiona.IdMasterUsers);
                cmd.Parameters.AddWithValue("IdWorkOrder", Attachments.IdWorkOrder);
                if (Attachments.IdWorkOrderSolutions != 0)
                    cmd.Parameters.AddWithValue("IdWorkOrderSolutions", Attachments.IdWorkOrderSolutions);
                cmd.Parameters.AddWithValue("NameAttachment", Attachments.NameAttachment);
                cmd.Parameters.AddWithValue("NameEncryptedAttachment", Attachments.NameEncryptedAttachment);
                cmd.Parameters.AddWithValue("Extension", Attachments.Extension);
                cmd.Parameters.AddWithValue("FileSizeKB", Attachments.FileSizeKB);
                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, false);
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.SaveAttachment({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task DeleteAttachment(long IdUserGestion, long IdWorkOrder, long NameEncrypted)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Upd_DeleteWorkOrderAttachments";
                cmd.Parameters.AddWithValue("IdUserGestion", IdUserGestion);
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                cmd.Parameters.AddWithValue("NameEncrypted", NameEncrypted);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.DeleteAttachment({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region ReportsControllers
        private static string ConvertStringFieldsUDF(List<FieldsUDF> ListFieldsUDF, bool SolutionField)
        {
            string FieldsUDF = "";
            List<FieldsUDF> ListUDF = new List<FieldsUDF>();
            ListUDF = ListFieldsUDF.Where(lq => lq.SolutionField == SolutionField).ToList();
            foreach (var item in ListUDF)
                FieldsUDF += $",{item.NameFieldsUDF} AS [{item.NameField}]";

            return FieldsUDF;
        }
        public async static Task<List<DataTable>> ListDTFullWorkOrder(Users UserActual, FiltersReports Filtres)
        {
            List<DataTable> ListDT = new List<DataTable>();
            try
            {
                //Se envian los Sitios al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                if (UserActual.Sitios != null)
                {
                    foreach (var Item in UserActual.Sitios)
                    {
                        var row = dtSitios.NewRow();
                        row["Id"] = Item.IdMasterSites;
                        dtSitios.Rows.Add(row);
                    }
                }
                //Se envian los Estados al SP
                DataTable dtEstados = new DataTable();
                dtEstados.Columns.Add("Id");
                foreach (var Item in Filtres.ArrayStatus)
                {
                    var row = dtEstados.NewRow();
                    row["Id"] = Item;
                    dtEstados.Rows.Add(row);
                }
                //Se envian los Grupos al SP
                DataTable dtGroups = new DataTable();
                dtGroups.Columns.Add("Id");
                foreach (var Item in Filtres.ArrayGroups)
                {
                    var row = dtGroups.NewRow();
                    dtGroups.Rows.Add(row["Id"] = Item);
                }
                /*WorkOrder*/
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Reports_Sel_ListWorkOrderNotPagin";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                cmd.Parameters.AddWithValue("dtEstados", dtEstados);
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                cmd.Parameters.AddWithValue("FechaInicio", Filtres.FechaInicio);
                cmd.Parameters.AddWithValue("FechaFin", Filtres.FechaFin);
                if (Filtres.IdTemplates != 0)
                    cmd.Parameters.AddWithValue("IdTemplates", Filtres.IdTemplates);
                ListDT.Add(await DAOConfig.GetDataTableExecuteCommand(cmd));
                ListDT[ListDT.Count - 1].TableName = "WorkOrder";

                /*WorkOrder_Solutions*/
                cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Reports_Sel_ListWorkOrderSolutionsNotPagin";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                cmd.Parameters.AddWithValue("dtEstados", dtEstados);
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                cmd.Parameters.AddWithValue("FechaInicio", Filtres.FechaInicio);
                cmd.Parameters.AddWithValue("FechaFin", Filtres.FechaFin);
                if (Filtres.IdTemplates != 0)
                    cmd.Parameters.AddWithValue("IdTemplates", Filtres.IdTemplates);
                ListDT.Add(await DAOConfig.GetDataTableExecuteCommand(cmd));
                ListDT[ListDT.Count - 1].TableName = "WorkOrderSolutions";

                if (Filtres.IdTemplates != 0 & Filtres.ArrayStatus.Length > 0 & Filtres.ArrayGroups.Length > 0)
                {
                    FieldsUDF Field = new FieldsUDF
                    {
                        Template = { IdTemplates = Filtres.IdTemplates }
                    };
                    List<FieldsUDF> ListUDF = await ListFieldsUDF(Field, null, true);
                    string FieldsUDF = ConvertStringFieldsUDF(ListUDF, false);
                    string FieldsSolutionsUDF = ConvertStringFieldsUDF(ListUDF, true);

                    string IdSites = string.Join(",", UserActual.Sitios.Select(lq => lq.IdMasterSites).ToArray());
                    /*WorkOrderSolutions_Fields*/
                    cmd = await DAOConfig.SqlCommandGeneralSD();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $@"SELECT WSF.IdWorkOrderSolutions{FieldsSolutionsUDF}
	                FROM WorkOrder_Solutions_Fields WSF WITH(NOLOCK)
	                INNER JOIN WorkOrder_Solutions WS WITH(NOLOCK) ON WS.IdWorkOrderSolutions=WSF.IdWorkOrderSolutions
	                INNER JOIN WorkOrder W WITH(NOLOCK) ON W.IdWorkOrder=WS.IdWorkOrder
	                INNER JOIN MasterGroups G WITH(NOLOCK) ON G.IdMasterGroups=W.IdMasterGroupsAssigned
	                WHERE G.IdMasterSites IN ({IdSites})
	                AND W.IdStatusDefinition IN ({string.Join(",", Filtres.ArrayStatus)})
	                AND W.IdMasterGroupsAssigned IN ({string.Join(",", Filtres.ArrayGroups)})
                    AND W.IdTemplates=@IdTemplates
	                AND (CONVERT(DATE,WS.DateLog) BETWEEN @FechaInicio AND @FechaFin);";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("FechaInicio", Filtres.FechaInicio);
                    cmd.Parameters.AddWithValue("FechaFin", Filtres.FechaFin);
                    cmd.Parameters.AddWithValue("IdTemplates", Filtres.IdTemplates);
                    ListDT.Add(await DAOConfig.GetDataTableExecuteCommand(cmd));
                    ListDT[ListDT.Count - 1].TableName = "WorkOrderSolutions_Fields";

                    if (FieldsUDF != "")
                    {
                        /*WorkOrder_Fields*/
                        cmd = await DAOConfig.SqlCommandGeneralSD();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $@"SELECT W.IdWorkOrder{FieldsUDF}
	                    FROM WorkOrder_Fields WS WITH(NOLOCK)
	                    INNER JOIN WorkOrder W WITH(NOLOCK) ON W.IdWorkOrder=WS.IdWorkOrder
	                    INNER JOIN MasterGroups G WITH(NOLOCK) ON G.IdMasterGroups=W.IdMasterGroupsAssigned
	                    WHERE G.IdMasterSites IN ({IdSites})
	                    AND W.IdStatusDefinition IN ({string.Join(",", Filtres.ArrayStatus)})
	                    AND W.IdMasterGroupsAssigned IN ({string.Join(",", Filtres.ArrayGroups)})
	                    AND W.IdTemplates=@IdTemplates
	                    AND (CONVERT(DATE,W.DateLog) BETWEEN @FechaInicio AND @FechaFin);";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("FechaInicio", Filtres.FechaInicio);
                        cmd.Parameters.AddWithValue("FechaFin", Filtres.FechaFin);
                        cmd.Parameters.AddWithValue("IdTemplates", Filtres.IdTemplates);
                        ListDT.Add(await DAOConfig.GetDataTableExecuteCommand(cmd));
                        ListDT[ListDT.Count - 1].TableName = "WorkOrder_Fields";
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ReportsControllers.DAOCommand.ListDTFullWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListDT;
        }
        #endregion
        #region SemaphoreControllers
        public async static Task<bool> VerifySiteSemaphore(Sites Sitio)
        {
            bool swi = false;
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT IdSemaphore FROM Semaphores WITH(NOLOCK) where IdMasterSites=@IdMasterSites;";
                cmd.Parameters.AddWithValue("IdMasterSites", Sitio.IdMasterSites);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    swi = true;
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SemaphoreControllers.DAOCommand.VerifySiteSemaphore({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return swi;
        }
        public async static Task<List<Semaphores>> ListSemaphore(List<Sites> SitiosUserActual, int? IdSemaphore = null, int? IdSubCategory = null)
        {
            List<Semaphores> ListSemaphore = new List<Semaphores>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtSitios = new DataTable();
                dtSitios.Columns.Add("Id");
                foreach (var Item in SitiosUserActual)
                {
                    var RowA = dtSitios.NewRow();
                    RowA["Id"] = Item.IdMasterSites;
                    dtSitios.Rows.Add(RowA);
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Semaphores_Sel_ListSemaphores";
                cmd.Parameters.AddWithValue("dtSitios", dtSitios);
                if (IdSemaphore != null) cmd.Parameters.AddWithValue("IdSemaphore", IdSemaphore);
                if (IdSubCategory != null) cmd.Parameters.AddWithValue("IdSubCategory", IdSubCategory);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListSemaphore = (from Linq in dt.Rows.Cast<DataRow>()
                                     select new Semaphores()
                                     {
                                         IdSemaphore = int.Parse(Linq["IdSemaphore"].ToString()),
                                         Category = {
                                            IdCategory=int.Parse(Linq["IdCategory"].ToString()),
                                            NameCategory=Linq["NameCategory"].ToString()
                                          },
                                         SubCategory = {
                                            IdCategory=int.Parse(Linq["IdSubCategory"].ToString()),
                                            NameCategory=Linq["SubNameCategory"].ToString()
                                          },
                                         Sitio = {
                                              IdMasterSites=int.Parse(Linq["IdMasterSites"].ToString()),
                                              NameSite=Linq["NameSite"].ToString()
                                          },
                                         SLA_HOUR = int.Parse(Linq["SLA_HOUR"].ToString()),
                                         GreenTo = int.Parse(Linq["GreenTo"].ToString()),
                                         OrangeTo = int.Parse(Linq["OrangeTo"].ToString()),
                                         DateLog = Linq["DateLog"].ToString(),
                                         State = bool.Parse(Linq["State"].ToString())
                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SemaphoreControllers.DAOCommand.ListSemaphore({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListSemaphore;
        }
        public async static Task SaveSemaphore(long IdUserGestiona, Semaphores Semaforo)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Semaphores_Upd_SaveSemaphores";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                if (Semaforo.IdSemaphore != 0)
                    cmd.Parameters.AddWithValue("IdSemaphore", Semaforo.IdSemaphore);
                if (Semaforo.Sitio.IdMasterSites != 0)
                    cmd.Parameters.AddWithValue("IdMasterSites", Semaforo.Sitio.IdMasterSites);
                if (Semaforo.SLA_HOUR != 0)
                    cmd.Parameters.AddWithValue("SLA_HOUR", Semaforo.SLA_HOUR);
                if (Semaforo.SubCategory.IdCategory != 0)
                    cmd.Parameters.AddWithValue("IdSubCategory", Semaforo.SubCategory.IdCategory);
                cmd.Parameters.AddWithValue("GreenTo", Semaforo.GreenTo);
                cmd.Parameters.AddWithValue("OrangeTo", Semaforo.OrangeTo);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Semaforo");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SemaphoreControllers.DAOCommand.SaveSemaphore({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        public async static Task DisabledEnabledSemaphore(long IdUserGestiona, int IdSemaphore, bool Activar)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Semaphores_Upd_DisabledSemaphore";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdSemaphore", IdSemaphore);
                cmd.Parameters.AddWithValue("State", Activar);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications(Activar == true ? "Activar" : "Desactivar", "Semaforo");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SemaphoreControllers.DAOCommand.DisabledEnabledSemaphore({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region DisabledDataControllers
        public async static Task<List<WorkOrder_DataImported>> ListDataImported(List<Groups> ListGroups)
        {
            List<WorkOrder_DataImported> ListData = new List<WorkOrder_DataImported>();
            try
            {
                //Se envian los perfiles al sp
                DataTable dtGroups = new DataTable();
                dtGroups.Columns.Add("Id");
                foreach (var Item in ListGroups)
                {
                    var RowA = dtGroups.NewRow();
                    RowA["Id"] = Item.IdMasterGroups;
                    dtGroups.Rows.Add(RowA);
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListDataImported";
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.Rows.Cast<DataRow>()
                                select new WorkOrder_DataImported()
                                {
                                    IdDataImported = int.Parse(Linq["IdDataImported"].ToString()),
                                    UserImport = new Users
                                    {
                                        FullName = Linq["FullNameImport"].ToString()
                                    },
                                    NameData = Linq["NameData"].ToString(),
                                    NumRecords = int.Parse(Linq["NumRecords"].ToString()),
                                    UserDesactivated = new Users
                                    {
                                        FullName = Linq["FullNameDesactivated"].ToString(),
                                    },
                                    DesactivationReason = Linq["DesactivationReason"].ToString(),
                                    DateDesactivation = Linq["DateDesactivation"].ToString(),
                                    DateLog = Linq["DateLog"].ToString(),
                                    State = bool.Parse(Linq["State"].ToString())
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.ListDataImported({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }
        public async static Task DisabledDataImport(int IdDataImported, string DesactivationReason, long IdMasterUsers)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Upd_DisabledImportData";
                cmd.Parameters.AddWithValue("IdDataImported", IdDataImported);
                cmd.Parameters.AddWithValue("DesactivationReason", DesactivationReason);
                cmd.Parameters.AddWithValue("IdMasterUserDesactivated", IdMasterUsers);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region Alertas
        public async static Task<List<WorkOrder>> ListAlertas(Users UserActual)
        {
            List<WorkOrder> ListWorkOrders = new List<WorkOrder>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_ListWorkOrder_Alarmas";
                cmd.Parameters.AddWithValue("Winuser", UserActual.Winuser);
                //cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListWorkOrders.Add(new WorkOrder
                        {
                            IdWorkOrder = long.Parse(dr["IdWorkOrder"].ToString()),
                            Title = dr["Title"].ToString(),
                            DateSAP = dr["DateSAP"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListWorkOrders;
        }
        #endregion
        #region Historicos
        public async static Task<List<HistoryState>> ListHistoricoEstado(Users UserActual, int perfil, int IdGroup)
        {
            List<HistoryState> HistoryState = new List<HistoryState>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);

                //List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                //DataTable dtGroups = new DataTable();
                //dtGroups.Columns.Add("Id");
                //if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                //{
                //    foreach (Groups Item in ListGrupos)
                //    {
                //        var row = dtGroups.NewRow();
                //        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                //    }
                //}

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Historico_Estados";
                cmd.Parameters.AddWithValue("Winuser", UserActual.Winuser);
                cmd.Parameters.AddWithValue("Agente", perfil);
                cmd.Parameters.AddWithValue("Group", IdGroup);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HistoryState.Add(new HistoryState
                        {
                            Cantidad = int.Parse(dr["Cantidad"].ToString()),
                            NameStatus = dr["Estado"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return HistoryState;
        }

        public async static Task<List<HistoryAsigne>> ListHistoricoAsignado(Users UserActual, int perfil, int IdGroup)
        {
            List<HistoryAsigne> HistoryState = new List<HistoryAsigne>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);

                //List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                //DataTable dtGroups = new DataTable();
                //dtGroups.Columns.Add("Id");
                //if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                //{
                //    foreach (Groups Item in ListGrupos)
                //    {
                //        var row = dtGroups.NewRow();
                //        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                //    }
                //}

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Historico_Asignados";
                cmd.Parameters.AddWithValue("Winuser", UserActual.Winuser);
                cmd.Parameters.AddWithValue("Agente", perfil);
                cmd.Parameters.AddWithValue("Group", IdGroup);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HistoryState.Add(new HistoryAsigne
                        {
                            Cantidad = int.Parse(dr["Cantidad"].ToString()),
                            Semana = dr["semana"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return HistoryState;
        }

        public async static Task<List<HistoryCompliance>> ListHistoricoCumplimiento(Users UserActual, int perfil, int IdGroup)
        {
            List<HistoryCompliance> HistoryState = new List<HistoryCompliance>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);
                //List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                //DataTable dtGroups = new DataTable();
                //dtGroups.Columns.Add("Id");
                //if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                //{
                //    foreach (Groups Item in ListGrupos)
                //    {
                //        var row = dtGroups.NewRow();
                //        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                //    }
                //}

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Historico_Cumplimiento";
                cmd.Parameters.AddWithValue("Winuser", UserActual.Winuser);
                cmd.Parameters.AddWithValue("Agente", perfil);
                cmd.Parameters.AddWithValue("Group", IdGroup);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HistoryState.Add(new HistoryCompliance
                        {
                            Cantidad = int.Parse(dr["Cantidad"].ToString()),
                            Semana = dr["semana"].ToString(),
                            Meta = int.Parse(dr["Meta"].ToString()),
                            Diferencia = int.Parse(dr["Diferencia"].ToString()),
                            Cumplimiento = int.Parse(dr["Cumplimiento"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return HistoryState;
        }

        public async static Task<List<HistoryPercentage>> ListHistoricoPorcentajeANS(Users UserActual, int perfil, int IdGroup)
        {
            List<HistoryPercentage> HistoryState = new List<HistoryPercentage>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);
                //List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                //DataTable dtGroups = new DataTable();
                //dtGroups.Columns.Add("Id");
                //if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                //{
                //    foreach (Groups Item in ListGrupos)
                //    {
                //        var row = dtGroups.NewRow();
                //        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                //    }
                //}
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Historico_Porcentaje_ANS";
                cmd.Parameters.AddWithValue("Winuser", UserActual.Winuser);
                cmd.Parameters.AddWithValue("Agente", perfil);
                cmd.Parameters.AddWithValue("Group", IdGroup);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HistoryState.Add(new HistoryPercentage
                        {
                            Cantidad = int.Parse(dr["Cantidad"].ToString()),
                            Dias = int.Parse(dr["dias"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return HistoryState;
        }

        public async static Task<List<Goals>> ListMetas(Users UserActual, int Agente, int IdGroup)
        {
            List<Goals> HistoryState = new List<Goals>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);
                //List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                //DataTable dtGroups = new DataTable();
                //dtGroups.Columns.Add("Id");
                //if (ListGrupos.Count > 0)
                //{
                //    foreach (Groups Item in ListGrupos)
                //    {
                //        var row = dtGroups.NewRow();
                //        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                //    }
                //}

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Metas";
                cmd.Parameters.AddWithValue("Winuser", UserActual.Winuser);
                cmd.Parameters.AddWithValue("Group", IdGroup);
                //cmd.Parameters.AddWithValue("Agente", Agente);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        HistoryState.Add(new Goals
                        {
                            IdMeta = int.Parse(dr["Id_Meta"].ToString()),
                            FechaActual = DateTime.Parse(dr["FechaActual"].ToString()),
                            Meta = int.Parse(dr["Meta"].ToString()),
                            IdMasterGroups = int.Parse(dr["IdMasterGroups"].ToString()),
                            Cerrados = int.Parse(dr["Cerrados"].ToString()),
                            Cumplimiento = int.Parse(dr["Cumplimiento"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return HistoryState;
        }


        public async static Task<List<Bases>> ListBases()
        {
            List<Bases> ListBases = new List<Bases>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Bases";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListBases = (from Linq in dt.Rows.Cast<DataRow>()
                                 select new Bases()
                                 {
                                     Id = int.Parse(Linq["ID"].ToString()),
                                     Descripcion = Linq["Descripcion"].ToString()
                                 }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListBases;
        }

        public async static Task<List<SINO>> ListSino()
        {
            List<SINO> ListSino = new List<SINO>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Sino";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListSino = (from Linq in dt.Rows.Cast<DataRow>()
                                select new SINO()
                                {
                                    Id = int.Parse(Linq["ID"].ToString()),
                                    Descripcion = Linq["Descripcion"].ToString()
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListSino;
        }

        public async static Task<List<Canal>> ListCanal()
        {
            List<Canal> ListCanal = new List<Canal>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Canal";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListCanal = (from Linq in dt.Rows.Cast<DataRow>()
                                 select new Canal()
                                 {
                                     Id = int.Parse(Linq["ID"].ToString()),
                                     Descripcion = Linq["Descripcion"].ToString()
                                 }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListCanal;
        }

        public async static Task<List<TipoReclamo>> ListTipoReclamo()
        {
            List<TipoReclamo> ListTipoReclamos = new List<TipoReclamo>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Tipo_Reclamo";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListTipoReclamos = (from Linq in dt.Rows.Cast<DataRow>()
                                        select new TipoReclamo()
                                        {
                                            Id = int.Parse(Linq["ID"].ToString()),
                                            Descripcion = Linq["Descripcion"].ToString()
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListTipoReclamos;
        }

        public async static Task<List<EstadoBitacora>> ListEstadoBitacora()
        {
            List<EstadoBitacora> ListEstadoBitacoras = new List<EstadoBitacora>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Estado_Bitacora";
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListEstadoBitacoras = (from Linq in dt.Rows.Cast<DataRow>()
                                           select new EstadoBitacora()
                                           {
                                               Id = int.Parse(Linq["ID"].ToString()),
                                               Descripcion = Linq["Descripcion"].ToString()
                                           }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>SitesControllers.DAOCommand.ListProfile({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListEstadoBitacoras;
        }

        public async static Task<List<Groups>> ListGroupsInfo(Users UserActual)
        {
            List<Groups> ListGroups = new List<Groups>();
            try
            {

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListGroups_Info";
                cmd.Parameters.AddWithValue("IdMasterUser", UserActual.IdMasterUsers);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListGroups.Add(new Groups
                        {
                            IdMasterGroups = int.Parse(dr["IdMasterGroups"].ToString()),
                            NameGroup = dr["NameGroup"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.ListGroups({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListGroups;
        }

        public async static Task<List<Groups>> ListGroupsInfo()
        {
            List<Groups> ListGroups = new List<Groups>();
            try
            {

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListGroups_Info";

                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListGroups.Add(new Groups
                        {
                            IdMasterGroups = int.Parse(dr["IdMasterGroups"].ToString()),
                            NameGroup = dr["NameGroup"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.ListGroups({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListGroups;
        }

        public async static Task<List<Goals>> ValMeta(int IdGroups, DateTime fecha)
        {
            List<Goals> Metas = new List<Goals>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Groups_Sel_ListMeta";
                cmd.Parameters.AddWithValue("IdGroups", IdGroups);
                cmd.Parameters.AddWithValue("fechaActual", fecha);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Metas = (from dr in dt.Rows.Cast<DataRow>()
                             select new Goals()
                             {
                                 IdMeta = int.Parse(dr["Id_Meta"].ToString()),
                                 FechaActual = DateTime.Parse(dr["FechaActual"].ToString()),
                                 IdMasterGroups = int.Parse(dr["IdMasterGroups"].ToString()),
                                 Meta = int.Parse(dr["Meta"].ToString())
                             }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>GroupsControllers.DAOCommand.ListUsersXGroup({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Metas;
        }

        public async static Task SaveMeta(long IdUserGestiona, int IdMasterGroups, int Meta, DateTime fecha)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Ins_Metas";
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                cmd.Parameters.AddWithValue("IdMasterGroups", IdMasterGroups);
                cmd.Parameters.AddWithValue("Meta", Meta);
                cmd.Parameters.AddWithValue("FechaActual", fecha);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
                Tools.LogAplications("Guardar", "Meta");
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>HomeController.DAOCommand.SaveMeta({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }
        #endregion
        #region WorkOrderTimeGroup

        public async static Task<List<WorkOrder_Assigned>> SelWorkOrderAssigned(WorkOrder_Assigned Params1, long IdUserGestiona)
        {
            List<WorkOrder_Assigned> ListData = new List<WorkOrder_Assigned>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Sel_WorkOrderAssigned";
                if (Params1.IdWorkOrder != 0) cmd.Parameters.AddWithValue("IdWorkOrder", Params1.IdWorkOrder);
                if (IdUserGestiona != 0) cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new WorkOrder_Assigned()
                                {
                                    IdWorkOrder = int.Parse(lq["IdWorkOrder"].ToString()),

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task SaveWorkOrderAssigned(long IdUserGestiona, long IdWorkOrder, int IdStatusDefinition, int IdMasterGroups)
        {
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_WorkOrderAssigned";
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                cmd.Parameters.AddWithValue("IdStatusDefinition", IdStatusDefinition);
                cmd.Parameters.AddWithValue("IdMasterGroups", IdMasterGroups);
                cmd.Parameters.AddWithValue("IdUserGestiona", IdUserGestiona);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }

        public async static Task<List<TimeList>> SelListaTiempos(Users UserActual)
        {
            List<TimeList> ListData = new List<TimeList>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);
                List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                DataTable dtGroups = new DataTable();
                dtGroups.Columns.Add("Id");
                if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                {
                    foreach (Groups Item in ListGrupos)
                    {
                        var row = dtGroups.NewRow();
                        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Lista_Tiempos";
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new TimeList()
                                {
                                    NombreAgente = lq["NombreAgente"].ToString(),
                                    IdWorkOrder = int.Parse(lq["IdWorkOrder"].ToString()),
                                    Grupo = lq["Grupo"].ToString(),
                                    FechaApertura = lq["FechaApertura"].ToString(),
                                    HoraApertura = lq["HoraApertura"].ToString(),
                                    FechaCierre = lq["FechaCierre"].ToString(),
                                    HoraCierre = lq["HoraCierre"].ToString(),
                                    DuracionGestion = int.Parse(lq["DuracionGestion"].ToString()),
                                    TiempoReal = int.Parse(lq["TiempoReal"].ToString()),
                                    Estado = lq["Estado"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<TimeList>> SelListaTiempos_2(Users UserActual)
        {
            List<TimeList> ListData = new List<TimeList>();
            try
            {
                var AllOrders = await ListPermisos(UserActual.Perfiles, 5);
                List<Groups> ListGrupos = await ListGroupsXUsers(UserActual.IdMasterUsers);
                DataTable dtGroups = new DataTable();
                dtGroups.Columns.Add("Id");
                if (ListGrupos.Count > 0 & AllOrders.Count > 0)
                {
                    foreach (Groups Item in ListGrupos)
                    {
                        var row = dtGroups.NewRow();
                        dtGroups.Rows.Add(row["Id"] = Item.IdMasterGroups);
                    }
                }
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_Lista_Tiempos_2";
                cmd.Parameters.AddWithValue("dtGroups", dtGroups);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new TimeList()
                                {
                                    NombreAgente = lq["NombreAgente"].ToString(),
                                    IdWorkOrder = int.Parse(lq["IdWorkOrder"].ToString()),
                                    Grupo = lq["Grupo"].ToString(),
                                    //FechaApertura = DateTime.Parse(lq["FechaApertura"].ToString()),
                                    //FechaDia = DateTime.Parse(lq["FechaDia"].ToString()),
                                    //FechaCierre = lq["FechaCierre"].ToString(),
                                    //TiempoTranscurrido = int.Parse(lq["TiempoTranscurrido"].ToString()),
                                    //Estado = lq["Estado"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        #endregion
        #region CaseHistory
        public async static Task<List<CaseHistorySummary>> ListCaseHistory(string ESTADO = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CaseHistorySummary> ListData = new List<CaseHistorySummary>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_CaseHistory_Summary";
                if (ESTADO != null && !ESTADO.Equals(""))
                {
                    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                }
                if (Fechainicio != null && Fechainicio != "") cmd.Parameters.AddWithValue("FechaInicio", Fechainicio);
                if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CaseHistorySummary()
                                {
                                    Estado = lq["Estado"].ToString(),
                                    Grupo = lq["Grupo"].ToString(),
                                    NumeroCasos = lq["NumeroCasos"].ToString()
                                    //IdWorkOrder = int.Parse(lq["IdWorkOrder"].ToString()),
                                    //Semaphore = lq["Semaphore"].ToString(),
                                    //Title = lq["Title"].ToString(),
                                    //Status = {
                                    //    IdStatusDefinition= int.Parse(lq["IdStatusDefinition"].ToString()),
                                    //    NameStatus=lq["NameStatusDefinition"].ToString()
                                    //},
                                    //Template = {
                                    //    IdTemplates = int.Parse(lq["IdTemplates"].ToString()),
                                    //    NameTemplate=lq["NameTemplate"].ToString()
                                    //},
                                    //UsersCreate = {
                                    //    IdMasterUsers=int.Parse(lq["IdMasterUsersCreate"].ToString()),
                                    //    FullName =lq["FullNameCreate"].ToString()
                                    //},
                                    //WorkOrderSolution = {
                                    //    IdWorkOrder=long.Parse(lq["IdWorkOrder"].ToString()),
                                    //    IdWorkOrderSolutions=long.Parse(lq["IdWorkOrderSolutions"].ToString()),
                                    //    WorkOrderEscalations = {
                                    //        IdWorkOrderEscalations=long.Parse(lq["IdWorkOrderSolutions"].ToString()),
                                    //        TypeScaling=lq["TypeScaling"].ToString(),
                                    //        DateLog=lq["DateEscalations"].ToString()
                                    //    }
                                    //},
                                    //GrupoAsignado = {
                                    //    IdMasterGroups=int.Parse(lq["IdMasterGroups"].ToString()),
                                    //    NameGroup=lq["NameGroup"].ToString()
                                    //},
                                    //DateModification = lq["DateModification"].ToString(),
                                    //DiasPQR = int.Parse(lq["DiasPQR"].ToString())

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }
        #endregion
        #region NegacionDeLinea
        public async static Task<List<NegacionLinea>> NegacionLinea_Alistamiento(long IdWorkOrder)
        {
            List<NegacionLinea> ListData = new List<NegacionLinea>();
            try
            {

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_AlistamientoDefinicionByIdWorkOrder";
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.Rows.Cast<DataRow>()
                                select new NegacionLinea()
                                {
                                    Id = int.Parse(Linq["Id"].ToString()),
                                    IdWorkOrder = long.Parse(Linq["IdWorkOrder"].ToString()),
                                    Base = Linq["Base"].ToString(),
                                    Imagen = Linq["Imagen"].ToString(),
                                    MIN = Linq["MIN"].ToString(),
                                    Nombre = Linq["Nombre"].ToString(),
                                    Apellido = Linq["Apellido"].ToString(),
                                    Curcode = Linq["Curcode"].ToString(),
                                    EsExportado = Linq["EsExportado"].ToString(),
                                    Ascard = Linq["Ascard"].ToString(),
                                    Canal = Linq["Canal"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListPermisos({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }


        public async static Task GuardarNegacionLinea_Alistamiento(string IdUserGestiona, NegacionLinea Solution)
        {
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            try
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Ins_AlistamientoDefinicionByIdWorkOrder";
                // cmd.Parameters.AddWithValue("Id", Solution.Id);
                cmd.Parameters.AddWithValue("IdWorkOrder", Solution.IdWorkOrder);
                cmd.Parameters.AddWithValue("Base", Solution.Base);
                cmd.Parameters.AddWithValue("Imagen", Solution.Imagen);
                cmd.Parameters.AddWithValue("MIN", Solution.MIN);

                cmd.Parameters.AddWithValue("FechaActivacion", Solution.FechaActivacion);
                cmd.Parameters.AddWithValue("Curcode", Solution.Curcode);
                cmd.Parameters.AddWithValue("Nombre", Solution.Nombre);
                cmd.Parameters.AddWithValue("Apellido", Solution.Apellido);
                cmd.Parameters.AddWithValue("Canal", Solution.Canal);
                cmd.Parameters.AddWithValue("Ascard", Solution.Ascard);
                cmd.Parameters.AddWithValue("FechaReposicion", Solution.FechaReposicion);
                cmd.Parameters.AddWithValue("Contrato", Solution.Contrato);
                cmd.Parameters.AddWithValue("Grabacion", Solution.Grabacion);
                cmd.Parameters.AddWithValue("Reasignacion", Solution.Reasignacion);
                cmd.Parameters.AddWithValue("Estado", Solution.Estado);
                cmd.Parameters.AddWithValue("Legalizado", Solution.Legalizado);
                cmd.Parameters.AddWithValue("Observaciones", Solution.Observaciones);
                cmd.Parameters.AddWithValue("RangoProbable", Solution.RangoProbable);
                cmd.Parameters.AddWithValue("DireccionInformaCliente", Solution.DireccionInformaCliente);
                cmd.Parameters.AddWithValue("CustomerID", Solution.CustomerID);
                cmd.Parameters.AddWithValue("Ciudad", Solution.Ciudad);
                cmd.Parameters.AddWithValue("Departamento", Solution.Departamento);
                cmd.Parameters.AddWithValue("FechaRadicacion", Solution.FechaRadicacion);
                cmd.Parameters.AddWithValue("Notificacion", Solution.Notificacion);
                cmd.Parameters.AddWithValue("FechaDesactivacion", Solution.FechaDesactivacion);
                cmd.Parameters.AddWithValue("PQR", Solution.PQR);
                cmd.Parameters.AddWithValue("Cedula", Solution.Cedula);
                cmd.Parameters.AddWithValue("AreaRadica", Solution.AreaRadica);
                cmd.Parameters.AddWithValue("TipoReclamo", Solution.TipoReclamo);

                cmd.Parameters.AddWithValue("UserLog", IdUserGestiona);
                cmd.Parameters.AddWithValue("UserLogUltimo", IdUserGestiona);

                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, true);

                Tools.LogAplications("Guardar", "WorkOrderSolutions");
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.SaveWorkOrderSolutions({Tools.GetLineErr(ex)}): {ex.Message}</br>";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
        }


        public async static Task<List<NegacionLinea>> NegacionLinea_AlistamientoExcel(long IdWorkOrder)
        {
            List<NegacionLinea> ListData = new List<NegacionLinea>();
            try
            {

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Upd_AlistamientoDefinicion_ExportarTodoByIdWorkOrder";
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.Rows.Cast<DataRow>()
                                select new NegacionLinea()
                                {

                                    IdWorkOrder = long.Parse(Linq["IdWorkOrder"].ToString()),
                                    Base = Linq["Base"].ToString(),
                                    Imagen = Linq["Imagen"].ToString(),
                                    MIN = Linq["MIN"].ToString(),
                                    FechaActivacion = Convert.ToDateTime(Linq["FechaActivacion"]),
                                    Curcode = Linq["Curcode"].ToString(),
                                    Nombre = Linq["Nombre"].ToString(),
                                    Apellido = Linq["Apellido"].ToString(),
                                    Canal = Linq["Canal"].ToString(),
                                    Ascard = Linq["Ascard"].ToString(),
                                    FechaReposicion = Convert.ToDateTime(Linq["FechaReposicion"]),
                                    Contrato = Linq["Contrato"].ToString(),
                                    Grabacion = Linq["Grabacion"].ToString(),
                                    Reasignacion = Linq["Reasignacion"].ToString(),
                                    Estado = Linq["Estado"].ToString(),
                                    Legalizado = Linq["Legalizado"].ToString(),
                                    Observaciones = Linq["Observaciones"].ToString(),
                                    RangoProbable = Linq["RangoProbable"].ToString(),
                                    DireccionInformaCliente = Linq["DireccionInformaCliente"].ToString(),
                                    CustomerID = Linq["CustomerID"].ToString(),
                                    Ciudad = Linq["Ciudad"].ToString(),
                                    Departamento = Linq["Departamento"].ToString(),
                                    FechaRadicacion = Convert.ToDateTime(Linq["FechaRadicacion"]),
                                    Notificacion = Linq["Notificacion"].ToString(),
                                    FechaDesactivacion = Linq["FechaDesactivacion"].ToString(),
                                    PQR = int.Parse(Linq["PQR"].ToString()),
                                    Cedula = int.Parse(Linq["Cedula"].ToString()),
                                    AreaRadica = Linq["AreaRadica"].ToString(),
                                    TipoReclamo = Linq["TipoReclamo"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListPermisos({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }

        public async static Task<List<NegacionLinea>> NegacionLinea_AlistamientoExcelPendiente(long IdWorkOrder)
        {
            List<NegacionLinea> ListData = new List<NegacionLinea>();
            try
            {

                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Upd_AlistamientoDefinicion_ExportarPendienteByIdWorkOrder";
                cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from Linq in dt.Rows.Cast<DataRow>()
                                select new NegacionLinea()
                                {
                                    IdWorkOrder = long.Parse(Linq["IdWorkOrder"].ToString()),
                                    Base = Linq["Base"].ToString(),
                                    Imagen = Linq["Imagen"].ToString(),
                                    MIN = Linq["MIN"].ToString(),

                                    FechaActivacion = Convert.ToDateTime(Linq["FechaActivacion"]),
                                    Curcode = Linq["Curcode"].ToString(),
                                    Nombre = Linq["Nombre"].ToString(),
                                    Apellido = Linq["Apellido"].ToString(),
                                    Canal = Linq["Canal"].ToString(),
                                    Ascard = Linq["Ascard"].ToString(),
                                    FechaReposicion = Convert.ToDateTime(Linq["FechaReposicion"]),
                                    Contrato = Linq["Contrato"].ToString(),
                                    Grabacion = Linq["Grabacion"].ToString(),
                                    Reasignacion = Linq["Reasignacion"].ToString(),
                                    Estado = Linq["Estado"].ToString(),
                                    Legalizado = Linq["Legalizado"].ToString(),
                                    Observaciones = Linq["Observaciones"].ToString(),
                                    RangoProbable = Linq["RangoProbable"].ToString(),
                                    DireccionInformaCliente = Linq["DireccionInformaCliente"].ToString(),
                                    CustomerID = Linq["CustomerID"].ToString(),
                                    Ciudad = Linq["Ciudad"].ToString(),
                                    Departamento = Linq["Departamento"].ToString(),
                                    FechaRadicacion = Convert.ToDateTime(Linq["FechaRadicacion"]),
                                    Notificacion = Linq["Notificacion"].ToString(),
                                    FechaDesactivacion = Linq["FechaDesactivacion"].ToString(),
                                    PQR = int.Parse(Linq["PQR"].ToString()),
                                    Cedula = int.Parse(Linq["Cedula"].ToString()),
                                    AreaRadica = Linq["AreaRadica"].ToString(),
                                    TipoReclamo = Linq["TipoReclamo"].ToString()
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>ProfilesControllers.DAOCommand.ListPermisos({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListData;
        }



        public async static Task EliminarNegacionLinea_Alistamiento(string IdUserGestiona, int Id)
        {
            SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
            try
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Upd_AlistamientoDefinicion_AnularById";
                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("UserLogUltimo", IdUserGestiona);

                await DAOConfig.MultiSetDataTableExecuteCommand(cmd, true);

                Tools.LogAplications("Guardar", "WorkOrderSolutions");
            }
            catch (Exception ex)
            {
                string Error = $"WorkOrderSolutionsControllers.DAOCommand.SaveWorkOrderSolutions({Tools.GetLineErr(ex)}): {ex.Message}</br>";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            //finally
            //{
            //    cmd.Connection.Close();
            //}
        }


        [HttpPost]
        public async static Task CargarExcelNegacionLinea(HttpPostedFileBase ArchivoExcel, string ServerPath, long IdWorkOrder)
        {
            //long IdWorkOrder = 1740027;
            ErrorViewModel mensaje = new ErrorViewModel();

            DataTable dtexcel;
            DataTable dtmirror = new DataTable();
            dtmirror.Columns.Add("MIN");
            dtmirror.Columns.Add("CUSCODE");
            dtmirror.Columns.Add("Solucion");
            dtmirror.Columns.Add("TMK");
            dtmirror.Columns.Add("Anexos");
            dtmirror.Columns.Add("Observaciones");
            dtmirror.Columns.Add("Contrato");
            dtmirror.Columns.Add("ContratoEquipo");
            dtmirror.Columns.Add("N_Apro");
            dtmirror.Columns.Add("FechaEvidente");
            dtmirror.Columns.Add("FechaCastigo");
            DataTable dtsheet;
            string sql = "", sheetName = "", sheetNamew = "";

            try
            {
                if (ArchivoExcel != null)
                {
                    string fullPath = ServerPath + Path.GetFileName(ArchivoExcel.FileName);
                    ArchivoExcel.SaveAs(fullPath);

                    using (var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        // Auto-detect format, supports:
                        //  - Binary Excel files (2.0-2003 format; *.xls)
                        //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                UseColumnDataType = true,
                                ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });
                            DataTableCollection sheets = result.Tables;
                            foreach (DataTable sheet in sheets)
                            {
                                if (sheet.TableName.Contains("hoja"))
                                {
                                    sheetName = sheet.TableName;
                                    break;
                                }
                            }
                            if (sheetName != "")
                            {
                                if (sheets[sheetName].Rows.Count > 0)
                                {
                                    SqlCommand cmd;

                                    string MIN = "";
                                    string CUSTCODE = "";
                                    string Solucion = "";
                                    string TMK = "";
                                    string Anexos = "";
                                    string Observaciones = "";
                                    string Contrato = "";
                                    string ContratoEquipo = "";
                                    string N_Apro = "";
                                    string FechaEvidente = "";
                                    string FechaCastigo = "";

                                    foreach (DataRow DR in sheets[sheetName].Rows)
                                    {
                                        cmd = await DAOConfig.SqlCommandGeneralSD();
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "Sp_Upd_AlistamientoDefinicion_DefinicionByIdWorkOrder";
                                        try
                                        {
                                            MIN = DR["MIN"].ToString();
                                            CUSTCODE = DR["CUSTCODE"].ToString();
                                            TMK = DR["TMK"].ToString();
                                            Solucion = DR["Solucion"].ToString();
                                            Anexos = DR["Anexos"].ToString();
                                            Observaciones = DR["Observaciones"].ToString();
                                            Contrato = DR["Contrato"].ToString();
                                            ContratoEquipo = DR["ContratoEquipo"].ToString();
                                            N_Apro = DR["N_Apro"].ToString();
                                            FechaEvidente = DR["FechaEvidente"].ToString();
                                            FechaCastigo = DR["FechaCastigo"].ToString();


                                            cmd.Parameters.AddWithValue("IdWorkOrder", IdWorkOrder);
                                            cmd.Parameters.AddWithValue("MIN", MIN);
                                            cmd.Parameters.AddWithValue("CUSCODE", CUSTCODE);
                                            cmd.Parameters.AddWithValue("Definicion_Solucion", Solucion);
                                            cmd.Parameters.AddWithValue("TMK", TMK);
                                            cmd.Parameters.AddWithValue("Definicion_Anexos", Anexos);
                                            cmd.Parameters.AddWithValue("Definicion_Observaciones", Observaciones);
                                            cmd.Parameters.AddWithValue("Definicion_Contrato", Contrato);
                                            cmd.Parameters.AddWithValue("Definicion_ContratoEquipo", ContratoEquipo);
                                            cmd.Parameters.AddWithValue("Definicion_N_Apro", N_Apro);
                                            cmd.Parameters.AddWithValue("Definicion_FechaEvidente", Convert.ToDateTime(FechaEvidente));
                                            cmd.Parameters.AddWithValue("Definicion_FechaCastigo", Convert.ToDateTime(FechaCastigo));

                                            await DAOConfig.SetDataTableExecuteCommand(cmd);
                                        }
                                        catch (Exception e)
                                        {
                                            string error = e.Message;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    //}
                }

            }
            catch (Exception ex)
            {
                throw new Exception(Tools.MsjError($"CargarExcel", ex));
            }



        }
        #endregion

        //Excel planilla ajuste
        public async static Task<DataTable> PlanillaAjustesMovil_Excel(string Fechainicio = null, string Fechafinal = null)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_ExportarExcelPlantillaajusteMovil";
                //if (cuenta != null && cuenta != "")
                //    cmd.Parameters.AddWithValue("CUENTA", cuenta);
                //if (subcuenta != null && subcuenta != "")
                //    cmd.Parameters.AddWithValue("SUBCUENTA", subcuenta);
                //if (DdlEstadoCoa != null && DdlEstadoCoa != "")
                //    cmd.Parameters.AddWithValue("DdlEstadoCoa", DdlEstadoCoa);
                if (Fechainicio != null && Fechainicio != "") cmd.Parameters.AddWithValue("FechaInicio", Fechainicio);
                if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);

                DataTable dt1 = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    dt = dt1;
                }
            }
            catch (Exception ex)
            {

                string Error = $"WorkOrderSolutionsControllers.DAOCommand.SaveWorkOrderSolutions({Tools.GetLineErr(ex)}): {ex.Message}</br>";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return dt;
        }

        //plantilla de busqueda pqr

        #region CasePQRmovilEscrita
        public async static Task<List<CasosPqrMovilEscrita>> ListCasePrepago(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_Prepago_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    PRE_Radicacion = lq["PRE_Radicacion"].ToString(),
                                    PRE_Tipo_Reclamo = lq["PRE_Tipo_Reclamo"].ToString(),
                                    PRE_Min = lq["PRE_Min"].ToString(),
                                    PRE_Nombre_Titular = lq["PRE_Nombre_Titular"].ToString(),
                                    PRE_CUSTCODE = lq["PRE_CUSTCODE"].ToString(),
                                    PRE_Valor = lq["PRE_Valor"].ToString(),
                                    PRE_Concepto = lq["PRE_Concepto"].ToString(),
                                    PRE_Analista = lq["PRE_Analista"].ToString(),
                                    PRE_Periodo_Ajustado_Desde = lq["PRE_Periodo_Ajustado_Desde"].ToString(),
                                    PRE_Periodo_Ajustado_Hasta = lq["PRE_Periodo_Ajustado_Hasta"].ToString(),
                                    PRE_Aliado = lq["PRE_Aliado"].ToString(),
                                    PRE_Estado = lq["PRE_Estado"].ToString(),
                                    PRE_Actualizacion_Anterior = lq["PRE_Actualizacion_Anterior"].ToString(),
                                    PRE_Ultima_Actualizacion = lq["PRE_Ultima_Actualizacion"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCasePrepago13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_Prepago13_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    PRE_Radicado_V = lq["PRE_Radicado_V"].ToString(),
                                    PRE_Tipo_Reclamo_V = lq["PRE_Tipo_Reclamo_V"].ToString(),
                                    PRE_Min_V = lq["PRE_Min_V"].ToString(),
                                    PRE_Nombre_Titular_V = lq["PRE_Nombre_Titular_V"].ToString(),
                                    PRE_CUSTCODE_V = lq["PRE_CUSTCODE_V"].ToString(),
                                    PRE_Valor_V = lq["PRE_Valor_V"].ToString(),
                                    PRE_Concepto_V = lq["PRE_Concepto_V"].ToString(),
                                    PRE_Analista_V = lq["PRE_Analista_V"].ToString(),
                                    PRE_Periodo_Ajustado_Desde_V = lq["PRE_Periodo_Ajustado_Desde_V"].ToString(),
                                    PRE_Periodo_Ajustado_Hasta_V = lq["PRE_Periodo_Ajustado_Hasta_V"].ToString(),
                                    PRE_Aliado_V = lq["PRE_Aliado_V"].ToString(),
                                    PRE_Estado_V = lq["PRE_Estado_V"].ToString(),
                                    //PRE_Actualizacion_Anterior_V = lq["PRE_Actualizacion_Anterior_V"].ToString(),
                                    PRE_Ultima_Actualizacion_V = lq["PRE_Ultima_Actualizacion_V"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCasePospago(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_Postpago_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    BPB_MIN = lq["BPB_MIN"].ToString(),
                                    BPB_CUN = lq["BPB_CUN"].ToString(),
                                    BPB_CUSTCODE = lq["BPB_CUSTCODE"].ToString(),
                                    BPB_CUSTOMER_ID = lq["BPB_CUSTOMER_ID"].ToString(),
                                    BPB_AREA_QUE_GENERO_INCONSISTENCIA = lq["BPB_AREA_QUE_GENERO_INCONSISTENCIA"].ToString(),
                                    BPB_USUARIO_RESPONSABLE = lq["BPB_USUARIO_RESPONSABLE"].ToString(),
                                    BPB_ANALISTA = lq["BPB_ANALISTA"].ToString(),
                                    BPB_USER_RED = lq["BPB_USER_RED"].ToString(),
                                    BPB_TIPO_RECLAMO = lq["BPB_TIPO_RECLAMO"].ToString(),
                                    BPB_Valor = lq["BPB_Valor"].ToString(),
                                    BPB_Justificación = lq["BPB_Justificación"].ToString(),
                                    BPB_Periodo_ajustar_desde = lq["BPB_Periodo_ajustar_desde"].ToString(),
                                    BPB_Periodo_ajustar_Hasta = lq["BPB_Periodo_ajustar_Hasta"].ToString(),
                                    BPB_SERVICIO = lq["BPB_SERVICIO"].ToString(),
                                    BPB_CTA_CONTABLE = lq["BPB_CTA_CONTABLE"].ToString(),
                                    BPB_IVA = lq["BPB_IVA"].ToString(),
                                    BPB_GERENCIA = lq["BPB_GERENCIA"].ToString(),
                                    BPB_CENTRALES = lq["BPB_CENTRALES"].ToString(),
                                    BPB_ALIADO = lq["BPB_ALIADO"].ToString(),
                                    BPB_CAUSAL = lq["BPB_CAUSAL"].ToString(),
                                    BPB_ESTADO = lq["BPB_ESTADO"].ToString(),
                                    BPB_ACTUALIZACION_ANTERIOR = lq["BPB_ACTUALIZACION_ANTERIOR"].ToString(),
                                    BPB_ULTIMA_ACTUALIZACION = lq["BPB_ULTIMA_ACTUALIZACION"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCasePospago13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_Postpago13_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    BPB_MIN_V = lq["BPB_MIN_V"].ToString(),
                                    BPB_CUN_V = lq["BPB_CUN_V"].ToString(),
                                    BPB_CUSTCODE_V = lq["BPB_CUSTCODE_V"].ToString(),
                                    BPB_CUSTOMER_ID_V = lq["BPB_CUSTOMER_ID_V"].ToString(),
                                    BPB_AREA_QUE_GENERO_INCONSISTENCIA_V = lq["BPB_AREA_QUE_GENERO_INCONSISTENCIA_V"].ToString(),
                                    BPB_AREA_SOLICITA_AJUSTE_V = lq["BPB_AREA_SOLICITA_AJUSTE_V"].ToString(),
                                    BPB_ANALISTA_V = lq["BPB_ANALISTA_V"].ToString(),
                                    BPB_USER_RED_V = lq["BPB_USER_RED_V"].ToString(),
                                    BPB_USUARIO_AJUSTE_V = lq["BPB_USUARIO_AJUSTE_V"].ToString(),
                                    BPB_TIPO_RECLAMO_V = lq["BPB_TIPO_RECLAMO_V"].ToString(),
                                    BPB_Valor_V = lq["BPB_Valor_V"].ToString(),
                                    BPB_Justificación_V = lq["BPB_Justificación_V"].ToString(),
                                    BPB_Periodo_a_ajustar_desde_V = lq["BPB_Periodo_a_ajustar_desde_V"].ToString(),
                                    BPB_Hasta_V = lq["BPB_Hasta_V"].ToString(),
                                    BPB_SERVICIO_V = lq["BPB_SERVICIO_V"].ToString(),
                                    BPB_CTA_CONTABLE_V = lq["BPB_CTA_CONTABLE_V"].ToString(),
                                    BPB_IVA_V = lq["BPB_IVA_V"].ToString(),
                                    BPB_GERENCIA_V = lq["BPB_GERENCIA_V"].ToString(),
                                    BPB_CENTRALES_V = lq["BPB_CENTRALES_V"].ToString(),
                                    BPB_ALIADO_V = lq["BPB_ALIADO_V"].ToString(),
                                    BPB_CAUSAL_V = lq["BPB_CAUSAL_V"].ToString(),
                                    BPB_ESTADO_V = lq["BPB_ESTADO_V"].ToString(),
                                    //BPB_ACTUALIZACION_ANTERIOR_V = lq["BPB_ACTUALIZACION_ANTERIOR_V"].ToString(),
                                    BPB_ULTIMA_ACTUALIZACION_V = lq["BPB_ULTIMA_ACTUALIZACION_V"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCaseAscard(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_Ascard_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    Ascard1_Numero_Credito = lq["Ascard1_Numero_Credito"].ToString(),
                                    Ascard1_Valor_Ajuste = lq["Ascard1_Valor_Ajuste"].ToString(),
                                    Ascard1_Descripcion_Motivo_Ajuste = lq["Ascard1_Descripcion_Motivo_Ajuste"].ToString(),
                                    Ascard1_Area_Solicita_Ajuste = lq["Ascard1_Area_Solicita_Ajuste"].ToString(),
                                    Ascard1_Comentario = lq["Ascard1_Comentario"].ToString(),
                                    Ascard1_Documento_Usuario = lq["Ascard1_Documento_Usuario"].ToString(),
                                    Ascard1_Reclamo_del_Usuario = lq["Ascard1_Reclamo_del_Usuario"].ToString(),
                                    Ascard1_Custcode_Asociado_al_Crédito = lq["Ascard1_Custcode_Asociado_al_Crédito"].ToString(),
                                    Ascard1_Motivo_Ajuste = lq["Ascard1_Motivo_Ajuste"].ToString(),
                                    Ascard1_Nombre_de_Quien_Solicita = lq["Ascard1_Nombre_de_Quien_Solicita"].ToString(),
                                    Ascard1_CUN_NR = lq["Ascard1_CUN_NR"].ToString(),
                                    Ascard1_Tipo_Reclamo = lq["Ascard1_Tipo_Reclamo"].ToString(),
                                    Ascard1_Aliado = lq["Ascard1_Aliado"].ToString(),
                                    Ascard1_Estado = lq["Ascard1_Estado"].ToString(),
                                    Ascard1_Actualizacion_Anterior = lq["Ascard1_Actualizacion_Anterior"].ToString(),
                                    Ascard1_Ultima_Actualizacion = lq["Ascard1_Ultima_Actualizacion"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCaseAscard13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_Ascard13_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    Ascard1_Numero_Credito_V = lq["Ascard1_Numero_Credito_V"].ToString(),
                                    Ascard1_Valor_Ajuste_V = lq["Ascard1_Valor_Ajuste_V"].ToString(),
                                    Ascard1_Descripcion_Motivo_ajuste_V = lq["Ascard1_Descripcion_Motivo_ajuste_V"].ToString(),
                                    Ascard1_Area_Solicitante_V = lq["Ascard1_Area_Solicitante_V"].ToString(),
                                    Ascard1_Comentario_V = lq["Ascard1_Comentario_V"].ToString(),
                                    Ascard1_Documento_Usuario_V = lq["Ascard1_Documento_Usuario_V"].ToString(),
                                    Ascard1_Reclamo_del_Usuario_V = lq["Ascard1_Reclamo_del_Usuario_V"].ToString(),
                                    Ascard1_Custcode_Asociado_al_Crédito_V = lq["Ascard1_Custcode_Asociado_al_Crédito_V"].ToString(),
                                    Ascard1_Motivo_Ajuste_V = lq["Ascard1_Motivo_Ajuste_V"].ToString(),
                                    Ascard1_Nombre_de_Quien_Solicita_V = lq["Ascard1_Nombre_de_Quien_Solicita_V"].ToString(),
                                    Ascard1_CUN_NR_V = lq["Ascard1_CUN_NR_V"].ToString(),
                                    Ascard1_Tipo_Reclamo_V = lq["Ascard1_Tipo_Reclamo_V"].ToString(),
                                    Ascard1_Aliado_V = lq["Ascard1_Aliado_V"].ToString(),
                                    Ascard1_Estado_V = lq["Ascard1_Estado_V"].ToString(),
                                    Ascard1_Ultima_Actualizacion_V = lq["Ascard1_Ultima_Actualizacion_V"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCaseCuotasAscard(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_CuotasAscard_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    Ascard2_Numero_de_credito = lq["Ascard2_Numero_de_credito"].ToString(),
                                    Ascard2_Valor_Nueva_Cuota = lq["Ascard2_Valor_Nueva_Cuota"].ToString(),
                                    Ascard2_Concepto = lq["Ascard2_Concepto"].ToString(),
                                    Ascard2_Área_soliciita_ajuste = lq["Ascard2_Área_soliciita_ajuste"].ToString(),
                                    Ascard2_Reclamo_Usuario = lq["Ascard2_Reclamo_Usuario"].ToString(),
                                    Ascard2_Cantidad_cuotas = lq["Ascard2_Cantidad_cuotas"].ToString(),
                                    Ascard2_Aliado = lq["Ascard2_Aliado"].ToString(),
                                    Ascard2_Estado = lq["Ascard2_Estado"].ToString(),
                                    Ascard2_Actualizacion_Anterior = lq["Ascard2_Actualizacion_Anterior"].ToString(),
                                    Ascard2_Ultima_Actualizacion = lq["Ascard2_Ultima_Actualizacion"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCaseCuotasAscard13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_CuotasAscard13_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    Ascard2_Numero_de_credito_V = lq["Ascard2_Numero_de_credito_V"].ToString(),
                                    Ascard2_Valor_Nueva_Cuota_V = lq["Ascard2_Valor_Nueva_Cuota_V"].ToString(),
                                    Ascard2_Concepto_V = lq["Ascard2_Concepto_V"].ToString(),
                                    Ascard2_Área_soliciita_ajuste_V = lq["Ascard2_Área_soliciita_ajuste_V"].ToString(),
                                    Ascard2_Reclamo_Usuario_V = lq["Ascard2_Reclamo_Usuario_V"].ToString(),
                                    Ascard2_Cantidad_cuotas_V = lq["Ascard2_Cantidad_cuotas_V"].ToString(),
                                    Ascard2_Aliado_V = lq["Ascard2_Aliado_V"].ToString(),
                                    Ascard2_Estado_V = lq["Ascard2_Estado_V"].ToString(),
                                    //Ascard2_Actualizacion_Anterior_V = lq["Ascard2_Actualizacion_Anterior_V"].ToString(),
                                    Ascard2_Ultima_Actualizacion_V = lq["Ascard2_Ultima_Actualizacion_V"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCaseEliminacionCentrales(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_EliminarCentrales_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    CEN_CUN = lq["CEN_CUN"].ToString(),
                                    CEN_Tipo_Documento = lq["CEN_Tipo_Documento"].ToString(),
                                    CEN_Nombre = lq["CEN_Nombre"].ToString(),
                                    CEN_Cedula = lq["CEN_Cedula"].ToString(),
                                    CEN_CUSTCODE_O_No_CREDITO_A_ELIMINAR = lq["CEN_CUSTCODE_O_No_CREDITO_A_ELIMINAR"].ToString(),
                                    CEN_Estado = lq["CEN_Estado"].ToString(),
                                    CEN_Motivo_Eliminacion = lq["CEN_Motivo_Eliminacion"].ToString(),
                                    CEN_Analista = lq["CEN_Analista"].ToString(),
                                    CEN_Estado_Solicitud = lq["CEN_Estado_Solicitud"].ToString(),
                                    CEN_Ultima_Actualizacion = lq["CEN_Ultima_Actualizacion"].ToString()


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }

        public async static Task<List<CasosPqrMovilEscrita>> ListCaseEliminacionCentrales13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListData = new List<CasosPqrMovilEscrita>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_PQR_EliminarCentrales13_Lista";
                //if (ESTADO != null && !ESTADO.Equals(""))
                //{
                //    cmd.Parameters.AddWithValue("Status", Int32.Parse(ESTADO));
                //}
                if (Idsolutions != null && Idsolutions != "") cmd.Parameters.AddWithValue("IdSolutions", Idsolutions);
                if (Cuscode != null && Cuscode != "") cmd.Parameters.AddWithValue("Cuscode", Cuscode);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                //if (Fechafinal != null && Fechafinal != "") cmd.Parameters.AddWithValue("FechaFinal", Fechafinal);
                DataTable dt = await DAOConfig.SetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListData = (from lq in dt.Rows.Cast<DataRow>()
                                select new CasosPqrMovilEscrita()
                                {
                                    WorKOrder = lq["WorKOrder"].ToString(),
                                    CEN_CUN_V = lq["CEN_CUN_V"].ToString(),
                                    CEN_Tipo_Documento_V = lq["CEN_Tipo_Documento_V"].ToString(),
                                    CEN_Nombre_V = lq["CEN_Nombre_V"].ToString(),
                                    CEN_Cedula_V = lq["CEN_Cedula_V"].ToString(),
                                    CEN_CUSTCODE_O_No_CREDITO_A_ELIMINAR_V = lq["CEN_CUSTCODE_O_No_CREDITO_A_ELIMINAR_V"].ToString(),
                                    CEN_Estado_V = lq["CEN_Estado_V"].ToString(),
                                    CEN_Motivo_Eliminacion_V = lq["CEN_Motivo_Eliminacion_V"].ToString(),
                                    CEN_Analista_V = lq["CEN_Analista_V"].ToString(),
                                    CEN_Estado_Solicitud_V = lq["CEN_Estado_Solicitud_V"].ToString(),
                                    //CEN_Actualizacion_Anterior_V = lq["CEN_Actualizacion_Anterior_V"].ToString(),
                                    CEN_Ultima_Actualizacion_V = lq["CEN_Ultima_Actualizacion_V"].ToString(),


                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }

            return ListData;
        }
        #endregion

        /// CARGA DATA PQR PREPAGO
        public async static Task GuardarPQRPrepago(DataTable dtexcel)
        {
            try
            {
                //MasterUsers CurrentUser = await DAOGeneral.DataCurrentUser();
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_BasePortalZec";
                if (dtexcel.Rows.Count != 0)
                {
                    foreach (DataRow row in dtexcel.Rows)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdWorkOrder", row["WorKOrder"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR401", row["PRE_Radicado"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARLIST162", row["PRE_Tipo_Reclamo"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR401", row["PRE_Nombre_Titular"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR403", row["PRE_Min"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR404", row["PRE_CUSTCODE"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR405", row["PRE_Valor"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR406", row["PRE_Concepto"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR407", row["PRE_Analista"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_DATE86", row["PRE_Periodo_Ajustado_Desde"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_DATE87", row["PRE_Periodo_Ajustado_Hasta"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARLIST163", row["PRE_Aliado"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_VARCHAR408", row["PRE_Estado"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_DATE88", row["PRE_Actualizacion_Anterior"].ToString());
                        cmd.Parameters.AddWithValue("@UDF_DATE89", row["PRE_Ultima_Actualizacion"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}GuardarGestionPortalZEC", ex));
                throw ex;
            }
        }

        //area que genero ajuste 
        public List<Gerencia> ObtenerArea()
        {
            DDisposition oP = new DDisposition();
            DataTable dt = oP.ObtenerArea();
            List<Gerencia> lstArea = new List<Gerencia>();

            foreach (DataRow r in dt.Rows)
            {
                Gerencia objetoArea = new Gerencia();
                objetoArea.id = Convert.ToInt32(r["id"]);
                objetoArea.area_genero_ajuste = Convert.ToString(r["area_genero_ajuste"]);
                objetoArea.gerencia = Convert.ToString(r["gerencia"]);
                lstArea.Add(objetoArea);
            }

            return lstArea;
        }

        //area que genero ajuste 
        public List<Cta_Contable> ObtenerCuenta()
        {
            DDisposition oP = new DDisposition();
            DataTable dt = oP.ObtenerCuenta();
            List<Cta_Contable> lstCuenta = new List<Cta_Contable>();

            foreach (DataRow r in dt.Rows)
            {
                Cta_Contable objetoCuenta = new Cta_Contable();
                objetoCuenta.ID = Convert.ToString(r["id"]);
                objetoCuenta.cod_cuenta = Convert.ToString(r["cod_cuenta"]);
                objetoCuenta.servicio = Convert.ToString(r["servicio"]);
                objetoCuenta.iva = Convert.ToString(r["iva"]);
                lstCuenta.Add(objetoCuenta);
            }

            return lstCuenta;
        }

        //GESION BOE IBM
        public async static Task<List<GestionBOE>> GetGestionBOE(string NoSR)
        {
            List<GestionBOE> Gestion = new List<GestionBOE>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_SelGestionBOE";
                if (NoSR != null)
                    cmd.Parameters.AddWithValue("NoSR", NoSR);

                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Gestion = (from Linq in dt.Rows.Cast<DataRow>()
                               select new GestionBOE()
                               {
                                   IdGestionBOE = Linq.IsNull("IdGestionBOE") ? default(int) : Linq.Field<int>("IdGestionBOE"),
                                   NoSR = Linq.IsNull("NoSR") ? default(string) : Linq.Field<string>("NoSR"),
                                   Segmento = Linq.IsNull("Segmento") ? default(int) : Linq.Field<int>("Segmento"),
                                   Tipologia = Linq.IsNull("Tipologia") ? default(int) : Linq.Field<int>("Tipologia"),
                                   Estado = Linq.IsNull("Estado") ? default(int) : Linq.Field<int>("Estado"),
                                   FechaActualizacionEstado = Linq.IsNull("FechaActualizacionEstado") ? default(DateTime) : Linq.Field<DateTime>("FechaActualizacionEstado"),
                                   SeguimientoPendiente = Linq.IsNull("SeguimientoPendiente") ? default(int) : Linq.Field<int>("SeguimientoPendiente"),
                                   FechaCreacion = Linq.IsNull("FechaCreacion") ? default(DateTime) : Linq.Field<DateTime>("FechaCreacion"),
                                   FechaEnvioACampo = Linq.IsNull("FechaEnvioACampo") ? default(DateTime) : Linq.Field<DateTime>("FechaEnvioACampo"),
                                   FechaProximaAccion = Linq.IsNull("FechaProximaAccion") ? default(DateTime) : Linq.Field<DateTime>("FechaProximaAccion"),
                                   FechaBackOffice = Linq.IsNull("FechaBackOffice") ? default(DateTime) : Linq.Field<DateTime>("FechaBackOffice"),
                                   DetalleGestion = Linq.IsNull("DetalleGestion") ? default(int) : Linq.Field<int>("DetalleGestion"),
                                   Canal = Linq.IsNull("Canal") ? default(int) : Linq.Field<int>("Canal"),
                                   Observaciones = Linq.IsNull("Observaciones") ? default(string) : Linq.Field<string>("Observaciones"),
                                   RecordCreationDate = Linq.IsNull("RecordCreationDate") ? default(DateTime) : Linq.Field<DateTime>("RecordCreationDate"),
                                   RecordCreationUser = Linq.IsNull("RecordCreationUser") ? default(string) : Linq.Field<string>("RecordCreationUser")

                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.ListDispositions({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return Gestion;
        }

        public async static Task<List<DiasHabiles>> DiasHabiles(int DiasHabiles)
        {
            List<DiasHabiles> ListDisp = new List<DiasHabiles>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_ListDiasHabiles";
                cmd.Parameters.AddWithValue("DiasHabiles", DiasHabiles);

                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListDisp = (from Linq in dt.Rows.Cast<DataRow>()
                                select new DiasHabiles()
                                {
                                    Fecha = Linq["Fecha"].ToString(),
                                    NombreDia = Linq["NombreDia"].ToString()

                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                string Error = $"TemplatesControllers.DAOCommand.ListDispositions({Tools.GetLineErr(ex)}): {ex.Message}";

                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListDisp;
        }

        public async static Task SaveGestionBOE(GestionBOE gestionBOE)
        {
            try
            {
                Users UserActual = await InforUserActual(true, true);
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Guardar_GestionBOE";
                cmd.Parameters.AddWithValue("NoSR", gestionBOE.NoSR == "" ? DBNull.Value : (object)gestionBOE.NoSR);
                cmd.Parameters.AddWithValue("Segmento", gestionBOE.Segmento == default(int) ? DBNull.Value : (object)gestionBOE.Segmento);
                cmd.Parameters.AddWithValue("Tipologia", gestionBOE.Tipologia == default(int) ? DBNull.Value : (object)gestionBOE.Tipologia);
                cmd.Parameters.AddWithValue("Estado", gestionBOE.Estado == default(int) ? DBNull.Value : (object)gestionBOE.Estado);
                cmd.Parameters.AddWithValue("FechaActualizacionEstado", gestionBOE.FechaActualizacionEstado == default(DateTime) ? DBNull.Value : (object)gestionBOE.FechaActualizacionEstado);
                cmd.Parameters.AddWithValue("SeguimientoPendiente", gestionBOE.SeguimientoPendiente == default(int) ? DBNull.Value : (object)gestionBOE.SeguimientoPendiente);
                cmd.Parameters.AddWithValue("FechaCreacion", gestionBOE.FechaCreacion == default(DateTime) ? DBNull.Value : (object)gestionBOE.FechaCreacion);
                cmd.Parameters.AddWithValue("FechaEnvioACampo", gestionBOE.FechaEnvioACampo == default(DateTime) ? DBNull.Value : (object)gestionBOE.FechaEnvioACampo);
                cmd.Parameters.AddWithValue("FechaProximaAccion", gestionBOE.FechaProximaAccion == default(DateTime) ? DBNull.Value : (object)gestionBOE.FechaProximaAccion);
                cmd.Parameters.AddWithValue("FechaBackOffice", gestionBOE.FechaBackOffice == default(DateTime) ? DBNull.Value : (object)gestionBOE.FechaBackOffice);
                cmd.Parameters.AddWithValue("DetalleGestion", gestionBOE.DetalleGestion == default(int) ? DBNull.Value : (object)gestionBOE.DetalleGestion);
                cmd.Parameters.AddWithValue("Canal", gestionBOE.Canal == default(int) ? DBNull.Value : (object)gestionBOE.Canal);
                cmd.Parameters.AddWithValue("Observaciones", gestionBOE.Observaciones == "" ? DBNull.Value : (object)gestionBOE.Observaciones);
                cmd.Parameters.AddWithValue("RecordCreationUser", UserActual.Winuser);
                await DAOConfig.SetDataTableExecuteCommand(cmd);
            }
            catch (Exception ex)
            {
                string Error = $"{ConstProject.NameProject}:<br/>DisabledDataControllers.DAOCommand.DisabledDataImport({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
        }


        //Tabla temporal 

        //public async static Task<List<ReclamoDatacredito>> SelTablaTemporal()
        public static List<ReclamoDatacredito> SelTablaTemporal()
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            List<ReclamoDatacredito> data = new List<ReclamoDatacredito>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_ReclamoDatacredito", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ReclamoDatacredito()
                            {
                                // datos del cliente 
                                //Id = Convert.ToInt32(dr["Id"].ToString()),
                                Numero_ID = dr["Numero_ID"].ToString(),
                                Tipo_Identificacion = dr["Tipo_Identificacion"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Entidad = dr["Entidad"].ToString(),
                                NIT = dr["NIT"].ToString(),
                                No_Cuenta = dr["No_Cuenta"].ToString(),
                                No_Reclamo = dr["No_Reclamo"].ToString(),
                                Reclamo_Entidad = dr["Reclamo_Entidad"].ToString(),
                                Estado = dr["Estado"].ToString(),
                                Tipo_Reclamo = dr["Tipo_Reclamo"].ToString(),
                                Subtipo_Reclamo = dr["Subtipo_Reclamo"].ToString(),
                                Leyenda_Reclamo = dr["Leyenda_Reclamo"].ToString(),
                                Tipo_Solucion = dr["Tipo_Solucion"].ToString(),
                                Fecha_Colocacion = dr["Fecha_Colocacion"].ToString(),
                                Fecha_Aplicacion = dr["Fecha_Aplicacion"].ToString(),
                                Canal = dr["Canal"].ToString(),
                                Origen = dr["Origen"].ToString(),
                                Empresa_Origen = dr["Empresa_Origen"].ToString(),
                                Estado_Robot = dr["Estado_Robot"].ToString(),
                                //EstadoProceso = dr["EstadoProceso"].ToString(),
                                //Observacion = dr["Observacion"].ToString(),
                                //Fecha_Procesamiento = Convert.ToDateTime(dr["Fecha_Procesamiento"].ToString()),
                                //HostName = dr["HostName"].ToString(),
                                //WinUser = dr["WinUser"].ToString(),


                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }

        public static List<ReclamoDatacredito> SelTabla()
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            List<ReclamoDatacredito> data = new List<ReclamoDatacredito>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_ReclamoDatacredito_prod", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ReclamoDatacredito()
                            {
                                // datos del cliente 
                                //Id = Convert.ToInt32(dr["Id"].ToString()),
                                Numero_ID = dr["Numero_ID"].ToString(),
                                Tipo_Identificacion = dr["Tipo_Identificacion"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Entidad = dr["Entidad"].ToString(),
                                NIT = dr["NIT"].ToString(),
                                No_Cuenta = dr["No_Cuenta"].ToString(),
                                No_Reclamo = dr["No_Reclamo"].ToString(),
                                Reclamo_Entidad = dr["Reclamo_Entidad"].ToString(),
                                Estado = dr["Estado"].ToString(),
                                Tipo_Reclamo = dr["Tipo_Reclamo"].ToString(),
                                Subtipo_Reclamo = dr["Subtipo_Reclamo"].ToString(),
                                Leyenda_Reclamo = dr["Leyenda_Reclamo"].ToString(),
                                Tipo_Solucion = dr["Tipo_Solucion"].ToString(),
                                Fecha_Colocacion = dr["Fecha_Colocacion"].ToString(),
                                Fecha_Aplicacion = dr["Fecha_Aplicacion"].ToString(),
                                Canal = dr["Canal"].ToString(),
                                Origen = dr["Origen"].ToString(),
                                Empresa_Origen = dr["Empresa_Origen"].ToString(),
                                Estado_Robot = dr["Estado_Robot"].ToString(),
                                Estad = dr["Estad"].ToString(),
                                SubEstad = dr["SubEstad"].ToString(),
                                //Fecha_Procesamiento = Convert.ToDateTime(dr["Fecha_Procesamiento"].ToString()),
                                //HostName = dr["HostName"].ToString(),
                                //WinUser = dr["WinUser"].ToString(),


                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }

        //Dispositions para reclamo datacredito
        public List<Disposition> ObtenerEstado()
        {
            DDisposition oP = new DDisposition();
            DataTable dt = oP.ObtenerEstado();
            List<Disposition> lst = new List<Disposition>();

            foreach (DataRow r in dt.Rows)
            {
                Disposition objetoCuenta = new Disposition();
                objetoCuenta.Id = Convert.ToInt32(r["Id"]);
                objetoCuenta.Descripcion = Convert.ToString(r["Descripcion"]);
                //objetoCuenta. = Convert.ToString(r["servicio"]);
                //objetoCuenta.iva = Convert.ToString(r["iva"]);
                lst.Add(objetoCuenta);
            }

            return lst;
        }

        public List<Disposition> ObtenerAllEstados()
        {
            DDisposition oP = new DDisposition();
            DataTable dt = oP.ObtenerAllEstados();
            List<Disposition> lst = new List<Disposition>();

            foreach (DataRow r in dt.Rows)
            {
                Disposition objetoCuenta = new Disposition();
                objetoCuenta.Id = Convert.ToInt32(r["Id"]);
                objetoCuenta.Descripcion = Convert.ToString(r["Descripcion"]);
                objetoCuenta.Estado = Convert.ToBoolean(r["Estado"]);
                objetoCuenta.Subestado = Convert.ToBoolean(r["Subestado"]);
                objetoCuenta.state = Convert.ToBoolean(r["state"]);
                objetoCuenta.FechaCreacion = Convert.ToDateTime(r["FechaCreacion"]);
                //objetoCuenta. = Convert.ToString(r["servicio"]);
                //objetoCuenta.iva = Convert.ToString(r["iva"]);
                lst.Add(objetoCuenta);
            }

            return lst;
        }

        public List<Disposition> ObtenerSubEstado()
        {
            DDisposition oP = new DDisposition();
            DataTable dt = oP.ObtenerSubEstado();
            List<Disposition> lst1 = new List<Disposition>();

            foreach (DataRow r in dt.Rows)
            {
                Disposition objetoCuenta = new Disposition();
                objetoCuenta.Id = Convert.ToInt32(r["Id"]);
                objetoCuenta.Descripcion = Convert.ToString(r["Descripcion"]);
                //objetoCuenta. = Convert.ToString(r["servicio"]);
                //objetoCuenta.iva = Convert.ToString(r["iva"]);
                lst1.Add(objetoCuenta);
            }

            return lst1;
        }

        public async static Task<bool> SaveTablaProduccion()
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Ins_ReclamoDatacredito_prod", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return resp;
        }
        public async static Task<bool> TruncateTable()
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = "truncate table [Tbl_ReclamoDatacredito]",
                    CommandType = CommandType.Text,
                    CommandTimeout = 3600
                };

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return resp;
        }


        public static string EsAgente(Users InforUser)
        {
            string result = null;
            foreach (var item in InforUser.Perfiles)
            {
                if (item.NameProfile.ToUpper().Contains("DATACREDITO"))
                {
                    result = "DATACREDITO"; //AGENTE
                }
                else if (item.NameProfile.ToUpper().Contains("BACKOFFICE"))
                {
                    result = "BACKOFFICE"; //CARGADATA
                }
                else if (item.NameProfile.ToUpper().Contains("SUPERCENTRALES"))
                {
                    result = "SUPERCENTRALES"; //GESTION DE ESTADOS
                }
                else if (item.NameProfile.ToUpper().Contains("SUPERCOLOCACION"))
                {
                    result = "SUPERCOLOCACION"; //supervisor de colocacion de presatamos
                }
            }
            return result;
        }

        public async static Task UpdateEstado(ReclamoDatacredito Reclamo)
        {
            //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
            bool Finaliza = false;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
                SqlCommand cmd = new SqlCommand("spr_Upd_ReclamoDatacredito_prod", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("No_Reclamo", Reclamo.No_Reclamo);
                cmd.Parameters.AddWithValue("Estad", Reclamo.Estad);
                if (Reclamo.SubEstad == "" || Reclamo.SubEstad == null || Reclamo.Estad == "Eliminación" || Reclamo.Estad == "Ratificar")
                {
                    cmd.Parameters.AddWithValue("SubEstad", "0");
                }
                else
                {
                    cmd.Parameters.AddWithValue("SubEstad", Reclamo.SubEstad);
                }
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}SaveTemplate", ex, Template));
            }
        }
        //public async static Task gestionarEstado(int? IdMasterSites = null)
        //{
        //    //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
        //    bool Finaliza = false;
        public async static Task<List<Disposition>> gestionarEstado(int? IdEstado = null)
        {
            List<Disposition> ListEstados = new List<Disposition>();
            try
            {
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Sel_AllEstado_Reclamo";

                if (IdEstado != null) cmd.Parameters.AddWithValue("Id", IdEstado);
                DataTable dt = await DAOConfig.GetDataTableExecuteCommand(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ListEstados = (from Linq in dt.Rows.Cast<DataRow>()
                                   select new Disposition()
                                   {
                                       Id = int.Parse(Linq["Id"].ToString()),
                                       Descripcion = Linq["Descripcion"].ToString(),
                                       Estado = bool.Parse(Linq["Subestado"].ToString()),
                                       Subestado = bool.Parse(Linq["Subestado"].ToString()),
                                       //FechaCreacion = Linq["FechaCreacion"].ToString(),
                                       state = bool.Parse(Linq["state"].ToString())
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}SaveTemplate", ex, Template));
            }
            return ListEstados;
        }

        public async static Task AddEstado(string Descripcion = null, bool? Estado = null)
        {
            //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
            bool Finaliza = false;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_INS_ReclamoEstado", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Descripcion", Descripcion);
                //cmd.Parameters.AddWithValue("Estado", Estado);
                //cmd.Parameters.AddWithValue("Subestado", Subestado);
                if (Estado == true)
                {
                    cmd.Parameters.AddWithValue("Estado", 1);
                    cmd.Parameters.AddWithValue("Subestado", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("Estado", 0);
                    cmd.Parameters.AddWithValue("Subestado", 1);
                }
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}SaveTemplate", ex, Template));
            }
        }

        public async static Task ActualizarEstado(int? Id = 0, string Descripcion = null, bool? state = null)
        {
            //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
            bool Finaliza = false;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_Upd_Estado_Reclamo", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Descripcion", Descripcion);
                //cmd.Parameters.AddWithValue("Estado", Estado);
                //cmd.Parameters.AddWithValue("Subestado", Subestado);
                if (state == true)
                {
                    cmd.Parameters.AddWithValue("state", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("state", 0);
                }
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}SaveTemplate", ex, Template));
            }
        }

        public async static Task AddListWorkOrder(long IdMasterUsers, ReclamoDatacredito Reclamo)
        {
            bool resp = true;
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_Ins_ReclamoDatacredito_ListWorkOrder", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("No_Reclamo", Reclamo.No_Reclamo);
                cmd.Parameters.AddWithValue("IdMasterUser", IdMasterUsers);
                //cmd.Parameters.AddWithValue("IdMasterUserAssigned", IdMasterUsers);
                //cmd.Parameters.AddWithValue("IdMasterUserScaled", IdMasterUsers);
                cmd.CommandTimeout = 60;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //resp = false;
            }

        }


        /// colocacion de preestamos
        /// 

        public static List<ColocacionPrestamo> SelTabla_ColocacionPrestamo(int? Id)
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MISConnection"].ConnectionString);
            List<ColocacionPrestamo> data = new List<ColocacionPrestamo>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_ColocacionPrestamo_prod", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", Id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ColocacionPrestamo()
                            {
                                // datos del cliente 
                                Id = Convert.ToInt32(dr["Id"].ToString()),
                                Identificacion = dr["Identificacion"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Email = dr["Email"].ToString(),
                                Especialidades = dr["Especialidades"].ToString(),
                                CiudadResidencia = dr["CiudadResidencia"].ToString(),
                                TipoCredito = dr["TipoCredito"].ToString(),
                                Tipificacion1 = dr["Tipificacion1"].ToString(),
                                Tipificacion2 = dr["Tipificacion2"].ToString(),
                                Tipificacion3 = dr["Tipificacion3"].ToString(),
                                Tipificacion4 = dr["Tipificacion4"].ToString(),
                                NombreArchivo = dr["NombreArchivo"].ToString(),
                                DataActiva = dr["DataActiva"].ToString(),
                                FechaCreacion = dr["FechaCreacion"].ToString(),
                                FechaCarga = dr["FechaCarga"].ToString(),
                                FechaModificacion = dr["FechaModificacion"].ToString(),
                                HostName = dr["HostName"].ToString(),
                                winuser = dr["winuser"].ToString(),
                                ingresos = dr["ingresos"].ToString(),
                                Valor_interesado = dr["Valor_interesado"].ToString(),
                                Valor_cuota = dr["Valor_cuota"].ToString(),
                                Plazo = dr["Plazo"].ToString(),
                                FechaLLamada = dr["FechaLLamada"].ToString(),
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }

        public static List<ColocacionPrestamo> SelTabla_ColocacionPrestamoId(int Id)
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MISConnection"].ConnectionString);
            List<ColocacionPrestamo> data = new List<ColocacionPrestamo>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_ColocacionPrestamo_prod", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", Id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ColocacionPrestamo()
                            {
                                // datos del cliente 
                                Id = Convert.ToInt32(dr["Id"].ToString()),
                                Identificacion = dr["Identificacion"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Email = dr["Email"].ToString(),
                                Especialidades = dr["Especialidades"].ToString(),
                                CiudadResidencia = dr["CiudadResidencia"].ToString(),
                                TipoCredito = dr["TipoCredito"].ToString(),
                                Tipificacion1 = dr["Tipificacion1"].ToString(),
                                Tipificacion2 = dr["Tipificacion2"].ToString(),
                                Tipificacion3 = dr["Tipificacion3"].ToString(),
                                Tipificacion4 = dr["Tipificacion4"].ToString(),
                                NombreArchivo = dr["NombreArchivo"].ToString(),
                                DataActiva = dr["DataActiva"].ToString(),
                                FechaCreacion = dr["FechaCreacion"].ToString(),
                                FechaCarga = dr["FechaCarga"].ToString(),
                                FechaModificacion = dr["FechaModificacion"].ToString(),
                                HostName = dr["HostName"].ToString(),
                                winuser = dr["winuser"].ToString(),

                                FechaLLamada = dr["FechaLLamada"].ToString(),


                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }

        //SelDispTipoCredito
        public static List<ColocacionPrestamo> SelDispTipoCredito(int? Id = null)
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            List<ColocacionPrestamo> data = new List<ColocacionPrestamo>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_DispColocacionPrestamo", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Controller", "ColocacionPrestamo");
                //if (Id != null) cmd.Parameters.AddWithValue("NombreSelect", "Tipo de Credito");
                if (Id != null) cmd.Parameters.AddWithValue("Id", Id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ColocacionPrestamo()
                            {
                                // datos del cliente 
                                Id = Convert.ToInt32(dr["Id"].ToString()),
                                Descripcion = dr["Descripcion"].ToString(),
                                Controller = dr["Controller"].ToString(),
                                NombreSelect = dr["NombreSelect"].ToString(),
                                DateLog = Convert.ToDateTime(dr["DateLog"].ToString()),
                                state = Convert.ToBoolean(dr["state"].ToString())


                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }
        public static List<ColocacionPrestamo> SelDispTipoCreditoId(ColocacionPrestamo colocacion)
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
            List<ColocacionPrestamo> data = new List<ColocacionPrestamo>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_DispColocacionPrestamo", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Controller", colocacion.Controller);
                //cmd.Parameters.AddWithValue("NombreSelect", colocacion.NombreSelect);
                if (colocacion.Id != null) cmd.Parameters.AddWithValue("Id", colocacion.Id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ColocacionPrestamo()
                            {
                                // datos del cliente 
                                Id = Convert.ToInt32(dr["Id"].ToString()),
                                Descripcion = dr["Descripcion"].ToString(),
                                Controller = dr["Controller"].ToString(),
                                NombreSelect = dr["NombreSelect"].ToString(),
                                DateLog = Convert.ToDateTime(dr["DateLog"].ToString()),
                                state = Convert.ToBoolean(dr["state"].ToString())


                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }

        public async static Task ActualizarDisposition(ColocacionPrestamo colocacion)
        {
            //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
            bool Finaliza = false;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_Upd_Disposition_ColocacionPrestamo", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", colocacion.Id);
                cmd.Parameters.AddWithValue("Controller", colocacion.Controller);
                cmd.Parameters.AddWithValue("NombreSelect", colocacion.NombreSelect);
                cmd.Parameters.AddWithValue("Descripcion", colocacion.Descripcion);
                if (colocacion.state == true)
                {
                    cmd.Parameters.AddWithValue("state", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("state", 0);
                }
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}SaveTemplate", ex, Template));
            }
        }

        public async static Task ActualizarRegistro(ColocacionPrestamo Registro)
        {
            //Se crea la transaccion y esta solo finaliza cuando Finaliza = true
            bool Finaliza = false;
            try
            {


                string Machine = Environment.MachineName;
                string user = HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MISConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("spr_Upd_Scare_ColocacionPrestamo", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", Registro.Id);
                cmd.Parameters.AddWithValue("TipoCredito", Registro.TipoCredito);
                cmd.Parameters.AddWithValue("Tipificacion1", Registro.Tipificacion1);
                if (Registro.Tipificacion2 != null) cmd.Parameters.AddWithValue("Tipificacion2", Registro.Tipificacion2);
                if (Registro.Tipificacion3 != null) cmd.Parameters.AddWithValue("Tipificacion3", Registro.Tipificacion3);
                if (Registro.Tipificacion4 != null) cmd.Parameters.AddWithValue("Tipificacion4", Registro.Tipificacion4);
                cmd.Parameters.AddWithValue("HostName", Machine);
                cmd.Parameters.AddWithValue("winuser", user);
                if (Registro.ingresos != null) cmd.Parameters.AddWithValue("ingresos", Registro.ingresos);
                if (Registro.Valor_interesado != null) cmd.Parameters.AddWithValue("Valor_interesado", Registro.Valor_interesado);
                if (Registro.Valor_cuota != null) cmd.Parameters.AddWithValue("Valor_cuota", Registro.Valor_cuota);
                if (Registro.Plazo != null) cmd.Parameters.AddWithValue("Plazo", Registro.Plazo);
                if (Registro.FechaLLamada != null)
                {
                    string f_marcatemporal = Registro.FechaLLamada.ToString().Split('T').ElementAt(0);
                    string hora = Registro.FechaLLamada.ToString().Split('T').ElementAt(1);
                    cmd.Parameters.AddWithValue("FechaLLamada", f_marcatemporal + " " + hora);
                }
                else
                {
                    cmd.Parameters.AddWithValue("FechaLLamada", "01/01/2000 12:00");
                }
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}SaveTemplate", ex, Template));
            }
        }

        public async static Task AddDisposition(long IdMasterUsers, ColocacionPrestamo colocacion)
        {
            bool resp = true;
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_Ins_ColocacionPrestamo_Disposition", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Controller", colocacion.Controller);
                cmd.Parameters.AddWithValue("Descripcion", colocacion.Descripcion);
                cmd.Parameters.AddWithValue("NombreSelect", colocacion.NombreSelect);
                //cmd.Parameters.AddWithValue("IdMasterUserAssigned", IdMasterUsers);
                //cmd.Parameters.AddWithValue("IdMasterUserScaled", IdMasterUsers);
                cmd.CommandTimeout = 60;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //resp = false;
            }

        }

        public static List<ColocacionPrestamo> SelTabla_ColocacionPrestamo_not()
        {
            bool resp = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MISConnection"].ConnectionString);
            List<ColocacionPrestamo> data = new List<ColocacionPrestamo>();
            try
            {
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_ColocacionPrestamo_prod", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from dr in dt.Rows.Cast<DataRow>()
                            select new ColocacionPrestamo()
                            {
                                // datos del cliente 
                                Id = Convert.ToInt32(dr["Id"].ToString()),
                                Identificacion = dr["Identificacion"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Email = dr["Email"].ToString(),
                                Especialidades = dr["Especialidades"].ToString(),
                                CiudadResidencia = dr["CiudadResidencia"].ToString(),
                                TipoCredito = dr["TipoCredito"].ToString(),
                                Tipificacion1 = dr["Tipificacion1"].ToString(),
                                Tipificacion2 = dr["Tipificacion2"].ToString(),
                                Tipificacion3 = dr["Tipificacion3"].ToString(),
                                Tipificacion4 = dr["Tipificacion4"].ToString(),
                                NombreArchivo = dr["NombreArchivo"].ToString(),
                                DataActiva = dr["DataActiva"].ToString(),
                                FechaCreacion = dr["FechaCreacion"].ToString(),
                                FechaCarga = dr["FechaCarga"].ToString(),
                                FechaModificacion = dr["FechaModificacion"].ToString(),
                                HostName = dr["HostName"].ToString(),
                                winuser = dr["winuser"].ToString(),
                                ingresos = dr["ingresos"].ToString(),
                                Valor_interesado = dr["Valor_interesado"].ToString(),
                                Valor_cuota = dr["Valor_cuota"].ToString(),
                                Plazo = dr["Plazo"].ToString(),
                                FechaLLamada = dr["FechaLLamada"].ToString(),

                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return data;
        }

        public static List<ColocacionPrestamo> ListAlertasColocacion()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MISConnection"].ConnectionString);
            List<ColocacionPrestamo> ListAlertColocacion = new List<ColocacionPrestamo>();
            try
            {
                //var AllOrders = await ListPermisos(UserActual.Perfiles, 5);

                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4);
                SqlCommand cmd = new SqlCommand("Sp_Sel_ColocacionPrestamo_Alert", con);//listo
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                if (dt != null && dt.Rows.Count > 0)
                {

                    ListAlertColocacion = (from dr in dt.Rows.Cast<DataRow>()
                                           select new ColocacionPrestamo()
                                           {


                                               Id = Convert.ToInt32(dr["Id"].ToString()),
                                               NombreCompleto = dr["NombreCompleto"].ToString(),
                                               FechaLLamada = dr["FechaLLamada"].ToString()

                                               // datos del cliente 
                                               //Id = Convert.ToInt32(dr["Id"].ToString()),
                                               //            Identificacion = dr["Identificacion"].ToString(),
                                               //            NombreCompleto = dr["NombreCompleto"].ToString(),
                                               //            Telefono = dr["Telefono"].ToString(),
                                               //            Email = dr["Email"].ToString(),
                                               //            Especialidades = dr["Especialidades"].ToString(),
                                               //            CiudadResidencia = dr["CiudadResidencia"].ToString(),
                                               //            TipoCredito = dr["TipoCredito"].ToString(),
                                               //            Tipificacion1 = dr["Tipificacion1"].ToString(),
                                               //            Tipificacion2 = dr["Tipificacion2"].ToString(),
                                               //            Tipificacion3 = dr["Tipificacion3"].ToString(),
                                               //            Tipificacion4 = dr["Tipificacion4"].ToString(),
                                               //            NombreArchivo = dr["NombreArchivo"].ToString(),
                                               //            DataActiva = dr["DataActiva"].ToString(),
                                               //            FechaCreacion = dr["FechaCreacion"].ToString(),
                                               //            FechaCarga = dr["FechaCarga"].ToString(),
                                               //            FechaModificacion = dr["FechaModificacion"].ToString(),
                                               //            HostName = dr["HostName"].ToString(),
                                               //            winuser = dr["winuser"].ToString(),
                                               //            ingresos = dr["ingresos"].ToString(),
                                               //            Valor_interesado = dr["Valor_interesado"].ToString(),
                                               //            Valor_cuota = dr["Valor_cuota"].ToString(),
                                               //            Plazo = dr["Plazo"].ToString(),
                                               //            FechaLLamada = dr["FechaLLamada"].ToString(),

                                           }).ToList();

                }
            }
            catch (Exception ex)
            {
                string Error = $"ListWorkOrdersCotrollers.DAOCommand.ListWorkOrder({Tools.GetLineErr(ex)}): {ex.Message}";
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
                throw new Exception(Error);
            }
            return ListAlertColocacion;
        }

    }
}







