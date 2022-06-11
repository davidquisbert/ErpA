using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Logica
{
    public class LLogica<E> : LLogicaLogica<ERPBDEntities, E>
    {
        protected override ERPBDEntities GetEsquema()
        {
            return new ERPBDEntities();
        }

        private static LLogica<E> logica;

        private LUsuario lUsuario;
        private LEmpresa lEmpresa;
        private LGestion lGestion;
        private LPeriodo lPeriodo;
        private LArticulo lArticulo;
        private LCuenta lCuenta;
        private LCategoria lCategoria;
        private LMoneda lMoneda;
        private LComprobante lComprobante;
        private LNota lNota;


        public static LLogica<E> Logica
        {
            get
            {
                if (logica == null)
                {
                    logica = new LLogica<E>();
                }
                return logica;
            }
        }

        public LUsuario LUsuario
        {
            get
            {
                if (lUsuario == null)
                {
                    lUsuario = new LUsuario();
                }
                return lUsuario;
            }
        }

        public LEmpresa LEmpresa
        {
            get
            {
                if (lEmpresa == null)
                {
                    lEmpresa = new LEmpresa();
                }
                return lEmpresa;
            }
        }

        public LGestion LGestion
        {
            get
            {
                if (lGestion == null)
                {
                    lGestion = new LGestion();
                }
                return lGestion;
            }
        }

        public LPeriodo LPeriodo
        {
            get
            {
                if (lPeriodo == null)
                {
                    lPeriodo = new LPeriodo();
                }
                return lPeriodo;
            }
        }
        public LArticulo LArticulo
        {
            get
            {
                if (lArticulo == null)
                {
                    lArticulo = new LArticulo();
                }
                return lArticulo;
            }
        }
        public LCuenta LCuenta
        {
            get
            {
                if (lCuenta == null)
                {
                    lCuenta = new LCuenta();
                }
                return lCuenta;
            }
        }
        public LCategoria LCategoria
        {
            get
            {
                if (lCategoria == null)
                {
                    lCategoria = new LCategoria();
                }
                return lCategoria;
            }
        }
        public LMoneda LMoneda
        {
            get
            {
                if (lMoneda == null)
                {
                    lMoneda = new LMoneda();
                }
                return lMoneda;
            }
        }
        public LComprobante LComprobante
        {
            get
            {
                if (lComprobante == null)
                {
                    lComprobante = new LComprobante();
                }
                return lComprobante;
            }
        }
        public LNota LNota
        {
            get
            {
                if (lNota == null)
                {
                    lNota = new LNota();
                }
                return lNota;
            }
        }


    }
}
