import React from 'react';
import axios from 'axios';
import './Supervisores.css'

class SupervisoresMarcarLimp extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            terrenos: '',
            trabalhadores:'',
            auth: "Bearer " + this.props.token,
            checkedTerr: [],
            checkedTrab: [],
            displayTableTerrenos: 1,
            displayTableTrabalhadores: 0,
            numberCheckedTerr: 0,
            numberCheckedTrab: 0,
            marcado: 0

        };

        this.terrenosCamara = this.terrenosCamara.bind(this);
        this.trabalhadoresCamara = this.trabalhadoresCamara.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        this.handleProxButton = this.handleProxButton.bind(this);
        this.handleVoltarButton = this.handleVoltarButton.bind(this);
        this.handleCheckTerr = this.handleCheckTerr.bind(this);
        this.handleCheckTrab = this.handleCheckTrab.bind(this);
        this.terrenostableCamara = this.terrenostableCamara.bind(this);
        this.trabalhadorestableCamara = this.trabalhadorestableCamara.bind(this);
        this.trabalhadorestable = this.trabalhadorestable.bind(this);
        this.handleMarcarLimpezaButton = this.handleMarcarLimpezaButton.bind(this);
    }

    componentDidMount()
    {
        this.terrenosCamara();
        this.trabalhadoresCamara();
    }

    handleCheckTerr(event)
    {
        this.state.checkedTerr[event.target.value] = event.target.checked;
    }

    handleCheckTrab(event)
    {
        this.state.checkedTrab[event.target.value] = event.target.checked;
    }


    terrenosCamara() 
    {
        axios.get('https://localhost:44301/supervisores/Terrenoscamara', {
                params: {
                    Username: this.props.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({terrenos: response.data});
                console.log(response.data);
            }) 
            .catch(response => {
                console.log(response);
            })
    }

    trabalhadoresCamara() 
    {
        axios.get('https://localhost:44301/supervisores/Trabalhadorescamara', {
                params: {
                    Username: this.props.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({trabalhadores: response.data});
                console.log(response.data);
            }) 
            .catch(response => {
                console.log(response);
            })
    }


    terrenostable(){
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr key={terreno.id_Terreno}>
               <td style={{textAlign: "left", paddingLeft: "2%"}}>
                    <div class="custom-control custom-checkbox">
                        <input style={{display: "inline", visibility: terreno.estadoLimpeza ? "hidden" : "visible"}} type="checkbox" key={terreno.id_Terreno} onChange={this.handleCheckTerr} defaultChecked={this.state.checkedTerr[index]? "checked": null} value={index} className="form-check-input" id="checkmark"/>
                    </div>
               </td>
               <td className="colexpand" style={{textAlign: "left"}}>
                    <p style={{display: "inline"}}>{terreno.morada} - {terreno.cod_postal}</p> 
               </td>
            </tr>
            ) : "Nenhuma zona encontrado.")
    }

    terrenostableCamara(){
        return( this.state.displayTableTerrenos ?
            <div>
                <table className="table table-responsive table-hover table-bordered ">
                    <tbody>
                        {this.terrenostable()}
                    </tbody>
                </table>
                <div class="container text-right">
                    <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={this.handleProxButton} value="Próximo" />
                </div>
                
            </div>
            : null)
    }

    trabalhadorestable(){
        return (this.state.trabalhadores.length > 0 ? this.state.trabalhadores.map((trab, index) =>
            <tr key={trab.username}>
               <td style={{textAlign: "left", paddingLeft: "2%"}}>
                    <div class="custom-control custom-checkbox">
                        <input style={{display: "inline"}} type="checkbox" key={trab.username} onChange={this.handleCheckTrab} defaultChecked={this.state.checkedTrab[index]? "checked": null} value={index} className="form-check-input" id="checkmark"/>
                    </div>
               </td>
               <td className="colexpand" style={{textAlign: "left"}}>
                    <p style={{display: "inline"}}>Equipa {trab.nome}</p> 
               </td>
            </tr>
            ) : "Nenhuma zona encontrado.")
    }

    trabalhadorestableCamara(){
        return( this.state.displayTableTrabalhadores ?
            <div>
                <table className="table table-responsive table-hover table-bordered ">
                    <tbody>
                        {this.trabalhadorestable()}
                    </tbody>
                </table>
                <div class="container text-left">
                    <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={this.handleVoltarButton} value="Voltar" />
                </div>
            </div>
            : null)
    }


    handleProxButton(event) {
        event.preventDefault();
        this.setState({displayTableTerrenos: 0});
        this.setState({displayTableTrabalhadores: 1});
        var i;
        var count = 0;
        for(i = 0; i < this.state.checkedTerr.length; i++)
        {
            if(this.state.checkedTerr[i]){
                count++;
            }
        }
        this.setState({numberCheckedTerr: count});
        
    }

    handleVoltarButton(event) {
        event.preventDefault();
        this.setState({displayTableTrabalhadores: 0});
        this.setState({displayTableTerrenos: 1});
        var i;
        var count = 0;
        for(i = 0; i < this.state.checkedTrab.length; i++)
        {
            if(this.state.checkedTrab[i]){
                count++;
            }
        }
        this.setState({numberCheckedTrab: count});        
    }

    handleMarcarLimpezaButton(event){
        event.preventDefault();
        
        if(!this.state.terrenos.length > 0) return null;
        if(!this.state.trabalhadores.length > 0) return null;
        var i;
        var j;
        for(i = 0; i < this.state.checkedTerr.length; i++)
        {   
            if(this.state.checkedTerr[i])
            {
                for(j = 0; j < this.state.checkedTrab.length; j++)
                {
                    if(this.state.checkedTrab[j])
                    {
                        axios({
                            method: 'post',
                            url: 'https://localhost:44301/supervisores/AgendarLimpeza',
                            data: JSON.stringify(this.props.username + ',' + this.state.trabalhadores[j].username + ',' + this.state.terrenos[i].id_Terreno),  
                            headers: {
                                "Content-Type": "application/json",
                                "Authorization": this.state.auth
                            }
                        })
                        .then(response => {
                            this.setState({marcado: 1});
                            console.log(response.data);
                        }) 
                        .catch(response => {
                            alert("Erro na marcação da limpeza.");
                            console.log(response);
                        })
                    }
                }
            }
        }
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
                                    <p  style={{textAlign: 'left'}} className="card-text login-text">{'Escolha os terrenos camarários para limpeza '}</p>
                                    <p  style={{textAlign: 'left'}} className="card-text login-text">{this.state.displayTableTerrenos == 1? '' :(this.state.numberCheckedTerr > 0 ?'Tem '+ this.state.numberCheckedTerr + (this.state.numberCheckedTerr > 1 ? ' terrenos selecionados' : ' terreno selecionado'):'')}</p>
                                    <p  style={{textAlign: 'left'}} className="card-text login-text">{this.state.displayTableTrabalhadores == 1? '' :(this.state.numberCheckedTrab > 0 ?'Tem '+ this.state.numberCheckedTrab + (this.state.numberCheckedTrab > 1 ? ' trabalhador selecionado' : ' trabalhadores selecionados'):'')}</p>
                                    {this.terrenostableCamara()}
                                    {this.trabalhadorestableCamara()}
                                    <p  style={{textAlign: 'left'}} className="card-text login-text">{this.state.marcado? 'Limpeza marcada com sucesso.': ''}</p>
                                    <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={ this.handleMarcarLimpezaButton} value="Marcar Limpeza" />                                    
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }
}

export default SupervisoresMarcarLimp;