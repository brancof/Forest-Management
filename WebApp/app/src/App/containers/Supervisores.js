import React from 'react';
import axios from 'axios';
import Heat from './Heat';
import './Supervisores.css'

class Supervisores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            terrenosLimpos: 0,
            zonas: '',
            terrenos: '',
            auth: "Bearer " + this.props.token,
            mapInfo: [],
        };
        this.concelhoSeguro = this.concelhoSeguro.bind(this);
        this.zonasConcelho = this.zonasConcelho.bind(this);

    }
    
    componentDidMount()
    {
        this.concelhoSeguro();
        this.zonasConcelho();
    }
    
    concelhoSeguro() 
    {
        axios.get('https://localhost:44301/supervisores/Seguranca', {
                params: {
                    Username: this.props.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({terrenosLimpos: response.data});
                console.log(response.data);                
            }) 
            .catch(response => {
                alert("Erro no carregamento de terrenos.");
                console.log(response);
            })
    }

    zonasConcelho() 
    {
        axios.get('https://localhost:44301/supervisores/Zonasconcelho', {
                params: {
                    Username: this.props.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({zonas: response.data});
                console.log(response.data);
                var mapZonas = [];
                response.data.map((zonas, index) => {
                    mapZonas[index]={
                        lat: zonas.latitude,
                        lng: zonas.longitude,
                        //weight: zonas.nivelCritico
                    };
                });
                this.setState({mapInfo: mapZonas});
            }) 
            .catch(response => {
                //alert("Erro no carregamento de terrenos.");
                console.log(response);
            })
    }

    render() {

        return (
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                <div className="card-block">
                                    <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                    <p className="card-text login-text">Gestão de Concelhos</p>
                                    <h5 style={{textAlign: 'left'}} className="card-title login-title">{this.props.user.concelho}</h5>
                                    <p style={{textAlign: 'left'}} className="card-text login-text">Número total de terrenos por limpar: {this.state.terrenosLimpos}</p>
            
                                    <div className="map-container">
                                        {this.state.mapInfo.length === 0 ? null : <Heat HeatData={this.state.mapInfo}/>}
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }
    
}

export default Supervisores;