using System;
using System.Collections.Generic;
using System.Text;

namespace IncendiosAlgoritmo
{
    class Zona
    {
        private String codPostal;
        private Double lat;
        private Double lon;
        private Double nivel;
        private int nIncendios;


        public Zona()
        {
            this.codPostal = "";
            this.lat = 0;
            this.lon = 0;
            this.nivel = 0;
            this.nIncendios = 0;
        }

        public Zona(String cod, Double lat, Double lon, Double nivel,  int nincendios)
        {
            this.codPostal = cod;
            this.lat = lat;
            this.lon = lon;
            this.nivel = nivel;
            this.nIncendios = nincendios;
        }

        public Zona(String cod, Double lat, Double lon, int nincendios)
        {
            this.codPostal = cod;
            this.lat = lat;
            this.lon = lon;
            this.nivel = 0;
            this.nIncendios = nincendios;
        }

        public String getCodPostal()
        {
            return this.codPostal;
        }

        public Double getLatitude()
        {
            return this.lat;
        }

        public Double getLongitude()
        {
            return this.lon;
        }

        public Double getNivel()
        {
            return this.nivel;
        }

        public int getnIncendios()
        {
            return this.nIncendios;
        }
    }
}
