using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;
using DXC.TFSM.Business.Model;

namespace DXC.TFSM.Business
{
    public class BssCars
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();
        readonly DxcSitefinityToyotaEntities Sitefinity = new DxcSitefinityToyotaEntities();
        
        public List<tbl_portal_C_Auto> GetAll()
        {
            //Business Logic here:
            //
            //*
            try
            {
                return tFSM_PortalEntities.tbl_portal_C_Auto.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public async Task<List<tbl_portal_C_Auto>> GetAllAsync()
        {
            var c = tFSM_PortalEntities.tbl_portal_C_Auto.ToList();
            await Task.Delay(2000);
            return c;
        }

        public List<USP_SELECT_CAT_AUTOS_Result> GetCatFromSitefinity(string carpetaRaiz, string subcarpeta)
        {
            return Sitefinity.USP_SELECT_CAT_AUTOS(carpetaRaiz, subcarpeta).ToList();
        }

        public Task<List<Cars>> GetListCars()
        {
            var lstAutos = GetCatFromSitefinity("home-autos", "auto");
            var lstModeloAutos = GetCatFromSitefinity("home-autos", "modelo");
            List<Cars> lstTmp = new List<Cars>();
            List<Cars> lstAutosPortal = new List<Cars>();

            //Llena una lista con los datos de las imagenes de autos cargadas en sitefinity.
            foreach (var auto in lstAutos)
            {
                foreach (var modelo in lstModeloAutos)
                {
                    if (auto.title_ == modelo.title_)
                    {
                        Cars newCar = new Cars();
                        newCar.Id = 0;
                        newCar.Auto = auto.title_;
                        newCar.PathCarImage = auto.item_default_url_;
                        newCar.PathNameImage = modelo.item_default_url_;
                        newCar.IsMatched = true;
                        newCar.Carpet = auto.SUBCARPETA;
                        lstTmp.Add(newCar);
                        //break;
                    }
                }
            }

            //Llena una lista de los autos cargados en el portal
            var lst = GetAll();

            foreach (var autoPortal in lst)
            {
                foreach (var autoSitefinity in lstTmp)
                {
                    if (autoPortal.des_auto.ToUpper() == autoSitefinity.Auto.ToUpper())
                    {
                        Cars auto = new Cars();
                        auto.Auto = autoPortal.des_auto;
                        auto.Id = autoPortal.id_auto;
                        auto.IsMatched = true;
                        auto.PathCarImage = autoSitefinity.PathCarImage;
                        auto.PathNameImage = autoSitefinity.PathNameImage;
                        lstAutosPortal.Add(auto);
                    }
                }
            }
            
            return Task.FromResult(lstAutosPortal.OrderBy(a=> a.Carpet).ToList());
        }

        public List<tbl_portal_C_Modelos> GetVersionById(int id) {
            //return tFSM_PortalEntities.tbl_portal_C_Modelos.Where(v => v.id_auto == id && (v.descripcion == DateTime.Now.AddYears(-1).Year.ToString() ||  v.descripcion == DateTime.Now.Year.ToString() || v.descripcion == DateTime.Now.AddYears(1) .Year .ToString())).ToList();
            return tFSM_PortalEntities.tbl_portal_C_Modelos.Where(v => v.id_auto == id).ToList();
        }
    }
}
