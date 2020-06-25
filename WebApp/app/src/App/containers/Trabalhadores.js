import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import TrabalhadoresLimpeza from "./TrabalhadoresLimpeza";
import { Switch, Route } from 'react-router-dom';
import './Trabalhadores.css'

  class Trabalhadores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            sugTerreno: [],
            sucesso: 0
        };

        this.sugestaoTerreno =  this.sugestaoTerreno.bind(this);
    }

    componentDidMount()
    {
        this.sugestaoTerreno();
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
                this.setState({sugTerreno : response.data});
                console.log(response.data);
            })
            .catch(response => {
                alert("Erro no carregamento do terreno sugerido.");
                console.log(response);
            })
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
                                    <p>Sugerimos que o terreno a limpar seja:</p>
                                    {this.state.sugTerreno.length === 0 ? null :<p>{this.state.sugTerreno[0].morada}</p>}
                                    <div className="map-containerDirection">
                                        {this.state.sugTerreno.length === 0 ? null :<DirectionsMap  Data={this.state.sugTerreno}/>}
                                    </div> 
                                </div>                               
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
            </Route>
                
                <Route path='/trabalhadores/limpeza'>
                    <TrabalhadoresLimpeza username={this.props.username} user={this.props.user} token={this.props.token} />
                </Route>

            </Switch>
        );
    }
}
export default Trabalhadores;