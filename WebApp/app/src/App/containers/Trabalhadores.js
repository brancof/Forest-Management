import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import TrabalhadoresLimpeza from "./TrabalhadoresLimpeza";
import { Switch, Route, Link } from 'react-router-dom';
import './Trabalhadores.css';
import Options from './Options';
import Notificacoes from './Notificacoes';

  class Trabalhadores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            sugTerrenos: [],
            sugTer: null,
            latitude: null,
            longitude: null,

            morada: null,
            sucesso: 0,
            idSelected: ""
        };

        this.getLocation = this.getLocation.bind(this);
        this.getCoordinates = this.getCoordinates.bind(this);
        this.getAddress = this.getAddress.bind(this);
        this.sugestaoTerreno =  this.sugestaoTerreno.bind(this);
        this.atualizaGPS = this.atualizaGPS.bind(this);
        this.terrenosPendentes = this.terrenosPendentes.bind(this);
        this.handleClickLink = this.handleClickLink.bind(this);
    }

    componentDidMount()
    {
        this.sugestaoTerreno();
        this.terrenosPendentes();
    }

    atualizaGPS(){
        axios({
            method: 'put',
            url: 'https://localhost:44301/trabalhadores/Localizacao',
            data: JSON.stringify(this.props.user.username + '|' + this.state.latitude + '|' + this.state.longitude), 
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
            }
        })
        .then(response => {
            //console.log(response);
            this.setState({sucesso: 1});
            this.state.sugTerrenos.unshift({ latitude: this.state.latitude, longitude: this.state.longitude});
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

    async sugestaoTerreno() {
        await axios.get('https://localhost:44301/trabalhadores/Sugestao', {
            params: {
                Username: this.props.username
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({sugTerrenos : response.data});
                this.setState({sugTer : this.state.sugTerrenos[0]});
                //console.log(response.data);
            })
            .catch(response => {
                alert("Erro no carregamento do terreno sugerido.");
                console.log(response);
            })
    }

    terrenosPendentes() {
        axios.get('https://localhost:44301/trabalhadores/LimpezasPendentes', {
            params: {
                Username: this.props.username
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({ terrenos: response.data , displayTable : 1});
                //console.log(response.data);
            })
            .catch(response => {
                alert("Erro no carregamento de terrenos pendentes.");
                console.log(response);
            })
    }

    handleClickLink(){
        this.setState({idSelected: this.state.sugTer.id_Terreno});
    }

    render() {
        return (

            <Switch>
                <Route exact path='/trabalhadores'>
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                <div className="card-block">
                                    <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                    <p className="card-text login-text">Gestão de Trabalho</p>
                                    <h5 style={{ textAlign: 'left' }} className="card-title login-title">{this.props.user.concelho}</h5>
                                    {this.state.sugTerrenos.length === 0 || this.state.sugTer === null?
                                        <p>Não existem terrenos que necessitem de limpeza</p>  
                                        :
                                        <div>
                                            <p>Sugerimos que o terreno a limpar seja:</p>
                                            <Link to='/trabalhadores/limpeza' class="btn btn-link" onClick={this.handleClickLink}>
                                                {this.state.sugTer.morada}
                                            </Link>                                    
                                            <div style={{marginBottom:"8%", marginTop:"3%"}}>
                                                <button style={{float:"Right"}} type="button" class="btn btn-dark btn-sm" onClick={this.getLocation}>Localiza-me</button>
                                            </div>
                                        </div>
                                    }
                                    <div className="map-containerDirection">
                                        {this.state.sugTerrenos.length === 0 && this.state.sucesso === 0 ? null :<DirectionsMap  Data={this.state.sugTerrenos}/>}
                                        {this.state.sucesso === 0 ? null :<DirectionsMap  Data={this.state.sugTerrenos}/>}
                                    </div> 
                                </div>                               
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
            </Route>
                
                <Route path='/trabalhadores/limpeza'>
                    <TrabalhadoresLimpeza username={this.props.username} user={this.props.user} token={this.props.token} idSelected={this.state.idSelected}/>
                </Route>
                
                <Route path='/trabalhadores/notificacoes'>
                    <Notificacoes user={this.props.user} username={this.props.username} change={{ user: this.props.change }} accounttype={this.props.accounttype} token={this.props.token} />
                </Route>

                <Route path='/trabalhadores/opcoes'>
                    <Options user={this.props.user} username={this.props.username} change={{ user: this.props.change }} token={this.props.token} accounttype={this.props.accounttype} />
                </Route>

            </Switch>
        );
    }
}
export default Trabalhadores;