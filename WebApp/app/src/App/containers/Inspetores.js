import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import Geolocation from './Geolocation';
import './Inspetores.css'


  class Inspetores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            percurso: [],
            latitude: null,
            longitude: null,
            morada: null
        };
        this.getLocation = this.getLocation.bind(this);
        this.getCoordinates = this.getCoordinates.bind(this);
        this.getAddress = this.getAddress.bind(this);
        this.sugestaoPercurso = this.sugestaoPercurso.bind(this);
        
    }

    componentDidMount()
    {
        this.sugestaoPercurso();
        
    }

    getLocation(){
        if(navigator.geolocation){
            navigator.geolocation.getCurrentPosition(this.getCoordinates, this.handleError, {enableHighAccuracy:true});
        } else {
            alert("Localização não suportada.")
        }
    }

    getCoordinates(position){
        this.setState({ latitude: position.coords.latitude, longitude: position.coords.longitude})
        this.getAddress();
    }

    getAddress() {
        fetch('https://maps.googleapis.com/maps/api/geocode/json?latlng='+this.state.latitude+','+ this.state.longitude+'&sensor=false&key=AIzaSyD94bNwC33Z03mXP2n1toNLXj8eCAQgOYQ')
        .then(response => response.json())
        .then(data => this.setState({
            morada : data.results[0].formatted_address
        }))
        .catch(error => alert(error))
    }

    handleError(error){
        switch(error.code) {
            case error.PERMISSION_DENIED:
                alert("Não permitiu saber a sua localização.")
                break;
            case error.POSITION_UNAVAILABLE:
                alert("Localização não disponível.")
                break;
            case error.TIMEOUT:
                alert("Excedido o tempo limite.")
                break;
            default: alert("Erro desconhecido.")
            }
    }

    async sugestaoPercurso() 
    {
        await axios.get('https://localhost:44301/inspetores/Sugestaoinspecao', {
                params: {
                    Username: this.props.user.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({percurso: response.data});
                console.log(response.data);
            }) 
            .catch(response => {
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
                                    <h4 className="card-title login-title">Nome Inspetor</h4>
                                    <p className="card-text login-text">Propriedades para Inspeção</p>
                                    <table className="table table-responsive table-hover table-bordered table-prop">
                                        <thead>
                                            <tr>
                                                <th scole="col colexpand">Morada</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                               <td className="linktext">Morada para Inspeção, nº 5, Braga</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <form>
                                        <input className="searchbox" type="text" placeholder="Procurar propriedade..." name="procurar" />
                                        <svg className="bi bi-search" width="1.2em" height="1.2em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path fillRule="evenodd" d="M10.442 10.442a1 1 0 011.415 0l3.85 3.85a1 1 0 01-1.414 1.415l-3.85-3.85a1 1 0 010-1.415z" clipRule="evenodd"/>
                                            <path fillRule="evenodd" d="M6.5 12a5.5 5.5 0 100-11 5.5 5.5 0 000 11zM13 6.5a6.5 6.5 0 11-13 0 6.5 6.5 0 0113 0z" clipRule="evenodd"/>
                                        </svg>
                                    </form>
                                    <div>
                                        <button onClick={this.getLocation}> Localização </button>
                                        <h4> Coordinates </h4>
                                        <p> morada: {this.state.morada} </p>
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

export default Inspetores;

/*
<div className="map-containerDirection">
                                        {this.state.percurso.length === 0 ? null :<DirectionsMap  Data={this.state.percurso}/>}
                                    </div>
                                    */
