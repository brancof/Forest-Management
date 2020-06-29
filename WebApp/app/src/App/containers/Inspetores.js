import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import { Switch, Route, Link } from 'react-router-dom';
import './Inspetores.css'
import InspetoresInspecionar from './InspetoresInspecionar';

function toLetters(num) {
    "use strict";
    var mod = num % 26,
        pow = num / 26 | 0,
        out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
    return pow ? toLetters(pow) + out : out;
}

  class Inspetores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            percurso: [],
            terrenos:'',
            latitude: null,
            longitude: null,
            morada: null,
            percursoMoradas: [],
            sucesso: 0,
            displayPercurso: 0,
            idSelected:""
        };
        this.getLocation = this.getLocation.bind(this);
        this.getCoordinates = this.getCoordinates.bind(this);
        this.getAddress = this.getAddress.bind(this);
        this.sugestaoPercurso = this.sugestaoPercurso.bind(this);
        this.atualizaGPS = this.atualizaGPS.bind(this);
        this.constPercurso = this.constPercurso.bind(this);
        
        
    }

    componentDidMount()
    {
        this.sugestaoPercurso();
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
                this.setState({percurso: response.data, terrenos: response.data});
                this.setState({displayPercurso: 1});
                console.log(response.data);
            }) 
            .catch(response => {
                console.log(response);
            })
    }

    atualizaGPS(){
        axios({
            method: 'put',
            url: 'https://localhost:44301/inspetores/Localizacao',
            data: JSON.stringify(this.props.user.username + '|' + this.state.latitude + '|' + this.state.longitude), 
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
            }
        })
        .then(response => {
            console.log(response);
            this.setState({sucesso: 1});
            this.state.percurso.unshift({ latitude: this.state.latitude, longitude: this.state.longitude});
        }) 
        .catch(response => {
            alert("Erro na atualização das coordenadas.");
            console.log(response);
        })
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
        this.atualizaGPS();
        
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

    constPercurso(){
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr>
                <td style={{textAlign: "left"}}>
                    <p>{toLetters(index + 1)}</p>
               </td>
               <td className="colexpand" style={{textAlign: "left"}}>
               {index === 0 && this.state.morada != null?
                    <div>
                        <p>{this.state.morada}</p>
                    </div>
                    :<Link to='/inspetores/inspecionar' class="btn btn-link" onClick={this.handleClickLink.bind(this, terreno.id_Terreno)}>
                        {terreno.morada + "-" + terreno.cod_postal}
                    </Link>
                }
               </td>
            </tr>
            ) :this.setState({displayPercurso: 0}))
    }
    
    handleClickLink(id){
        this.setState({idSelected: id});
    }
  
    render() {
        return (

            <Switch>
                <Route exact path='/inspetores'>
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                <div className="card-block">
                                    <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                    <p className="card-text login-text">Propriedades para Inspeção</p>
                                    {this.state.displayPercurso === 0? "Não existem terrenos para inspecionar.":
                                        <table style={{maxHeight: "200px"}} className="table table-responsive table-hover">
                                            <tbody>
                                                {this.constPercurso()}
                                            </tbody>
                                        </table>
                                    }
                                    <div class="text-left">
                                        <button type="button" class="btn btn-dark" onClick={this.getLocation}>Localização</button>
                                    </div>
                                    <div className="map-containerDirection">
                                        {this.state.percurso.length === 0 ? null :<DirectionsMap  Data={this.state.percurso}/>}
                                        {this.state.sucesso === 0 ? null :<DirectionsMap  Data={this.state.percurso}/>}
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
            </Route>
                
                <Route path='/inspetores/inspecionar'>
                    <InspetoresInspecionar username={this.props.username} user={this.props.user} token={this.props.token} idSelected={this.state.idSelected} terrenos={this.state.terrenos}/>
                </Route>

            </Switch>
        );
    }
}

export default Inspetores;

/*
<form>
                                        <input className="searchbox" type="text" placeholder="Procurar propriedade..." name="procurar" />
                                        <svg className="bi bi-search" width="1.2em" height="1.2em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path fillRule="evenodd" d="M10.442 10.442a1 1 0 011.415 0l3.85 3.85a1 1 0 01-1.414 1.415l-3.85-3.85a1 1 0 010-1.415z" clipRule="evenodd"/>
                                            <path fillRule="evenodd" d="M6.5 12a5.5 5.5 0 100-11 5.5 5.5 0 000 11zM13 6.5a6.5 6.5 0 11-13 0 6.5 6.5 0 0113 0z" clipRule="evenodd"/>
                                        </svg>
                                    </form>
                                    */
