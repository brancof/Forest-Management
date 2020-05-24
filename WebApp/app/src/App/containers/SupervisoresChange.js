import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";
import Heat from './Heat';
import './Supervisores.css'

class SupervisoresChange extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            terrenosLimpos: 0,
            zonas: '',
            terrenos: '',
            mapInfo: [],
            checked: [],
            nifAntigo : 0,
            nifNovo: 0,
            boolAlterado: 0,
            sucesso: 0

        };
        this.concelhoSeguro = this.concelhoSeguro.bind(this);
        this.concelhoSeguro();
        this.zonasConcelho = this.zonasConcelho.bind(this);
        this.zonasConcelho();

        this.loadTerrenos = this.loadTerrenos.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
        this.handleChangeNifNovo = this.handleChangeNifNovo.bind(this);
        this.handleChangeNifAntigo = this.handleChangeNifAntigo.bind(this);
        this.handleChangeNifProprietario = this.handleChangeNifProprietario.bind(this);
        this.handleProcurarButton = this.handleProcurarButton.bind(this);
    }

    concelhoSeguro() 
    {
        axios.get('https://localhost:44301/supervisores/Seguranca', {
                params: {
                    Username: this.props.username,
                    Password: this.props.password
                }
            })
            .then(response => {
                //alert("Login efectuado com successo.");
                this.setState({terrenosLimpos: response.data});
                console.log(response.data);                
                //this.forceUpdate();
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
                }
            })
            .then(response => {
                //alert("Login efectuado com successo.");
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
                
                //this.forceUpdate();
            }) 
            .catch(response => {
                alert("Erro no carregamento de terrenos.");
                console.log(response);
            })
    }


    handleCheck(event)
    {
        this.state.checked[event.target.value] = event.target.checked;
    }

    terrenostable(){
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr key={terreno.id_Terreno}>
               <td style={{textAlign: "left", paddingLeft: "5%"}}>
                   <input style={{display: "inline"}} type="checkbox"  disabled={this.state.boolAlterado} key={terreno.id_Terreno} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark"/>
               <p style={{display: "inline"}}>{terreno.morada} - {terreno.cod_postal}</p></td>
               <td style={{textAlign: "left"}}>{terreno.nif}</td> 
            </tr>
            ) : null)
    }

    novoNif(){
        return ( this.state.terrenos.length > 0 && !this.state.sucesso ?  
        <div className="form-group">
             <p style={{textAlign: "left"}} >{'Insira o Nif do novo proprietário '}</p>
            <input type="text" className="form-control" id="nifInput" onChange={this.handleChangeNifNovo} placeholder="Nif"></input>
            <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleChangeNifProprietario} value="Alterar" />
        </div>: null)
    }

    handleChangeNifProprietario(event)
    {
        event.preventDefault();
        
        if(!this.state.terrenos.length > 0) return;
        var i;
        for(i = 0; i < this.state.checked.length; i++)
        {
            if(this.state.checked[i])
            {
                axios({
                    method: 'put',
                    url: 'https://localhost:44301/supervisores/Trocaprop',
                    data: JSON.stringify(this.props.username + ',' + this.props.password + ',' + this.state.terrenos[i].id_Terreno + ',' + this.state.nifNovo), 
                    headers: {
                        "Content-Type": "application/json"
                    }
                })
                    .then(response => {
                        console.log(response);
                        this.setState({boolAlterado: 1});
                        this.setState({sucesso: 1});
                        this.loadTerrenos(this.state.nifNovo);
                    }) 
                    .catch(response => {
                        alert("Erro na troca de proprietário.");
                        console.log(response);
                    })
            }
        }

    }

    handleChangeNifAntigo(event) {
        this.setState({nifAntigo: event.target.value});
    }

    handleChangeNifNovo(event) {
        this.setState({nifNovo: event.target.value});
    }

    loadTerrenos(nif){
        axios.get('https://localhost:44301/supervisores/Terrenosnif', {
            params: {
                Username: this.props.username,
                Nif: nif
            }
        })
        .then(response => {
            //alert("Login efectuado com successo.");
            this.setState({terrenos: response.data});
            console.log(response.data);
            response.data.map((terreno, index) => {
                this.state.checked[index]=false;
            });                
            //this.forceUpdate();
        }) 
        .catch(response => {
            alert("Erro no carregamento de terrenos.");
            console.log(response);
        })
    }

    handleProcurarButton(event) {
        event.preventDefault();
        this.loadTerrenos(this.state.nifAntigo);
        this.setState({boolAlterado: 0});
        this.setState({sucesso: 0});
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
                                    <p className="card-text login-text">{''}</p>
                                    <div className="form-group">
                                        <p style={{textAlign: 'left'}}>{'Insira o Nif do atual proprietário '}</p>
                                        <input type="text" className="form-control" id="nifInput" onChange={this.handleChangeNifAntigo} placeholder="Nif"></input>
                                        <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleProcurarButton} value="Procurar" />
                                    </div>
                                    <table className="table table-hover table-bordered table-prop">
                                        <tbody>
                                            {this.terrenostable()}
                                        </tbody>
                                    </table>
                                    <div className="form-group">
                                        <p style={{textAlign: 'left'}}>{this.state.sucesso ? 'O terreno foi alterado de proprietário com sucesso.' : ''}</p>
                                    </div>
                                    {this.novoNif()}
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }

/*
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
                                        <Heat HeatData={this.state.mapInfo}/>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }*/
    
}

export default SupervisoresChange;