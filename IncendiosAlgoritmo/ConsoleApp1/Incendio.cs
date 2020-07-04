using System;
using System.Collections.Generic;
using System.Text;

namespace IncendiosAlgoritmo
{
    class Incendio
    {
        private String id;
        private DateTime dataAlerta;
        private DateTime dataExtincao;
        private String tipo;
        private Double area;
        private String zona;
        private Coordenadas cord;
        private Double distancia; //apenas usado para o algoritmo


        public Incendio()
        {
            this.id = "";
            this.dataAlerta = new DateTime();
            this.dataExtincao = new DateTime();
            this.tipo = "";
            this.area = 0;
            this.zona = "";
            this.cord = new Coordenadas();
            this.distancia = Int32.MaxValue; ;
        }

        public Incendio(String id, DateTime dataAlerta, DateTime dataExtincao, String tipo, Double area, String z, Coordenadas cord)
        {
            this.id = id;
            this.dataAlerta = dataAlerta;
            this.dataExtincao = dataExtincao;
            this.tipo = tipo;
            this.area = area;
            this.zona = z;
            this.cord = cord;
            this.distancia = Int32.MaxValue; ;
        }

        public DateTime getDataAlerta()
        {
            return this.dataAlerta;
        }

        public DateTime getDataExtincao()
        {
            return this.dataExtincao;
        }

        public Coordenadas getCoordenadas()
        {
            return this.cord;
        }

        public String getId()
        {
            return this.id;
        }

        public String getTipo()
        {
            return this.tipo;
        }

        public Double getArea()
        {
            return this.area;
        }

        public String getZona()
        {
            return this.zona;
        }

        public Double getDistancia()
        {
            return this.distancia;
        }

        public void setDistancia(Double c)
        {
            this.distancia = c;
        }

        public void setZona(String c)
        {
            this.zona = c;
        }
    }
}
